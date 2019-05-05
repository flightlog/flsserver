using System;
using System.Net.Mail;
using FLS.Server.Interfaces;
using FLS.Server.Service.Accounting;
using FLS.Server.Service.Email;
using FLS.Server.Service.Exporting;
using FLS.Server.Service.Jobs;
using NLog;

namespace FLS.Server.Service
{
    public class WorkflowService : BaseService
    {
        private readonly AircraftReportEmailBuildService _aircraftReportEmailService;
        private readonly FlightService _flightService;
        private readonly ClubService _clubService;
        private readonly PlanningDayService _planningDayService;
        private readonly AircraftReservationService _aircraftReservationService;
        private readonly AircraftService _aircraftService;
        private readonly PlanningDayEmailBuildService _planningDayEmailService;
        private readonly PersonService _personService;
        private readonly DeliveryService _deliveryService;
        private readonly UserService _userService;
        private readonly EmailBuildService _emailService;
        private readonly FlightInformationEmailBuildService _flightInformationEmailService;
        private readonly LicenceExpireEmailBuildService _licenceExpireEmailBuildService;
        private readonly DataAccessService _dataAccessService;
        private readonly IdentityService _identityService;
        private readonly AccountingEmailBuildService _accountingEmailBuildService;
        private readonly ExcelExporter _deliveryExcelExporter;
        private readonly SettingService _settingService;

        public WorkflowService(FlightService flightService, 
            ClubService clubService, 
            PlanningDayService planningDayService,
            AircraftReservationService aircraftReservationService,
            AircraftService aircraftService,
            AircraftReportEmailBuildService aircraftReportEmailService,
            PlanningDayEmailBuildService planningDayEmailService,
            PersonService personService,
            DeliveryService deliveryService, 
            UserService userService, 
            EmailBuildService emailService,
            FlightInformationEmailBuildService flightInformationEmailService,
            LicenceExpireEmailBuildService licenceExpireEmailBuildService,
            DataAccessService dataAccessService, 
            IdentityService identityService,
            AccountingEmailBuildService accountingEmailBuildService,
            ExcelExporter deliveryExcelExporter,
            SettingService settingService)
            : base(dataAccessService, identityService)
        {
            _flightService = flightService;
            _clubService = clubService;
            _planningDayService = planningDayService;
            _aircraftReservationService = aircraftReservationService;
            _aircraftService = aircraftService;
            _aircraftReportEmailService = aircraftReportEmailService; 
            _planningDayEmailService = planningDayEmailService;
            _personService = personService;
            _deliveryService = deliveryService;
            _userService = userService;
            _emailService = emailService;
            _flightInformationEmailService = flightInformationEmailService;
            _licenceExpireEmailBuildService = licenceExpireEmailBuildService;
            _dataAccessService = dataAccessService;
            _identityService = identityService;
            _accountingEmailBuildService = accountingEmailBuildService;
            _deliveryExcelExporter = deliveryExcelExporter;
            _settingService = settingService;
            Logger = LogManager.GetCurrentClassLogger();
        }

        public void ExecuteDailyFlightValidationJob()
        {
            Logger.Info("Execute daily flight validation job.");
            var job = new DailyFlightValidationJob(_flightService, _clubService, _dataAccessService);
            job.Execute();
        }

        public void ExecuteDailyReportJob()
        {
            Logger.Info("Execute daily report job.");
            var job = new DailyReportJob(_flightService, _clubService, _personService, _flightInformationEmailService);
            job.Execute();
        }

        public void ExecuteAircraftStatisticReportJob()
        {
            Logger.Info("Execute monthly report job.");
            var job = new AircraftStatisticReportJob(_aircraftReportEmailService, _flightService, _aircraftService);
            job.Execute();
        }

        public void ExecutePlanningDayMailJob()
        {
            Logger.Info("Execute planning day mail job.");
            var job = new PlanningDayNotificationJob(_clubService, _planningDayService,
                _aircraftReservationService, _planningDayEmailService, _settingService);
            job.Execute();
        }

        public void ExecuteLicenceNotificationJob()
        {
            Logger.Info("Execute licence notification job.");
            var job = new LicenceNotificationJob(_licenceExpireEmailBuildService, _personService);
            job.Execute();
        }

        public void ExecuteWorkflows()
        {
            Logger.Info("Execute workflows...");
            var count = 0;
            //TODO: add settings to database
            if (DateTime.Now.Hour == 12)
            {
                count++;
                ExecutePlanningDayMailJob();
            }

            //TODO: add settings to database
            if (DateTime.Now.Hour == 22)
            {
                count++;
                ExecuteDailyFlightValidationJob();

                count++;
                ExecuteDailyReportJob();

                count++;
                ExecuteLicenceNotificationJob();

                count++;
                ExecuteDeliveryCreationJob();
            }

            //TODO: add settings to database
            if (DateTime.Now.Day == 2 && DateTime.Now.Hour == 23)
            {
                count++;
                ExecuteAircraftStatisticReportJob();

                count++;
                ExecuteDeliveryMailExportJob();
            }
            
            Logger.Info(string.Format("Executed {0} workflows.", count));
        }

        public void ExecuteTestMailJob()
        {
            Logger.Info("Execute test mail job.");
            try
            {
                var email = new MailMessage();
                email.To.Add("test@glider-fls.ch");
                email.Subject = "Test-Email";
                email.Body = "Test-Body";
                _emailService.SendEmail(email);
                Logger.Info(string.Format("Executed test mail job successfully to {0}.", email.To.ToString()));
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error while executing test mail: Message: {ex.Message}");
            }
        }

        public void ExecuteDeliveryMailExportJob()
        {
            Logger.Info("Execute delivery mail export job.");
            var job = new DeliveryMailExportJob(_deliveryService, _deliveryExcelExporter, _clubService, _accountingEmailBuildService);
            job.Execute();
        }

        public void ExecuteDeliveryCreationJob()
        {
            Logger.Info("Execute delivery creation job.");
            var job = new DeliveryCreationJob(_deliveryService, _clubService);
            job.Execute();
        }
    }
}
