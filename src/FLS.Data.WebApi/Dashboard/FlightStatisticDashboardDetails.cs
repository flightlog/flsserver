using System;
using System.Collections.Generic;

namespace FLS.Data.WebApi.Dashboard
{
    /// <summary>
    /// http://www.trainingsbarometer.de/
    /// http://www.aeroclub.bz/images/public/cms/files/cod_7853736342.pdf
    /// </summary>
    public class FlightStatisticDashboardDetails
    {
        public FlightStatisticDashboardDetails()
        {
            MonthlyFlightHours = new Dictionary<DateTime, double>();
            MonthlyLandings = new Dictionary<DateTime, int>();
        }

        public string FlightStatisticName { get; set; }

        public Dictionary<DateTime, int> MonthlyLandings { get; set; }

        public Dictionary<DateTime, double> MonthlyFlightHours { get; set; }
    }
}
