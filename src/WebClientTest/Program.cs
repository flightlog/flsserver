using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WebClientTest
{
    /// <summary>
    /// see: http://typecastexception.com/post/2014/09/21/ASPNET-Identity-20-Introduction-to-Working-with-Identity-20-and-Web-API-22.aspx
    /// </summary>
    class Program
    {
        private static string BaseURL = "http://apitest.glider-fls.ch/";
        static void Main(string[] args)
        {
            using (var client = new HttpClient())
            {
                string userName = "fgzo";
                string password = "fgzo";
                
                //Gets the token for a user (which is already in the database (registered))
                string token = GetToken(userName, password);

                Console.WriteLine("");
                Console.WriteLine("Token response:");
                Console.WriteLine(token);

                //gets the access token value
                var json = JObject.Parse(token);
                var accessToken = json["access_token"].ToString();

                Console.WriteLine("");
                Console.WriteLine("Access Token value:");
                Console.WriteLine(accessToken);

                //sets the Bearer authorization header with the access token value 
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                Console.WriteLine("");
                Console.WriteLine("Authorization header:");
                Console.WriteLine(client.DefaultRequestHeaders.Authorization.ToString());

                var response = client.GetAsync(BaseURL + "api/v1/invoices").Result;

                Console.WriteLine("");
                Console.WriteLine("Result:");
                Console.WriteLine(response.StatusCode.ToString());
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }

            Console.Read();
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
                var response = client.PostAsync(BaseURL + "Token", content).Result;

                return response.Content.ReadAsStringAsync().Result;
            }
        }
    }
}
