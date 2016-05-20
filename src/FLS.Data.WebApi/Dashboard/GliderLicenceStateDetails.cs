using System;

namespace FLS.Data.WebApi.Dashboard
{
    /// <summary>
    /// http://www.trainingsbarometer.de/
    /// http://www.aeroclub.bz/images/public/cms/files/cod_7853736342.pdf
    /// </summary>
    public class GliderLicenceStateDetails
    {
        public string LicenceStateKey { get; set; }

        public string LicenceStateInformation { get; set; }

        public int LastMonthsCount { get; set; }

        public double FlightTimeInHours { get; set; }

        public int Landings { get; set; }

        public double FlightTimeInHoursRequired { get; set; }

        public int LandingsRequired { get; set; }

        public int NumberOfCheckFlights { get; set; }

        public int NumberOfCheckFlightsRequired { get; set; }

    }
}
