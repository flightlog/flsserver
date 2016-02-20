namespace FLS.Data.WebApi.Aircraft
{
    public class AircraftTypeListItem
    {
        public int AircraftTypeId { get; set; }

        public string AircraftTypeName { get; set; }

        public bool? HasEngine { get; set; }

        public bool? RequiresTowingInfo { get; set; }

        public bool? MayBeTowingAircraft { get; set; }
    }
}
