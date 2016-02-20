using System;

namespace FLS.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime Truncate(this DateTime date, long resolution)
        {
            return new DateTime(date.Ticks - (date.Ticks % resolution), date.Kind);
        }

        public static DateTime SetAsUtc(this DateTime datetime)
        {
            if (datetime.Kind == DateTimeKind.Local)
            {
                return datetime.ToUniversalTime();
            }
            else if (datetime.Kind == DateTimeKind.Unspecified)
            {
                return DateTime.SpecifyKind(datetime, DateTimeKind.Utc);
            }
            else
            {
                return datetime;
            }
        }

        public static DateTime? SetAsUtc(this DateTime? datetime)
        {
            if (datetime.HasValue == false) return null;

            if (datetime.Value.Kind == DateTimeKind.Local)
            {
                return datetime.Value.ToUniversalTime();
            }
            else if (datetime.Value.Kind == DateTimeKind.Unspecified)
            {
                return DateTime.SpecifyKind(datetime.Value, DateTimeKind.Utc);
            }
            else
            {
                return datetime;
            }
        }

    }
}