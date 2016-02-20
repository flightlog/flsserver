using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace FLS.Common.Comparer
{
    public static class ObjectComparer
    {
        /// <summary>
        /// Compares the properties of two objects of the same type and returns if all properties are equal.
        /// </summary>
        /// <param name="objectA">The first object to compare.</param>
        /// <param name="objectB">The second object to compre.</param>
        /// <param name="ignoreVirtualProperties">if set to <c>true</c> [ignore virtual properties].</param>
        /// <param name="ignoreList">A list of property names to ignore from the comparison.</param>
        /// <returns>
        ///   <c>true</c> if all property values are equal, otherwise <c>false</c>.
        /// </returns>
        public static bool AreObjectsEqual(object objectA, object objectB, bool ignoreVirtualProperties = true, params string[] ignoreList)
        {
            bool result;

            if (objectA != null && objectB != null)
            {
                Type objectType;

                objectType = objectA.GetType();

                foreach (
                    PropertyInfo propertyInfo in
                        objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                  .Where(p => p.CanRead && !ignoreList.Contains(p.Name)))
                {
                    if (ignoreVirtualProperties && propertyInfo.GetGetMethod().IsVirtual)
                    {
                        continue;
                    }

                    object valueA;
                    object valueB;

                    valueA = propertyInfo.GetValue(objectA, null);
                    valueB = propertyInfo.GetValue(objectB, null);

                    // if it is a primative type, value type or implements IComparable, just directly try and compare the value
                    if (CanDirectlyCompare(propertyInfo.PropertyType))
                    {
                        if (AreValuesEqual(valueA, valueB) == false)
                        {
                            Debug.WriteLine("Mismatch with property '{0}.{1}' found.", objectType.FullName,
                                            propertyInfo.Name);
                            return false;
                        }
                    }
                        // if it implements IEnumerable, then scan any items
                    else if (typeof (IEnumerable).IsAssignableFrom(propertyInfo.PropertyType))
                    {
                        IEnumerable<object> collectionItems1;
                        IEnumerable<object> collectionItems2;
                        int collectionItemsCount1;
                        int collectionItemsCount2;

                        // null check
                        if (valueA == null && valueB != null || valueA != null && valueB == null)
                        {
                            Debug.WriteLine("Mismatch with property '{0}.{1}' found.", objectType.FullName,
                                            propertyInfo.Name);
                            return false;
                        }
                        else if (valueA != null && valueB != null)
                        {
                            collectionItems1 = ((IEnumerable) valueA).Cast<object>();
                            collectionItems2 = ((IEnumerable) valueB).Cast<object>();
                            collectionItemsCount1 = collectionItems1.Count();
                            collectionItemsCount2 = collectionItems2.Count();

                            // check the counts to ensure they match
                            if (collectionItemsCount1 != collectionItemsCount2)
                            {
                                Debug.WriteLine("Collection counts for property '{0}.{1}' do not match.",
                                                objectType.FullName, propertyInfo.Name);
                                return false;
                            }
                                // and if they do, compare each item... this assumes both collections have the same order
                            else
                            {
                                for (int i = 0; i < collectionItemsCount1; i++)
                                {
                                    object collectionItem1;
                                    object collectionItem2;
                                    Type collectionItemType;

                                    collectionItem1 = collectionItems1.ElementAt(i);
                                    collectionItem2 = collectionItems2.ElementAt(i);
                                    collectionItemType = collectionItem1.GetType();

                                    if (CanDirectlyCompare(collectionItemType))
                                    {
                                        if (!AreValuesEqual(collectionItem1, collectionItem2))
                                        {
                                            Debug.WriteLine(
                                                "Item {0} in property collection '{1}.{2}' does not match.", i,
                                                objectType.FullName, propertyInfo.Name);
                                            return false;
                                        }
                                    }
                                    else if (
                                        !AreObjectsEqual(collectionItem1, collectionItem2, ignoreVirtualProperties,
                                                         ignoreList))
                                    {
                                        Debug.WriteLine(
                                            "Item {0} in property collection '{1}.{2}' does not match.", i,
                                            objectType.FullName, propertyInfo.Name);
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                    else if (propertyInfo.PropertyType.IsClass)
                    {
                        if (AreObjectsEqual(propertyInfo.GetValue(objectA, null), propertyInfo.GetValue(objectB, null),
                                             ignoreVirtualProperties, ignoreList) == false)
                        {
                            Debug.WriteLine("Mismatch with property '{0}.{1}' found.", objectType.FullName,
                                            propertyInfo.Name);
                            return false;
                        }
                    }
                    else
                    {
                        Debug.WriteLine("Cannot compare property '{0}.{1}'.", objectType.FullName, propertyInfo.Name);
                        return false;
                    }
                }
            }
            else
            {
                return Equals(objectA, objectB);
            }

            return true;
        }

        /// <summary>
        /// Determines whether value instances of the specified type can be directly compared.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if this value instances of the specified type can be directly compared; otherwise, <c>false</c>.
        /// </returns>
        private static bool CanDirectlyCompare(Type type)
        {
            return typeof(IComparable).IsAssignableFrom(type) || type.IsPrimitive || type.IsValueType;
        }

        /// <summary>
        /// Compares two values and returns if they are the same.
        /// </summary>
        /// <param name="valueA">The first value to compare.</param>
        /// <param name="valueB">The second value to compare.</param>
        /// <returns><c>true</c> if both values match, otherwise <c>false</c>.</returns>
        private static bool AreValuesEqual(object valueA, object valueB)
        {
            bool result;
            IComparable selfValueComparer;

            selfValueComparer = valueA as IComparable;

            if (valueA == null && valueB != null || valueA != null && valueB == null)
                result = false; // one of the values is null
            else if (selfValueComparer != null && selfValueComparer.CompareTo(valueB) != 0)
                result = false; // the comparison using IComparable failed
            else if (!object.Equals(valueA, valueB))
                result = false; // the comparison using Equals failed
            else
                result = true; // match

            return result;
        }

    }
}
