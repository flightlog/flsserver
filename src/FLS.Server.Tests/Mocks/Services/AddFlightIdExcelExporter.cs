using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLS.Server.Service.Exporting;

namespace FLS.Server.Tests.Mocks.Services
{
    public class AddFlightIdExcelExporter : ExcelExporter
    {
        public AddFlightIdExcelExporter()
            : base(true)
        { }
    }
}
