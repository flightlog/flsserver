using System;

namespace FLS.Data.WebApi.Articles
{
    public class ArticleOverview : FLSBaseData
    {
        public Guid ArticleId { get; set; }

        public string ArticleNumber { get; set; }

        public string ArticleName { get; set; }

        public bool IsActive { get; set; }

        public override Guid Id
        {
            get { return ArticleId; }
            set { ArticleId = value; }
        }
    }
}
