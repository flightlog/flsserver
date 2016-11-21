using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FLS.Data.WebApi.User;
using FLS.Server.TestInfrastructure;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FLS.Server.Tests.Infrastructure.WebApi
{
    /// <summary>
    /// <see cref="http://www.aaron-powell.com/posts/2014-01-12-integration-testing-katana-with-auth.html"/>
    /// </summary>
    [TestClass]
    public abstract class BaseAuthenticatedTests : BaseServerTest
    {
        protected Guid ClubId { get; set; }

        protected UserDetails MyUserDetails { get; set; }

        private string _token;

        public bool IsAuthenticated { get; set; }

        public void Logout()
        {
            IsAuthenticated = false;
            _token = string.Empty;
            MyUserDetails = null;
            ClubId = Guid.Empty;
            Logger.Info($"Logged out!");
        }

        protected override void PostSetup(TestServer server)
        {
            LoginAsClubAdmin();
        }

        protected void LoginAsClubAdmin()
        {
            Logout();
            Login(TestConfigurationSettings.Instance.TestClubAdminUsername, TestConfigurationSettings.Instance.TestClubAdminPassword);
        }

        protected void LoginAsWorkflow()
        {
            Logout();
            Login(TestConfigurationSettings.Instance.TestWorkflowUsername, TestConfigurationSettings.Instance.TestWorkflowPassword);
        }

        protected void LoginAsSystemAdmin()
        {
            Logout();
            Login(TestConfigurationSettings.Instance.TestSystemAdminUsername, TestConfigurationSettings.Instance.TestSystemAdminPassword);
        }

        protected void LoginAsClubUser()
        {
            Logout();
            Login(TestConfigurationSettings.Instance.TestClubUserUsername, TestConfigurationSettings.Instance.TestClubUserPassword);
        }

        protected void Login(string username, string password, bool ignoreFailedLogin = false)
        {
            var tokenDetails = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("password", password)
                };

            var tokenPostData = new FormUrlEncodedContent(tokenDetails);
            //var tokenResult = TestServer.CreateRequest("/Token").And(x => x.Content = tokenPostData).PostAsync().Result;
            var tokenResult = TestServer.HttpClient.PostAsync("/Token", tokenPostData).Result;

            if (ignoreFailedLogin == false)
            {
                Assert.AreEqual(HttpStatusCode.OK, tokenResult.StatusCode,
                    string.Format("Error while getting Token! Result: {0}", tokenResult.StatusCode));

                var body = JObject.Parse(tokenResult.Content.ReadAsStringAsync().Result);

                _token = (string) body["access_token"];
                IsAuthenticated = true;

                MyUserDetails = GetAsync<UserDetails>("/api/v1/users/my").Result;

                Assert.IsNotNull(MyUserDetails, "MyUserDetails is NULL");

                ClubId = MyUserDetails.ClubId;

                Logger.Info($"Logged in as user: {MyUserDetails.UserName}");
            }
        }

        protected async Task<TResult> GetAsync<TResult>()
        {
            return await GetAsync<TResult>(Uri);
        }

        protected override async Task<HttpResponseMessage> GetAsync()
        {
            return await GetAsync(Uri);
        }

        protected async Task<TResult> GetAsync<TResult>(string uri)
        {
            var response = await GetAsync(uri);

            //http://stackoverflow.com/questions/23576726/using-readasasynct-to-deserialize-complex-json-object

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));

            var jsonAsString = response.Content.ReadAsStringAsync().Result;

            var output = JsonConvert.DeserializeObject<TResult>(jsonAsString);

            return output;
        }

        protected override async Task<HttpResponseMessage> GetAsync(string uri)
        {
            return await TestServer.CreateRequest(uri)
                               .AddHeader("Authorization", "Bearer " + _token)
                               .GetAsync();
        }

        protected override async Task<HttpResponseMessage> PostAsync<TModel>(TModel model)
        {
            return await PostAsync(model, Uri);
        }

        protected override async Task<HttpResponseMessage> PostAsync<TModel>(TModel model, string uri)
        {
            return await TestServer.CreateRequest(uri)
                .AddHeader("Authorization", "Bearer " + _token)
                .And(request => request.Content = new ObjectContent(typeof(TModel), model, new JsonMediaTypeFormatter()))
                .PostAsync();
        }

        protected override async Task<HttpResponseMessage> PutAsync<TModel>(TModel model, string uri)
        {
            //maybe it is a hack, but it works
            using (var client = new HttpClient(TestServer.Handler))
            {
                client.Timeout = TimeOut;
                var content = new ObjectContent(typeof(TModel), model, new JsonMediaTypeFormatter());
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                client.DefaultRequestHeaders.Add("X-HTTP-Method-Override", "PUT");
                return await client.PostAsync(TestServer.HttpClient.BaseAddress + uri, content);
            }
        }

        protected override async Task<HttpResponseMessage> DeleteAsync(string uri)
        {
            //maybe it is a hack, but it works
            using (var client = new HttpClient(TestServer.Handler))
            {
                client.Timeout = TimeOut;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                client.DefaultRequestHeaders.Add("X-HTTP-Method-Override", "DELETE");
                return await client.DeleteAsync(TestServer.HttpClient.BaseAddress + uri);
            }
        }

        protected async Task<HttpResponseMessage> DeletePostAsync<TModel>(TModel model, string uri)
        {
            //maybe it is a hack, but it works
            using (var client = new HttpClient(TestServer.Handler))
            {
                client.Timeout = TimeOut;
                var content = new ObjectContent(typeof(TModel), model, new JsonMediaTypeFormatter());
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                client.DefaultRequestHeaders.Add("X-HTTP-Method-Override", "DELETE");
                return await client.PostAsync(TestServer.HttpClient.BaseAddress + uri, content);
            }
        }

        protected TResult GetModelFromResponse<TResult>(HttpResponseMessage response)
        {
            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));

            var jsonAsString = response.Content.ReadAsStringAsync().Result;

            var output = JsonConvert.DeserializeObject<TResult>(jsonAsString);

            return output;
        }
    }
}
