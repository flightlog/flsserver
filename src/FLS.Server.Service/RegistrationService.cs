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
using Newtonsoft.Json;
using NLog;
using AircraftType = FLS.Data.WebApi.Aircraft.AircraftType;
using UserAccountState = FLS.Data.WebApi.User.UserAccountState;

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

        public List<DateTime> GetTrialFlightsDates(string clubKey)
        {

            var trialFlightDates = new List<DateTime>();

            if (clubKey.ToUpper() == "FGZO")
            {
                trialFlightDates.Add(new DateTime(2017, 5, 6));
                trialFlightDates.Add(new DateTime(2017, 7, 1));
                trialFlightDates.Add(new DateTime(2017, 9, 2));
            }

            return trialFlightDates;
        }

        public void RegisterForTrialFlight(TrialFlightRegistrationDetails trialFlightRegistrationDetails)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                try
                {
                    Logger.Info($"New trial flight registration with following data (JSON): {JsonConvert.SerializeObject(trialFlightRegistrationDetails)}");
                }
                catch (Exception exception)
                {
                    Logger.Error(exception);
                }

                try
                {
                    var club =
                        context.Clubs.FirstOrDefault(
                            x => x.ClubKey.ToUpper() == trialFlightRegistrationDetails.ClubKey.ToUpper());

                    if (club == null)
                    {
                        Logger.Error(
                            $"Club with ClubKey: {trialFlightRegistrationDetails.ClubKey} not found! Trial flight registration could not be finished for person {trialFlightRegistrationDetails.Lastname} {trialFlightRegistrationDetails.Firstname}!");
                        throw new ApplicationException(
                            $"Club with ClubKey: {trialFlightRegistrationDetails.ClubKey} not found! Trial flight registration could not be finished for person {trialFlightRegistrationDetails.Lastname} {trialFlightRegistrationDetails.Firstname}!");
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

                        user =
                            context.Users.FirstOrDefault(
                                x =>
                                        x.ClubId == club.ClubId && x.AccountState == (int) UserAccountState.Active);

                        if (user == null)
                        {
                            throw new ApplicationException(
                                $"No active club user or admin user in club with ClubKey: {club.ClubKey} found.");
                        }
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
                        EmailPrivate = trialFlightRegistrationDetails.PrivateEmail,
                        HasGliderTraineeLicence = true
                    };

                    var personClub = new PersonClub
                    {
                        ClubId = club.ClubId,
                        IsGliderTrainee = true
                    };
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

                        personClub = new PersonClub {ClubId = club.ClubId};
                        invoicePerson.PersonClubs.Add(personClub);

                        context.Persons.Add(invoicePerson);
                    }

                    var aircraft = context.Aircrafts.FirstOrDefault(x => x.AircraftOwnerClubId == club.ClubId && x.NrOfSeats == 2 && x.AircraftTypeId == (int)AircraftType.Glider);
                    var aircraftReservationInfo = string.Empty;

                    if (aircraft == null)
                    {
                        Logger.Warn($"No double seats glider aircraft found with owned club {club.Clubname}!");
                        aircraftReservationInfo =
                            "Reservation konnte NICHT gemacht werden. Grund: Kein Doppelsitzer für Club gefunden!";
                    }

                    if (club.HomebaseId.HasValue == false)
                    {
                        Logger.Warn("No homebase or location defined for trial flight day!");

                        if (aircraft == null)
                        {
                            aircraftReservationInfo = "Reservation konnte NICHT gemacht werden. Grund: Kein Doppelsitzer für Club gefunden und kein Heimflugplatz für Club definiert!";
                        }
                        else
                        {
                            aircraftReservationInfo =
                                "Reservation konnte NICHT gemacht werden. Grund: Keine Heimflugplatz für Club definiert!";
                        }
                    }
                    
                    if (aircraft != null && club.HomebaseId.HasValue)
                    {
                        var reservation = new AircraftReservation()
                        {
                            AircraftId = aircraft.AircraftId,
                            ClubId = club.ClubId,
                            Start = trialFlightRegistrationDetails.SelectedDay.Date,
                            End = trialFlightRegistrationDetails.SelectedDay.Date.AddDays(1).AddTicks(-1),
                            IsAllDayReservation = true,
                            PilotPerson = person,
                            LocationId = club.HomebaseId.Value,
                            ReservationTypeId = 2, //Schulung/Check-Flug
                            Remarks = "Schnupperflug-Kandidat"
                        };

                        context.AircraftReservations.Add(reservation);

                        aircraftReservationInfo =
                                "Reservation wurde erstellt.";
                    }

                    context.SaveChanges();

                    if (string.IsNullOrWhiteSpace(person.EmailPrivate) == false)
                    {
                        var email = _registrationEmailBuildService.CreateTrialFlightRegistrationEmailForTrialPilot(person,
                            trialFlightRegistrationDetails.SelectedDay, person.EmailPrivate, club.ClubId, club.HomebaseName);
                        _registrationEmailBuildService.SendEmail(email);
                    }
                    else
                    {
                        Logger.Info($"No private email set for trial flight pilot {person.DisplayName}. Could not send email to pilot.");
                    }

                    if (string.IsNullOrWhiteSpace(club.SendTrialFlightRegistrationOperatorEmailTo) == false)
                    {
                        var organisationEmail =
                            _registrationEmailBuildService.CreateTrialFlightRegistrationEmailForOrganisator(
                                trialFlightRegistrationDetails,
                                club.SendTrialFlightRegistrationOperatorEmailTo, club.ClubId, club.HomebaseName,
                                aircraftReservationInfo);
                        _registrationEmailBuildService.SendEmail(organisationEmail);
                    }
                    else
                    {
                        Logger.Warn($"Club {club.Clubname} has no email recipient set for send trial flight registration operator email.");
                    }
                }
                catch (Exception exception)
                {
                    Logger.Error(exception,
                        $"Error while trying to prepare and save trial flight registration. Message: {exception.Message}.");

                    _registrationEmailBuildService.SendSystemErrorEmail("Error in RegisterForTrialFlight-Method", exception.Message, exception.StackTrace, JsonConvert.SerializeObject(trialFlightRegistrationDetails));

                    throw new ApplicationException("Es trat ein interner Fehler auf. Der System-Verantwortliche wurde informiert. Falls Sie in den nächsten 2 Tagen keine Rückmeldung erhalten, wenden Sie sich bitte an das Sekretariat. Für die Umstände bitten wir uns um Entschuldigung.");
                }
            }
        }
    }
}
