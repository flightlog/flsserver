using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using FLS.Server.WebApi;
using Microsoft.Owin.Testing;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace FLS.Server.Tests.Infrastructure.WebApi
{
    /// <summary>
    /// <see cref="http://www.aaron-powell.com/posts/2014-01-12-integration-testing-katana-with-auth.html"/>
    /// </summary>
    [TestClass]
    public abstract class BaseServerTest : BaseTest
    {
        protected TestServer TestServer { get; set; }

        protected TimeSpan TimeOut { get; set; }

        //TODO: make this method as assemblyInitialize? --> http://shawnmclean.com/integration-test-asp-net-webapi-with-owin-and-authentication/
        [TestInitialize]
        public void SetupTestServer()
        {
            Console.WriteLine("TestInitialize: BaseServerTest.Setup()");
            TimeOut = new TimeSpan(0, 10, 0); //10 min
            UnityContainer = new UnityContainer(); //UnityConfig.GetEmptyContainer();
            TestStartup.RegisterTypes(UnityContainer);
            TestServer = TestServer.Create<TestStartup>();
            
            TestServer.HttpClient.Timeout = TimeOut;

            PostSetup(TestServer);
        }

        [TestInitialize]
        protected virtual void PostSetup(TestServer server)
        {
        }

        [TestCleanup]
        public void Teardown()
        {
            Console.WriteLine("TestCleanup: BaseServerTest.Teardown()");
            if (TestServer != null)
            {
                Console.WriteLine("TestCleanup: BaseServerTest.Teardown(): TestServer.Dispose()");
                TestServer.Dispose();
            }
        }

        protected abstract string Uri { get; }

        protected abstract string RoutePrefix { get; }

        protected virtual async Task<HttpResponseMessage> GetAsync()
        {
            return await GetAsync(Uri);
        }

        protected virtual async Task<HttpResponseMessage> PostAsync<TModel>(TModel model)
        {
            return await PostAsync(model, Uri);
        }

        protected virtual async Task<HttpResponseMessage> GetAsync(string uri)
        {
            return await TestServer.CreateRequest(uri).GetAsync();
        }

        protected virtual async Task<HttpResponseMessage> PostAsync<TModel>(TModel model, string uri)
        {
            return await TestServer.CreateRequest(uri)
                .And(request => request.Content = new ObjectContent(typeof(TModel), model, new JsonMediaTypeFormatter()))
                .PostAsync();
        }

        protected virtual async Task<HttpResponseMessage> PutAsync<TModel>(TModel model, string uri)
        {
            //maybe it is a hack, but it works
            using (var client = new HttpClient(TestServer.Handler))
            {
                client.Timeout = TimeOut;
                var content = new ObjectContent(typeof(TModel), model, new JsonMediaTypeFormatter());
                return await client.PutAsync(TestServer.HttpClient.BaseAddress + uri, content);
            }
        }

        protected virtual async Task<HttpResponseMessage> DeleteAsync(string uri)
        {
            //maybe it is a hack, but it works
            using (var client = new HttpClient(TestServer.Handler))
            {
                client.Timeout = TimeOut;
                return await client.DeleteAsync(TestServer.HttpClient.BaseAddress + uri);
            }
        }

        public TModel ConvertToModel<TModel>(HttpResponseMessage response)
        {
            var jsonAsString = response.Content.ReadAsStringAsync().Result;

            var output = JsonConvert.DeserializeObject<TModel>(jsonAsString);
            return output;
        }
    }
}
