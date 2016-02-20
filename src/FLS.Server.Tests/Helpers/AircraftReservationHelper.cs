using System.Linq;
using FLS.Data.WebApi.AircraftReservation;
using FLS.Server.Data.DbEntities;
using FLS.Server.Service;
using FLS.Server.TestInfrastructure;
using Foundation.ObjectHydrator;

namespace FLS.Server.Tests.Helpers
{
    public class AircraftReservationHelper
    {
        private readonly AircraftHelper _aircraftHelper;
        private readonly PersonHelper _personHelper;
        private readonly LocationHelper _locationHelper;
        private readonly DataAccessService _dataAccessService;

        public AircraftReservationHelper(AircraftHelper aircraftHelper, PersonHelper personHelper,
            LocationHelper locationHelper, DataAccessService dataAccessService)
        {
            _aircraftHelper = aircraftHelper;
            _personHelper = personHelper;
            _locationHelper = locationHelper;
            _dataAccessService = dataAccessService;
        }

        public AircraftReservationType GetFirstAircraftReservationType()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                return context.AircraftReservationTypes.FirstOrDefault();
            }
        }

        public AircraftReservationDetails CreateAircraftReservationDetails()
        {
            var reservation = new AircraftReservationDetails();
            reservation.AircraftId = _aircraftHelper.GetFirstAircraft().AircraftId;
            reservation.Start = TestUtilities.GetRandomDateTimeInFuture();
            reservation.End = reservation.Start.AddHours(4);
            reservation.Remarks = "Test";
            reservation.IsAllDayReservation = false;
            reservation.PilotPersonId = _personHelper.GetFirstPerson().PersonId;
            reservation.LocationId = _locationHelper.GetFirstLocation().LocationId;
            reservation.InstructorPersonId = _personHelper.GetFirstPerson().PersonId;
            reservation.ReservationTypeId = GetFirstAircraftReservationType().AircraftReservationTypeId;

            return reservation;
        }

        public AircraftReservationOverview CreateAircraftReservationOverview()
        {
            var hydrator = new Hydrator<AircraftReservationOverview>();
            var reservation = hydrator.GetSingle();

            return reservation;
        }
    }
}
