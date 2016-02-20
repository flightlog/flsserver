using System;
using FLS.Data.WebApi.Invoicing;
using FLS.Server.Service.Exporting;
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
        private Logger _logger = LogManager.GetCurrentClassLogger();

        protected Logger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }

        public InvoiceReportJob(InvoiceService invoiceService, UserService userService, IdentityService identityService)
        {
            _invoiceService = invoiceService;
            _userService = userService;
            _identityService = identityService;
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

                //TODO: remove user logic. 
                var user = _userService.GetUser("workflow");

                _identityService.SetUser(user);
                var invoices = _invoiceService.GetFlightInvoiceDetails(fromDate, toDate);
                ExcelExporter.ExportInvoicesToExcel(invoices, @"c:\temp\invoices\");

                foreach (var invoice in invoices)
                {
                    var flightInvoiceBooking = new FlightInvoiceBooking
                        {
                            FlightId = invoice.FlightId,
                            IncludesTowFlightId = invoice.IncludesTowFlightId,
                            InvoiceDate = DateTime.Now.Date,
                            InvoiceNumber = $"Workflow {DateTime.Now.ToShortTimeString()}"
                    };

                    _invoiceService.SetFlightAsInvoiced(flightInvoiceBooking);
                }

                Logger.Info("Flights invoiced and exported to excel.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error while processing flights for invoicing. Error: {ex.Message}");
            }
        }
        
    }
}