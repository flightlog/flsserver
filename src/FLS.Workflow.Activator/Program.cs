using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using NLog;
using Newtonsoft.Json.Linq;

namespace FLS.Workflow.Activator
{
    class Program
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            try
            {
                var actionUri = "";

                Logger.Debug("Workflow service command tool started");

                if (args.Length == 0)
                {
                    Logger.Debug("Executing whole workflow as no argument is set");
                }
                else if (args.Length == 1)
                {
                    Logger.Debug(string.Format("Parsing argument: {0}", args[0]));
                    if (args[0].ToLower() == "?" || args[0].ToLower() == "help")
                    {
                        Console.WriteLine("Use 'flightvalidation','dailyreport','monthlyreport','planning','testmail','invoice'");
                        return;
                    }
                    else if (args[0].ToLower() == "flightvalidation")
                    {
                        actionUri = "flightvalidation";
                    }
                    else if (args[0].ToLower() == "dailyreport")
                    {
                        actionUri = "dailyreports";
                    }
                    else if (args[0].ToLower() == "monthlyreport")
                    {
                        actionUri = "monthlyreports";
                    }
                    else if (args[0].ToLower() == "planning")
                    {
                        actionUri = "planningdaymails";
                    }
                    else if (args[0].ToLower() == "testmail")
                    {
                        actionUri = "testmails";
                    }
                    else if (args[0].ToLower() == "deliverycreation")
                    {
                        actionUri = "deliverycreation";
                    }
                    else if (args[0].ToLower() == "deliverymailexport")
                    {
                        actionUri = "deliverymailexport";
                    }
                    else
                    {
                        Logger.Debug("Wrong argument passed.");
                    }
                }

                using (var client = new HttpClient())
                {
                    //Gets the token for a user (which is already in the database (registered))
                    string token = GetToken(AppSettings.Default.Username, AppSettings.Default.Password);

                    //gets the access token value
                    var json = JObject.Parse(token);
                    var accessToken = json["access_token"].ToString();

                    Logger.Debug(string.Format("Access Token: {0}", accessToken));

                    //sets the Bearer authorization header with the access token value 
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);


                    var response = client.GetAsync(AppSettings.Default.BaseUri + AppSettings.Default.WorkflowControllerUri + actionUri).Result;

                    if (response.IsSuccessStatusCode == false)
                    {
                        Logger.Error(string.Format("Error while trying to execute workflows: {0}, Message: {1}", response.StatusCode, response.ReasonPhrase));
                    }
                    else
                    {
                        Logger.Info("Started workflows successfully. For details please have a look in the database system logs.");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            Logger.Debug("Workflow service command tool finished");
        }

        static string GetToken(string userName, string password)
        {
            var pairs = new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>( "grant_type", "password" ), 
                            new KeyValuePair<string, string>( "username", userName ), 
                            new KeyValuePair<string, string> ( "Password", password )
                        };

            var content = new FormUrlEncodedContent(pairs);

            using (var client = new HttpClient())
            {
                var response = client.PostAsync(AppSettings.Default.BaseUri + "Token", content).Result;
                if (response.IsSuccessStatusCode == false)
                {
                    Logger.Error(string.Format("Could not get token from server. Error Statuscode: {0}", response.StatusCode));
                    throw new Exception(response.ReasonPhrase);
                }

                return response.Content.ReadAsStringAsync().Result;
            }
        }
    }
}
