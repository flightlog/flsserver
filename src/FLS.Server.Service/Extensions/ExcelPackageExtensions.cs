using System.IO;
using OfficeOpenXml;

namespace FLS.Server.Service.Extensions
{
    public static class ExcelPackageExtensions
    {
        public static MemoryStream ToMemoryStream(this ExcelPackage excelPackage)
        {
            var bytes = excelPackage.GetAsByteArray();
            var memoryStream = new MemoryStream(bytes);
            return memoryStream;
        }
    }
}
