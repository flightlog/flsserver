using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLS.Data.WebApi.Aircraft;
using FLS.Data.WebApi.Location;
using FLS.Data.WebApi.Person;

namespace FLS.Server.Interfaces
{
    public interface IPersonService
    {
        PilotPersonDetails GetPilotPersonDetails(Guid personId);

        PilotPersonDetails GetPilotPersonDetails(Guid personId, Guid clubId);
    }
}
