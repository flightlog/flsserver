using FLS.Data.WebApi.Flight;
using FLS.Server.Data.Enums;

namespace FLS.Server.Data.Extensions
{
    public static class FlightValidationStateExtensions
    {
        public static string ToFlightValidationState(this FlightValidationState flightValidationStateId)
        {
            string returnValue;

            switch (flightValidationStateId)
            {
                case FlightValidationState.NotValidated:
                    returnValue = "NotValidated";
                    break;
                case FlightValidationState.Invalid:
                    returnValue = "Invalid";
                    break;
                case FlightValidationState.Valid:
                    returnValue = "Valid";
                    break;
                default:
                    returnValue = string.Empty;
                    break;
            }

            return returnValue;
        }
    }
}