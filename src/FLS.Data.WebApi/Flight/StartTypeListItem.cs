namespace FLS.Data.WebApi.Flight
{
    public class StartTypeListItem 
    {

        public int StartTypeId { get; set; }

        //[StringLength(100, ErrorMessage = ConstantUIStrings.MaxLength100)]
        //[Display(Name = ConstantUIStrings.StartTypeName)]
        public string StartTypeName { get; set; }

        public bool IsForGliderFlights { get; set; }

        public bool IsForTowFlights { get; set; }

        public bool IsForMotorFlights { get; set; }


        public bool IsWinchStart { get; set; }
    }
}
