using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Common.Validators;
using FLS.Data.WebApi.Licensing;
using FLS.Data.WebApi.Registrations;
using FLS.Data.WebApi.Resources;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Mapping;
using FLS.Server.Data.Resources;
using FLS.Server.Service.Email;
using NLog;
using AircraftType = FLS.Data.WebApi.Aircraft.AircraftType;

namespace FLS.Server.Service
{
    public class RegistrationService : BaseService
    {
        private readonly DataAccessService _dataAccessService;
        private readonly PersonService _personService;
        private readonly RegistrationEmailBuildService _registrationEmailBuildService;

        public RegistrationService(DataAccessService dataAccessService, IdentityService identityService, 
            PersonService personService, RegistrationEmailBuildService registrationEmailBuildService)
            : base(dataAccessService, identityService)
        {
            _personService = personService;
            _registrationEmailBuildService = registrationEmailBuildService;
            _dataAccessService = dataAccessService;
            Logger = LogManager.GetCurrentClassLogger();
        }

        public List<DateTime> GetTrialFlightsDates()
        {
            var trialFlightDates = new List<DateTime>();
            trialFlightDates.Add(new DateTime(2017, 5, 6));
            trialFlightDates.Add(new DateTime(2017, 7, 1));
            trialFlightDates.Add(new DateTime(2017, 9, 2));

            return trialFlightDates;
        }

        public void RegisterForTrialFlight(TrialFlightRegistrationDetails trialFlightRegistrationDetails)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var club =
                    context.Clubs.FirstOrDefault(
                        x => x.ClubKey.ToUpper() == trialFlightRegistrationDetails.ClubKey.ToUpper());

                if (club == null)
                {
                    Logger.Error($"Club with ClubKey: {trialFlightRegistrationDetails.ClubKey} not found! Trial flight registration could not be finished for person {trialFlightRegistrationDetails.Lastname} {trialFlightRegistrationDetails.Firstname}!");
                    throw new ApplicationException($"Club with ClubKey: {trialFlightRegistrationDetails.ClubKey} not found! Trial flight registration could not be finished for person {trialFlightRegistrationDetails.Lastname} {trialFlightRegistrationDetails.Firstname}!");
                }

                var user =
                    context.Users.FirstOrDefault(
                        x =>
                            x.ClubId == club.ClubId &&
                            x.UserRoles.Any(
                                role =>
                                    role.Role.RoleApplicationKeyString ==
                                    RoleApplicationKeyStrings.ClubAdministrator));

                if (user == null)
                {
                    Logger.Error($"No club admin user found in club with ClubKey: {club.ClubKey}");
                    throw new ApplicationException($"No club admin user found in club with ClubKey: {club.ClubKey}");
                }

                _dataAccessService.IdentityService.SetUser(user);

                var person = new Person()
                {
                    Lastname = trialFlightRegistrationDetails.Lastname,
                    Firstname = trialFlightRegistrationDetails.Firstname,
                    AddressLine1 = trialFlightRegistrationDetails.AddressLine1,
                    Zip = trialFlightRegistrationDetails.ZipCode,
                    City = trialFlightRegistrationDetails.City,
                    CountryId = trialFlightRegistrationDetails.CountryId,
                    PrivatePhone = trialFlightRegistrationDetails.PrivatePhoneNumber,
                    BusinessPhone = trialFlightRegistrationDetails.BusinessPhoneNumber,
                    MobilePhone = trialFlightRegistrationDetails.MobilePhoneNumber,
                    EmailPrivate = trialFlightRegistrationDetails.PrivateEmail
                };

                var personClub = new PersonClub { ClubId = club.ClubId };
                person.PersonClubs.Add(personClub);

                context.Persons.Add(person);

                if (trialFlightRegistrationDetails.InvoiceAddressIsSame == false)
                {
                    var invoicePerson = new Person()
                    {
                        Lastname = trialFlightRegistrationDetails.InvoiceToLastname,
                        Firstname = trialFlightRegistrationDetails.InvoiceToFirstname,
                        AddressLine1 = trialFlightRegistrationDetails.InvoiceToAddressLine1,
                        Zip = trialFlightRegistrationDetails.InvoiceToZipCode,
                        City = trialFlightRegistrationDetails.InvoiceToCity,
                        CountryId = trialFlightRegistrationDetails.InvoiceToCountryId,
                    };

                    personClub = new PersonClub { ClubId = club.ClubId };
                    invoicePerson.PersonClubs.Add(personClub);

                    context.Persons.Add(invoicePerson);
                }

                var aircraft = context.Aircrafts.FirstOrDefault(x => x.Immatriculation.ToUpper() == "HB-3256");

                if (aircraft == null)
                {
                    Logger.Warn("Aircraft HB-3256 not fount!");
                    aircraft = context.Aircrafts.FirstOrDefault();
                }

                if (aircraft == null)
                {
                    Logger.Error("No aircraft found!");
                }
                else if (club.HomebaseId.HasValue == false)
                {
                    Logger.Warn("No homebase or location defined for trial flight day!");
                }
                else
                {
                    var reservation = new AircraftReservation()
                    {
                        AircraftId = aircraft.AircraftId,
                        ClubId = club.ClubId,
                        IsAllDayReservation = true,
                        PilotPerson = person,
                        LocationId = club.HomebaseId.Value,
                        ReservationTypeId = 2 //Schulung/Check-Flug
                    };

                    context.AircraftReservations.Add(reservation);
                }

                context.SaveChanges();

                if (string.IsNullOrWhiteSpace(person.EmailPrivate) == false)
                {
                    var email = _registrationEmailBuildService.CreateTrialFlightRegistrationEmailForTrialPilot(person,
                        trialFlightRegistrationDetails.SelectedDay, person.EmailPrivate, club.ClubId, club.HomebaseName);
                    _registrationEmailBuildService.SendEmail(email);
                }

            }
        }
    }
}
