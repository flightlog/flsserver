using System.Net.Http.Headers;
using FLS.Server.TestInfrastructure;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using Owin;

namespace FLS.Server.Web.Tests
{
    /// <summary>
    /// <see cref="http://www.aaron-powell.com/posts/2014-01-12-integration-testing-katana-with-auth.html"/>
    /// </summary>
    [TestClass]
    public abstract class BaseServerTest
    {
        protected TestServer TestServer { get; set; }

        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            DatabasePreparer.Instance.PrepareDatabaseForTests();
        }

        [TestInitialize]
        public void Setup()
        {
            TestServer = TestServer.Create(app =>
            {
                var startup = new Startup();
                startup.ConfigureAuth(app);

                var config = new HttpConfiguration();

                //set Json format as default, but if i want to use the XML format. I'll just append the xml=true.
                //http://stackoverflow.com/questions/9847564/how-do-i-get-asp-net-web-api-to-return-json-instead-of-xml-using-chrome
                config.Formatters.XmlFormatter.MediaTypeMappings.Add(new QueryStringMapping("xml", "true", "application/xml"));
                config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

                WebApiConfig.Register(config);

                app.UseWebApi(config);
            });

            PostSetup(TestServer);
        }

        protected virtual void PostSetup(TestServer server)
        {
        }

        [TestCleanup]
        public void Teardown()
        {
            if (TestServer != null)
                TestServer.Dispose();
        }

        protected abstract string Uri { get; }

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
                var content = new ObjectContent(typeof(TModel), model, new JsonMediaTypeFormatter());
                return await client.PutAsync(TestServer.HttpClient.BaseAddress + uri, content);
            }
        }

        protected virtual async Task<HttpResponseMessage> DeleteAsync(string uri)
        {
            //maybe it is a hack, but it works
            using (var client = new HttpClient(TestServer.Handler))
            {
                return await client.DeleteAsync(TestServer.HttpClient.BaseAddress + uri);
            }
        }
    }
}
