using FLS.Data.WebApi.Reporting;

namespace FLS.Server.Service.Email.Model
{
    public class EmailAircraftFlightReport : AircraftFlightReport
    {
        public string RecipientName { get; set; }
        public string FLSUrl { get; set; }
        public string SenderName { get; set; }

        public EmailAircraftFlightReport(AircraftFlightReport report)
            : base(report)
        {
        }
    }
}
