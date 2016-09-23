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
        string GetExtensionStringValue(string key, Guid clubId);

        byte[] GetExtensionBinaryValue(string key, Guid clubId);

        void SaveExtensionStringValue(string key, string value, Guid clubId);

        void SaveExtensionBinaryValue(string key, byte[] value, Guid clubId);

        void DeleteExtensionValue(string key, Guid clubId);
    }
}
