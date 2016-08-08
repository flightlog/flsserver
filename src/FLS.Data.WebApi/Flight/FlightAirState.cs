
namespace FLS.Data.WebApi.Flight
{
    /// <summary>
    /// defines the values of the flight status
    /// </summary>
    public enum FlightAirState
    {
        New = 0,
        FlightPlanOpen = 5,
        MightBeStarted = 8,
        Started = 10,
        MightBeLandedOrInAir = 15,
        Landed = 20,
        FlightPlanClosed = 25
    }
}