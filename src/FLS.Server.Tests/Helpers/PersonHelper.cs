using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Common.Validators;
using FLS.Data.WebApi.Person;
using FLS.Server.Data.DbEntities;
using FLS.Server.Service;
using FLS.Server.Tests.Extensions;
using Foundation.ObjectHydrator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests.Helpers
{
    public class PersonHelper : BaseHelper
    {
        public PersonHelper(DataAccessService dataAccessService, IdentityService identityService)
            : base(dataAccessService, identityService)
        {
        }
        
        public PilotPersonDetails CreateGliderPilotPersonDetails(Guid countryId)
        {
            var personDetails = new PilotPersonDetails()
            {
                CountryId = countryId,
                Lastname = "Huter-Pilot",
                Firstname = "Peter",
                AddressLine1 = "Segelflugstrasse 199",
                ZipCode = "1234",
                City = "Segelflug-City",
                Birthday = new DateTime(1965, 11, 13),
                BusinessEmail = "test@glider-fls.ch",
                HasGliderPilotLicence = true,
                HasGliderPassengerLicence = true,
                Region = "ZH",
                LicenceNumber = "CH.FCL.19999",
                MedicalLaplExpireDate = DateTime.Now.Date.AddMonths(4)
            };

            var ownClubData = new ClubRelatedPersonDetails
            {
                MemberNumber = "10000",
                IsGliderPilot = true,
            };

            personDetails.ClubRelatedPersonDetails = ownClubData;

            return personDetails;
        }

        public PilotPersonDetails CreateGliderInstructorPersonDetails(Guid countryId)
        {
            var personDetails = new PilotPersonDetails()
            {
                CountryId = countryId,
                Lastname = "Ingold-Instructor",
                Firstname = "Hansi",
                AddressLine1 = "Instruktorstrasse 25",
                ZipCode = "4321",
                City = "Instruktor-City",
                Birthday = new DateTime(1955, 4, 23),
                BusinessEmail = "test@glider-fls.ch",
                HasGliderPilotLicence = true,
                HasGliderPassengerLicence = true,
                HasGliderInstructorLicence = true,
                Region = "ZH",
                LicenceNumber = "CH.FCL.4567",
                GliderInstructorLicenceExpireDate = DateTime.Now.Date.AddMonths(24)
            };

            var ownClubData = new ClubRelatedPersonDetails
            {
                MemberNumber = "23456",
                IsGliderPilot = true,
                IsGliderInstructor = true,
            };

            personDetails.ClubRelatedPersonDetails = ownClubData;

            return personDetails;
        }

        public PilotPersonDetails CreateGliderTraineePersonDetails(Guid countryId)
        {
            var personDetails = new PilotPersonDetails()
            {
                CountryId = countryId,
                Lastname = "Müller-Schüler",
                Firstname = "Peter",
                AddressLine1 = "Schulstrasse 13",
                ZipCode = "6677",
                City = "Schulcity",
                BusinessEmail = "test@glider-fls.ch",
                HasGliderTraineeLicence = true,
                Region = "ZH",
                LicenceNumber = "CH.FCL.99388",
            };

            var ownClubData = new ClubRelatedPersonDetails
            {
                MemberNumber = "77777",
                IsGliderTrainee = true,
            };

            personDetails.ClubRelatedPersonDetails = ownClubData;

            return personDetails;
        }

        public PilotPersonDetails CreateTowPilotPersonDetails(Guid countryId)
        {
            var personDetails = new PilotPersonDetails()
            {
                CountryId = countryId,
                Lastname = "Müller-Tower",
                Firstname = "Heinz",
                AddressLine1 = "Towing-Strasse 100",
                ZipCode = "3434",
                City = "Towcity",
                BusinessEmail = "test@glider-fls.ch",
                HasTowPilotLicence = true,
                HasMotorPilotLicence = true,
                Region = "ZH",
                LicenceNumber = "CH.FCL.33445",
            };

            var ownClubData = new ClubRelatedPersonDetails
            {
                MemberNumber = "7855",
                IsTowPilot = true,
                IsMotorPilot = true,
            };

            personDetails.ClubRelatedPersonDetails = ownClubData;

            return personDetails;
        }

        public PilotPersonDetails CreateWinchOperatorPilotPersonDetails(Guid countryId)
        {
            var personDetails = new PilotPersonDetails()
            {
                CountryId = countryId,
                Lastname = "Huber-Winch-Pilot",
                Firstname = "Gunther",
                AddressLine1 = "Windstrasse 2",
                ZipCode = "9876",
                City = "Wind-City",
                BusinessEmail = "test@glider-fls.ch",
                HasGliderPilotLicence = true,
                HasGliderPassengerLicence = true,
                HasWinchOperatorLicence = true,
                Region = "ZH",
                LicenceNumber = "CH.FCL.34567",
            };

            var ownClubData = new ClubRelatedPersonDetails
            {
                MemberNumber = "87392",
                IsGliderPilot = true,
                IsWinchOperator = true,
            };

            personDetails.ClubRelatedPersonDetails = ownClubData;

            return personDetails;
        }

        public PilotPersonDetails CreatePersonDetails(Guid countryId)
        {
            var hydrator = new Hydrator<PilotPersonDetails>();
            var personDetails = hydrator.GetSingle();

            personDetails.CountryId = countryId;
            personDetails.PersonId = Guid.Empty;
            if (personDetails.Lastname.Length > 80) personDetails.Lastname = personDetails.Lastname.Substring(0, 80);
            personDetails.Lastname = personDetails.Lastname + DateTime.Now.Ticks;

            var ownClubData = new ClubRelatedPersonDetails
            {
                MemberNumber = DateTime.Now.Ticks.ToString()
            };

            personDetails.ClubRelatedPersonDetails = ownClubData;

            return personDetails;
        }

        public PilotPersonFullDetails CreatePersonFullDetails(Guid clubId, Guid countryId)
        {
            var hydrator = new Hydrator<PilotPersonFullDetails>();
            var personDetails = hydrator.GetSingle();

            personDetails.CountryId = countryId;
            personDetails.PersonId = Guid.Empty;
            if (personDetails.Lastname.Length > 80) personDetails.Lastname = personDetails.Lastname.Substring(0, 80);
            personDetails.Lastname = personDetails.Lastname + DateTime.Now.Ticks;

            var ownClubData = new ClubRelatedPersonDetails
            {
                MemberNumber = DateTime.Now.Ticks.ToString()
            };

            personDetails.ClubRelatedPersonDetails = ownClubData;
            personDetails.RemoveMetadataInfo();
            
            return personDetails;
        }

        public Person GetFirstPerson()
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.Persons.FirstOrDefault();
            }
        }

        public Person GetFirstPerson(Guid clubId)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.Persons.FirstOrDefault(q => q.PersonClubs.Any(pc => pc.ClubId == clubId));
            }
        }

        public Person GetFirstPassengerPerson(Guid clubId)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.Persons.FirstOrDefault(q => q.PersonClubs.Any(pc => pc.ClubId == clubId) && q.HasGliderPilotLicence == false && q.HasGliderTraineeLicence == false);
            }
        }

        public Person GetFirstGliderPilotPerson(Guid clubId)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.Persons.FirstOrDefault(q => q.PersonClubs.Any(pc => pc.ClubId == clubId) && q.HasGliderPilotLicence);
            }
        }

        public Person GetLastGliderPilotPerson(Guid clubId)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                var persons = context.Persons.Where(q => q.PersonClubs.Any(pc => pc.ClubId == clubId) && q.HasGliderPilotLicence).OrderBy(y => y.Lastname);
                return persons.FirstOrDefault();
            }
        }

        public Person GetFirstGliderTraineePerson(Guid clubId)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.Persons.FirstOrDefault(q => q.PersonClubs.Any(pc => pc.ClubId == clubId) && q.HasGliderTraineeLicence);
            }
        }

        public Person GetFirstTowingPilotPerson(Guid clubId)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.Persons.FirstOrDefault(q => q.PersonClubs.Any(pc => pc.ClubId == clubId) && q.HasTowPilotLicence);
            }
        }

        public Person GetFirstGliderInstructorPerson(Guid clubId)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.Persons.FirstOrDefault(q => q.PersonClubs.Any(pc => pc.ClubId == clubId) && q.HasGliderInstructorLicence);
            }
        }

        public List<Person> GetPersons(Guid clubId)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.Persons.Where(q => q.PersonClubs.Any(pc => pc.ClubId == clubId)).ToList();
            }
        }

        public Person GetDifferentPerson(Guid? personId)
        {
            if (personId == null) return GetFirstPerson();

            using (var context = DataAccessService.CreateDbContext())
            {
                foreach (var person in context.Persons)
                {
                    if (person.PersonId != personId)
                    {
                        return person;
                    }
                }
            }

            return null;
        }

        public void InsertPerson(Person person)
        {
            Assert.IsNotNull(person);

            using (var context = DataAccessService.CreateDbContext())
            {
                context.Persons.Add(person);
                context.SaveChanges();
            }
        }
    }
}
