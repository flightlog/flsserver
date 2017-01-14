using System;

namespace FLS.Data.WebApi.Articles
{
    public class ArticleOverviewSearchFilter
    {
        public string ArticleNumber { get; set; }

        public string ArticleName { get; set; }

        public bool? IsActive { get; set; }
    }
}
