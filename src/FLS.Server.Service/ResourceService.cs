using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;
using NLog;

namespace FLS.Server.Service
{
    internal static class ResourceService
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private static Logger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }

        private static Dictionary<string, string> EmailTemplates = new Dictionary<string, string>();


        internal static string GetEmailTemplateFullFilename(string templateFile)
        {
            if (EmailTemplates.ContainsKey(templateFile))
            {
                return EmailTemplates[templateFile];
            }

            string templateFullFilename = string.Empty;
            var subDirectory = @"Email\Templates";
            string appPath = string.Empty;

            try
            {
                appPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;

                if (string.IsNullOrEmpty(appPath) == false)
                {
                    templateFullFilename = Path.Combine(appPath, subDirectory,
                                                        templateFile);

                    if (File.Exists(templateFullFilename))
                    {
                        EmailTemplates.Add(templateFile, templateFullFilename);
                        return templateFullFilename;
                    }
                    else
                    {
                        Logger.Warn(string.Format("Email-Template-File {0} not found with method: System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath", templateFullFilename));
                    }
                }

                appPath = AppDomain.CurrentDomain.BaseDirectory;

                if (string.IsNullOrEmpty(appPath) == false)
                {
                    templateFullFilename = Path.Combine(appPath, subDirectory,
                                                        templateFile);

                    if (File.Exists(templateFullFilename))
                    {
                        EmailTemplates.Add(templateFile, templateFullFilename);
                        return templateFullFilename;
                    }
                    else
                    {
                        Logger.Warn(string.Format("Email-Template-File {0} not found with method: AppDomain.CurrentDomain.BaseDirectory", templateFullFilename));
                    }
                }

                if (HttpContext.Current != null)
                {
                    templateFullFilename = Path.Combine(HttpContext.Current.Server.MapPath("~/bin"), subDirectory,
                                            templateFile);

                    if (File.Exists(templateFullFilename))
                    {
                        EmailTemplates.Add(templateFile, templateFullFilename);
                        return templateFullFilename;
                    }
                    else
                    {
                        Logger.Warn(string.Format("Email-Template-File {0} not found with method: HttpContext.Current.Server.MapPath(~/bin)", templateFullFilename));
                    }

                    templateFullFilename = Path.Combine(HttpContext.Current.Server.MapPath("~/"), subDirectory,
                                            templateFile);

                    if (File.Exists(templateFullFilename))
                    {
                        EmailTemplates.Add(templateFile, templateFullFilename);
                        return templateFullFilename;
                    }
                    else
                    {
                        Logger.Warn(string.Format("Email-Template-File {0} not found with method: HttpContext.Current.Server.MapPath(~/)", templateFullFilename));
                    }

                    templateFullFilename = Path.Combine(HttpContext.Current.Server.MapPath("~/"), templateFile);

                    if (File.Exists(templateFullFilename))
                    {
                        EmailTemplates.Add(templateFile, templateFullFilename);
                        return templateFullFilename;
                    }
                    else
                    {
                        Logger.Warn(string.Format("Email-Template-File {0} not found with method: HttpContext.Current.Server.MapPath(~/)", templateFullFilename));
                    }
                }
                else
                {
                    Logger.Warn("HttpContext.Current is null");
                }

                var bin = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                if (string.IsNullOrEmpty(bin) == false)
                {
                    templateFullFilename = Path.Combine(bin, subDirectory, templateFile);

                    if (File.Exists(templateFullFilename))
                    {
                        EmailTemplates.Add(templateFile, templateFullFilename);
                        return templateFullFilename;
                    }
                    else
                    {
                        Logger.Warn(string.Format("Email-Template-File {0} not found with method: Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) and subdirectory", templateFullFilename));
                    }

                    templateFullFilename = Path.Combine(bin, templateFile);

                    if (File.Exists(templateFullFilename))
                    {
                        EmailTemplates.Add(templateFile, templateFullFilename);
                        return templateFullFilename;
                    }
                    else
                    {
                        Logger.Warn(string.Format("Email-Template-File {0} not found with method: Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)", templateFullFilename));
                    }
                }
                else
                {
                    Logger.Warn(string.Format("Email-Template-File {0} not found with method: Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) (is null)", templateFullFilename));
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error while trying to find path to email template: {ex.Message}");
            }

            Logger.Error(string.Format("Email-Template-File {0} not found with any method!!!", templateFullFilename));

            return templateFullFilename;
        }
    }
}
