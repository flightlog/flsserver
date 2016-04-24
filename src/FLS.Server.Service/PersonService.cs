using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using FLS.Common.Extensions;
using FLS.Common.Validators;
using FLS.Data.WebApi.Person;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Exceptions;
using FLS.Server.Data.Mapping;
using FLS.Server.Data.Resources;
using FLS.Server.Interfaces;
using FLS.Server.Service.Email;
using FLS.Server.Service.Exporting;
using NLog;

namespace FLS.Server.Service
{
    public class PersonService : BaseService, IPersonService
    {
        private readonly AddressListEmailBuildService _addressListEmailBuildService;
        private readonly DataAccessService _dataAccessService;
        private readonly IdentityService _identityService;

        public PersonService(AddressListEmailBuildService addressListEmailBuildService,
            DataAccessService dataAccessService, IdentityService identityService)
            : base(dataAccessService, identityService)
        {
            _addressListEmailBuildService = addressListEmailBuildService;
            _dataAccessService = dataAccessService;
            _identityService = identityService;
            Logger = LogManager.GetCurrentClassLogger();
        }

        #region Person
        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedPersons">if set to <c>true</c> only club related persons will be returned.</param>
        /// <returns></returns>
        public List<PilotPersonListItem> GetPilotPersonListItems(bool onlyClubRelatedPersons)
        {
            var persons = GetPersons(onlyClubRelatedPersons);
            return PreparePersonListItems(persons);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedPilots">if set to <c>true</c> only club related pilots will be returned.</param>
        /// <returns></returns>
        public List<PilotPersonListItem> GetGliderPilotPersonListItems(bool onlyClubRelatedPilots)
        {
            return GetPilotPersonListItemInternal(onlyClubRelatedPilots, person => person.HasGliderPilotLicence || person.HasGliderTraineeLicence);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedPilots">if set to <c>true</c> only club related pilots will be returned.</param>
        /// <returns></returns>
        public List<PilotPersonListItem> GetGliderObserverPilotPersonListItems(bool onlyClubRelatedPilots)
        {
            return GetPilotPersonListItemInternal(onlyClubRelatedPilots, person => person.HasGliderPilotLicence);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedInstuctors">if set to <c>true</c> only club related instructors will be returned.</param>
        /// <returns></returns>
        public List<PilotPersonListItem> GetGliderInstructorPersonListItems(bool onlyClubRelatedInstuctors)
        {
            return GetPilotPersonListItemInternal(onlyClubRelatedInstuctors, person => person.HasGliderInstructorLicence);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedPilots">if set to <c>true</c> only club related pilots will be returned.</param>
        /// <returns></returns>
        public List<PilotPersonListItem> GetTowingPilotPersonListItems(bool onlyClubRelatedPilots)
        {
            return GetPilotPersonListItemInternal(onlyClubRelatedPilots, person => person.HasTowPilotLicence);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedPilots">if set to <c>true</c> only club related pilots will be returned.</param>
        /// <returns></returns>
        public List<PilotPersonListItem> GetMotorPilotPersonListItems(bool onlyClubRelatedPilots)
        {
            return GetPilotPersonListItemInternal(onlyClubRelatedPilots, person => person.HasMotorPilotLicence);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedTrainees">if set to <c>true</c> only club related trainees will be returned.</param>
        /// <returns></returns>
        public List<PilotPersonListItem> GetGliderTraineePersonListItems(bool onlyClubRelatedTrainees)
        {
            return GetPilotPersonListItemInternal(onlyClubRelatedTrainees, person => person.HasGliderTraineeLicence);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedWinchOperators">if set to <c>true</c> [only club related winch operators].</param>
        /// <returns></returns>
        public List<PilotPersonListItem> GetWinchOperatorPersonListItems(bool onlyClubRelatedWinchOperators)
        {
            return GetPilotPersonListItemInternal(onlyClubRelatedWinchOperators, person => person.HasWinchOperatorLicence);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedPassengers">if set to <c>true</c> [only club related passengers].</param>
        /// <returns></returns>
        public List<PassengerListItem> GetPassengerListItems(bool onlyClubRelatedPassengers)
        {
            return GetPassengerListItemInternal(onlyClubRelatedPassengers, 
                person => person.HasGliderInstructorLicence == false
                && person.HasGliderPAXLicence == false
                && person.HasGliderPilotLicence == false
                && person.HasGliderTraineeLicence == false
                && person.HasMotorPilotLicence == false
                && person.HasTMGLicence == false
                && person.HasTowPilotLicence == false
                && person.HasWinchOperatorLicence == false);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedPersons">if set to <c>true</c> only club related persons will be returned.</param>
        /// <returns></returns>
        public List<PilotPersonOverview> GetPilotPersonOverviews(bool onlyClubRelatedPersons)
        {
            return GetPersonOverviewsInternal(onlyClubRelatedPersons, person => true);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedPilots">if set to <c>true</c> only club related pilots will be returned.</param>
        /// <returns></returns>
        public List<PilotPersonOverview> GetGliderPilotPersonOverviews(bool onlyClubRelatedPilots)
        {
            return GetPersonOverviewsInternal(onlyClubRelatedPilots, person => person.HasGliderPilotLicence || person.HasGliderTraineeLicence);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedPilots">if set to <c>true</c> only club related pilots will be returned.</param>
        /// <returns></returns>
        public List<PilotPersonOverview> GetGliderObserverPilotPersonOverviews(bool onlyClubRelatedPilots)
        {
            return GetPersonOverviewsInternal(onlyClubRelatedPilots, person => person.HasGliderPilotLicence);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedInstuctors">if set to <c>true</c> only club related instructors will be returned.</param>
        /// <returns></returns>
        public List<PilotPersonOverview> GetGliderInstructorPersonOverviews(bool onlyClubRelatedInstuctors)
        {
            return GetPersonOverviewsInternal(onlyClubRelatedInstuctors, person => person.HasGliderInstructorLicence);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedPilots">if set to <c>true</c> only club related pilots will be returned.</param>
        /// <returns></returns>
        public List<PilotPersonOverview> GetTowingPilotPersonOverviews(bool onlyClubRelatedPilots)
        {
            return GetPersonOverviewsInternal(onlyClubRelatedPilots, person => person.HasTowPilotLicence);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedTrainees">if set to <c>true</c> only club related trainees will be returned.</param>
        /// <returns></returns>
        public List<PilotPersonOverview> GetGliderTraineePilotPersonOverviews(bool onlyClubRelatedTrainees)
        {
            return GetPersonOverviewsInternal(onlyClubRelatedTrainees, person => person.HasGliderTraineeLicence);
        }

        public List<PilotPersonOverview> GetWinchOperatorPilotPersonOverviews(bool onlyClubRelatedWinchOperators)
        {
            return GetPersonOverviewsInternal(onlyClubRelatedWinchOperators, person => person.HasWinchOperatorLicence);
        }

        public List<PassengerOverview> GetPassengerOverviews(bool onlyClubRelatedPassengers)
        {
            return GetPassengerOverviewsInternal(onlyClubRelatedPassengers,
                person => person.HasGliderInstructorLicence == false
                && person.HasGliderPAXLicence == false
                && person.HasGliderPilotLicence == false
                && person.HasGliderTraineeLicence == false
                && person.HasMotorPilotLicence == false
                && person.HasTMGLicence == false
                && person.HasTowPilotLicence == false
                && person.HasWinchOperatorLicence == false);
        }

        private List<PilotPersonListItem> GetPilotPersonListItemInternal(bool onlyClubPersons,
                                                                      Expression<Func<Person, bool>> personTypeFilter)
        {
            var persons = GetPersons(onlyClubPersons, personTypeFilter);
            return PreparePersonListItems(persons);
        }

        private List<PassengerListItem> GetPassengerListItemInternal(bool onlyClubPersons,
                                                                      Expression<Func<Person, bool>> personTypeFilter)
        {
            var persons = GetPersons(onlyClubPersons, personTypeFilter);
            return PreparePassengerListItems(persons);
        }

        private List<PilotPersonOverview> GetPersonOverviewsInternal(bool onlyClubPersons,
                                                                      Expression<Func<Person, bool>> personTypeFilter)
        {
            var persons = GetPersons(onlyClubPersons, personTypeFilter);
            return PreparePersonOverviews(persons);
        }

        private List<PassengerOverview> GetPassengerOverviewsInternal(bool onlyClubPersons,
                                                                      Expression<Func<Person, bool>> personTypeFilter)
        {
            var persons = GetPersons(onlyClubPersons, personTypeFilter);
            return PreparePassengerOverviews(persons);
        }

        private List<PilotPersonListItem> PreparePersonListItems(List<Person> persons)
        {
            return persons.Select(p => p.ToPilotPersonListItem()).ToList();
        }

        private List<PassengerListItem> PreparePassengerListItems(List<Person> persons)
        {
            return persons.Select(p => p.ToPassengerListItem()).ToList(); 
        }

        private List<PilotPersonOverview> PreparePersonOverviews(List<Person> persons)
        {
            var personOverviewList = persons.Select(p => p.ToPilotPersonOverview()).ToList(); 
            SetPersonOverviewSecurity(personOverviewList);
            return personOverviewList.ToList();
        }

        private List<PassengerOverview> PreparePassengerOverviews(List<Person> persons)
        {
            var personOverviewList = persons.Select(p => p.ToPassengerOverview()).ToList(); 
            SetPersonOverviewSecurity(personOverviewList);
            return personOverviewList.ToList();
        }
        
        internal PilotPersonDetails GetPilotPersonDetailsInternal(Guid personId, Guid clubId, bool controlAccess = true)
        {
            var person = GetPerson(personId, controlAccess);

            var personDetails = person.ToPilotPersonDetails(clubId);
            SetPersonDetailsSecurity(personDetails, person);

            return personDetails;
        }

        public PilotPersonDetails GetPilotPersonDetails(Guid personId)
        {
            var person = GetPerson(personId);

            var personDetails = person.ToPilotPersonDetails(CurrentAuthenticatedFLSUserClubId);
            SetPersonDetailsSecurity(personDetails, person);

            return personDetails;
        }

        public PilotPersonDetails GetPilotPersonDetails(Guid personId, Guid clubId)
        {
            var person = GetPerson(personId, false);

            var personDetails = person.ToPilotPersonDetails(clubId);
            SetPersonDetailsSecurity(personDetails, person);

            return personDetails;
        }

        public PilotPersonFullDetails GetPilotPersonFullDetails(Guid personId)
        {
            var person = GetPerson(personId);

            var personFullDetails = person.ToPilotPersonFullDetails(CurrentAuthenticatedFLSUserClubId);
            SetPersonDetailsSecurity(personFullDetails, person);

            return personFullDetails;
        }

        public PassengerDetails GetPassengerDetails(Guid personId)
        {
            var person = GetPerson(personId);

            var passengerDetails = person.ToPassengerDetails(CurrentAuthenticatedFLSUserClubId);
            SetPersonDetailsSecurity(passengerDetails, person);

            return passengerDetails;
        }

        public List<PilotPersonDetails> GetPersonDetailsModifiedSince(DateTime modifiedSince)
        {
            List<Person> personResult = null;

            if (IsCurrentUserInRoleSystemAdministrator)
            {
                personResult = GetPersons(false, p => p.CreatedOn >= modifiedSince || p.ModifiedOn >= modifiedSince);
            }
            else
            {
                personResult = GetPersons(true, p => p.CreatedOn >= modifiedSince || p.ModifiedOn >= modifiedSince);
            }

            return PreparePersonDetailsList(personResult);
        }

        public List<PilotPersonFullDetails> GetPersonFullDetailsModifiedSince(DateTime modifiedSince)
        {
            List<Person> personResult = null;

            if (IsCurrentUserInRoleSystemAdministrator)
            {
                personResult = GetPersons(false, p => p.CreatedOn >= modifiedSince || p.ModifiedOn >= modifiedSince);
            }
            else
            {
                personResult = GetPersons(true, p => p.CreatedOn >= modifiedSince || p.ModifiedOn >= modifiedSince);
            }

            return PreparePersonFullDetailsList(personResult);
        }

        public List<PilotPersonDetails> GetPersonDetailsDeletedSince(DateTime deletedSince)
        {
            List<Person> personResult = null;

            if (IsCurrentUserInRoleSystemAdministrator)
            {
                personResult = GetDeletedPersons(false, deletedSince);
            }
            else
            {
                personResult = GetDeletedPersons(true, deletedSince);
            }

            return PreparePersonDetailsList(personResult);
        }

        public List<PilotPersonFullDetails> GetPersonFullDetailsDeletedSince(DateTime deletedSince)
        {
            List<Person> personResult = null;

            if (IsCurrentUserInRoleSystemAdministrator)
            {
                personResult = GetDeletedPersons(false, deletedSince);
            }
            else
            {
                personResult = GetDeletedPersons(true, deletedSince);
            }

            return PreparePersonFullDetailsList(personResult);
        }

        public List<PilotPersonDetails> GetClubsPilotPersonDetailsList()
        {
            var persons = GetPersons(true, person => person.PersonClubs.Any(q => q.IsPassenger == false));

            return PreparePersonDetailsList(persons);
        }

        private List<PilotPersonDetails> PreparePersonDetailsList(List<Person> persons)
        {
            var resultList = new List<PilotPersonDetails>();

            foreach (var person in persons)
            {
                var personDetail = person.ToPilotPersonDetails(CurrentAuthenticatedFLSUserClubId);

                personDetail.NotNull("PersonDetail");
                SetPersonDetailsSecurity(personDetail, person);
                resultList.Add(personDetail);
            }

            return resultList.ToList();
        }

        private List<PilotPersonFullDetails> PreparePersonFullDetailsList(List<Person> persons)
        {
            var resultList = new List<PilotPersonFullDetails>();

            foreach (var person in persons)
            {
                var personDetail = person.ToPilotPersonFullDetails(CurrentAuthenticatedFLSUserClubId);

                personDetail.NotNull("PersonDetail");
                SetPersonDetailsSecurity(personDetail, person);
                resultList.Add(personDetail);
            }

            return resultList.ToList();
        }

        internal List<Person> GetPersonsWithExpiredLicences(DateTime expireDate)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                List<Person> persons = null;

                persons = context.Persons
                        .Where(p => (p.MedicalClass1ExpireDate.HasValue 
                        && DbFunctions.TruncateTime(p.MedicalClass1ExpireDate.Value) == expireDate.Date)
                            || (p.MedicalClass2ExpireDate.HasValue &&
                            DbFunctions.TruncateTime(p.MedicalClass2ExpireDate.Value) == expireDate.Date)
                            || (p.MedicalLaplExpireDate.HasValue &&
                            DbFunctions.TruncateTime(p.MedicalLaplExpireDate.Value) == expireDate.Date)
                            || (p.GliderInstructorLicenceExpireDate.HasValue &&
                            DbFunctions.TruncateTime(p.GliderInstructorLicenceExpireDate.Value) == expireDate.Date))
                        .OrderBy(pe => pe.Lastname)
                        .ToList();

                return persons;
            }
        }

        internal List<Person> GetPersons(bool onlyClubPersons)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                List<Person> persons = null;

                if (onlyClubPersons)
                {
                    persons = context.Persons.Include(Constants.Country).Include(Constants.PersonClubs)
                        .Where(p => p.PersonClubs.Any(ppc => ppc.ClubId == CurrentAuthenticatedFLSUser.ClubId))
                        .OrderBy(pe => pe.Lastname)
                        .ToList();
                }
                else
                {
                    persons = context.Persons.Include(Constants.Country).OrderBy(pe => pe.Lastname).ToList();
                }

                return persons;
            }
        }

        internal List<Person> GetPersons(bool onlyClubPersons, Expression<Func<Person, bool>> personTypeFilter)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                List<Person> persons = null;

                if (onlyClubPersons)
                {
                    persons = context.Persons.Include(Constants.Country).Include(Constants.PersonClubs)
                        .Where(p => p.PersonClubs.Any(ppc => ppc.ClubId == CurrentAuthenticatedFLSUser.ClubId))
                        .Where(personTypeFilter)
                        .OrderBy(pe => pe.Lastname)
                        .ToList();
                }
                else
                {
                    persons = context.Persons.Include(Constants.Country).Where(personTypeFilter).OrderBy(pe => pe.Lastname).ToList();
                }

                return persons;
            }
        }

        internal List<Person> GetDeletedPersons(bool onlyClubPersons, DateTime deletedSince)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                List<Person> persons = null;

                if (onlyClubPersons)
                {
                    persons = context.Persons.Include(Constants.Country).Include(Constants.PersonClubs)
                        .Where(p => p.PersonClubs.Any(ppc => ppc.ClubId == CurrentAuthenticatedFLSUser.ClubId))
                        .Where(p => p.DeletedOn >= deletedSince)
                        .OrderBy(pe => pe.Lastname)
                        .ToList();
                }
                else
                {
                    persons = context.Persons.Include(Constants.Country).Include(Constants.PersonClubs)
                        .Where(p => p.DeletedOn >= deletedSince)
                        .OrderBy(pe => pe.Lastname)
                        .ToList();
                }

                return persons;
            }
        }

        internal Person GetPerson(Guid personId, bool controlAccess = true)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                Person person = null;

                if (IsCurrentUserInRoleSystemAdministrator || controlAccess == false)
                {
                    person = context.Persons.Include(Constants.Users)
                        .Include(Constants.PersonPersonCategories).Include(Constants.PersonClubs).Where(p => p.PersonId == personId)
                        .ToList()
                        .FirstOrDefault();
                }
                else
                {
                    person = context.Persons.Include(Constants.Users)
                        .Include(Constants.PersonPersonCategories).Include(Constants.PersonClubs)
                                .Where(p => p.PersonId == personId && p.PersonClubs
                                .Any(pc => pc.ClubId == CurrentAuthenticatedFLSUser.ClubId))
                                .ToList()
                                .FirstOrDefault(); 
                }
                
                return person;
            }
        }
        
        public void InsertPersonDetails(PilotPersonDetails personDetails)
        {
            personDetails.ArgumentNotNull("personDetails");

            var person = personDetails.ToPerson(CurrentAuthenticatedFLSUserClubId);

            person.EntityNotNull("Person", Guid.Empty);

            InsertPerson(person);

            //Map it back to details
            person.ToPilotPersonDetails(CurrentAuthenticatedFLSUserClubId, personDetails);
        }

        public void InsertPersonFullDetails(PilotPersonFullDetails personFullDetails)
        {
            personFullDetails.ArgumentNotNull("personFullDetails");

            var person = personFullDetails.ToPerson(CurrentAuthenticatedFLSUserClubId);

            person.EntityNotNull("Person", Guid.Empty);

            InsertPerson(person);

            //Map it back to details
            person.ToPilotPersonFullDetails(CurrentAuthenticatedFLSUserClubId, personFullDetails);
        }

        public void InsertPassengerDetails(PassengerDetails passengerDetails)
        {
            passengerDetails.ArgumentNotNull("passengerDetails");

            var person = passengerDetails.ToPerson(CurrentAuthenticatedFLSUserClubId);

            person.EntityNotNull("Person", Guid.Empty);

            InsertPerson(person);

            //Map it back to details
            person.ToPassengerDetails(CurrentAuthenticatedFLSUserClubId, passengerDetails);
        }

        internal void InsertPerson(Person person)
        {
            person.ArgumentNotNull("person");

            using (var context = _dataAccessService.CreateDbContext())
            {
                context.Persons.Add(person);
                context.SaveChanges();
            }
        }

        public void UpdatePersonDetails(PilotPersonDetails currentPersonDetails)
        {
            currentPersonDetails.ArgumentNotNull("currentPersonDetails");
            var original = GetPerson(currentPersonDetails.PersonId);
            original.EntityNotNull("Person", currentPersonDetails.PersonId);

            using (var context = _dataAccessService.CreateDbContext())
            {
                context.Persons.Attach(original);
                currentPersonDetails.ToPerson(CurrentAuthenticatedFLSUserClubId, original);
                
                if (context.ChangeTracker.HasChanges())
                {
                    context.SaveChanges();

                    //Map it back to details
                    original.ToPilotPersonDetails(CurrentAuthenticatedFLSUserClubId, currentPersonDetails);
                }
            }
        }

        public void UpdatePersonFullDetails(PilotPersonFullDetails currentPersonFullDetails)
        {
            currentPersonFullDetails.ArgumentNotNull("currentPersonFullDetails");
            var original = GetPerson(currentPersonFullDetails.PersonId);
            bool isDeleted = false;

            if (original == null)
            {
                //we didn't find the original record (may be it was deleted earlier)
                isDeleted = true;
                original = new Person();
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                if (isDeleted)
                {
                    currentPersonFullDetails.ToPerson(CurrentAuthenticatedFLSUserClubId, original);
                    context.Persons.Add(original);
                }
                else
                {
                    context.Persons.Attach(original);
                    currentPersonFullDetails.ToPerson(CurrentAuthenticatedFLSUserClubId, original);
                }

                if (context.ChangeTracker.HasChanges())
                {
                    context.SaveChanges();

                    //Map it back to details
                    original.ToPilotPersonFullDetails(CurrentAuthenticatedFLSUserClubId, currentPersonFullDetails);
                }
            }
        }
        
        public void UpdatePassengerDetails(PassengerDetails currentPassengerDetails)
        {
            currentPassengerDetails.ArgumentNotNull("currentPassengerDetails");
            var original = GetPerson(currentPassengerDetails.PersonId);
            original.EntityNotNull("Person", currentPassengerDetails.PersonId);

            using (var context = _dataAccessService.CreateDbContext())
            {
                context.Persons.Attach(original);
                currentPassengerDetails.ToPerson(CurrentAuthenticatedFLSUserClubId, original);

                if (context.ChangeTracker.HasChanges())
                {
                    context.SaveChanges();

                    //Map it back to details
                    original.ToPassengerDetails(CurrentAuthenticatedFLSUserClubId, currentPassengerDetails);
                }
            }
        }

        public void DeletePerson(Guid personId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var original = context.Persons.FirstOrDefault(l => l.PersonId == personId);
                original.EntityNotNull("Person", personId);

                context.Persons.Remove(original);
                context.SaveChanges();
            }
        }

        public void DeletePersonFullDetails(Guid personId, DateTime deletedOn)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var original = context.Persons.ToList().FirstOrDefault(l => l.PersonId == personId);
                original.EntityNotNull("Person", personId);

                if (original.CreatedOn.SetAsUtc() > deletedOn.SetAsUtc())
                {
                    throw new BadRequestException("Deleted on date is before created on date.");
                }

                original.SetPropertyValue("DeletedOn", deletedOn.SetAsUtc());
                original.DoNotUpdateTimeStampsInMetaData = true;
                context.Persons.Remove(original);
                context.SaveChanges();
            }
        }
        #endregion Person

        #region Security
        private void SetPersonOverviewSecurity(IEnumerable<PersonOverview> list)
        {
            if (CurrentAuthenticatedFLSUser == null)
            {
                Logger.Warn(string.Format("CurrentAuthenticatedFLSUser is NULL. Can't set correct security flags to the object."));
                foreach (var overview in list)
                {
                    overview.CanUpdateRecord = false;
                    overview.CanDeleteRecord = false;

                    //reset communication properties to null as the user is not allowed to see these details
                    overview.MobilePhoneNumber = string.Empty;
                    overview.PrivateEmail = string.Empty;
                }

                return;
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                foreach (var personOverview in list)
                {
                    if (IsCurrentUserInRoleSystemAdministrator ||
                        IsCurrentUserInRoleClubAdministrator ||
                        IsOwner(context.Persons.First(a => a.PersonId == personOverview.PersonId)))
                    {
                        personOverview.CanUpdateRecord = true;
                        personOverview.CanDeleteRecord = true;
                    }
                    else
                    {
                        personOverview.CanUpdateRecord = false;
                        personOverview.CanDeleteRecord = false;

                        //reset communication properties to null as the user is not allowed to see these details
                        personOverview.MobilePhoneNumber = string.Empty;
                        personOverview.PrivateEmail = string.Empty;
                    }
                }
            }
        }

        private void SetPersonDetailsSecurity(PersonDetails details, Person person)
        {
            if (details == null)
            {
                Logger.Error(string.Format("PersonDetails is null while trying to set security properties"));
                return;
            }

            if (CurrentAuthenticatedFLSUser == null)
            {
                Logger.Warn(string.Format("CurrentAuthenticatedFLSUser is NULL. Can't set correct security flags to the object."));
                details.CanUpdateRecord = false;
                details.CanDeleteRecord = false;
                return;
            }

            //TOOD: Check relation to club to set correct security level
            if (IsCurrentUserInRoleClubAdministrator || IsOwner(person))
            {
                details.CanUpdateRecord = true;
                details.CanDeleteRecord = true;
            }
            else
            {
                details.CanUpdateRecord = false;
                details.CanDeleteRecord = false;
            }
        }
        #endregion Security

        public PilotPersonDetails GetMyPersonDetails()
        {
            if (_identityService.CurrentAuthenticatedFLSUser.PersonId.HasValue)
            {
                return GetPilotPersonDetails(_identityService.CurrentAuthenticatedFLSUser.PersonId.Value);
            }

            return null;
        }

        public MemoryStream GetPersonListExcelPackageMemoryStream()
        {
            var personList = GetClubsPilotPersonDetailsList();
            var bytes = ExcelExporter.GetPersonExcelPackage(personList, $"Adressliste");
            return bytes.ToMemoryStream();
        }

        public void SendPersonListExcelToUsersEmailAddress()
        {
            var personList = GetClubsPilotPersonDetailsList();
            var bytes = ExcelExporter.GetPersonExcelPackage(personList, $"Adressliste");
            var message = _addressListEmailBuildService.CreateAddressListEmail(CurrentAuthenticatedFLSUser, bytes);
            _addressListEmailBuildService.SendEmail(message);
        }
    }
}
