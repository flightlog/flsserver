using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using FLS.Common.Extensions;
using FLS.Common.Paging;
using FLS.Common.Validators;
using FLS.Data.WebApi;
using FLS.Data.WebApi.DataExchange;
using FLS.Data.WebApi.Person;
using FLS.Data.WebApi.User;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Exceptions;
using FLS.Server.Data.Mapping;
using FLS.Server.Data.Resources;
using FLS.Server.Interfaces;
using FLS.Server.Service.Email;
using FLS.Server.Service.Exporting;
using NLog;
using OfficeOpenXml;
using FLS.Server.Service.Extensions;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;
using FLS.Server.Data.Objects.Person;

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
        public List<PersonListItem> GetPilotPersonListItems(bool onlyClubRelatedPersons)
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
        public List<PersonListItem> GetGliderPilotPersonListItems(bool onlyClubRelatedPilots)
        {
            return GetPilotPersonListItemInternal(onlyClubRelatedPilots,
                person => person.HasGliderPilotLicence || person.HasGliderTraineeLicence);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedPilots">if set to <c>true</c> only club related pilots will be returned.</param>
        /// <returns></returns>
        public List<PersonListItem> GetGliderObserverPilotPersonListItems(bool onlyClubRelatedPilots)
        {
            return GetPilotPersonListItemInternal(onlyClubRelatedPilots, person => person.HasGliderPilotLicence);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedInstuctors">if set to <c>true</c> only club related instructors will be returned.</param>
        /// <returns></returns>
        public List<PersonListItem> GetGliderInstructorPersonListItems(bool onlyClubRelatedInstuctors)
        {
            return GetPilotPersonListItemInternal(onlyClubRelatedInstuctors, person => person.HasGliderInstructorLicence);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedPilots">if set to <c>true</c> only club related pilots will be returned.</param>
        /// <returns></returns>
        public List<PersonListItem> GetTowingPilotPersonListItems(bool onlyClubRelatedPilots)
        {
            return GetPilotPersonListItemInternal(onlyClubRelatedPilots, person => person.HasTowPilotLicence);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedPilots">if set to <c>true</c> only club related pilots will be returned.</param>
        /// <returns></returns>
        public List<PersonListItem> GetMotorPilotPersonListItems(bool onlyClubRelatedPilots)
        {
            return GetPilotPersonListItemInternal(onlyClubRelatedPilots,
                person => person.HasMotorPilotLicence || person.HasTMGLicence);
        }

        public List<PersonListItem> GetMotorInstructorPersonListItems(bool onlyClubRelatedInstuctors)
        {
            return GetPilotPersonListItemInternal(onlyClubRelatedInstuctors, person => person.HasMotorInstructorLicence
                || (person.HasGliderInstructorLicence && person.HasTMGLicence));
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedTrainees">if set to <c>true</c> only club related trainees will be returned.</param>
        /// <returns></returns>
        public List<PersonListItem> GetGliderTraineePersonListItems(bool onlyClubRelatedTrainees)
        {
            return GetPilotPersonListItemInternal(onlyClubRelatedTrainees, person => person.HasGliderTraineeLicence);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedWinchOperators">if set to <c>true</c> [only club related winch operators].</param>
        /// <returns></returns>
        public List<PersonListItem> GetWinchOperatorPersonListItems(bool onlyClubRelatedWinchOperators)
        {
            return GetPilotPersonListItemInternal(onlyClubRelatedWinchOperators,
                person => person.HasWinchOperatorLicence);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedPassengers">if set to <c>true</c> [only club related passengers].</param>
        /// <returns></returns>
        public List<PersonListItem> GetPassengerListItems(bool onlyClubRelatedPassengers)
        {
            var persons = GetPersons(onlyClubRelatedPassengers, person => person.HasGliderInstructorLicence == false
                                                                          && person.HasGliderPAXLicence == false
                                                                          && person.HasGliderPilotLicence == false
                                                                          && person.HasGliderTraineeLicence == false
                                                                          && person.HasMotorPilotLicence == false
                                                                          && person.HasTMGLicence == false
                                                                          && person.HasTowPilotLicence == false
                                                                          && person.HasWinchOperatorLicence == false);
            return persons.Select(p => p.ToPersonListItem()).ToList();
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedPersons">if set to <c>true</c> only club related persons will be returned.</param>
        /// <returns></returns>
        public List<PersonOverview> GetPilotPersonOverviews(bool onlyClubRelatedPersons)
        {
            return GetPersonOverviewsInternal(onlyClubRelatedPersons, person => true);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedPilots">if set to <c>true</c> only club related pilots will be returned.</param>
        /// <returns></returns>
        public List<PersonOverview> GetGliderPilotPersonOverviews(bool onlyClubRelatedPilots)
        {
            return GetPersonOverviewsInternal(onlyClubRelatedPilots,
                person => person.HasGliderPilotLicence || person.HasGliderTraineeLicence);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedPilots">if set to <c>true</c> only club related pilots will be returned.</param>
        /// <returns></returns>
        public List<PersonOverview> GetGliderObserverPilotPersonOverviews(bool onlyClubRelatedPilots)
        {
            return GetPersonOverviewsInternal(onlyClubRelatedPilots, person => person.HasGliderPilotLicence);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedInstuctors">if set to <c>true</c> only club related instructors will be returned.</param>
        /// <returns></returns>
        public List<PersonOverview> GetGliderInstructorPersonOverviews(bool onlyClubRelatedInstuctors)
        {
            return GetPersonOverviewsInternal(onlyClubRelatedInstuctors, person => person.HasGliderInstructorLicence);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedPilots">if set to <c>true</c> only club related pilots will be returned.</param>
        /// <returns></returns>
        public List<PersonOverview> GetTowingPilotPersonOverviews(bool onlyClubRelatedPilots)
        {
            return GetPersonOverviewsInternal(onlyClubRelatedPilots, person => person.HasTowPilotLicence);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedTrainees">if set to <c>true</c> only club related trainees will be returned.</param>
        /// <returns></returns>
        public List<PersonOverview> GetGliderTraineePilotPersonOverviews(bool onlyClubRelatedTrainees)
        {
            return GetPersonOverviewsInternal(onlyClubRelatedTrainees, person => person.HasGliderTraineeLicence);
        }

        public List<PersonOverview> GetWinchOperatorPilotPersonOverviews(bool onlyClubRelatedWinchOperators)
        {
            return GetPersonOverviewsInternal(onlyClubRelatedWinchOperators, person => person.HasWinchOperatorLicence);
        }

        public List<PersonOverview> GetPassengerOverviews(bool onlyClubRelatedPassengers)
        {
            var persons = GetPersons(onlyClubRelatedPassengers, person => person.HasGliderInstructorLicence == false
                                                                          && person.HasGliderPAXLicence == false
                                                                          && person.HasGliderPilotLicence == false
                                                                          && person.HasGliderTraineeLicence == false
                                                                          && person.HasMotorPilotLicence == false
                                                                          && person.HasTMGLicence == false
                                                                          && person.HasTowPilotLicence == false
                                                                          && person.HasWinchOperatorLicence == false);

            var personOverviewList = persons.Select(p => p.ToPersonOverview(CurrentAuthenticatedFLSUserClubId)).ToList();
            SetPersonOverviewSecurity(personOverviewList);
            return personOverviewList.ToList();
        }

        public PagedList<PersonOverview> GetPagedPersonOverview(int? pageStart, int? pageSize,
            PageableSearchFilter<PersonOverviewSearchFilter> pageableSearchFilter)
        {
            if (pageableSearchFilter == null)
                pageableSearchFilter = new PageableSearchFilter<PersonOverviewSearchFilter>();
            if (pageableSearchFilter.SearchFilter == null)
                pageableSearchFilter.SearchFilter = new PersonOverviewSearchFilter();
            if (pageableSearchFilter.Sorting == null || pageableSearchFilter.Sorting.Any() == false)
            {
                pageableSearchFilter.Sorting = new Dictionary<string, string>();
                pageableSearchFilter.Sorting.Add("Lastname", "asc");
                pageableSearchFilter.Sorting.Add("Firstname", "asc");
            }

            //needs to remap related table columns for correct sorting
            //http://stackoverflow.com/questions/3515105/using-first-with-orderby-and-dynamicquery-in-one-to-many-related-tables
            foreach (var sort in pageableSearchFilter.Sorting.Keys.ToList())
            {
                if (sort == "CountryName")
                {
                    pageableSearchFilter.Sorting.Add("Country.CountryName", pageableSearchFilter.Sorting[sort]);
                    pageableSearchFilter.Sorting.Remove(sort);
                }
                else if (sort == "AddressLine")
                {
                    pageableSearchFilter.Sorting.Add("AddressLine1", pageableSearchFilter.Sorting[sort]);
                    pageableSearchFilter.Sorting.Add("AddressLine2", pageableSearchFilter.Sorting[sort]);
                    pageableSearchFilter.Sorting.Remove(sort);
                }
                else if (sort == "ZipCode")
                {
                    pageableSearchFilter.Sorting.Add("Zip", pageableSearchFilter.Sorting[sort]);
                    pageableSearchFilter.Sorting.Remove(sort);
                }
                else if (sort == "PrivateEmail")
                {
                    pageableSearchFilter.Sorting.Add("EmailPrivate", pageableSearchFilter.Sorting[sort]);
                    pageableSearchFilter.Sorting.Remove(sort);
                }
                else if (sort == "MobilePhoneNumber")
                {
                    pageableSearchFilter.Sorting.Add("MobilePhone", pageableSearchFilter.Sorting[sort]);
                    pageableSearchFilter.Sorting.Remove(sort);
                }
                else if (sort == "MemberStateName")
                {
                    //TODO: Add ability to sort for member state name
                    pageableSearchFilter.Sorting.Remove(sort);
                }
            }

            if (pageableSearchFilter.Sorting == null || pageableSearchFilter.Sorting.Any() == false)
            {
                pageableSearchFilter.Sorting = new Dictionary<string, string>();
                pageableSearchFilter.Sorting.Add("Lastname", "asc");
                pageableSearchFilter.Sorting.Add("Firstname", "asc");
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                var persons = context.Persons.Include(Constants.Country).Include(Constants.PersonClubs)
                    .Include($"{Constants.PersonClubs}.MemberState")
                    .OrderByPropertyNames(pageableSearchFilter.Sorting);

                var filter = pageableSearchFilter.SearchFilter;
                persons = persons.WhereIf(filter.Lastname,
                    person => person.Lastname.Contains(filter.Lastname));
                persons = persons.WhereIf(filter.Firstname,
                    person => person.Firstname.Contains(filter.Firstname));
                persons = persons.WhereIf(filter.AddressLine,
                    person => person.AddressLine1.Contains(filter.AddressLine)
                              || person.AddressLine2.Contains(filter.AddressLine));
                persons = persons.WhereIf(filter.ZipCode,
                    person => person.Zip.Contains(filter.ZipCode));
                persons = persons.WhereIf(filter.City,
                    person => person.City.Contains(filter.City));
                persons = persons.WhereIf(filter.CountryName,
                    person => person.Country.CountryName.Contains(filter.CountryName));
                persons = persons.WhereIf(filter.PrivateEmail,
                    person => person.EmailPrivate.Contains(filter.PrivateEmail));
                persons = persons.WhereIf(filter.MobilePhoneNumber,
                    person => person.MobilePhone.Contains(filter.MobilePhoneNumber));
                persons = persons.WhereIf(filter.MemberStateName,
                    person =>
                            person.PersonClubs.Any(x => x.MemberState.MemberStateName.Contains(filter.MemberStateName)));
                persons = persons.WhereIf(filter.LicenceNumber,
                    person => person.LicenceNumber.Contains(filter.LicenceNumber));

                if (filter.HasGliderInstructorLicence.HasValue)
                    persons =
                        persons.Where(
                            person => person.HasGliderInstructorLicence == filter.HasGliderInstructorLicence.Value);
                if (filter.HasGliderPilotLicence.HasValue)
                    persons = persons.Where(person => person.HasGliderPilotLicence == filter.HasGliderPilotLicence.Value);
                if (filter.HasGliderTraineeLicence.HasValue)
                    persons =
                        persons.Where(person => person.HasGliderTraineeLicence == filter.HasGliderTraineeLicence.Value);
                if (filter.HasMotorPilotLicence.HasValue)
                    persons = persons.Where(person => person.HasMotorPilotLicence == filter.HasMotorPilotLicence.Value);
                if (filter.HasTMGLicence.HasValue)
                    persons = persons.Where(person => person.HasTMGLicence == filter.HasTMGLicence.Value);
                if (filter.HasTowPilotLicence.HasValue)
                    persons = persons.Where(person => person.HasTowPilotLicence == filter.HasTowPilotLicence.Value);
                if (filter.HasGliderPassengerLicence.HasValue)
                    persons =
                        persons.Where(person => person.HasGliderPAXLicence == filter.HasGliderPassengerLicence.Value);
                if (filter.HasWinchOperatorLicence.HasValue)
                    persons =
                        persons.Where(person => person.HasWinchOperatorLicence == filter.HasWinchOperatorLicence.Value);
                if (filter.HasMotorInstructorLicence.HasValue)
                    persons =
                        persons.Where(
                            person => person.HasMotorInstructorLicence == filter.HasMotorInstructorLicence.Value);

                if (filter.OnlyClubRelatedPersons.HasValue == false || filter.OnlyClubRelatedPersons.Value)
                {
                    persons =
                        persons.Where(p => p.PersonClubs.Any(ppc => ppc.ClubId == CurrentAuthenticatedFLSUserClubId));
                }

                if (filter.MatchedPersonCategories != null && filter.MatchedPersonCategories.Any())
                {
                    persons = persons.Where(x =>
                        x.PersonPersonCategories.Any(ppc =>
                            filter.MatchedPersonCategories.Contains(ppc.PersonCategoryId)));
                }

                var pagedQuery = new PagedQuery<Person>(persons, pageStart, pageSize);

                var overviewList =
                    pagedQuery.Items.ToList().Select(x => x.ToPersonOverview(CurrentAuthenticatedFLSUserClubId))
                        .Where(obj => obj != null)
                        .ToList();

                SetPersonOverviewSecurity(overviewList);

                var pagedList = new PagedList<PersonOverview>(overviewList, pagedQuery.PageStart,
                    pagedQuery.PageSize, pagedQuery.TotalRows);

                return pagedList;
            }
        }

        private List<PersonListItem> GetPilotPersonListItemInternal(bool onlyClubPersons,
            Expression<Func<Person, bool>> personTypeFilter)
        {
            var persons = GetPersons(onlyClubPersons, personTypeFilter);
            return PreparePersonListItems(persons);
        }

        private List<PersonOverview> GetPersonOverviewsInternal(bool onlyClubPersons,
            Expression<Func<Person, bool>> personTypeFilter)
        {
            var persons = GetPersons(onlyClubPersons, personTypeFilter);
            var personOverviewList = persons.Select(p => p.ToPersonOverview(CurrentAuthenticatedFLSUserClubId)).ToList();
            SetPersonOverviewSecurity(personOverviewList);
            return personOverviewList.ToList();
        }

        private List<PersonListItem> PreparePersonListItems(List<Person> persons)
        {
            return persons.Select(p => p.ToPersonListItem()).ToList();
        }

        internal PersonDetails GetPilotPersonDetailsInternal(Guid personId, Guid clubId, bool controlAccess = true)
        {
            var person = GetPerson(personId, controlAccess);

            var personDetails = person.ToPersonDetails(clubId);
            SetPersonDetailsSecurity(personDetails, person);

            return personDetails;
        }

        public PersonDetails GetPersonDetails(Guid personId)
        {
            var person = GetPerson(personId);

            var personDetails = person.ToPersonDetails(CurrentAuthenticatedFLSUserClubId);
            SetPersonDetailsSecurity(personDetails, person);

            return personDetails;
        }

        public PersonDetails GetPersonDetails(Guid personId, Guid clubId)
        {
            var person = GetPerson(personId, false);

            var personDetails = person.ToPersonDetails(clubId);
            SetPersonDetailsSecurity(personDetails, person);

            return personDetails;
        }

        public PersonDetails GetPilotPersonDetails(string memberNumber)
        {
            var person = GetPerson(memberNumber);

            var personDetails = person.ToPersonDetails(CurrentAuthenticatedFLSUserClubId);
            SetPersonDetailsSecurity(personDetails, person);

            return personDetails;
        }

        public PersonFullDetails GetPilotPersonFullDetails(string memberNumber)
        {
            var person = GetPerson(memberNumber);

            var personFullDetails = person.ToPersonFullDetails(CurrentAuthenticatedFLSUserClubId);
            SetPersonDetailsSecurity(personFullDetails, person);

            return personFullDetails;
        }

        public PersonFullDetails GetPilotPersonFullDetails(Guid personId)
        {
            var person = GetPerson(personId);

            var personFullDetails = person.ToPersonFullDetails(CurrentAuthenticatedFLSUserClubId);
            SetPersonDetailsSecurity(personFullDetails, person);

            return personFullDetails;
        }

        public List<PersonDetails> GetPersonDetailsModifiedSince(DateTime modifiedSince)
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

        public List<PersonFullDetails> GetPersonFullDetailsModifiedSince(DateTime modifiedSince)
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

        public List<PersonDetails> GetPersonDetailsDeletedSince(DateTime deletedSince)
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

        public List<PersonFullDetails> GetPersonFullDetailsDeletedSince(DateTime deletedSince)
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

        public List<PersonDetails> GetClubsPilotPersonDetailsList()
        {
            var persons = GetPersons(true, person => person.PersonClubs.Any(q => q.IsPassenger == false));

            return PreparePersonDetailsList(persons);
        }

        private List<PersonDetails> PreparePersonDetailsList(List<Person> persons)
        {
            var resultList = new List<PersonDetails>();

            foreach (var person in persons)
            {
                var personDetail = person.ToPersonDetails(CurrentAuthenticatedFLSUserClubId);

                personDetail.NotNull("PersonDetail");
                SetPersonDetailsSecurity(personDetail, person);
                resultList.Add(personDetail);
            }

            return resultList.ToList();
        }

        private List<PersonFullDetails> PreparePersonFullDetailsList(List<Person> persons)
        {
            var resultList = new List<PersonFullDetails>();

            foreach (var person in persons)
            {
                var personDetail = person.ToPersonFullDetails(CurrentAuthenticatedFLSUserClubId);

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
                                    DbFunctions.TruncateTime(p.GliderInstructorLicenceExpireDate.Value) ==
                                    expireDate.Date)
                                || (p.MotorInstructorLicenceExpireDate.HasValue &&
                                    DbFunctions.TruncateTime(p.MotorInstructorLicenceExpireDate.Value) ==
                                    expireDate.Date)
                                || (p.PartMLicenceExpireDate.HasValue &&
                                    DbFunctions.TruncateTime(p.PartMLicenceExpireDate.Value) ==
                                    expireDate.Date))
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
                        .Include($"{Constants.PersonClubs}.MemberState")
                        .Where(p => p.PersonClubs.Any(ppc => ppc.ClubId == CurrentAuthenticatedFLSUser.ClubId))
                        .Where(personTypeFilter)
                        .OrderBy(pe => pe.Lastname)
                        .ToList();
                }
                else
                {
                    persons = context.Persons.Include(Constants.Country)
                        .Where(personTypeFilter)
                        .Where(p => p.EnableAddress)
                        .OrderBy(pe => pe.Lastname).ToList();
                }

                return persons;
            }
        }

        internal List<Person> GetDeletedPersons(bool onlyClubPersons, DateTime deletedSince)
        {
            using (var context = _dataAccessService.CreateDeletedDbContext())
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

        internal Person GetPerson(string memberNumber, bool controlAccess = true)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                Person person = null;

                if (IsCurrentUserInRoleSystemAdministrator || controlAccess == false)
                {
                    person = context.Persons.Include(Constants.Users)
                        .Include(Constants.PersonPersonCategories)
                        .Include(Constants.PersonClubs)
                        .Where(p => p.PersonClubs.Any(pc => pc.MemberNumber == memberNumber))
                        .ToList()
                        .FirstOrDefault();
                }
                else
                {
                    person = context.Persons.Include(Constants.Users)
                        .Include(Constants.PersonPersonCategories).Include(Constants.PersonClubs)
                        .Where(
                            p =>
                                p.PersonClubs.Any(
                                    pc =>
                                        pc.MemberNumber == memberNumber &&
                                        pc.ClubId == CurrentAuthenticatedFLSUserClubId))
                        .ToList()
                        .FirstOrDefault();
                }

                return person;
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
                        .Include(Constants.PersonPersonCategories)
                        .Include(Constants.PersonPersonCategories + ".PersonCategory")
                        .Include(Constants.PersonClubs)
                        .Where(p => p.PersonId == personId)
                        .ToList()
                        .FirstOrDefault();
                }
                else
                {
                    person = context.Persons.Include(Constants.Users)
                        .Include(Constants.PersonPersonCategories)
                        .Include(Constants.PersonPersonCategories + ".PersonCategory")
                        .Include(Constants.PersonClubs)
                        .Where(p => p.PersonId == personId && p.PersonClubs
                                        .Any(pc => pc.ClubId == CurrentAuthenticatedFLSUser.ClubId))
                        .ToList()
                        .FirstOrDefault();
                }

                return person;
            }
        }

        public void InsertPersonDetails(PersonDetails personDetails)
        {
            personDetails.ArgumentNotNull("personDetails");

            var person = personDetails.ToPerson(CurrentAuthenticatedFLSUserClubId);

            person.EntityNotNull("Person", Guid.Empty);

            InsertPerson(person);

            //Map it back to details
            var inserted = GetPerson(person.PersonId);
            inserted.ToPersonDetails(CurrentAuthenticatedFLSUserClubId, personDetails);
        }

        public void InsertPersonFullDetails(PersonFullDetails personFullDetails)
        {
            personFullDetails.ArgumentNotNull("personFullDetails");

            var person = personFullDetails.ToPerson(CurrentAuthenticatedFLSUserClubId);

            person.EntityNotNull("Person", Guid.Empty);

            InsertPerson(person);

            //Map it back to details
            var inserted = GetPerson(person.PersonId);
            inserted.ToPersonFullDetails(CurrentAuthenticatedFLSUserClubId, personFullDetails);
        }

        public void InsertPassengerDetails(PersonDetails passengerDetails)
        {
            passengerDetails.ArgumentNotNull("passengerDetails");

            var person = passengerDetails.ToPerson(CurrentAuthenticatedFLSUserClubId);

            person.EntityNotNull("Person", Guid.Empty);

            InsertPerson(person);

            //Map it back to details
            var inserted = GetPerson(person.PersonId);
            inserted.ToPersonDetails(CurrentAuthenticatedFLSUserClubId, passengerDetails);
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

        public void UpdatePersonDetails(PersonDetails currentPersonDetails)
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
                    var updated = GetPerson(currentPersonDetails.PersonId);
                    updated.ToPersonDetails(CurrentAuthenticatedFLSUserClubId, currentPersonDetails);
                }
            }
        }

        public void UpdatePersonFullDetails(PersonFullDetails currentPersonFullDetails)
        {
            currentPersonFullDetails.ArgumentNotNull("currentPersonFullDetails");
            var original = GetPerson(currentPersonFullDetails.PersonId);
            original.EntityNotNull("Person", currentPersonFullDetails.PersonId);

            using (var context = _dataAccessService.CreateDbContext())
            {
                context.Persons.Attach(original);
                currentPersonFullDetails.ToPerson(CurrentAuthenticatedFLSUserClubId, original);

                if (context.ChangeTracker.HasChanges())
                {
                    context.SaveChanges();

                    //Map it back to details
                    original.ToPersonFullDetails(CurrentAuthenticatedFLSUserClubId, currentPersonFullDetails);
                }
            }
        }

        public void UpdatePassengerDetails(PersonDetails currentPassengerDetails)
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
                    original.ToPersonDetails(CurrentAuthenticatedFLSUserClubId, currentPassengerDetails);
                }
            }
        }

        public void DeletePerson(Guid personId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var original = context.Persons.Include(Constants.PersonClubs)
                    .FirstOrDefault(l => l.PersonId == personId);
                original.EntityNotNull("Person", personId);

                if (context.FlightCrews.Any(x => x.PersonId == original.PersonId)
                    ||
                    context.AircraftReservations.Any(
                        x =>
                            x.PilotPersonId == original.PersonId ||
                            (x.SecondCrewPersonId.HasValue && x.SecondCrewPersonId.Value == original.PersonId))
                    || context.PlanningDayAssignments.Any(x => x.AssignedPersonId == original.PersonId))
                {
                    throw new ConstraintException(
                        $"The person {original.DisplayName} has some related active data records and can not be deleted!");
                }

                context.Persons.Remove(original);
                context.SaveChanges();
            }
        }

        public void DeletePersonFullDetails(Guid personId, DateTime deletedOn)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var original =
                    context.Persons.Include(Constants.PersonClubs).ToList().FirstOrDefault(l => l.PersonId == personId);
                original.EntityNotNull("Person", personId);

                if (original.CreatedOn.SetAsUtc() > deletedOn.SetAsUtc())
                {
                    throw new BadRequestException("Deleted on date is before created on date.");
                }

                if (context.FlightCrews.Any(x => x.PersonId == original.PersonId)
                    ||
                    context.AircraftReservations.Any(
                        x =>
                            x.PilotPersonId == original.PersonId ||
                            (x.SecondCrewPersonId.HasValue && x.SecondCrewPersonId.Value == original.PersonId))
                    || context.PlanningDayAssignments.Any(x => x.AssignedPersonId == original.PersonId))
                {
                    throw new ConstraintException(
                        $"The person {original.DisplayName} has some related active data records and can not be deleted!");
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
                Logger.Warn(
                    string.Format("CurrentAuthenticatedFLSUser is NULL. Can't set correct security flags to the object."));
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
                Logger.Warn(
                    string.Format("CurrentAuthenticatedFLSUser is NULL. Can't set correct security flags to the object."));
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

        public PersonDetails GetMyPersonDetails()
        {
            if (_identityService.CurrentAuthenticatedFLSUser.PersonId.HasValue)
            {
                return GetPersonDetails(_identityService.CurrentAuthenticatedFLSUser.PersonId.Value);
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

        public ImportJob<PersonDetails> ImportPersonExcelFile(byte[] fileContentBytes, bool realImport = false)
        {
            var importJob = new ImportJob<PersonDetails>()
            {
                ImportObjects = new List<ImportObject<PersonDetails>>(),
                AdditionalEntitiesToCreate = new SortedDictionary<int, List<object>>()
            };

            using (var package = new ExcelPackage(fileContentBytes.ToMemoryStream()))
            {
                var workSheet = package.Workbook.Worksheets[1];
                var ignoreColumns = new List<string>()
                {
                    "ErstelltAm"
                };

                var mapping = new Dictionary<string, string>()
                {
                    {"AdressNrADR", "ClubRelatedPersonDetails.MemberNumber"},
                    {"Adresszeile1", "AddressLine2"},
                    {"EMail", "PrivateEmail"},
                    {"Fax", "FaxNumber"},
                    {"GeburtsDatum", "Birthday"},
                    {"LandPRO", "CountryCode"},
                    {"Natel", "MobilePhoneNumber"},
                    {"Name", "Lastname"},
                    {"Ort", "City"},
                    {"PLZ", "ZipCode"},
                    {"KantonPRO", "Region"},
                    {"Strasse", "AddressLine1"},
                    {"TelDir", "BusinessPhoneNumber"},
                    {"TelPrivat", "PrivatePhoneNumber"},
                    {"TelZentrale", "BusinessPhoneNumber"},
                    {"Vorname", "Firstname"},
                    {"AdressGruppen", "AddressCategories" }
                };

                var entityList = workSheet.ToList<ImportPersonDetails>(ignoreColumns, mapping);

                //re-map temporary mappings for country
                mapping["LandPRO"] = "CountryId";
                var mappedProperties = mapping.Values.ToList();

                try
                {
                    var persons = new List<Person>();

                    using (var context = _dataAccessService.CreateDbContext())
                    {
                        persons = context.Persons.Include(Constants.Country)
                            .Include(Constants.PersonPersonCategories)
                            .Include(Constants.PersonPersonCategories + ".PersonCategory")
                            .Include(Constants.PersonClubs)
                            .Include($"{Constants.PersonClubs}.MemberState")
                            .OrderBy(pe => pe.Lastname).ToList();

                        var countries = context.Countries.ToList();
                        var personCategories = context.PersonCategories.ToList();
                        var countryIdCh = countries.First(x => x.CountryCodeIso2 == "CH").CountryId;

                        foreach (var personDetail in entityList)
                        {
                            var importObj = new ImportObject<PersonDetails>()
                            {
                                ImportDataRecord = personDetail
                            };

                            importJob.ImportObjects.Add(importObj);

                            if (string.IsNullOrWhiteSpace(personDetail.CountryCode) == false)
                            {
                                var country =
                                    countries.FirstOrDefault(
                                        x => x.CountryCodeIso2.ToUpper() == personDetail.CountryCode.ToUpper());

                                if (country != null)
                                {
                                    personDetail.CountryId = country.CountryId;
                                }
                            }
                            else
                            {
                                personDetail.CountryId = countryIdCh;
                            }

                            if (string.IsNullOrWhiteSpace(personDetail.AddressCategories) == false)
                            {
                                string[] groups = personDetail.AddressCategories.Split(',');
                                foreach (var group in groups)
                                {
                                    if (string.IsNullOrWhiteSpace(group)) continue;

                                    var personCategory =
                                        personCategories.FirstOrDefault(x => x.CategoryName.ToLower() == group.ToLower());

                                    if (personCategory == null)
                                    {
                                        //create new category
                                        personCategory = new PersonCategory()
                                        {
                                            CategoryName = group,
                                            ClubId = CurrentAuthenticatedFLSUserClubId,
                                            PersonCategoryId = Guid.NewGuid()
                                        };

                                        personCategories.Add(personCategory);
                                        context.PersonCategories.Add(personCategory);

                                        if (personDetail.ClubRelatedPersonDetails == null)
                                            personDetail.ClubRelatedPersonDetails = new ClubRelatedPersonDetails();
                                            
                                        personDetail.ClubRelatedPersonDetails.PersonCategoryIds.Add(personCategory.PersonCategoryId);
                                    }
                                    else
                                    {
                                        if (personDetail.ClubRelatedPersonDetails == null)
                                            personDetail.ClubRelatedPersonDetails = new ClubRelatedPersonDetails();

                                        personDetail.ClubRelatedPersonDetails.PersonCategoryIds.Add(
                                            personCategory.PersonCategoryId);
                                    }
                                }
                            }

                            var validationResult = new List<ValidationResult>();
                            var validatorContext = new ValidationContext(personDetail);
                            if (Validator.TryValidateObject(personDetail, validatorContext, validationResult, true) == false)
                            {
                                importObj.ImportState = ImportState.ValidationError;
                                importObj.ErrorMessage = "";

                                foreach (var result in validationResult)
                                {
                                    var props = string.Join(",", result.MemberNames);
                                    importObj.ErrorMessage += $"Error: {result.ErrorMessage}, Properties: {props}";
                                }

                                importObj.HasError = true;
                                continue;
                            }
                            
                            if (string.IsNullOrWhiteSpace(personDetail.ZipCode))
                            {
                                importObj.ImportState = ImportState.ImportError;
                                importObj.ErrorMessage = "ZipCode is empty";
                                importObj.HasError = true;
                                continue;
                            }

                            try
                            {
                                //entityList.FindDuplicates(x => x.Lastname.ToLower() && x.Firstname.ToLower());

                                var matchedPersons =
                                    persons.FindAll(x => x.Lastname.ToLower() == personDetail.Lastname.ToLower()
                                                         && x.Firstname.ToLower() == personDetail.Firstname.ToLower());

                                if (matchedPersons.Count == 0)
                                {
                                    importObj.ImportState = ImportState.ImportedSuccessfully;
                                    var newRecord = importObj.ImportDataRecord.ToPerson(CurrentAuthenticatedFLSUserClubId, mappedProperties);
                                    context.Persons.Add(newRecord);
                                }
                                else if (matchedPersons.Count == 1)
                                {
                                    if (string.IsNullOrWhiteSpace(matchedPersons[0].EmailPrivate) == false
                                        && matchedPersons[0].EmailPrivate == personDetail.PrivateEmail)
                                    {
                                        //update record
                                        importObj.ImportState = ImportState.UpdatedSuccessfully;
                                        importObj.ServerDataRecord = matchedPersons[0].ToPersonDetails(CurrentAuthenticatedFLSUserClubId);
                                        importObj.ImportDataRecord.ToPerson(CurrentAuthenticatedFLSUserClubId, mappedProperties, matchedPersons[0]);

                                    }
                                    else if (string.IsNullOrWhiteSpace(matchedPersons[0].Zip) == false
                                        && matchedPersons[0].Zip != personDetail.ZipCode)
                                    {
                                        // datarecords does not match with ZipCode --> create new record
                                        importObj.ImportState = ImportState.ImportedSuccessfully;
                                        var newRecord =
                                            importObj.ImportDataRecord.ToPerson(CurrentAuthenticatedFLSUserClubId,
                                                mappedProperties);
                                        context.Persons.Add(newRecord);
                                    }
                                    else
                                    {
                                        importObj.ImportState = ImportState.UpdatedSuccessfully;
                                        importObj.ServerDataRecord = matchedPersons[0].ToPersonDetails(CurrentAuthenticatedFLSUserClubId);
                                        importObj.ImportDataRecord.ToPerson(CurrentAuthenticatedFLSUserClubId, mappedProperties, matchedPersons[0]);
                                    }
                                }
                                else
                                {
                                    var matchedPersonsZip = matchedPersons.FindAll(x => x.Zip == personDetail.ZipCode);

                                    if (matchedPersonsZip.Count == 0)
                                    {
                                        importObj.ImportState = ImportState.ImportedSuccessfully;
                                        var newRecord = importObj.ImportDataRecord.ToPerson(CurrentAuthenticatedFLSUserClubId, mappedProperties);
                                        context.Persons.Add(newRecord);
                                    }
                                    else if (matchedPersonsZip.Count == 1)
                                    {
                                        importObj.ImportState = ImportState.UpdatedSuccessfully;
                                        importObj.ServerDataRecord = matchedPersonsZip[0].ToPersonDetails(CurrentAuthenticatedFLSUserClubId);
                                        importObj.ImportDataRecord.ToPerson(CurrentAuthenticatedFLSUserClubId, mappedProperties, matchedPersonsZip[0]);
                                    }
                                    else
                                    {
                                        //TODO: Search also for MemberNumber
                                        importObj.ImportState = ImportState.Duplicate;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                if (importObj.ImportState == ImportState.UpdatedSuccessfully)
                                    importObj.ImportState = ImportState.UpdateError;
                                else
                                    importObj.ImportState = ImportState.ImportError;

                                importObj.ErrorMessage = ex.Message;
                                importObj.HasError = true;
                                Logger.Trace(ex, $"Error while trying to import PersonDetails: {importObj.ImportDataRecord}");
                            }
                        }

                        if (context.ChangeTracker.HasChanges())
                        {
                            context.GetValidationErrors();
                            context.SaveChanges();
                        }
                    }
                }
                catch (Exception exception)
                {
                    Logger.Error(exception, "Error while trying to import person excel list");
                }
            }

            return importJob;
        }
    }
}
