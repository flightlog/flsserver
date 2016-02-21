using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FLS.Server.WebApi.Models
{
    public class ExternalChallangeResultViewModel
    {
        public bool Failed { get; set; }
        public string ErrorMessage { get; set; }
    }
}