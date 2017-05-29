using System.Collections.Generic;
using FLS.Data.WebApi.Accounting;

namespace FLS.Server.Interfaces
{
    public interface IDeliveryExcelExporter
    {
        byte[] ExportDeliveriesToExcel(List<DeliveryDetails> deliveryDetailList);
    }
}