using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLS.Data.WebApi.Aircraft;
using FLS.Data.WebApi.Location;

namespace FLS.Server.Interfaces
{
    public interface IExtensionService
    {
        string GetExtensionStringValue(string key);

        void SaveExtensionStringValue(string key, string value);

        void DeleteExtensionValue(string key);
    }
}
