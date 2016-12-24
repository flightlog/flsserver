using System;
using System.Linq;
using FLS.Data.WebApi.Invoicing;
using FLS.Server.Service.Email;
using FLS.Server.Service.Exporting;
using FLS.Server.Service.Invoicing;
using NLog;
using Quartz;

namespace FLS.Server.Service.Jobs
{
    /// <summary>
    /// The job for the quartz.net scheduler (http://quartznet.sourceforge.net/) which sends the monthly reports to the club.
    /// Every time, the scheduler execute this job a new instance of this class is created.
    /// </summary>
    public class InvoiceReportJob : IJob
    {
        private readonly InvoiceService _invoiceService;
        private readonly UserService _userService;
        private readonly IdentityService _identityService;
        private readonly ClubService _clubService;
        private readonly InvoiceEmailBuildService _emailBuildService;
        private Logger _logger = LogManager.GetCurrentClassLogger();

        protected Logger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }

        public InvoiceReportJob(InvoiceService invoiceService, UserService userService, IdentityService identityService, 
            ClubService clubService, InvoiceEmailBuildService emailBuildService)
        {
            _invoiceService = invoiceService;
            _userService = userService;
            _identityService = identityService;
            _clubService = clubService;
            _emailBuildService = emailBuildService;
        }

        /// <summary>
        /// Every time when the scheduler executes a job this method is called.
        /// </summary>
        /// <param name="context">not used</param>
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Logger.Info("Executing invoice report job");

                ProcessInvoices();

                Logger.Info("Executed invoice report job.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error while executing monthly workflow job. Error: {ex.Message}");
            }
        }

        private void ProcessInvoices()
        {
            try
            {
                //as the Job runs on 3rd each month, we have to catch the correct year by add some minus days (for january)
                var fromDate = new DateTime(DateTime.Now.AddDays(-10).Year, 1, 1);
                var toDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var clubs = _clubService.GetClubs(false);

                foreach (var club in clubs.Where(c => string.IsNullOrWhiteSpace(c.SendInvoiceReportsTo) == false))
                {
                    try
                    {
                        var invoices = _invoiceService.GetFlightInvoiceDetails(fromDate, toDate, club.ClubId);

                        if (invoices.Any() == false)
                        {
                            var noInvoiceMessage = _emailBuildService.CreateNoInvoiceEmail(club.SendInvoiceReportsTo, fromDate, toDate);
                            _emailBuildService.SendEmail(noInvoiceMessage);
                            Logger.Info($"No Flights to invoice. Sent no invoice email message to club: {club.Clubname}");
                            continue;
                        }

                        var zipBytes = ExcelExporter.ExportInvoicesToExcel(invoices);

                        var message = _emailBuildService.CreateInvoiceEmail(club.SendInvoiceReportsTo, zipBytes, fromDate, toDate);
                        _emailBuildService.SendEmail(message);

                        foreach (var invoice in invoices)
                        {
                            var flightInvoiceBooking = new FlightInvoiceBooking
                            {
                                FlightId = invoice.FlightId,
                                InvoiceDate = DateTime.Now.Date,
                                InvoiceNumber = $"Workflow {DateTime.Now.ToShortTimeString()}"
                            };

                            _invoiceService.SetFlightAsInvoiced(flightInvoiceBooking);
                        }

                        Logger.Info($"{invoices.Count} Flights invoiced and exported to excel.");
                    }
                    catch (Exception e)
                    {
                        Logger.Error(e, $"Error while processing flights for invoicing for club {club.Clubname}. Error: {e.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error while processing flights for invoicing. Error: {ex.Message}");
            }
        }
        
    }
}