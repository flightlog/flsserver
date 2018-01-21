using System;
using FLS.Server.Data.DbEntities;
using FLS.Server.Service.Email;
using NLog;
using Quartz;

namespace FLS.Server.Service.Jobs
{
    public class LicenceNotificationJob : IJob
    {
        private readonly LicenceExpireEmailBuildService _licenceExpireEmailBuildService;
        private readonly PersonService _personService;
        private Logger _logger = LogManager.GetCurrentClassLogger();

        protected Logger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }

        public LicenceNotificationJob(LicenceExpireEmailBuildService licenceExpireEmailBuildService, 
            PersonService personService)
        {
            _licenceExpireEmailBuildService = licenceExpireEmailBuildService;
            _personService = personService;
        }

        /// <summary>
        /// Every time when the scheduler executes a job this method is called.
        /// </summary>
        /// <param name="context">not used</param>
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Logger.Info("Executing monthly report job");

                var withinDaysOfExpiration = 60;
                var expireDate = DateTime.Today.AddDays(withinDaysOfExpiration);
                var persons = _personService.GetPersonsWithExpiredLicences(expireDate);

                foreach (var person in persons)
                {
                    if (string.IsNullOrWhiteSpace(person.EmailAddressForCommunication))
                    {
                        Logger.Info($"{person.DisplayName} has no email address for sending licence expire notification");
                        continue;
                    }

                    if (person.MedicalLaplExpireDate.HasValue && person.MedicalLaplExpireDate.Value <= expireDate)
                    {
                        ProcessLicenceExpireEmail(person, "LAPL Medical", person.MedicalLaplExpireDate.Value);
                    }

                    if (person.MedicalClass1ExpireDate.HasValue && person.MedicalClass1ExpireDate.Value <= expireDate)
                    {
                        ProcessLicenceExpireEmail(person, "Class 1 Medical", person.MedicalClass1ExpireDate.Value);
                    }

                    if (person.MedicalClass2ExpireDate.HasValue && person.MedicalClass2ExpireDate.Value <= expireDate)
                    {
                        ProcessLicenceExpireEmail(person, "Class 2 Medical", person.MedicalClass2ExpireDate.Value);
                    }

                    if (person.GliderInstructorLicenceExpireDate.HasValue && person.GliderInstructorLicenceExpireDate.Value <= expireDate)
                    {
                        ProcessLicenceExpireEmail(person, "Segelfluglehrer-Lizenz", person.GliderInstructorLicenceExpireDate.Value);
                    }

                    if (person.MotorInstructorLicenceExpireDate.HasValue && person.MotorInstructorLicenceExpireDate.Value <= expireDate)
                    {
                        ProcessLicenceExpireEmail(person, "Motorfluglehrer-Lizenz", person.MotorInstructorLicenceExpireDate.Value);
                    }

                    if (person.PartMLicenceExpireDate.HasValue && person.PartMLicenceExpireDate.Value <= expireDate)
                    {
                        ProcessLicenceExpireEmail(person, "Part-M Lizenz", person.PartMLicenceExpireDate.Value);
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error while executing monthly workflow job. Error: {ex.Message}");
            }
        }

        private void ProcessLicenceExpireEmail(Person person, string licenceName, DateTime expireDate)
        {
            try
            {
                var message = _licenceExpireEmailBuildService.CreateLicenceExpireEmail(person, licenceName, expireDate);
                _licenceExpireEmailBuildService.SendEmail(message);

                Logger.Info($"Sent {licenceName} licence expire notification to {person.EmailAddressForCommunication}");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error while processing license expire notification and sending per email for {person.DisplayName}. Error: {ex.Message}");
            }
        }
    }
}