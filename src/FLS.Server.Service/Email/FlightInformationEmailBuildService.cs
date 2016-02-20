using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using Alpinely.TownCrier;
using FLS.Common.Extensions;
using FLS.Common.Validators;
using FLS.Data.WebApi.Reporting;
using FLS.Server.Data.DbEntities;
using FLS.Server.Service.Email.Model;

namespace FLS.Server.Service.Email
{
    public class FlightInformationEmailBuildService : EmailBuildService
    {
        public FlightInformationEmailBuildService(DataAccessService dataAccessService, 
            IEmailSendService emailSendService, TemplateService templateService)
            : base(dataAccessService, emailSendService, templateService)
        {
            
        }

        /// <summary>
        /// Creates the flight report email which will be sent to all crew members of a flight
        /// which would like to have the flight report.
        /// </summary>
        /// <param name="todaysFlights">The todays flights.</param>
        /// <param name="person">The person.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        internal MailMessage CreateFlightReportEmail(List<Flight> todaysFlights, Person person)
        {
            todaysFlights.ArgumentNotNull("todaysFlights");
            person.ArgumentNotNull("person");

            var factory = new MergedEmailFactory(new VelocityTemplateParser("FlightInfoModel"));

            var flightInfoModel = new FlightInfoModel();
            flightInfoModel.RecipientName = person.Firstname;
            flightInfoModel.SenderName = SystemData.SystemSenderEmailAddress;
            flightInfoModel.Date = DateTime.Now.Date.ToShortDateString();

            flightInfoModel.FLSUrl = SystemData.BaseURL;
     
            List<FlightInfoRow> flights = new List<FlightInfoRow>();

            foreach (Flight flight in todaysFlights.OrderBy(f => f.StartDateTime))
            {
                var flightInfoRow = flight.ToFlightInfoRow();
                if (flightInfoRow != null) flights.Add(flightInfoRow);
            }

            flightInfoModel.Flights = flights.ToArray();
            string messageSubject = string.Empty;

            if (todaysFlights.Any())
            {
                var flightsFrom = todaysFlights.OrderBy(f => f.FlightDate).First().FlightDate;
                var flightsTo = todaysFlights.OrderByDescending(f => f.FlightDate).First().FlightDate;

                if (flightsFrom.HasValue && flightsTo.HasValue)
                {
                    if (Equals(flightsFrom, flightsTo) == false)
                    {
                        messageSubject = $"Flug-Informationen vom {flightsFrom.Value.ToShortDateString()} bis {flightsTo.Value.ToShortDateString()}";
                        flightInfoModel.Date = $"{flightsFrom.Value.ToShortDateString()} bis {flightsTo.Value.ToShortDateString()}";
                    }
                    else
                    {
                        messageSubject = $"Flug-Informationen vom {flightsFrom.Value.ToShortDateString()}";
                        flightInfoModel.Date = flightsFrom.Value.ToShortDateString();
                    }
                }
                else
                {
                    messageSubject = $"Flug-Informationen vom {DateTime.Now.ToShortDateString()}";
                    flightInfoModel.Date = DateTime.Now.ToShortDateString();
                }
            }

            var tokenValues = new Dictionary<string, object>
                {
                    {"FlightInfoModel", flightInfoModel}
                };

            return base.BuildEmail("flightreport", factory, tokenValues, messageSubject, person.EmailAddressForCommunication.SanitizeEmailAddress());
        }

    }
}
