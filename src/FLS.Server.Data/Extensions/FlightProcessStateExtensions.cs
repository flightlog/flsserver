using FLS.Data.WebApi.Flight;
using FLS.Server.Data.Enums;

namespace FLS.Server.Data.Extensions
{
    public static class FlightProcessStateExtensions
    {
        public static string ToFlightProcessState(this FlightProcessState flightProcessStateId)
        {
            string returnValue;

            switch (flightProcessStateId)
            {
                case FlightProcessState.NotProcessed:
                    returnValue = "NotProcessed";
                    break;
                case FlightProcessState.Invalid:
                    returnValue = "Invalid";
                    break;
                case FlightProcessState.Valid:
                    returnValue = "Valid";
                    break;
                case FlightProcessState.Locked:
                    returnValue = "Locked";
                    break;
                case FlightProcessState.Delivered:
                    returnValue = "Delivered";
                    break;
                default:
                    returnValue = string.Empty;
                    break;
            }

            return returnValue;
        }
    }
}