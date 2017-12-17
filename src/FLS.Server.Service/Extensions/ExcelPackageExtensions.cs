using System.IO;
using OfficeOpenXml;
using System.Collections.Generic;
using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.UI;

namespace FLS.Server.Service.Extensions
{
    public static class ExcelPackageExtensions
    {
        public static MemoryStream ToMemoryStream(this ExcelPackage excelPackage)
        {
            var bytes = excelPackage.GetAsByteArray();
            var memoryStream = new MemoryStream(bytes);
            return memoryStream;
        }

        /// <summary>
        /// https://long2know.com/2016/05/parsing-excel-to-a-list-of-objects/
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="worksheet"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this ExcelWorksheet worksheet, List<string> ignoreColumns = null,
            Dictionary<string, string> map = null) where T : new()
        {
            //DateTime Conversion
            var convertDateTime = new Func<double, DateTime>(excelDate =>
            {
                if (excelDate < 1)
                    throw new ArgumentException("Excel dates cannot be smaller than 0.");

                var dateOfReference = new DateTime(1900, 1, 1);

                if (excelDate > 60d)
                    excelDate = excelDate - 2;
                else
                    excelDate = excelDate - 1;
                return dateOfReference.AddDays(excelDate);
            });

            var props = typeof(T).GetProperties()
                .Select(prop =>
                {
                    var displayAttribute =
                        (DisplayAttribute) prop.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();
                    return new
                    {
                        Name = prop.Name,
                        DisplayName = displayAttribute == null ? prop.Name : displayAttribute.Name,
                        Order =
                        displayAttribute == null || !displayAttribute.GetOrder().HasValue ? 999 : displayAttribute.Order,
                        PropertyInfo = prop,
                        PropertyType = prop.PropertyType,
                        HasDisplayName = displayAttribute != null
                    };
                })
                .Where(prop => !string.IsNullOrWhiteSpace(prop.DisplayName))
                .ToList();

            if (ignoreColumns == null)
                ignoreColumns = new List<string>();
            var retList = new List<T>();
            var columns = new List<ExcelMap>();

            var start = worksheet.Dimension.Start;
            var end = worksheet.Dimension.End;
            var startCol = start.Column;
            var startRow = start.Row;
            var endCol = end.Column;
            var endRow = end.Row;

            // Assume first row has column names
            for (int col = startCol; col <= endCol; col++)
            {
                var cellValue = (worksheet.Cells[startRow, col].Value ?? string.Empty).ToString().Trim();
                if (string.IsNullOrWhiteSpace(cellValue) == false
                    && ignoreColumns.Contains(cellValue) == false)
                {
                    columns.Add(new ExcelMap()
                    {
                        Name = cellValue,
                        MappedTo = map == null || map.Count == 0
                            ? cellValue
                            : map.ContainsKey(cellValue) ? map[cellValue] : string.Empty,
                        Index = col
                    });
                }
            }

            // Now iterate over all the rows
            for (int rowIndex = startRow + 1; rowIndex <= endRow; rowIndex++)
            {
                var item = new T();
                columns.ForEach(column =>
                {
                    var value = worksheet.Cells[rowIndex, column.Index].Value;
                    var valueStr = value == null ? string.Empty : value.ToString().Trim();


                    if (column.MappedTo.Contains("."))
                    {
                        try
                        {
                            string[] bits = column.MappedTo.Split('.');
                            object target = item;
                            for (int i = 0; i < bits.Length - 1; i++)
                            {
                                PropertyInfo propertyToGet = target.GetType().GetProperty(bits[i]);
                                target = propertyToGet.GetValue(target, null);

                                if (target == null && propertyToGet.PropertyType.IsClass)
                                {
                                    target = Activator.CreateInstance(propertyToGet.PropertyType);
                                }
                            }
                            PropertyInfo propertyToSet = target.GetType().GetProperty(bits.Last());
                            var parsedValue = GetParsedValue<T>(valueStr, propertyToSet.PropertyType, convertDateTime,
                                value);
                            propertyToSet.SetValue(target, parsedValue);
                        }
                        catch (Exception ex)
                        {
                            // Indicate parsing error on row?
                        }
                    }
                    else
                    {
                        var prop = string.IsNullOrWhiteSpace(column.MappedTo)
                            ? null
                            : props.First(p => p.Name.Contains(column.MappedTo));

                        if (prop != null)
                        {
                            var propertyType = prop.PropertyType;
                            var parsedValue = GetParsedValue<T>(valueStr, propertyType, convertDateTime, value);

                            try
                            {
                                prop.PropertyInfo.SetValue(item, parsedValue);
                            }
                            catch (Exception ex)
                            {
                                // Indicate parsing error on row?
                            }
                        }
                    }
                });

                retList.Add(item);
            }

            return retList;
        }

        private static object GetParsedValue<T>(string valueStr, Type propertyType,
            Func<double, DateTime> convertDateTime, object value)
            where T : new()
        {
            object parsedValue = null;

            if (valueStr.ToUpper() == "NULL")
                parsedValue = null;
            else if (propertyType == typeof(int?) || propertyType == typeof(int))
            {
                int val;
                if (!int.TryParse(valueStr, out val))
                {
                    val = default(int);
                }

                parsedValue = val;
            }
            else if (propertyType == typeof(short?) || propertyType == typeof(short))
            {
                short val;
                if (!short.TryParse(valueStr, out val))
                    val = default(short);
                parsedValue = val;
            }
            else if (propertyType == typeof(long?) || propertyType == typeof(long))
            {
                long val;
                if (!long.TryParse(valueStr, out val))
                    val = default(long);
                parsedValue = val;
            }
            else if (propertyType == typeof(decimal?) || propertyType == typeof(decimal))
            {
                decimal val;
                if (!decimal.TryParse(valueStr, out val))
                    val = default(decimal);
                parsedValue = val;
            }
            else if (propertyType == typeof(double?) || propertyType == typeof(double))
            {
                double val;
                if (!double.TryParse(valueStr, out val))
                    val = default(double);
                parsedValue = val;
            }
            else if (propertyType == typeof(DateTime?) || propertyType == typeof(DateTime))
            {
                try
                {
                    DateTime val;
                    if (DateTime.TryParse(valueStr, out val))
                    {
                        return val;
                    }
                }
                catch (Exception)
                {
                }

                try
                {
                    if (value == null && propertyType == typeof(DateTime?))
                    {
                        parsedValue = null;
                    }
                    else
                    {
                        parsedValue = convertDateTime((double)value);
                    }
                }
                catch (Exception)
                {
                    if (propertyType == typeof(DateTime?))
                    {
                        parsedValue = null;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else if (propertyType == typeof(Guid?) || propertyType == typeof(Guid))
            {
                Guid val;
                if (!Guid.TryParse(valueStr, out val))
                    val = Guid.Empty;
                parsedValue = val;
            }
            else if (propertyType == typeof(Boolean?) || propertyType == typeof(Boolean))
            {
                bool val;
                if (valueStr == "0")
                    val = false;
                else if (valueStr == "1")
                    val = true;
                else if (!bool.TryParse(valueStr, out val))
                    val = default(bool);
                parsedValue = val;
            }
            else if (propertyType.IsEnum)
            {
                try
                {
                    parsedValue = Enum.ToObject(propertyType, int.Parse(valueStr));
                }
                catch
                {
                    parsedValue = Enum.ToObject(propertyType, 0);
                }
            }
            else if (propertyType == typeof(string))
            {
                parsedValue = valueStr;
            }
            else
            {
                try
                {
                    parsedValue = Convert.ChangeType(value, propertyType);
                }
                catch
                {
                    parsedValue = valueStr;
                }
            }
            return parsedValue;
        }
    }

    internal class ExcelMap
    {
        public ExcelMap()
        {
        }

        public int Index { get; set; }
        public string MappedTo { get; set; }
        public string Name { get; set; }
    }
}
