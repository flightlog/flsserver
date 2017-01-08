using FLS.Data.WebApi.Flight;
using FLS.Server.Data.Enums;

namespace FLS.Server.Data.Extensions
{
    public static class FlightAirStateExtensions
    {
        public static string ToFlightAirState(this FlightAirState flightAirStateId)
        {
            string returnValue;

            switch (flightAirStateId)
            {
                case FlightAirState.New:
                    returnValue = "New";
                    break;
                case FlightAirState.FlightPlanOpen:
                    returnValue = "FlightPlanOpen";
                    break;
                case FlightAirState.MightBeStarted:
                    returnValue = "MightBeStarted";
                    break;
                case FlightAirState.Started:
                    returnValue = "Started";
                    break;
                case FlightAirState.MightBeLandedOrInAir:
                    returnValue = "MightBeLandedOrInAir";
                    break;
                case FlightAirState.Landed:
                    returnValue = "Landed";
                    break;
                case FlightAirState.FlightPlanClosed:
                    returnValue = "FlightPlanClosed";
                    break;
                default:
                    returnValue = string.Empty;
                    break;
            }

            return returnValue;
        }
    }
}