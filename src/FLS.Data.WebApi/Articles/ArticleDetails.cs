using System;
using System.ComponentModel.DataAnnotations;

namespace FLS.Data.WebApi.Articles
{
    public class ArticleDetails : FLSBaseData
    {
        public Guid ArticleId { get; set; }

        [Required]
        [StringLength(50)]
        public string ArticleNumber { get; set; }

        [Required]
        [StringLength(250)]
        public string ArticleName { get; set; }

        [StringLength(250)]
        public string ArticleInfo { get; set; }

        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public override Guid Id
        {
            get { return ArticleId; }
            set { ArticleId = value; }
        }
    }
}
