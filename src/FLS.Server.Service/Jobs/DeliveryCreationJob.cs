using System;
using System.Linq;
using FLS.Server.Service.Accounting;
using NLog;

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
        public void Execute()
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
                var clubs = _clubService.GetClubs(false);

                foreach (var club in clubs.Where(c => c.RunDeliveryCreationJob))
                {
                    try
                    {
                        var deliveries = _deliveryService.CreateDeliveriesFromFlights(club.ClubId);
                        
                        Logger.Info($"{deliveries.Count} deliveries for flights of club {club.Clubname} created.");
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