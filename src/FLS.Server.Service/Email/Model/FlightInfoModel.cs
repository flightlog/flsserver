namespace FLS.Server.Service.Email.Model
{
    public class FlightInfoModel
    {
        public string RecipientName { get; set; }
        public string FLSUrl { get; set; }
        public string SenderName { get; set; }
        public string Date { get; set; }
        public FlightInfoRow[] Flights { get; set; }
    }
}
