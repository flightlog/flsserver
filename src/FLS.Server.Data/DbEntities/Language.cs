using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FLS.Server.Data.DbEntities
{
    public partial class Language
    {
        public Language()
        {
            LanguageTranslations = new HashSet<LanguageTranslation>();
        }

        public int LanguageId { get; set; }

        [Required]
        [StringLength(5)]
        public string LanguageKey { get; set; }

        [Required]
        [StringLength(50)]
        public string LanguageName { get; set; }

        public string Remarks { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedOn { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ModifiedOn { get; set; }

        public int Id
        {
            get { return LanguageId; }
            set { LanguageId = value; }
        }

        public virtual ICollection<LanguageTranslation> LanguageTranslations { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<EmailTemplate> EmailTemplates { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            Type type = GetType();
            sb.Append("[");
            sb.Append(type.Name);
            sb.Append(" -> ");
            foreach (FieldInfo info in type.GetFields())
            {
                sb.Append(string.Format("{0}: {1}, ", info.Name, info.GetValue(this)));
            }

            Type tColl = typeof(ICollection<>);
            foreach (PropertyInfo info in type.GetProperties())
            {
                Type t = info.PropertyType;
                if (t.IsGenericType && tColl.IsAssignableFrom(t.GetGenericTypeDefinition()) ||
                    t.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == tColl)
                    || (t.Namespace != null && t.Namespace.Contains("FLS.Server.Data.DbEntities")))
                {
                    continue;
                }

                sb.Append(string.Format("{0}: {1}, ", info.Name, info.GetValue(this, null)));
            }

            sb.Append(" <- ");
            sb.Append(type.Name);
            sb.AppendLine("]");

            return sb.ToString();
        }
    }
}
