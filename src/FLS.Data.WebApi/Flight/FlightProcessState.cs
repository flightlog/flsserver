
namespace FLS.Data.WebApi.Flight
{
    /// <summary>
    /// defines the values of the flight status
    /// </summary>
    public enum FlightProcessState
    {
        NotProcessed = 0,
        Locked = 40,
        Delivered = 45,
        Invoiced = 50,
        PartialPaid = 55,
        Paid = 60
    }
}