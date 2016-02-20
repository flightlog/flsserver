using System;

namespace FLS.Data.WebApi.Dashboard
{
    /// <summary>
    /// http://www.trainingsbarometer.de/
    /// http://www.aeroclub.bz/images/public/cms/files/cod_7853736342.pdf
    /// </summary>
    public class SafetyDashboardDetails
    {
        public int Starts { get; set; }

        public double FlightTimeInHours { get; set; }

        public int StatisticBasedOnLastMonths { get; set; }

    }
}
