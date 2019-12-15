using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FLS.Common.Converters;
using FLS.Server.Data.Objects.Aircraft;
using FLS.Server.Service.Accounting;
using FLS.Server.Service.Extensions;
using Newtonsoft.Json;
using NLog;

namespace FLS.Server.Service.Jobs
{
    /// <summary>
    /// The job for the quartz.net scheduler (http://quartznet.sourceforge.net/) which sends the monthly reports to the club.
    /// Every time, the scheduler execute this job a new instance of this class is created.
    /// </summary>
    public class AircraftDatabaseSyncJob : IJob
    {
        private readonly AircraftService _aircraftService;
        private readonly DataAccessService _dataAccessService;
        private Logger _logger = LogManager.GetCurrentClassLogger();

        protected Logger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }

        public AircraftDatabaseSyncJob(AircraftService aircraftService, DataAccessService dataAccessService)
        {
            _aircraftService = aircraftService;
            _dataAccessService = dataAccessService;
        }

        /// <summary>
        /// Every time when the scheduler executes a job this method is called.
        /// </summary>
        public void Execute()
        {
            try
            {
                Logger.Info("Executing Aircraft database sync job");

                ProcessSynchronisation();

                Logger.Info("Executed Aircraft database sync job.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error while executing Aircraft database sync job. Error: {ex.Message}");
            }
        }

        private void ProcessSynchronisation()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var ddbTask = Task.Run(async () => await httpClient.GetAsync("http://ddb.glidernet.org/download?j=1"));

                    if (ddbTask.Result.IsSuccessStatusCode)
                    {
                        var ddbContent = ddbTask.Result.Content.ReadAsStringAsync();
                        var ognDevices = JsonConvert.DeserializeObject<OgnDevices>(ddbContent.Result, new JsonBooleanConverter());

                        if (ognDevices == null || ognDevices.Devices == null || ognDevices.Devices.Any() == false)
                        {
                            Logger.Info("No devices collected from OGN database! Check the OGN DDB or the connection to it.");
                        }

                        using (var context = _dataAccessService.CreateDbContext())
                        {
                            var aircrafts = context.Aircrafts
                                .OrderBy(a => a.Immatriculation)
                                .ToList();

                            foreach (var aircraft in aircrafts)
                            {
                                var immatriculation = aircraft.Immatriculation.Replace("-", "").ToUpper();

                                var ognDevice = ognDevices.Devices.FirstOrDefault(x =>
                                    x.Registration.Replace("-", "").ToUpper() == immatriculation);

                                if (ognDevice == null)
                                {
                                    Logger.Info($"Aircraft {aircraft.Immatriculation} not found in OGN database.");
                                    continue;
                                }

                                if (ognDevice.IsTracked == false || ognDevice.IsIdentified == false)
                                {
                                    Logger.Info($"Aircraft {aircraft.Immatriculation} is tracked = {ognDevice.IsTracked} and is identified = {ognDevice.IsIdentified}.");
                                }

                                //update data within FLS database entries
                                aircraft.FLARMId = ognDevice.DeviceId;
                                aircraft.AircraftModel = ognDevice.AircraftModel;
                                aircraft.CompetitionSign = ognDevice.Cn;
                            }

                            context.SaveChanges();
                        }
                    }
                    
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error while processing synchronisation. Error: {ex.Message}");
            }
        }
        
    }
}