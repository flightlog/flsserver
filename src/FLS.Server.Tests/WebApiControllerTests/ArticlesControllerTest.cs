using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FLS.Common.Extensions;
using FLS.Data.WebApi.Articles;
using FLS.Server.Tests.Infrastructure.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests.WebApiControllerTests
{
    [TestClass]
    public class ArticlesControllerTest : BaseAuthenticatedTests
    {
        [TestMethod]
        [TestCategory("WebApi")]
        public void GetArticleOverviewWebApiTest()
        {
            InsertArticleDetailsWebApiTest();

            var response = GetAsync<IEnumerable<ArticleOverview>>("/api/v1/articles").Result;

            Assert.IsTrue(response.Any());
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void GetArticleDetailsWebApiTest()
        {
            InsertArticleDetailsWebApiTest();

            var response = GetAsync<IEnumerable<ArticleOverview>>("/api/v1/articles").Result;

            Assert.IsTrue(response.Any());

            var id = response.First().ArticleId;

            var result = GetAsync<ArticleDetails>("/api/v1/articles/" + id).Result;

            Assert.AreEqual(id, result.Id);
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void InsertArticleDetailsWebApiTest()
        {
            var article = new ArticleDetails();
            article.ArticleNumber = DateTime.Now.Ticks.ToString();
            article.ArticleName = "Article @ " + DateTime.Now.Ticks;
            article.ArticleInfo = "Test-Article";
            article.IsActive = true;
            Assert.AreEqual(article.Id, Guid.Empty);

            var response = PostAsync(article, "/api/v1/articles").Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseDetails = ConvertToModel<ArticleDetails>(response);
            Assert.IsTrue(responseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseDetails));
        }
        
        [TestMethod]
        [TestCategory("WebApi")]
        public void UpdateArticleDetailsWebApiTest()
        {
            InsertArticleDetailsWebApiTest();

            var response = GetAsync<IEnumerable<ArticleOverview>>("/api/v1/articles").Result;

            Assert.IsTrue(response.Any());

            var id = response.First().ArticleId;

            var result = GetAsync<ArticleDetails>("/api/v1/articles/" + id).Result;

            Assert.AreEqual(id, result.Id);

            result.Description = "Updated on " + DateTime.Now.ToShortTimeString();

            var putResult = PutAsync(result, "/api/v1/articles/" + id).Result;

            Assert.IsTrue(putResult.IsSuccessStatusCode);
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void DeleteArticleDetailsWebApiTest()
        {
            var response = GetAsync<IEnumerable<ArticleOverview>>("/api/v1/articles").Result;

            Assert.IsTrue(response.Any());

            var id = response.First().ArticleId;

            var delResult = DeleteAsync("/api/v1/articles/" + id).Result;

            Assert.IsTrue(delResult.IsSuccessStatusCode);
        }

        [TestMethod]
        [TestCategory("WebApi")]
        public void LastArticleSynchWebApiTest()
        {
            var response = GetAsync<Nullable<DateTime>>("/api/v1/articles/lastsync").Result;

            DateTime lastSync = response.GetValueOrDefault(DateTime.MinValue);

            var setResponse = PutAsync(DateTime.Now, "/api/v1/articles/lastsync").Result;

            Assert.IsTrue(setResponse.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", setResponse.StatusCode));
            var responseDetails = ConvertToModel<Nullable<DateTime>>(setResponse);
            Assert.IsTrue(lastSync < responseDetails.GetValueOrDefault(DateTime.MinValue));
        }

        protected override string Uri
        {
            get { return RoutePrefix; }
        }

        protected override string RoutePrefix
        {
            get { return "/api/v1/articles"; }
        }
    }
}
