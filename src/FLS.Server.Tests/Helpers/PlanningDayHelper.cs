using System;
using System.Linq;
using FLS.Data.WebApi.PlanningDay;
using FLS.Server.Data.DbEntities;
using FLS.Server.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests.Helpers
{
    public class PlanningDayHelper : BaseHelper
    {
        private readonly PlanningDayService _planningDayService;

        public PlanningDayHelper(PlanningDayService planningDayService, DataAccessService dataAccessService, IdentityService identityService)
            : base(dataAccessService, identityService)
        {
            _planningDayService = planningDayService;
        }

        public PlanningDayOverview GetFirstPlanningDayOverview()
        {
            var planningDays = _planningDayService.GetPlanningDayOverview(DateTime.MinValue);
            Assert.IsTrue(planningDays.Any());
            return planningDays.FirstOrDefault();
        }
    }
}
