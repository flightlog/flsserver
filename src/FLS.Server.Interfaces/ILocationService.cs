using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLS.Data.WebApi.Aircraft;
using FLS.Data.WebApi.Location;

namespace FLS.Server.Interfaces
{
    public interface ILocationService
    {
        LocationDetails GetLocationDetailsByIcaoCode(string icaoCode);
    }
}
