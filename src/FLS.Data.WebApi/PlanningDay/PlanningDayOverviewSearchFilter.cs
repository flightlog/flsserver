using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace FLS.Data.WebApi.PlanningDay
{
    public class PlanningDayOverviewSearchFilter
    {
        public DateTimeFilter Day { get; set; }

        public string LocationName { get; set; }

        public string Remarks { get; set; }

        public string TowingPilotName { get; set; }

        public string FlightOperatorName { get; set; }

        public string InstructorName { get; set; }

        public string NumberOfAircraftReservations { get; set; }

        public bool OnlyPlanningDaysInFuture { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            Type type = GetType();
            sb.Append("[");
            sb.Append(type.Name);
            sb.Append(" -> ");
            foreach (FieldInfo info in type.GetFields())
                sb.Append(string.Format("{0}: {1}, ", info.Name, info.GetValue(this)));

            foreach (PropertyInfo info in type.GetProperties())
                sb.Append(string.Format("{0}: {1}, ", info.Name, info.GetValue(this, null)));
            sb.Append(" <- ");
            sb.Append(type.Name);
            sb.AppendLine("]");

            return sb.ToString();
        }
    }
}
