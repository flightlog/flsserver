using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

namespace FLS.Server.Service.Invoicing
{
    public class InvoiceMappingFactory
    {
        private readonly DataAccessService _dataAccessService;
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public InvoiceMappingFactory(DataAccessService dataAccessService)
        {
            _dataAccessService = dataAccessService;
        }

        
        internal InvoiceMapping CreateInvoiceMapping()
        {
            var invoiceMapping = new InvoiceMapping();
            //TODO: add rule data to database from private SVN repository and implement flexible solution for invoice mapping
            return invoiceMapping;
        }

        internal Guid GetAircraftId(string immatriculation)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var aircraft =
                    context.Aircrafts.FirstOrDefault(a => a.Immatriculation.ToUpper() == immatriculation.ToUpper());

                if (aircraft != null)
                {
                    return aircraft.AircraftId;
                }

                Logger.Warn(string.Format("Aircraft with immatriculation: {0} not found!",
                                                                immatriculation));

                return Guid.Empty;
            }
        }

        internal Guid GetLocationId(string icaoCode)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var location =
                    context.Locations.FirstOrDefault(
                        a => a.IcaoCode.ToUpper() == icaoCode.ToUpper());

                if (location != null)
                {
                    return location.LocationId;
                }

                Logger.Warn(string.Format("Location with IcaoCode: {0} not found!", icaoCode));

                return Guid.Empty;
            }
        }
    }
}