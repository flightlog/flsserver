using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using FLS.Data.WebApi;
using FLS.Data.WebApi.AircraftReservation;
using FLS.Data.WebApi.Settings;
using FLS.Data.WebApi.Resources;
using FLS.Server.Service;

namespace FLS.Server.WebApi.Controllers
{
    /// <summary>
    /// Api controller for setting entities.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/settings")]
    public class SettingsController : ApiController
    {
        private readonly SettingService _settingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsController"/> class.
        /// </summary>
        public SettingsController(SettingService settingService)
        {
            _settingService = settingService;
        }
        
        /// <summary>
        /// Gets the setting details.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("page/{pageStart:int?}/{pageSize:int?}")]
        [ResponseType(typeof(PagedList<SettingDetails>))]
        public IHttpActionResult GetPagedSettingDetails([FromBody]PageableSearchFilter<SettingDetailsSearchFilter> pageableSearchFilter, int? pageStart = 1, int? pageSize = 100)
        {
            var settings = _settingService.GetPagedSettingDetails(pageStart, pageSize, pageableSearchFilter);
            return Ok(settings);
        }
        
        /// <summary>
        /// Inserts the specified setting details.
        /// </summary>
        /// <param name="settingDetails">The setting details.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(SettingDetails))]
        public IHttpActionResult InsertOrUpdate([FromBody] SettingDetails settingDetails)
        {
            _settingService.InsertOrUpdateSettingDetails(settingDetails);
            return Ok(settingDetails);
        }
        
        /// <summary>
        /// Deletes the specified setting identifier.
        /// </summary>
        /// <param name="searchFilter">The settings unique user/club key.</param>
        /// <returns></returns>
        [HttpDelete]
        [ResponseType(typeof(void))]
        public IHttpActionResult Delete([FromBody] SettingDetailsSearchFilter searchFilter)
        {
            _settingService.DeleteSetting(searchFilter.SettingKey, searchFilter.ClubId, searchFilter.UserId);
            return Ok();
        }

        /// <summary>
        /// Gets a specified settings value.
        /// </summary>
        /// <param name="searchFilter">The settings unique user/club key.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("key")]
        [ResponseType(typeof(string))]
        public IHttpActionResult GetValue([FromBody] SettingDetailsSearchFilter searchFilter)
        {
            var value = _settingService.GetSettingValue(searchFilter.SettingKey, searchFilter.ClubId, searchFilter.UserId);
            return Ok(value);
        }
    }
}
