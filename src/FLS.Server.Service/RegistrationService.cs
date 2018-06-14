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
using FLS.Common.Exceptions;

namespace FLS.Server.Service
{
    public class RegistrationService : BaseService
    {
        private readonly DataAccessService _dataAccessService;
        private readonly PersonService _personService;
        private readonly RegistrationEmailBuildService _registrationEmailBuildService;
        private readonly SettingService _settingService;

        public RegistrationService(DataAccessService dataAccessService, IdentityService identityService,
            PersonService personService, RegistrationEmailBuildService registrationEmailBuildService,
            SettingService settingService)
            : base(dataAccessService, identityService)
        {
            _personService = personService;
            _registrationEmailBuildService = registrationEmailBuildService;
            _settingService = settingService;
            _dataAccessService = dataAccessService;
            Logger = LogManager.GetCurrentClassLogger();
        }

        public List<DateTime> GetTrialFlightsDates(string clubKey)
        {
            try
            {
                var eventDates = _settingService.GetSettingValue<List<DateTime>>(SettingKey.TrialFlightEventDates, clubKey);

                return eventDates;
            }
            catch (EntityNotFoundException entityNotFoundException)
            {
                Logger.Error(entityNotFoundException);
                return new List<DateTime>();
            }
        }

        public void RegisterForTrialFlight(TrialFlightRegistrationDetails trialFlightRegistrationDetails)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                try
                {
                    Logger.Info(
                        $"New trial flight registration with following data (JSON): {JsonConvert.SerializeObject(trialFlightRegistrationDetails)}");
                }
                catch (Exception exception)
                {
                    Logger.Error(exception);
                }

                try
                {
                    var club =
                        context.Clubs.Include("Homebase").FirstOrDefault(
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
                            EmailPrivate = trialFlightRegistrationDetails.NotificationEmail
                        };

                        personClub = new PersonClub {ClubId = club.ClubId};
                        invoicePerson.PersonClubs.Add(personClub);

                        context.Persons.Add(invoicePerson);
                    }

                    var aircraft =
                        context.Aircrafts.FirstOrDefault(
                            x =>
                                x.AircraftOwnerClubId == club.ClubId && x.NrOfSeats == 2 &&
                                x.AircraftTypeId == (int) AircraftType.Glider);
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
                            aircraftReservationInfo =
                                "Reservation konnte NICHT gemacht werden. Grund: Kein Doppelsitzer für Club gefunden und kein Heimflugplatz für Club definiert!";
                        }
                        else
                        {
                            aircraftReservationInfo =
                                "Reservation konnte NICHT gemacht werden. Grund: Keine Heimflugplatz für Club definiert!";
                        }
                    }

                    var trialFlightSetting =
                        context.Settings.FirstOrDefault(
                            x => x.SettingKey == SettingKey.TrialFlightAircraftReservationFlightTypeId
                                 && x.ClubId == club.ClubId);

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
                            Remarks = "Schnupperflug-Kandidat"
                        };

                        if (trialFlightSetting != null && string.IsNullOrWhiteSpace(trialFlightSetting.SettingValue) == false)
                        {
                            try
                            {
                                var id = JsonConvert.DeserializeObject<Guid>(trialFlightSetting.SettingValue);
                                reservation.FlightTypeId = id;
                            }
                            catch (Exception ex)
                            {
                                Logger.Error(ex, "Could not deserialize FlightTypeId from settings value");
                            }
                        }

                        context.AircraftReservations.Add(reservation);

                        aircraftReservationInfo =
                                "Reservation wurde erstellt.";
                    }

                    context.SaveChanges();

                    if (trialFlightRegistrationDetails.InvoiceAddressIsSame &&
                        string.IsNullOrWhiteSpace(trialFlightRegistrationDetails.PrivateEmail) == false)
                    {
                        var email =
                                _registrationEmailBuildService.CreateTrialFlightRegistrationEmailForTrialPilot(trialFlightRegistrationDetails, person.EmailPrivate, club.ClubId,
                                    club.HomebaseName);
                        _registrationEmailBuildService.SendEmail(email);
                    }
                    else if (trialFlightRegistrationDetails.InvoiceAddressIsSame == false 
                        && string.IsNullOrWhiteSpace(trialFlightRegistrationDetails.NotificationEmail) == false)
                    {
                        var email =
                                _registrationEmailBuildService.CreateTrialFlightRegistrationEmailForTrialPilot(trialFlightRegistrationDetails, trialFlightRegistrationDetails.NotificationEmail, club.ClubId,
                                    club.HomebaseName);
                        _registrationEmailBuildService.SendEmail(email);
                    }
                    else
                    {
                        Logger.Info($"No email entered for trial flight pilot {person.DisplayName}. Could not send email to trial flight candidate.");
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

        public void RegisterForPassengerFlight(PassengerFlightRegistrationDetails passengerFlightRegistrationDetails)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                try
                {
                    Logger.Info(
                        $"New passenger flight registration with following data (JSON): {JsonConvert.SerializeObject(passengerFlightRegistrationDetails)}");
                }
                catch (Exception exception)
                {
                    Logger.Error(exception);
                }

                try
                {
                    var club =
                        context.Clubs.Include("Homebase").FirstOrDefault(
                            x => x.ClubKey.ToUpper() == passengerFlightRegistrationDetails.ClubKey.ToUpper());

                    if (club == null)
                    {
                        Logger.Error(
                            $"Club with ClubKey: {passengerFlightRegistrationDetails.ClubKey} not found! Passenger flight registration could not be finished for person {passengerFlightRegistrationDetails.Lastname} {passengerFlightRegistrationDetails.Firstname}!");
                        throw new ApplicationException(
                            $"Club with ClubKey: {passengerFlightRegistrationDetails.ClubKey} not found! Passenger flight registration could not be finished for person {passengerFlightRegistrationDetails.Lastname} {passengerFlightRegistrationDetails.Firstname}!");
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
                                        x.ClubId == club.ClubId && x.AccountState == (int)UserAccountState.Active);

                        if (user == null)
                        {
                            throw new ApplicationException(
                                $"No active club user or admin user in club with ClubKey: {club.ClubKey} found.");
                        }
                    }

                    _dataAccessService.IdentityService.SetUser(user);

                    var person = new Person()
                    {
                        Lastname = passengerFlightRegistrationDetails.Lastname,
                        Firstname = passengerFlightRegistrationDetails.Firstname,
                        AddressLine1 = passengerFlightRegistrationDetails.AddressLine1,
                        Zip = passengerFlightRegistrationDetails.ZipCode,
                        City = passengerFlightRegistrationDetails.City,
                        CountryId = passengerFlightRegistrationDetails.CountryId,
                        PrivatePhone = passengerFlightRegistrationDetails.PrivatePhoneNumber,
                        BusinessPhone = passengerFlightRegistrationDetails.BusinessPhoneNumber,
                        MobilePhone = passengerFlightRegistrationDetails.MobilePhoneNumber,
                        EmailPrivate = passengerFlightRegistrationDetails.PrivateEmail,
                        HasGliderTraineeLicence = false
                    };

                    var personClub = new PersonClub
                    {
                        ClubId = club.ClubId,
                        IsGliderTrainee = false
                    };
                    person.PersonClubs.Add(personClub);

                    context.Persons.Add(person);

                    if (passengerFlightRegistrationDetails.InvoiceAddressIsSame == false)
                    {
                        var invoicePerson = new Person()
                        {
                            Lastname = passengerFlightRegistrationDetails.InvoiceToLastname,
                            Firstname = passengerFlightRegistrationDetails.InvoiceToFirstname,
                            AddressLine1 = passengerFlightRegistrationDetails.InvoiceToAddressLine1,
                            Zip = passengerFlightRegistrationDetails.InvoiceToZipCode,
                            City = passengerFlightRegistrationDetails.InvoiceToCity,
                            CountryId = passengerFlightRegistrationDetails.InvoiceToCountryId,
                            EmailPrivate = passengerFlightRegistrationDetails.NotificationEmail
                        };

                        personClub = new PersonClub { ClubId = club.ClubId };
                        invoicePerson.PersonClubs.Add(personClub);

                        context.Persons.Add(invoicePerson);
                    }
                    
                    context.SaveChanges();

                    if (passengerFlightRegistrationDetails.InvoiceAddressIsSame &&
                        string.IsNullOrWhiteSpace(passengerFlightRegistrationDetails.PrivateEmail) == false)
                    {
                        var email =
                                _registrationEmailBuildService.CreatePassengerFlightRegistrationEmailForPassenger(passengerFlightRegistrationDetails, person.EmailPrivate, club.ClubId);
                        _registrationEmailBuildService.SendEmail(email);
                    }
                    else if (passengerFlightRegistrationDetails.InvoiceAddressIsSame == false
                        && string.IsNullOrWhiteSpace(passengerFlightRegistrationDetails.NotificationEmail) == false)
                    {
                        var email =
                                _registrationEmailBuildService.CreatePassengerFlightRegistrationEmailForPassenger(passengerFlightRegistrationDetails, passengerFlightRegistrationDetails.NotificationEmail, club.ClubId);
                        _registrationEmailBuildService.SendEmail(email);
                    }
                    else
                    {
                        Logger.Info($"No email entered for passenger {person.DisplayName}. Could not send email to passenger candidate.");
                    }

                    if (string.IsNullOrWhiteSpace(club.SendPassengerFlightRegistrationOperatorEmailTo) == false)
                    {
                        var organisationEmail =
                            _registrationEmailBuildService.CreatePassengerFlightRegistrationEmailForOrganisator(
                                passengerFlightRegistrationDetails,
                                club.SendPassengerFlightRegistrationOperatorEmailTo, club.ClubId);
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
                        $"Error while trying to prepare and save passenger flight registration. Message: {exception.Message}.");

                    _registrationEmailBuildService.SendSystemErrorEmail("Error in RegisterForPassengerFlight-Method", exception.Message, exception.StackTrace, JsonConvert.SerializeObject(passengerFlightRegistrationDetails));

                    throw new ApplicationException("Es trat ein interner Fehler auf. Der System-Verantwortliche wurde informiert. Falls Sie in den nächsten 2 Tagen keine Rückmeldung erhalten, wenden Sie sich bitte an das Sekretariat. Für die Umstände bitten wir uns um Entschuldigung.");
                }
            }
        }
    }
}
