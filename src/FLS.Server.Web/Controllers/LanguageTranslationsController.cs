using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using FLS.Server.Service;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for language entities.
    /// </summary>
    [RoutePrefix("api/v1/translations")]
    public class LanguageTranslationsController : ApiController
    {
        private readonly LanguageService _languageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageTranslationsController"/> class.
        /// </summary>
        public LanguageTranslationsController(LanguageService languageService)
        {
            _languageService = languageService;
        }

        /// <summary>
        /// Gets the language overviews.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(Dictionary<string, string>))]
        public IHttpActionResult GetTranslation([FromUri] string lang)
        {
            var translation = _languageService.GetTranslation(lang);
            return Ok(translation);
        }
    }
}
