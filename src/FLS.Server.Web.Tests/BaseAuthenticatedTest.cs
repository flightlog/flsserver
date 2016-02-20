using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FLS.Server.Web.Tests
{
    /// <summary>
    /// <see cref="http://www.aaron-powell.com/posts/2014-01-12-integration-testing-katana-with-auth.html"/>
    /// </summary>
    [TestClass]
    public abstract class BaseAuthenticatedTests : BaseServerTest
    {
        protected virtual string Username
        {
            get { return "fgzo"; }
        }

        protected virtual string Password
        {
            get { return "fgzo"; }
        }

        private string token;

        protected override void PostSetup(TestServer server)
        {
            var tokenDetails = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", Username),
                    new KeyValuePair<string, string>("password", Password)
                };

            var tokenPostData = new FormUrlEncodedContent(tokenDetails);
            var tokenResult = server.HttpClient.PostAsync("/Token", tokenPostData).Result;
            Assert.AreEqual(HttpStatusCode.OK, tokenResult.StatusCode);

            var body = JObject.Parse(tokenResult.Content.ReadAsStringAsync().Result);

            token = (string) body["access_token"];
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

            Assert.IsTrue(response.IsSuccessStatusCode);

            var jsonAsString = response.Content.ReadAsStringAsync().Result;

            var output = JsonConvert.DeserializeObject<TResult>(jsonAsString);

            return output;
        }

        protected override async Task<HttpResponseMessage> GetAsync(string uri)
        {
            return await TestServer.CreateRequest(uri)
                               .AddHeader("Authorization", "Bearer " + token)
                               .GetAsync();
        }

        protected override async Task<HttpResponseMessage> PostAsync<TModel>(TModel model)
        {
            return await PostAsync(model, Uri);
        }

        protected override async Task<HttpResponseMessage> PostAsync<TModel>(TModel model, string uri)
        {
            return await TestServer.CreateRequest(uri)
                .AddHeader("Authorization", "Bearer " + token)
                .And(request => request.Content = new ObjectContent(typeof(TModel), model, new JsonMediaTypeFormatter()))
                .PostAsync();
        }

        protected override async Task<HttpResponseMessage> PutAsync<TModel>(TModel model, string uri)
        {
            //maybe it is a hack, but it works
            using (var client = new HttpClient(TestServer.Handler))
            {
                var content = new ObjectContent(typeof(TModel), model, new JsonMediaTypeFormatter());
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return await client.PutAsync(TestServer.HttpClient.BaseAddress + uri, content);
            }
        }

        protected override async Task<HttpResponseMessage> DeleteAsync(string uri)
        {
            //maybe it is a hack, but it works
            using (var client = new HttpClient(TestServer.Handler))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return await client.DeleteAsync(TestServer.HttpClient.BaseAddress + uri);
            }
        }
    }
}
