using System;
using FLS.Common.Validators;
using FLS.Data.WebApi.Aircraft;
using FLS.Server.Data.DbEntities;

namespace FLS.Server.Data.Mapping
{
    public static class CounterUnitExtensions
    {
        public static long ToCounterValue(this TimeSpan duration, CounterUnitType counterUnitType)
        {
            counterUnitType.ArgumentNotNull("counterUnitType");
            long result = 0;

            if (counterUnitType.CounterUnitTypeKeyName.ToLower() == "min")
            {
                result = Convert.ToInt64(duration.TotalMinutes);
            }
            else if (counterUnitType.CounterUnitTypeKeyName.ToLower() == "100min")
            {
                result = duration.Hours * 100 + Convert.ToInt64(duration.Minutes*100/60);
            }
            else if (counterUnitType.CounterUnitTypeKeyName.ToLower() == "sec")
            {
                result = duration.Hours * 3600 + duration.Minutes * 60 + duration.Seconds;
            }
            else if (counterUnitType.CounterUnitTypeKeyName.ToLower() == "6sec")
            {
                result = duration.Hours * 600 + duration.Minutes * 10 + Convert.ToInt64(duration.Seconds/6);
            }

            return result;
        }
    }
}
