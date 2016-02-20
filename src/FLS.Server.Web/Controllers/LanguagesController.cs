using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using FLS.Data.WebApi.Globalization;
using FLS.Server.Service;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for language entities.
    /// </summary>
    [RoutePrefix("api/v1/languages")]
    public class LanguagesController : ApiController
    {
        private readonly LanguageService _languageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguagesController"/> class.
        /// </summary>
        public LanguagesController(LanguageService languageService)
        {
            _languageService = languageService;
        }

        /// <summary>
        /// Gets the language overviews.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Route("listitems")]
        [ResponseType(typeof(List<LanguageListItem>))]
        public IHttpActionResult GetLanguageListItems()
        {
            var languages = _languageService.GetLanguageListItems();
            return Ok(languages);
        }
    }
}
