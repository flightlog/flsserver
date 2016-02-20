
namespace FLS.Data.WebApi.Flight
{
    /// <summary>
    /// defines the values of the flight status
    /// </summary>
    public enum FlightState
    {
        New = 0,
        FlightPlanOpen = 5,
        Started = 10,
        Landed = 20,
        FlightPlanClosed = 25,
        Invalid = 28,
        Valid = 30,
        Locked = 40,
        Invoiced = 50,
        PartialPaid = 55,
        Paid = 60
    }
}