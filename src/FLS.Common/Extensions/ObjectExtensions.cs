using System.Reflection;

namespace FLS.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static bool HasMethod(this object objectToCheck, string methodName)
        {
            try
            {
                var type = objectToCheck.GetType();
                return type.GetMethod(methodName) != null;
            }
            catch (AmbiguousMatchException)
            {
                // ambiguous means there is more than one result,
                // which means: a method with that name does exist
                return true;
            }
        }

        public static bool HasProperty(this object objectToCheck, string propertyName)
        {
            try
            {
                var type = objectToCheck.GetType();
                return type.GetProperty(propertyName) != null;
            }
            catch (AmbiguousMatchException)
            {
                // ambiguous means there is more than one result,
                // which means: a method with that name does exist
                return true;
            }
        }

        public static void SetPropertyValue(this object objectToUpdate, string propertyName, object value)
        {
            try
            {
                var type = objectToUpdate.GetType();
                var prop = type.GetProperty(propertyName);
                prop.SetValue(objectToUpdate, value, null);
            }
            catch (AmbiguousMatchException)
            {
                // ambiguous means there is more than one result,
                // which means: a method with that name does exist
            }
        }
    }
}
