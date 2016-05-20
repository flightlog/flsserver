using FLS.Data.WebApi.Invoicing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLS.Data.WebApi.Flight;
using FLS.Data.WebApi.Person;
using FLS.Server.Data.DbEntities;

namespace FLS.Server.Interfaces.Invoicing
{
    public interface IInvoiceService
    {
        List<FlightInvoiceDetails> CreateFlightInvoiceDetails(List<Flight> flightsToInvoice);
    }
}
