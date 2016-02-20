using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using FLS.Data.WebApi.Location;
using FLS.Server.Service;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for countries
    /// </summary>
    //[Authorize]
    [RoutePrefix("api/v1/countries")]
    public class CountriesController : ApiController
    {
        private readonly LocationService _locationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountriesController"/> class.
        /// </summary>
        public CountriesController(LocationService locationService)
        {
            _locationService = locationService;
        }

        /// <summary>
        /// Gets the country list items.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Route("listitems")]
        [ResponseType(typeof(List<CountryListItem>))]
        public IHttpActionResult GetCountryListItems()
        {
            var locations = _locationService.GetCountryListItems();
            return Ok(locations);
        }

        /// <summary>
        /// Gets the country overviews.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("overview")]
        [ResponseType(typeof(List<CountryOverview>))]
        public IHttpActionResult GetCountryOverviews()
        {
            var locations = _locationService.GetCountryOverviews();
            return Ok(locations);
        }

    }
}
