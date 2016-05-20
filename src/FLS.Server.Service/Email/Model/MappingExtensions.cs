using System.Collections.Generic;
using FLS.Common.Validators;
using FLS.Data.WebApi.AircraftReservation;
using FLS.Data.WebApi.PlanningDay;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;

namespace FLS.Server.Service.Email.Model
{
    public static class MappingExtensions
    {
        #region PlanningDayInfoModel

        public static PlanningDayInfoModel ToPlanningDayInfoModel(this PlanningDayOverview overview, SystemData systemData, List<AircraftReservationOverview> aircraftReservationOverview)
        {
            var planningDayInfoModel = new PlanningDayInfoModel
            {
                SenderName = systemData.SystemSenderEmailAddress,
                Date = overview.Day.ToShortDateString(),
                FLSUrl = systemData.BaseURL,
                LocationName = overview.LocationName,
                FlightOperatorName = overview.FlightOperatorName,
                TowPilotName = overview.TowingPilotName,
                InstructorName = overview.InstructorName,
                Remarks = overview.Remarks
            };

            if (string.IsNullOrEmpty(planningDayInfoModel.Remarks))
            {
                planningDayInfoModel.Remarks = "Keine";
            }

            var reservationInfoRows = new List<ReservationInfoRow>();

            if (aircraftReservationOverview != null && aircraftReservationOverview.Count > 0)
            {
                foreach (var reservation in aircraftReservationOverview)
                {
                    var infoRow = new ReservationInfoRow()
                        {
                            AircraftImmatriculation = reservation.Immatriculation,
                            PilotName = reservation.PilotName,
                            ReservationTypeName = reservation.ReservationTypeName,
                            InstructorName = reservation.InstructorName
                        };

                    if (reservation.IsAllDayReservation)
                    {
                        infoRow.ReservationPeriod = "Ganzer Tag";
                    }
                    else
                    {
                        infoRow.ReservationPeriod = string.Format("{0} - {1}", reservation.Start.ToLocalTime().ToShortTimeString(),
                                                                  reservation.End.ToLocalTime().ToShortTimeString());
                    }

                    reservationInfoRows.Add(infoRow);
                }

                planningDayInfoModel.AircraftReservations = reservationInfoRows.ToArray();
            }

            return planningDayInfoModel;
        }
        #endregion PlanningDayInfoModel

        public static FlightInfoRow ToFlightInfoRow(this Flight flight)
        {
            var flightInfoRow = new FlightInfoRow();

            if (string.IsNullOrWhiteSpace(flight.Comment) == false)
            {
                flightInfoRow.FlightComment = flight.Comment;
            }

            if (flight.IsSoloFlight)
            {
                flightInfoRow.IsSoloFlight = "X";
            }
            else
            {
                flightInfoRow.IsSoloFlight = string.Empty;
            }

            if (flight.Pilot != null)
            {
                flightInfoRow.PilotName = flight.PilotDisplayName;
            }

            if (flight.Instructor != null)
            {
                flightInfoRow.SecondCrewName = flight.InstructorDisplayName;
            }
            else if (flight.CoPilot != null)
            {
                flightInfoRow.SecondCrewName = flight.CoPilotDisplayName;
            }
            else if (flight.Passenger != null)
            {
                flightInfoRow.SecondCrewName = flight.PassengerDisplayName;
            }

            if (flight.FlightType != null)
            {
                flightInfoRow.FlightTypeName = flight.FlightType.FlightTypeName;
            }

            if (flight.Aircraft != null)
            {
                flightInfoRow.AircraftImmatriculation = flight.Aircraft.Immatriculation;
            }

            if (flight.StartTypeId.HasValue)
            {
                flightInfoRow.StartType = ((AircraftStartType)flight.StartTypeId.Value).ToFlightStartTypeString();
            }

            if (flight.FlightDate.HasValue)
            {
                flightInfoRow.FlightDate = flight.FlightDate.Value.ToShortDateString();
            }

            if (flight.StartDateTime.HasValue)
            {
                flightInfoRow.StartTimeLocal = flight.StartDateTime.Value.ToLocalTime().ToShortTimeString();
            }

            if (flight.LdgDateTime.HasValue)
            {
                flightInfoRow.LdgTimeLocal = flight.LdgDateTime.Value.ToLocalTime().ToShortTimeString();
            }

            flightInfoRow.FlightDuration = flight.Duration.ToString(@"h\:mm");

            if (flight.StartLocation != null)
            {
                flightInfoRow.StartLocation = flight.StartLocation.LocationName;
            }

            if (flight.LdgLocation != null)
            {
                flightInfoRow.LdgLocation = flight.LdgLocation.LocationName;
            }

            if (flight.TowFlight != null)
            {
                flightInfoRow.TowAircraftImmatriculation = flight.TowFlight.AircraftImmatriculation;

                if (flight.TowFlight.StartDateTime.HasValue)
                {
                    flightInfoRow.TowStartTimeLocal =
                        flight.TowFlight.StartDateTime.Value.ToLocalTime().ToShortTimeString();
                }

                if (flight.TowFlight.LdgDateTime.HasValue)
                {
                    flightInfoRow.TowLdgTimeLocal = flight.TowFlight.LdgDateTime.Value.ToLocalTime().ToShortTimeString();
                }

                if (flight.TowFlight.Pilot != null)
                {
                    flightInfoRow.TowPilotName = flight.TowFlight.PilotDisplayName;
                }

                flightInfoRow.TowFlightDuration = flight.TowFlight.Duration.ToString(@"h\:mm");
            }

            return flightInfoRow;
        }

        public static string ToFlightStartTypeString(this AircraftStartType startType)
        {
            var startTypeString = "Schleppstart";

            switch (startType)
            {
                case AircraftStartType.TowingByAircraft:
                    startTypeString = "Schleppstart";
                    break;
                case AircraftStartType.WinchLaunch:
                    startTypeString = "Windenstart";
                    break;
                case AircraftStartType.SelfStart:
                    startTypeString = "Eigenstart";
                    break;
                case AircraftStartType.ExternalStart:
                    startTypeString = "Externer Start";
                    break;
                case AircraftStartType.MotorFlightStart:
                    startTypeString = "Motorflugstart";
                    break;
            }

            return startTypeString;
        }
    }
}
