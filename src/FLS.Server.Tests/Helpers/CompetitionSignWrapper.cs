using System.ComponentModel.DataAnnotations;

namespace FLS.Server.Tests.Helpers
{
    public class CompetitionSignWrapper
    {
        [StringLength(3)]
        public string CompetitionSign { get; set; }
    }
}
