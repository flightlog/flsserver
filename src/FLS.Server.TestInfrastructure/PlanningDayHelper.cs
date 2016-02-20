using System;
using System.Linq;
using FLS.Server.Data.DbEntities;
using FLS.Server.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.TestInfrastructure
{
    public class PlanningDayHelper : BaseHelper
    {
        private readonly PlanningDayService _planningDayService;

        public PlanningDayHelper(PlanningDayService planningDayService, DataAccessService dataAccessService, IdentityService identityService)
            : base(dataAccessService, identityService)
        {
            _planningDayService = planningDayService;
        }

        public PlanningDay GetFirstPlanningDay()
        {
            var planningDays = _planningDayService.GetPlanningDays(DateTime.MinValue);
            Assert.IsTrue(planningDays.Any());
            return planningDays.FirstOrDefault();
        }
    }
}
