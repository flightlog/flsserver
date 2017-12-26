using FLS.Data.WebApi.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLS.Server.Data.Objects.Person
{
    public class ImportPersonDetails : PersonDetails
    {
        public string CountryCode { get; set; }
        public string AddressCategories { get; set; }
    }
}
