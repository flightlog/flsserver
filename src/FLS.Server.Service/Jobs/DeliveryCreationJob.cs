using System;
using System.Linq;
using FLS.Server.Service.Accounting;
using NLog;
using Quartz;

namespace FLS.Server.Service.Jobs
{
    /// <summary>
    /// The job for the quartz.net scheduler (http://quartznet.sourceforge.net/) which sends the monthly reports to the club.
    /// Every time, the scheduler execute this job a new instance of this class is created.
    /// </summary>
    public class DeliveryCreationJob : IJob
    {
        private readonly DeliveryService _deliveryService;
        private readonly ClubService _clubService;
        private Logger _logger = LogManager.GetCurrentClassLogger();

        protected Logger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }

        public DeliveryCreationJob(DeliveryService deliveryService, ClubService clubService)
        {
            _deliveryService = deliveryService;
            _clubService = clubService;
        }

        /// <summary>
        /// Every time when the scheduler executes a job this method is called.
        /// </summary>
        /// <param name="context">not used</param>
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Logger.Info("Executing Delivery creation job");

                ProcessDeliveries();

                Logger.Info("Executed Delivery creation job.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error while executing delivery creation job. Error: {ex.Message}");
            }
        }

        private void ProcessDeliveries()
        {
            try
            {
                //as the Job runs on 3rd each month, we have to catch the correct year by add some minus days (for january)
                var fromDate = new DateTime(DateTime.Now.AddDays(-10).Year, 1, 1);
                var toDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var clubs = _clubService.GetClubs(false);

                foreach (var club in clubs.Where(c => c.RunDeliveryCreationJob))
                {
                    try
                    {
                        var invoices = _deliveryService.CreateDeliveriesFromFlights(fromDate, toDate, club.ClubId);
                        
                        Logger.Info($"{invoices.Count} deliveries for flights created.");
                    }
                    catch (Exception e)
                    {
                        Logger.Error(e, $"Error while processing flights for delivery creation for club {club.Clubname}. Error: {e.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error while processing flights for delivery creation. Error: {ex.Message}");
            }
        }
        
    }
}