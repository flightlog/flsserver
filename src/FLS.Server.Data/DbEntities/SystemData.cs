using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FLS.Server.Data.DbEntities
{
    [Table("SystemData")]
    public partial class SystemData
    {
        public SystemData()
        {
            
        }

        [Key]
        public Guid SystemId { get; set; }

        [Required]
        [StringLength(250)]
        public string BaseURL { get; set; }

        [Required]
        [StringLength(100)]
        public string ReportSenderEmailAddress { get; set; }

        [Required]
        [StringLength(100)]
        public string SystemSenderEmailAddress { get; set; }

        [Required]
        public bool UseSmtpAuthentication { get; set; }

        [Required]
        public bool UseSSLforSmtpConnection { get; set; }
        
        [StringLength(100)]
        public string SmtpUsername { get; set; }

        [StringLength(100)]
        public string SmtpPassword { get; set; }

        [Required]
        [StringLength(100)]
        public string SmtpServer { get; set; }

        public int SmtpPort { get; set; }

        public int MaxUserLoginAttempts { get; set; }

        [Required]
        public bool Testmode { get; set; }

        [StringLength(100)]
        public string TestmodeEmailPickupDirectory { get; set; }

        [Required]
        public bool DebugMode { get; set; }

        [Required]
        public bool SendToBccRecipients { get; set; }

        [StringLength(250)]
        public string BccRecipientEmailAddresses { get; set; }

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
