using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLS.Server.Data.Resources
{
    public static class ErrorMessage
    {
        public static string DbEntityValidationException = "{{Error_DbEntityValidationException}}";
        public static string GeneralDatabaseException = "{{Error_GeneralDatabaseException}}";
        public static string UserNotAuthenticated = "{{Error_UserNotAuthenticated}}";
        public static string SoftDeleteDatabaseException = "{{Error_SoftDeleteDatabaseException}}";
        public static string ArgumentOutOfRangeException = "{{Error_ArgumentOutOfRange}}";
        public static string ReservationTypeNotFound = "{{Error_ReservationTypeNotFound}}";
        public static string StartLocationsOfGliderAndTowFlightsNotEqual = "{{Error_StartLocationsOfGliderAndTowFlightsNotEqual}}";
        public static string StartTimeOfGliderAndTowFlightsNotEqual = "{{Error_StartTimeOfGliderAndTowFlightsNotEqual}}";
        public static string TowFlightWithoutGliderFlightIsNotValid = "{{Error_TowFlightWithoutGliderFlightIsNotValid}}";
        public static string InternalServerException = "{{Error_InternalServerException}}";
        public static string NotInRoleClubAdmin = "{{Error_NotInRole_ClubAdmin}}";
        public static string NotInRoleSystemAdmin = "{{Error_NotInRole_SystemAdmin}}";
        public static string InvalidCastException = "{{Error_InvalidCastException}}";
        public static string CantDeleteClubDuoToActiveUsers = "{{Error_CantDeleteClubDueToActiveUsers}}";
        public static string NotInSameClub = "{{Error_NotInSameClub}}";

    }
}
