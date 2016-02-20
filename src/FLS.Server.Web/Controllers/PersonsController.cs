﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using FLS.Data.WebApi.Person;
using FLS.Server.Service;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for person entities.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/persons")]
    public class PersonsController : ApiController
    {
        private readonly PersonService _personService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonsController"/> class.
        /// </summary>
        public PersonsController(PersonService personService)
        {
            _personService = personService;
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Route("listitems")]
        [ResponseType(typeof(List<PilotPersonListItem>))]
        public IHttpActionResult GetPersonListItems()
        {
            var persons = _personService.GetPilotPersonListItems(true);
            return Ok(persons);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.)
        /// </summary>
        /// <param name="onlyClubRelatedPersons">if set to <c>true</c> only club related persons will be returned.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{onlyClubRelatedPersons:bool}")]
        [Route("listitems/{onlyClubRelatedPersons:bool}")]
        [ResponseType(typeof(List<PilotPersonListItem>))]
        public IHttpActionResult GetPersonListItems(bool onlyClubRelatedPersons)
        {
            var persons = _personService.GetPilotPersonListItems(onlyClubRelatedPersons);
            return Ok(persons);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedPilots">if set to <c>true</c> only club related pilots will be returned.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("gliderpilots/{onlyClubRelatedPilots:bool}")]
        [Route("gliderpilots/listitems/{onlyClubRelatedPilots:bool}")]
        [ResponseType(typeof(List<PilotPersonListItem>))]
        public IHttpActionResult GetGliderPilotPersonListItems(bool onlyClubRelatedPilots)
        {
            var persons = _personService.GetGliderPilotPersonListItems(onlyClubRelatedPilots);
            return Ok(persons);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedInstuctors">if set to <c>true</c> only club related instructors will be returned.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("gliderinstructors/{onlyClubRelatedInstuctors:bool}")]
        [Route("gliderinstructors/listitems/{onlyClubRelatedInstuctors:bool}")]
        [ResponseType(typeof(List<PilotPersonListItem>))]
        public IHttpActionResult GetGliderInstructorPersonListItems(bool onlyClubRelatedInstuctors)
        {
            var persons = _personService.GetGliderInstructorPersonListItems(onlyClubRelatedInstuctors);
            return Ok(persons);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedPilots">if set to <c>true</c> only club related pilots will be returned.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("towingpilots/{onlyClubRelatedPilots:bool}")]
        [Route("towingpilots/listitems/{onlyClubRelatedPilots:bool}")]
        [ResponseType(typeof(List<PilotPersonListItem>))]
        public IHttpActionResult GetTowingPilotPersonListItems(bool onlyClubRelatedPilots)
        {
            var persons = _personService.GetTowingPilotPersonListItems(onlyClubRelatedPilots);
            return Ok(persons);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedPilots">if set to <c>true</c> only club related pilots will be returned.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("motorpilots/{onlyClubRelatedPilots:bool}")]
        [Route("motorpilots/listitems/{onlyClubRelatedPilots:bool}")]
        [ResponseType(typeof(List<PilotPersonListItem>))]
        public IHttpActionResult GetMotorPilotPersonListItems(bool onlyClubRelatedPilots)
        {
            var persons = _personService.GetMotorPilotPersonListItems(onlyClubRelatedPilots);
            return Ok(persons);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedTrainees">if set to <c>true</c> only club related trainees will be returned.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("glidertrainees/{onlyClubRelatedTrainees:bool}")]
        [Route("glidertrainees/listitems/{onlyClubRelatedTrainees:bool}")]
        [ResponseType(typeof(List<PilotPersonListItem>))]
        public IHttpActionResult GetGliderTraineePersonListItems(bool onlyClubRelatedTrainees)
        {
            var persons = _personService.GetGliderTraineePersonListItems(onlyClubRelatedTrainees);
            return Ok(persons);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedWinchOperators">if set to <c>true</c> only club related winch operators will be returned.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("winchoperators/{onlyClubRelatedWinchOperators:bool}")]
        [Route("winchoperators/listitems/{onlyClubRelatedWinchOperators:bool}")]
        [ResponseType(typeof(List<PilotPersonListItem>))]
        public IHttpActionResult GetWinchOperatorPersonListItems(bool onlyClubRelatedWinchOperators)
        {
            var persons = _personService.GetWinchOperatorPersonListItems(onlyClubRelatedWinchOperators);
            return Ok(persons);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedPassengers">if set to <c>true</c> only club related passengers will be returned.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("passengers/{onlyClubRelatedPassengers:bool}")]
        [Route("passengers/listitems/{onlyClubRelatedPassengers:bool}")]
        [ResponseType(typeof(List<PassengerListItem>))]
        public IHttpActionResult GetPassengerListItems(bool onlyClubRelatedPassengers)
        {
            var persons = _personService.GetPassengerListItems(onlyClubRelatedPassengers);
            return Ok(persons);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedPersons">if set to <c>true</c> only club related persons will be returned.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("overview/{onlyClubRelatedPersons:bool}")]
        [ResponseType(typeof(List<PilotPersonOverview>))]
        public IHttpActionResult GetPersonOverviews(bool onlyClubRelatedPersons)
        {
            var persons = _personService.GetPilotPersonOverviews(onlyClubRelatedPersons);
            return Ok(persons);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedPilots">if set to <c>true</c> only club related pilots will be returned.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("gliderpilots/overview/{onlyClubRelatedPilots:bool}")]
        [ResponseType(typeof(List<PilotPersonOverview>))]
        public IHttpActionResult GetGliderPilotPersonOverviews(bool onlyClubRelatedPilots)
        {
            var persons = _personService.GetGliderPilotPersonOverviews(onlyClubRelatedPilots);
            return Ok(persons);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedPilots">if set to <c>true</c> only club related pilots will be returned.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("gliderobserverpilots/{onlyClubRelatedPilots:bool}")]
        [Route("gliderobserverpilots/listitems/{onlyClubRelatedPilots:bool}")]
        [ResponseType(typeof(List<PilotPersonListItem>))]
        public IHttpActionResult GetGliderObserverPilotPersonListItems(bool onlyClubRelatedPilots)
        {
            var persons = _personService.GetGliderObserverPilotPersonListItems(onlyClubRelatedPilots);
            return Ok(persons);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedPilots">if set to <c>true</c> only club related pilots will be returned.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("gliderobserverpilots/overview/{onlyClubRelatedPilots:bool}")]
        [ResponseType(typeof(List<PilotPersonOverview>))]
        public IHttpActionResult GetGliderObserverPilotPersonOverviews(bool onlyClubRelatedPilots)
        {
            var persons = _personService.GetGliderObserverPilotPersonOverviews(onlyClubRelatedPilots);
            return Ok(persons);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedInstuctors">if set to <c>true</c> only club related instructors will be returned.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("gliderinstructors/overview/{onlyClubRelatedInstuctors:bool}")]
        [ResponseType(typeof(List<PilotPersonOverview>))]
        public IHttpActionResult GetGliderInstructorPersonOverviews(bool onlyClubRelatedInstuctors)
        {
            var persons = _personService.GetGliderInstructorPersonOverviews(onlyClubRelatedInstuctors);
            return Ok(persons);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedPilots">if set to <c>true</c> only club related pilots will be returned.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("towingpilots/overview/{onlyClubRelatedPilots:bool}")]
        [ResponseType(typeof(List<PilotPersonOverview>))]
        public IHttpActionResult GetTowingPilotPersonOverviews(bool onlyClubRelatedPilots)
        {
            var persons = _personService.GetTowingPilotPersonOverviews(onlyClubRelatedPilots);
            return Ok(persons);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedTrainees">if set to <c>true</c> only club related trainees will be returned.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("glidertrainees/overview/{onlyClubRelatedTrainees:bool}")]
        [ResponseType(typeof(List<PilotPersonOverview>))]
        public IHttpActionResult GetGliderTraineePersonOverviews(bool onlyClubRelatedTrainees)
        {
            var persons = _personService.GetGliderTraineePilotPersonOverviews(onlyClubRelatedTrainees);
            return Ok(persons);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedWinchOperators">if set to <c>true</c> only club related winch operators will be returned.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("winchoperators/overview/{onlyClubRelatedWinchOperators:bool}")]
        [ResponseType(typeof(List<PilotPersonOverview>))]
        public IHttpActionResult GetWinchOperatorPersonOverviews(bool onlyClubRelatedWinchOperators)
        {
            var persons = _personService.GetWinchOperatorPilotPersonOverviews(onlyClubRelatedWinchOperators);
            return Ok(persons);
        }

        /// <summary>
        /// Get the persons ordered by lastname. If all persons will be queried but the user has no system admin rights
        /// only the main data of each person is be returned (no email, no phone numbers, etc.) 
        /// </summary>
        /// <param name="onlyClubRelatedPassengers">if set to <c>true</c> only club related passengers will be returned.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("passengers/overview/{onlyClubRelatedPassengers:bool}")]
        [ResponseType(typeof(List<PassengerOverview>))]
        public IHttpActionResult GetPassengerPersonOverviews(bool onlyClubRelatedPassengers)
        {
            var persons = _personService.GetPassengerOverviews(onlyClubRelatedPassengers);
            return Ok(persons);
        }

        /// <summary>
        /// Gets the person details.
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{personId}")]
        [ResponseType(typeof(PilotPersonDetails))]
        public IHttpActionResult GetPersonDetails(Guid personId)
        {
            var personDetails = _personService.GetPilotPersonDetails(personId);
            return Ok(personDetails);
        }

        /// <summary>
        /// Gets my person details.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("my")]
        [ResponseType(typeof(PilotPersonDetails))]
        public IHttpActionResult GetMyPersonDetails()
        {
            var personDetails = _personService.GetMyPersonDetails();
            return Ok(personDetails);
        }

        /// <summary>
        /// Gets the person full details.
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("fulldetails/{personId}")]
        [ResponseType(typeof(PilotPersonFullDetails))]
        public IHttpActionResult GetPersonFullDetails(Guid personId)
        {
            var personDetails = _personService.GetPilotPersonFullDetails(personId);
            return Ok(personDetails);
        }

        /// <summary>
        /// Gets the passenger details.
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("passengers/{personId}")]
        [ResponseType(typeof(PassengerDetails))]
        public IHttpActionResult GetPassengerDetails(Guid personId)
        {
            var personDetails = _personService.GetPassengerDetails(personId);
            return Ok(personDetails);
        }

        /// <summary>
        /// Gets the person details which are new modified since.
        /// </summary>
        /// <param name="modifiedSince">The modified since.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("modified/{modifiedSince:datetime:regex(\\d{4}-\\d{2}-\\d{2})}")]
        [Route("modified/{*modifiedSince:datetime:regex(\\d{4}/\\d{2}/\\d{2})}")]
        [ResponseType(typeof(List<PilotPersonDetails>))]
        public IHttpActionResult GetPersonDetailsModifiedSince(DateTime modifiedSince)
        {
            var personDetails = _personService.GetPersonDetailsModifiedSince(modifiedSince);
            return Ok(personDetails);
        }

        /// <summary>
        /// Gets the person details deleted since.
        /// </summary>
        /// <param name="deletedSince">The deleted since.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("deleted/{deletedSince:datetime:regex(\\d{4}-\\d{2}-\\d{2})}")]
        [Route("deleted/{*deletedSince:datetime:regex(\\d{4}/\\d{2}/\\d{2})}")]
        [ResponseType(typeof(List<PilotPersonDetails>))]
        public IHttpActionResult GetPersonDetailsDeletedSince(DateTime deletedSince)
        {
            var personDetails = _personService.GetPersonDetailsDeletedSince(deletedSince);
            return Ok(personDetails);
        }

        /// <summary>
        /// Gets the person details which are new modified since.
        /// </summary>
        /// <param name="modifiedSince">The modified since.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("fulldetails/modified/{modifiedSince:datetime:regex(\\d{4}-\\d{2}-\\d{2})}")]
        [Route("fulldetails/modified/{*modifiedSince:datetime:regex(\\d{4}/\\d{2}/\\d{2})}")]
        [ResponseType(typeof(List<PilotPersonFullDetails>))]
        public IHttpActionResult GetPersonFullDetailsModifiedSince(DateTime modifiedSince)
        {
            var personDetails = _personService.GetPersonFullDetailsModifiedSince(modifiedSince);
            return Ok(personDetails);
        }

        /// <summary>
        /// Gets the person details deleted since.
        /// </summary>
        /// <param name="deletedSince">The deleted since.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("fulldetails/deleted/{deletedSince:datetime:regex(\\d{4}-\\d{2}-\\d{2})}")]
        [Route("fulldetails/deleted/{*deletedSince:datetime:regex(\\d{4}/\\d{2}/\\d{2})}")]
        [ResponseType(typeof(List<PilotPersonFullDetails>))]
        public IHttpActionResult GetPersonFullDetailsDeletedSince(DateTime deletedSince)
        {
            var personDetails = _personService.GetPersonFullDetailsDeletedSince(deletedSince);
            return Ok(personDetails);
        }

        /// <summary>
        /// Inserts the person details.
        /// </summary>
        /// <param name="personDetails">The person details.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(PilotPersonDetails))]
        public IHttpActionResult InsertPersonDetails([FromBody] PilotPersonDetails personDetails)
        {
            _personService.InsertPersonDetails(personDetails);
            return Ok(personDetails);
        }

        /// <summary>
        /// Inserts the passenger details.
        /// </summary>
        /// <param name="passengerDetails">The passenger details.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("passengers")]
        [ResponseType(typeof(PassengerDetails))]
        public IHttpActionResult InsertPassengerDetails([FromBody] PassengerDetails passengerDetails)
        {
            _personService.InsertPassengerDetails(passengerDetails);
            return Ok(passengerDetails);
        }

        /// <summary>
        /// Inserts the person details.
        /// </summary>
        /// <param name="personFullDetails">The person full details.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("fulldetails")]
        [ResponseType(typeof(PilotPersonFullDetails))]
        public IHttpActionResult InsertPersonFullDetails([FromBody] PilotPersonFullDetails personFullDetails)
        {
            _personService.InsertPersonFullDetails(personFullDetails);
            return Ok(personFullDetails);
        }

        /// <summary>
        /// Updates the person details.
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <param name="personDetails">The person details.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{personId}")]
        [ResponseType(typeof(PilotPersonDetails))]
        public IHttpActionResult UpdatePersonDetails(Guid personId, [FromBody]PilotPersonDetails personDetails)
        {
            _personService.UpdatePersonDetails(personDetails);
            return Ok(personDetails);
        }

        /// <summary>
        /// Updates the passenger details.
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <param name="passengerDetails">The passenger details.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("passengers/{personId}")]
        [ResponseType(typeof(PassengerDetails))]
        public IHttpActionResult UpdatePassengerDetails(Guid personId, [FromBody]PassengerDetails passengerDetails)
        {
            _personService.UpdatePassengerDetails(passengerDetails);
            return Ok(passengerDetails);
        }

        /// <summary>
        /// Updates the person details.
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <param name="personFullDetails">The person full details.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("fulldetails/{personId}")]
        [ResponseType(typeof(PilotPersonFullDetails))]
        public IHttpActionResult UpdatePersonFullDetails(Guid personId, [FromBody]PilotPersonFullDetails personFullDetails)
        {
            _personService.UpdatePersonFullDetails(personFullDetails);
            return Ok(personFullDetails);
        }

        /// <summary>
        /// Deletes the specified person.
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{personId}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Delete(Guid personId)
        {
            _personService.DeletePerson(personId);
            return Ok();
        }

        /// <summary>
        /// Deletes the specified person identifier.
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <param name="deletedOn">The deleted timestamp.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("fulldetails/{personId}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult DeletePersonFullDetails(Guid personId, [FromBody]DateTime deletedOn)
        {
            _personService.DeletePersonFullDetails(personId, deletedOn);
            return Ok();
        }

        /// <summary>
        /// Sends address list of clubs pilots as excel to logged in user's notification email address 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("addresslist/excel/email")]
        [ResponseType(typeof(void))]
        public IHttpActionResult SendAddressListByEmail()
        {
            _personService.SendPersonListExcelToUsersEmailAddress();
            
            return Ok();
        }

        ///// <summary>
        ///// Gets address list of clubs pilots as excel
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("addresslist/excel")]
        //[ResponseType(typeof(void))]
        //public IHttpActionResult GetAddressList()
        //{
        //    return Ok();
        //    //var mediaType =
        //    //    MediaTypeHeaderValue.Parse("application/octet-stream");
        //    //var fileName = "Addresslist.xlsx";
        //    //var memoryStream = _personService.GetPersonListExcelPackageMemoryStream();
        //    //var response = Request.CreateResponse(HttpStatusCode.OK);
        //    //response.Content = new StreamContent(memoryStream);
        //    //response.Content.Headers.ContentType = mediaType;
        //    //response.Content.Headers.ContentDisposition =
        //    //    new ContentDispositionHeaderValue("fileName") { FileName = fileName };
        //    //return Ok(response);
        //}
    }
}
