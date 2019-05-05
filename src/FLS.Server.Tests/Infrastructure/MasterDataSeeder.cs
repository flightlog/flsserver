using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using NLog;

namespace FLS.Server.Tests.Infrastructure
{
    public class MasterDataSeeder
    {
        private const string delim = ";";

        // token format: {KEY:VAL} - eg: {UserID:admin@testdata.local}
        private static readonly Regex matchTokenRegex = new Regex(@"(?:\{)(.[^\:\{\}]*)(?:\:)(.[^\:\{\}]*)(?:\})");

        private static readonly Type[] importExportTypes = new Type[] {
            typeof(bool), typeof(bool?),
            typeof(int), typeof(int?),
            typeof(float), typeof(float?),
            typeof(double), typeof(double?),
            typeof(Guid), typeof(Guid?),
            typeof(DateTime), typeof(DateTime?),
            typeof(string)
        };

        private string _baseDir;
        private DbContext _dbContext;
        private string _prefix;
        private ILogger _logger;
        private IDictionary<string, Func<string, object>> _tokenResolvers;

        public MasterDataSeeder(DbContext db, string baseDir, string prefix, ILogger logger, IDictionary<string, Func<string, object>> tokenResolvers = null)
        {
            _dbContext = db;
            _baseDir = baseDir;
            _prefix = prefix;
            _logger = logger;
            _tokenResolvers = tokenResolvers;

            logger.Info($"Master data seeder initialized: {db}, {baseDir}, {prefix}");
        }

        public void EntityToCsv<TEntity>() where TEntity : class
        {
            var entityType = typeof(TEntity);

            _logger.Info($"Exporting entity to csv: {entityType.Name}");

            var properties = entityType.GetProperties().Where(p => importExportTypes.Contains(p.PropertyType));
            using (var file = new StreamWriter(Path.Combine(_baseDir, (_prefix ?? "") + entityType.Name + ".csv")))
            {
                file.WriteLine(string.Join(delim, properties.Select(p => p.Name)));
                foreach (var row in _dbContext.Set<TEntity>())
                    file.WriteLine(string.Join(delim, properties.Select(p => ToStringValue(p, row))));
            }
        }

        public void MergeEntityFromCsv<TEntity>(string fileName, string overrideKeyProperty = null) where TEntity : class, new()
        {
            var entityType = typeof(TEntity);

            _logger.Info($"Importing entity from csv: {fileName} -> {entityType.Name}");

            using (var file = new StreamReader(Path.Combine(_baseDir, fileName)))
            {
                var headerLine = file.ReadLine();
                var columnNames = headerLine.Split(new string[] { delim }, StringSplitOptions.None);

                var objContext = ((IObjectContextAdapter)_dbContext).ObjectContext;
                var objSet = objContext.CreateObjectSet<TEntity>();

                List<int> keyIndices = new List<int>();
                if (!string.IsNullOrEmpty(overrideKeyProperty))
                {
                    var idx = Array.IndexOf(columnNames, overrideKeyProperty);
                    if (idx < 0)
                        throw new InvalidOperationException("Input file doesn't contain all required keys!");
                    keyIndices.Add(idx);
                }
                else
                    keyIndices.AddRange(GetKeyIndicesOfEntity(objSet, columnNames));

                while (!file.EndOfStream)
                {
                    EntityKey entityKey = null;

                    try
                    {
                        entityKey = null;

                        var curLineValues = file.ReadLine().Split(new string[] { delim }, StringSplitOptions.None);

                        object dbEntity = null;

                        if (!string.IsNullOrEmpty(overrideKeyProperty))
                        {
                            // use explicit property and construct FirstOrDefault
                            var overridenKeyProp = entityType.GetProperty(overrideKeyProperty);
                            var param = Expression.Parameter(typeof(TEntity));
                            var overridenKeyVal = ParseStringValue(overridenKeyProp, curLineValues[keyIndices.First()]);
                            var condition =
                                Expression.Lambda<Func<TEntity, bool>>(
                                    Expression.Equal(
                                        Expression.Property(param, overrideKeyProperty),
                                        Expression.Constant(overridenKeyVal, overridenKeyProp.PropertyType)
                                    ),
                                    param
                                );

                            dbEntity = objSet.OfType<TEntity>().FirstOrDefault(condition);

                            // add original key properties to keyIndices Collection. To avoid try-override key property.
                            keyIndices.AddRange(GetKeyIndicesOfEntity(objSet, columnNames));
                        }
                        else
                        {
                            // use declared key properties
                            var keyObject = new TEntity();
                            foreach (var idx in keyIndices)
                            {
                                var keyProperty = entityType.GetProperty(columnNames[idx]);
                                ParseStringValue(keyProperty, keyObject, curLineValues[idx]);
                            }

                            entityKey = objContext.CreateEntityKey(objSet.EntitySet.Name, keyObject);

                            if (!objContext.TryGetObjectByKey(entityKey, out dbEntity))
                                dbEntity = null;
                        }


                        if (dbEntity == null)
                        {
                            _logger.Debug("Entity {0} : {1}{2} doesn't exist yet - add.", entityType.Name, entityKey?.EntityKeyValues, overrideKeyProperty);
                            dbEntity = new TEntity();

                            // fill key values
                            foreach (var idx in keyIndices)
                            {
                                var keyProperty = entityType.GetProperty(columnNames[idx]);
                                ParseStringValue(keyProperty, dbEntity, curLineValues[idx]);
                            }

                            objSet.AddObject((TEntity)dbEntity);
                        }

                        // overwrite other values
                        for (int i = 0; i < columnNames.Length; i++)
                        {
                            if (keyIndices.Contains(i))
                                continue;

                            var property = entityType.GetProperty(columnNames[i]);
                            ParseStringValue(property, dbEntity, curLineValues[i]);
                        }

                        _logger.Debug($"Entity {entityType.Name} : {entityKey?.EntityKeyValues}{overrideKeyProperty} values set.");

                        _dbContext.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        _logger.Error("Unable to update entity {0} : {1}{2} - {3}", entityType.Name, entityKey?.EntityKeyValues, overrideKeyProperty, e);
                    }
                }
            }
        }

        private IEnumerable<int> GetKeyIndicesOfEntity<TEntity>(ObjectSet<TEntity> objSet, string[] columnNames) where TEntity : class
        {
            var keyNames = objSet.EntitySet.ElementType.KeyMembers.Select(m => m.Name);

            foreach (var keyName in keyNames)
            {
                var idx = Array.IndexOf(columnNames, keyName);
                if (idx < 0)
                    throw new InvalidOperationException("Input file doesn't contain all required keys!");
                yield return idx;
            }
        }

        private string ToStringValue(PropertyInfo p, object row)
        {
            var val = p.GetValue(row);
            if (val == null)
                return "NULL";
            return $"\"" + val.ToString().Replace("\"", "\\\"") + "\"";
        }

        private void ParseStringValue(PropertyInfo p, object row, string value)
        {
            p.SetValue(row, ParseStringValue(p, value));
        }

        private object ParseStringValue(PropertyInfo p, string value)
        {
            object parsed = null;

            if (value != "NULL")
            {
                value = value.Trim('\"');

                if (_tokenResolvers != null)
                {
                    var tokenMatch = matchTokenRegex.Match(value);
                    if (tokenMatch.Success)
                    {
                        if (_tokenResolvers.ContainsKey(tokenMatch.Groups[1].Value))
                            parsed = _tokenResolvers[tokenMatch.Groups[1].Value](tokenMatch.Groups[2].Value);
                    }
                }

                if (parsed == null)
                {
                    if (p.PropertyType == typeof(string))
                        parsed = value;
                    else if (p.PropertyType == typeof(bool))
                    {
                        if (value == "1")
                        {
                            parsed = true;
                        }
                        else if (value == "0")
                        {
                            parsed = false;
                        }
                        else
                        {
                            try
                            {
                                parsed = Convert.ToBoolean(value);
                            }
                            catch (Exception ex)
                            {
                                throw new FormatException("The string is not a recognized as a valid boolean value.");
                            }
                        }
                    }
                    else
                    {
                        Type parseMethodType = p.PropertyType;
                        if (parseMethodType.IsGenericType) // normally Nullable<T> - extract parse method of T (NULL is handled above)
                            parseMethodType = parseMethodType.GenericTypeArguments.First();

                        var parseMethod = parseMethodType.GetMethod("Parse", new Type[] { typeof(string) });
                        parsed = parseMethod.Invoke(null, new[] { value });
                    }
                }
            }

            return parsed;
        }
    }
}
