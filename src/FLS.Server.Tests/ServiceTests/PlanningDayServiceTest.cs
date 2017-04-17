using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        public void PlanningDayTest()
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                //DECLARE @clubId as uniqueidentifier
                //SET @clubId = (SELECT TOP 1 ClubId FROM Clubs WHERE ClubKey = 'FGZO')

                //SELECT PlanningDays.PlanningDayId, PlanningDays.Day, Locations.LocationName, PlanningDays.Remarks, 
                //				Persons.Lastname, Persons.Firstname, 
                //				PlanningDayAssignmentTypes.AssignmentTypeName,                          
                //						 COUNT(AircraftReservations.AircraftId) AS NrOfReservations
                //FROM PlanningDayAssignmentTypes INNER JOIN
                //                         PlanningDayAssignments ON PlanningDayAssignmentTypes.PlanningDayAssignmentTypeId = PlanningDayAssignments.AssignmentTypeId INNER JOIN
                //                         PlanningDays ON PlanningDayAssignments.AssignedPlanningDayId = PlanningDays.PlanningDayId INNER JOIN
                //                         Persons ON PlanningDayAssignments.AssignedPersonId = Persons.PersonId INNER JOIN
                //                         Locations ON PlanningDays.LocationId = Locations.LocationId

                //                         LEFT JOIN AircraftReservations ON AircraftReservations.ClubId = @clubId

                //                            AND AircraftReservations.LocationId = PlanningDays.LocationId

                //                            AND CAST(AircraftReservations.Start AS date) = PlanningDays.Day

                //where PlanningDays.ClubId = @clubId
                //and PlanningDays.Day < '2017-01-01'
                //GROUP by PlanningDays.PlanningDayId, PlanningDays.Day, Locations.LocationName, PlanningDays.Remarks, 
                //				Persons.Lastname, Persons.Firstname, 
                //				PlanningDayAssignmentTypes.AssignmentTypeName
                //HAVING COUNT(AircraftReservations.AircraftId) > 0

                //var planningDays = context.PlanningDays
                //    .Include("Location")
                //    .Include("PlanningDayAssignments")
                //    .Include("PlanningDayAssignments.AssignmentType")
                //    .Include("PlanningDayAssignments.AssignedPerson")
                //    .Where(q => q.ClubId == clubId && DbFunctions.TruncateTime(q.Day) >= fromDate.Date)
                //    .OrderBy(pe => pe.Day)
                //    .ToList();

                //var reservations = context.AircraftReservations
                //    .Include(Constants.Aircraft)
                //    .Include("PilotPerson")
                //    .Include("Location")
                //    .Include("InstructorPerson")
                //    .Include("ReservationType")
                //    .Where(r => r.ClubId == planningDay.ClubId
                //        && DbFunctions.TruncateTime(r.Start) == planningDay.Day.Date
                //        && r.LocationId == planningDay.LocationId)
                //    .OrderBy(pe => pe.Start)
                //    .ToList();

                var club = context.Clubs.FirstOrDefault(x => x.ClubStateId != 0); //select club which is not a system club

                var planningDays = context.PlanningDays
                    .Include("Location")
                    .Include("PlanningDayAssignments")
                    .Include("PlanningDayAssignments.AssignmentType")
                    .Include("PlanningDayAssignments.AssignedPerson")
                    .Where(x => x.ClubId == club.ClubId)
                    .Select(p => new PlanningDayOverview
                    {
                        Day = p.Day,
                        PlanningDayId = p.PlanningDayId,
                        LocationId = p.LocationId,
                        LocationName = p.Location.LocationName,
                        FlightOperatorName = p.PlanningDayAssignments.Where(x => x.AssignmentType.AssignmentTypeName.ToLower() == "segelflugleiter" && x.AssignedPerson != null).Select(ap => ap.AssignedPerson.Firstname + " " + ap.AssignedPerson.Lastname).FirstOrDefault(),
                        TowingPilotName = p.PlanningDayAssignments.Where(x => x.AssignmentType.AssignmentTypeName.ToLower() == "schlepppilot" && x.AssignedPerson != null).Select(ap => ap.AssignedPerson.Firstname + " " + ap.AssignedPerson.Lastname).FirstOrDefault(),
                        InstructorName = p.PlanningDayAssignments.Where(x => x.AssignmentType.AssignmentTypeName.ToLower() == "fluglehrer" && x.AssignedPerson != null).Select(ap => ap.AssignedPerson.Firstname + " " + ap.AssignedPerson.Lastname).FirstOrDefault()
                    }).ToList();

                var reservations = context.AircraftReservations.Where(r => r.ClubId == club.ClubId).GroupBy(r => new { r.LocationId, Day = DbFunctions.TruncateTime(r.Start) }).Select(r => new { r.Key, Count = r.Count() }).ToList();

                foreach (var planningDay in planningDays)
                {
                    var countInfo = reservations.FirstOrDefault(r => r.Key.Day == planningDay.Day.Date && r.Key.LocationId == planningDay.LocationId);
                    planningDay.NumberOfAircraftReservations = countInfo?.Count ?? 0;
                }

                planningDays.ForEach(x => Logger.Debug($"{x.ToString()}"));
            }
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
            //filter.SearchFilter.Day = $"{day.Day}.{day.Month}";

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

            pageableSearchFilter.SearchFilter.Day = new DateTimeFilter()
            {
                To = new DateTime(2017, 1, 15),
                From = new DateTime(2017, 1, 15)
            };

            Logger.Debug($"Filter: {pageableSearchFilter}");

            var filter = pageableSearchFilter.SearchFilter;

            var planningDays = overview.AsQueryable().OrderByPropertyNames(pageableSearchFilter.Sorting);
            //var searchResult = overview.Search(y => y.Day.ToString("dd.MM.yyyy")).Containing(filter.Day.ToLower());


            //planningDays = planningDays.WhereIf(filter.Day,
            //    x => x.Day.DateContainsSearchText(filter.Day));

            var pagedQuery = new PagedQuery<PlanningDayOverview>(planningDays, 1, 100);

            var result = pagedQuery.Items.ToList();
        }


    }
}
