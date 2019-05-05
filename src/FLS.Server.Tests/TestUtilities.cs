using System;

namespace FLS.Server.TestInfrastructure
{
    public static class TestUtilities
    {
        public static int GetRandomInteger(int minValue = int.MinValue, int maxValue = int.MaxValue)
        {
            Random r = new Random();
            int x = r.Next(minValue, maxValue);
            return x;
        }

        public static DateTime GetRandomDateTimeOfToday()
        {
            var date = DateTime.Now.Date;

            date = date.AddHours(GetRandomInteger(8, 20));
            date = date.AddMinutes(GetRandomInteger(0, 60));

            return date;
        }

        public static DateTime GetRandomDateTimeInFuture()
        {
            var date = DateTime.Now.Date.AddDays(GetRandomInteger(1, 300));

            date = date.AddHours(GetRandomInteger(8, 20));
            date = date.AddMinutes(GetRandomInteger(0, 60));

            return date;
        }

        public static DateTime GetRandomDateTimeInPast()
        {
            var date = DateTime.Now.Date.AddDays(0 - GetRandomInteger(1, 300));

            date = date.AddHours(GetRandomInteger(8, 20));
            date = date.AddMinutes(GetRandomInteger(0, 60));

            return date;
        }
    }
}
