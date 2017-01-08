using FLS.Server.Data.Enums;

namespace FLS.Server.Data.Extensions
{
    public static class CostCenterUnitTypeExtensions
    {
        public static string ToUnitTypeString(this CostCenterUnitType unitType)
        {
            string unitTypeString;

            switch (unitType)
            {
                case CostCenterUnitType.PerFlightMinute:
                    unitTypeString = "Minuten";
                    break;
                case CostCenterUnitType.PerStartOrFlight:
                    unitTypeString = "Start";
                    break;
                case CostCenterUnitType.PerLanding:
                    unitTypeString = "Landung";
                    break;
                default:
                    unitTypeString = string.Empty;
                    break;
            }

            return unitTypeString;
        }
    }
}