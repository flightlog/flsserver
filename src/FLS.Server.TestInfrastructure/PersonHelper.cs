using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Data.WebApi.Club;
using FLS.Data.WebApi.Person;
using FLS.Server.Data;
using FLS.Server.Data.DbEntities;
using FLS.Server.Service;
using FLS.Server.TestInfrastructure.Extensions;
using Foundation.ObjectHydrator;

namespace FLS.Server.TestInfrastructure
{
    public class PersonHelper : BaseHelper
    {
        public PersonHelper(DataAccessService dataAccessService, IdentityService identityService)
            : base(dataAccessService, identityService)
        {
        }

        public PilotPersonDetails CreatePersonDetails(Guid clubId, Guid countryId)
        {
            var hydrator = new Hydrator<PilotPersonDetails>();
            var personDetails = hydrator.GetSingle();

            personDetails.CountryId = countryId;
            personDetails.PersonId = Guid.Empty;
            if (personDetails.Lastname.Length > 80) personDetails.Lastname = personDetails.Lastname.Substring(0, 80);
            personDetails.Lastname = personDetails.Lastname + DateTime.Now.Ticks;

            var ownClubData = new ClubRelatedPersonDetails
            {
                ClubId = clubId,
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
                ClubId = clubId,
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

        public Person GetFirstGliderPilotPerson(Guid clubId)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.Persons.FirstOrDefault(q => q.PersonClubs.Any(pc => pc.ClubId == clubId) && q.HasGliderPilotLicence);
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
    }
}
