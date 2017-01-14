using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using FLS.Data.WebApi.Flight;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
using FLS.Server.Service;
using FLS.Server.TestInfrastructure;
using FLS.Server.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using FLS.Common.Extensions;
using FLS.Common.Paging;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Accounting;
using FLS.Data.WebApi.Accounting.Testing;
using FLS.Data.WebApi.AircraftReservation;
using FLS.Data.WebApi.PlanningDay;
using NinjaNye.SearchExtensions;

namespace FLS.Server.Tests.ServiceTests
{
    [TestClass]
    public class PlanningDayServiceTest : BaseTest
    {
        [TestMethod]
        [TestCategory("Service")]
        public void PlanningDayOverviewTest()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var planningDays = PlanningDayService.GetPlanningDayOverview(new DateTime(2000, 1, 1));
            sw.Stop();
            planningDays.ForEach(p => Logger.Debug($"{p.ToString()}"));
            Logger.Debug($"GetPlanningDayOverview takes: {sw.ElapsedMilliseconds} ms");

            sw = new Stopwatch();
            sw.Start();
            var planningDaysNew = PlanningDayService.GetPlanningDayOverviewWithDirectSelection(new DateTime(2000, 1, 1));
            sw.Stop();
            planningDaysNew.ForEach(p => Logger.Debug($"{p.ToString()}"));
            Logger.Debug($"GetPlanningDayOverviewNew takes: {sw.ElapsedMilliseconds} ms");

            var planningDayOverview = planningDaysNew.First();
            var planningDayDetails = PlanningDayService.GetPlanningDayDetails(planningDayOverview.PlanningDayId);
            var reservation = new AircraftReservationDetails();
            reservation.AircraftId = GetFirstAircraft().AircraftId;
            reservation.IsAllDayReservation = true;
            reservation.Start = planningDayDetails.Day;
            reservation.LocationId = planningDayDetails.LocationId;
            reservation.PilotPersonId = GetFirstPerson(CurrentIdentityUser.ClubId).PersonId;
            reservation.ReservationTypeId = 1;
            AircraftReservationService.InsertAircraftReservationDetails(reservation);

            sw = new Stopwatch();
            sw.Start();
            var planningDays2 = PlanningDayService.GetPlanningDayOverview(new DateTime(2000, 1, 1));
            sw.Stop();
            planningDays2.ForEach(p => Logger.Debug($"{p.ToString()}"));
            Logger.Debug($"GetPlanningDayOverview takes: {sw.ElapsedMilliseconds} ms");

            sw = new Stopwatch();
            sw.Start();
            var planningDaysNew2 = PlanningDayService.GetPlanningDayOverviewWithDirectSelection(new DateTime(2000, 1, 1));
            sw.Stop();
            planningDaysNew2.ForEach(p => Logger.Debug($"{p.ToString()}"));
            Logger.Debug($"GetPlanningDayOverviewNew takes: {sw.ElapsedMilliseconds} ms");

        }

        [TestMethod]
        [TestCategory("Service")]
        public void PagedPlanningDayOverviewTest()
        {
            var overview = PlanningDayService.GetPlanningDayOverview();
            overview.ForEach(p => Logger.Debug($"{p.ToString()}"));

            Logger.Debug("Creating filter for test");
            var filter = new PageableSearchFilter<PlanningDayOverviewSearchFilter>();
            filter.SearchFilter = new PlanningDayOverviewSearchFilter();
            filter.Sorting = new Dictionary<string, string>();
            filter.Sorting.Add("FlightOperatorName", "asc");
            filter.Sorting.Add("Day", "desc");
            filter.SearchFilter.OnlyPlanningDaysInFuture = true;
            filter.SearchFilter.NumberOfAircraftReservations = "0";
            var day = DateTime.Now.AddDays(1).AddYears(-1);
            filter.SearchFilter.Day = $"{day.Day}.{day.Month}";

            Logger.Debug($"Filter: {filter}");

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var planningDays = PlanningDayService.GetPagedPlanningDayOverview(1, 100, filter);
            sw.Stop();
            planningDays.Items.ForEach(p => Logger.Debug($"{p.ToString()}"));
            Logger.Debug($"GetPlanningDayOverview takes: {sw.ElapsedMilliseconds} ms");
        }

        [TestMethod]
        [TestCategory("Service")]
        public void DateTimeQueryTest()
        {
            var overview = PlanningDayService.GetPlanningDayOverview();
            overview.ForEach(p => p.Day = new DateTime(2017, 1, 15));
            overview.ForEach(p => Logger.Debug($"{p.ToString()}"));

            Logger.Debug("Creating filter for test");
            var pageableSearchFilter = new PageableSearchFilter<PlanningDayOverviewSearchFilter>();
            pageableSearchFilter.SearchFilter = new PlanningDayOverviewSearchFilter();
            pageableSearchFilter.Sorting = new Dictionary<string, string>();
            pageableSearchFilter.Sorting.Add("FlightOperatorName", "asc");
            pageableSearchFilter.Sorting.Add("Day", "desc");
            pageableSearchFilter.SearchFilter.OnlyPlanningDaysInFuture = true;

            pageableSearchFilter.SearchFilter.Day = "15.1.";

            Logger.Debug($"Filter: {pageableSearchFilter}");

            var filter = pageableSearchFilter.SearchFilter;

            var planningDays = overview.AsQueryable().OrderByPropertyNames(pageableSearchFilter.Sorting);
            //var searchResult = overview.Search(y => y.Day.ToString("dd.MM.yyyy")).Containing(filter.Day.ToLower());


            planningDays = planningDays.WhereIf(filter.Day,
                x => x.Day.DateContainsSearchText(filter.Day));

            var pagedQuery = new PagedQuery<PlanningDayOverview>(planningDays, 1, 100);

            var result = pagedQuery.Items.ToList();
        }


    }
}
