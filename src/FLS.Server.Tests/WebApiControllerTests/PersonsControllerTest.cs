using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using FLS.Common.Extensions;
using FLS.Data.WebApi.Person;
using FLS.Server.Tests.Infrastructure.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests.WebApiControllerTests
{
    [TestClass]
    public class PersonsControllerTest : BaseAuthenticatedTests
    {
        [TestMethod]
        [TestCategory("WebApi")]
        public void GetPersonListItemsWebApiTest()
        {
            var response = GetAsync<IEnumerable<PersonListItem>>("/api/v1/persons/listitems").Result;

            Assert.IsTrue(response.Any());
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void GetPersonOverviewWebApiTest()
        {
            var response = GetAsync<IEnumerable<PersonOverview>>("/api/v1/persons/true").Result;

            Assert.IsTrue(response.Any());
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void GetPersonDetailsWebApiTest()
        {
            var response = GetAsync<IEnumerable<PersonListItem>>("/api/v1/persons/listitems").Result;

            Assert.IsTrue(response.Any());

            var id = response.First().PersonId;

            var result = GetAsync<PersonDetails>("/api/v1/persons/" + id).Result;

            Assert.AreEqual(id, result.Id);
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void InsertPersonDetailsWebApiTest()
        {
            var country = GetCountry("CH");
            var personDetails = CreatePersonDetails(country.CountryId);

            var response = PostAsync(personDetails, "/api/v1/persons").Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseDetails = ConvertToModel<PersonDetails>(response);
            Assert.IsTrue(responseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseDetails));
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void GetPersonDetailsByMemberNumberWebApiTest()
        {
            var country = GetCountry("CH");
            var personDetails = CreatePersonDetails(country.CountryId);

            var response = PostAsync(personDetails, "/api/v1/persons").Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseDetails = ConvertToModel<PersonDetails>(response);
            Assert.IsTrue(responseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseDetails));

            var personDetailsResponse = GetAsync<PersonDetails>("/api/v1/persons/membernumber/" + personDetails.ClubRelatedPersonDetails.MemberNumber).Result;

            Assert.IsNotNull(personDetailsResponse);

            Assert.AreEqual(personDetailsResponse.Id, responseDetails.Id);
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void GetPilotPersonFullDetailsWebApiTest()
        {
            InsertPersonFullDetailsWebApi();

            var response = GetAsync<IEnumerable<PersonListItem>>("/api/v1/persons/listitems").Result;

            Assert.IsTrue(response.Any());

            var id = response.First().PersonId;

            var result = GetAsync<PersonFullDetails>("/api/v1/persons/fulldetails/" + id).Result;

            Assert.AreEqual(id, result.Id);
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void InsertPersonFullDetailsWebApiTest()
        {
            InsertPersonFullDetailsWebApi();
        }

        private PersonFullDetails InsertPersonFullDetailsWebApi()
        {
            var country = GetCountry("CH");
            var personFullDetails = CreatePersonFullDetails(ClubId, country.CountryId);

            var timeStamp = DateTime.UtcNow;
            personFullDetails.CreatedOn = timeStamp;

            var response = PostAsync(personFullDetails, "/api/v1/persons/fulldetails").Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseDetails = ConvertToModel<PersonFullDetails>(response);
            Assert.IsTrue(responseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseDetails));
            Assert.IsTrue(responseDetails.CreatedByUserId == MyUserDetails.UserId);
            Assert.IsTrue(responseDetails.CreatedOn.SetAsUtc() == timeStamp.SetAsUtc());

            return responseDetails;
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void UpdatePersonDetailsWebApiTest()
        {
            var response = GetAsync<IEnumerable<PersonListItem>>("/api/v1/persons/listitems").Result;

            Assert.IsTrue(response.Any());

            var id = response.First().PersonId;

            var result = GetAsync<PersonDetails>("/api/v1/persons/" + id).Result;

            Assert.AreEqual(id, result.Id);

            result.AddressLine2 = "Updated on " + DateTime.Now.ToShortTimeString();

            var putResult = PutAsync(result, "/api/v1/persons/" + id).Result;

            Assert.IsTrue(putResult.IsSuccessStatusCode);
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void UpdatePersonClubRelatedDataDetailsWebApiTest()
        {
            var response = GetAsync<IEnumerable<PersonListItem>>("/api/v1/persons/listitems").Result;

            Assert.IsTrue(response.Any());

            var id = response.First().PersonId;

            var result = GetAsync<PersonDetails>("/api/v1/persons/" + id).Result;

            Assert.AreEqual(id, result.Id);

            Assert.IsNotNull(result.ClubRelatedPersonDetails);

            result.ClubRelatedPersonDetails.MemberNumber = result.ClubRelatedPersonDetails.MemberNumber + "1";

            result.ClubRelatedPersonDetails.IsGliderInstructor = true;

            var putResult = PutAsync(result, "/api/v1/persons/" + id).Result;

            Assert.IsTrue(putResult.IsSuccessStatusCode);

            var updatedPilotDetails = ConvertToModel<PersonDetails>(putResult);

            Assert.IsTrue(updatedPilotDetails.ClubRelatedPersonDetails.IsGliderInstructor);
            Assert.AreEqual(result.ClubRelatedPersonDetails.MemberNumber, updatedPilotDetails.ClubRelatedPersonDetails.MemberNumber, "MemberNumber does not match");
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void UpdatePersonFullDetailsWebApiTest()
        {
            var response = GetAsync<IEnumerable<PersonListItem>>("/api/v1/persons/listitems").Result;

            Assert.IsTrue(response.Any());

            var id = response.First().PersonId;

            var result = GetAsync<PersonFullDetails>("/api/v1/persons/fulldetails/" + id).Result;

            Assert.AreEqual(id, result.Id);

            result.AddressLine2 = "Updated on " + DateTime.Now.ToShortTimeString();
            var timeStamp = DateTime.UtcNow.AddMinutes(1);
            result.ModifiedOn = timeStamp;

            var putResult = PutAsync<PersonFullDetails>(result, "/api/v1/persons/fulldetails/" + id).Result;

            Assert.IsTrue(putResult.IsSuccessStatusCode);
            var personFullDetails = ConvertToModel<PersonFullDetails>(putResult);
            Assert.IsTrue(personFullDetails.ModifiedByUserId == MyUserDetails.UserId);
            Assert.IsTrue(personFullDetails.ModifiedOn.SetAsUtc() == timeStamp.SetAsUtc());
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void DeletePersonDetailsWebApiTest()
        {
            var response = GetAsync<IEnumerable<PersonOverview>>("/api/v1/persons").Result;

            Assert.IsTrue(response.Any());

            var id = response.First().PersonId;

            var delResult = DeleteAsync("/api/v1/persons/" + id).Result;

            Assert.IsTrue(delResult.IsSuccessStatusCode);
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void DeletePersonFullDetailsWebApiTest()
        {
            var personDetails = InsertPersonFullDetailsWebApi();
            var deletedOn = DateTime.UtcNow;
            var delResult = DeletePostAsync(deletedOn, "/api/v1/persons/fulldetails/" + personDetails.PersonId).Result;

            Assert.IsTrue(delResult.IsSuccessStatusCode);

            personDetails = InsertPersonFullDetailsWebApi();
            deletedOn = DateTime.UtcNow.AddDays(-10);
            delResult = DeletePostAsync(deletedOn, "/api/v1/persons/fulldetails/" + personDetails.PersonId).Result;

            Assert.IsTrue(delResult.IsSuccessStatusCode == false && delResult.StatusCode == HttpStatusCode.BadRequest);
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void SendAddressListAsExcelWebApiTest()
        {
            var response = GetAsync("/api/v1/persons/addresslist/excel/email").Result;
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        //TODO: Check logic of test and server side
        [TestMethod]
        [TestCategory("WebApi")]
        public void DeletedSincePersonFullDetailsWebApiTest()
        {
            var personDetails = InsertPersonFullDetailsWebApi();
            var deletedOn = DateTime.UtcNow;
            var delResult = DeletePostAsync(deletedOn, "/api/v1/persons/fulldetails/" + personDetails.PersonId).Result;

            Assert.IsTrue(delResult.IsSuccessStatusCode);

            var deletedRecords = GetAsync<IEnumerable<PersonFullDetails>>("/api/v1/persons/fulldetails/deleted/" + deletedOn.AddDays(-1).ToString("yyyy-MM-dd")).Result;

            Assert.IsTrue(deletedRecords.Any());
        }

        //TODO: Check logic of test and server side
        [Ignore]
        [TestMethod]
        [TestCategory("WebApi")]
        public void RecoverPersonFullDetailsWebApiTest()
        {
            var personDetails = InsertPersonFullDetailsWebApi();
            var deletedOn = DateTime.UtcNow;
            var delResult = DeletePostAsync(deletedOn, "/api/v1/persons/fulldetails/" + personDetails.PersonId).Result;

            Assert.IsTrue(delResult.IsSuccessStatusCode);

            personDetails.AddressLine2 = "Recovered on " + DateTime.Now.ToShortTimeString();
            var timeStamp = DateTime.Now;
            personDetails.ModifiedOn = timeStamp;

            var putResult = PutAsync<PersonFullDetails>(personDetails, "/api/v1/persons/fulldetails/" + personDetails.PersonId).Result;

            Assert.IsTrue(putResult.IsSuccessStatusCode);
            var personFullDetails = ConvertToModel<PersonFullDetails>(putResult);
            Assert.IsTrue(personFullDetails.ModifiedByUserId == MyUserDetails.UserId);
            Assert.IsTrue(personFullDetails.ModifiedOn.SetAsUtc() == timeStamp.SetAsUtc());
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void UploadPersonExcelFileWebApiTest()
        {
            var content = new MultipartFormDataContent();
            var fullfilename = @"C:\Projects\flsserver\FGZOAdressexportFLSformatted.xlsx";
            var filename = "FGZOAdressexportFLSformatted.xlsx";
            var filestream = new FileStream(fullfilename, FileMode.Open);
            var fileName = System.IO.Path.GetFileName(filename);
            content.Add(new StreamContent(filestream), "file", fileName);

            var result = PostFileAsync(content, "/api/v1/persons/upload").Result;

            Assert.IsTrue(result.IsSuccessStatusCode);
        }

        protected override string Uri
        {
            get { return RoutePrefix; }
        }

        protected override string RoutePrefix
        {
            get { return "/api/v1/persons"; }
        }
    }
}
