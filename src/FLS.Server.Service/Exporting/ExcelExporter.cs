using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using FLS.Common.Extensions;
using FLS.Data.WebApi.Accounting;
using FLS.Data.WebApi.Person;
using FLS.Data.WebApi.Reporting;
using FLS.Server.Interfaces;
using Ionic.Zip;
using NLog;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace FLS.Server.Service.Exporting
{
    /// <summary>
    /// Excel exporter helper class which uses EPPlus library.
    /// </summary>
    public class ExcelExporter
    {
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private bool _addFlightIdColumn = false;

        public ExcelExporter()
        {
#if DEBUG
            _addFlightIdColumn = true;
#endif
        }

        public byte[] ExportDeliveriesToExcel(List<DeliveryDetails> deliveryDetailList)
        {
            var recipientSortedDeliveries = new Dictionary<string, List<DeliveryDetails>>();
            foreach (var ruleBasedDelivery in deliveryDetailList)
            {
                //check if there are some line items in the invoice, if not, check next invoice
                if (ruleBasedDelivery.DeliveryItems.Any() == false)
                {
                    Logger.Warn($"Delivery (ID: {ruleBasedDelivery.DeliveryId}) without items found. Will exclude it from export into excel!");
                    continue;
                }

                var recipientName = ruleBasedDelivery.RecipientDetails.RecipientName;
                if (string.IsNullOrWhiteSpace(recipientName))
                {
                    Logger.Warn($"Delivery (ID: {ruleBasedDelivery.DeliveryId}) has no recipient name or is null!");
                    recipientName = "NO_RECIPIENT";
                }

                if (recipientSortedDeliveries.ContainsKey(recipientName) == false)
                {
                    var deliveryList = new List<DeliveryDetails>();
                    deliveryList.Add(ruleBasedDelivery);
                    recipientSortedDeliveries.Add(recipientName, deliveryList);
                }
                else
                {
                    recipientSortedDeliveries[recipientName].Add(ruleBasedDelivery);
                }
            }

            using (var memoryStream = new MemoryStream())
            {
                using (ZipFile zip = new ZipFile())
                {
                    foreach (var recipient in recipientSortedDeliveries.Keys)
                    {
                        var filename = $"Rechnung {DateTime.Now.Date.ToString("yyyy-MM-dd")} {recipient}.xlsx";
                        filename = filename.SanitizeFilename();


                        using (ExcelPackage package = new ExcelPackage())
                        {
                            var nrOfColumns = 14;
                            // add a new worksheet to the empty workbook
                            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Tabelle");
                            //Add the headers
                            worksheet.Cells[1, 1].Value = "Flugnr";
                            worksheet.Cells[1, 2].Value = "Pilot";
                            worksheet.Cells[1, 3].Value = "Mitgliedernummer";
                            worksheet.Cells[1, 4].Value = "Empfänger";
                            worksheet.Cells[1, 5].Value = "Adressnummer";
                            worksheet.Cells[1, 6].Value = "Flugdatum";
                            worksheet.Cells[1, 7].Value = "Immatrikulation";
                            worksheet.Cells[1, 8].Value = "Flugart";
                            worksheet.Cells[1, 9].Value = "Position";
                            worksheet.Cells[1, 10].Value = "Produkt";
                            worksheet.Cells[1, 11].Value = "Produktbezeichnung";
                            worksheet.Cells[1, 12].Value = "Anzahl";
                            worksheet.Cells[1, 13].Value = "Einheit";
                            worksheet.Cells[1, 14].Value = "Schulung";

                            if (_addFlightIdColumn)
                            {
                                worksheet.Cells[1, 15].Value = "FlightId";
                                nrOfColumns = 15;
                            }
                            worksheet.Cells[1, 1, 1, nrOfColumns].Style.Font.Bold = true;

                            int flightNr = 1;
                            int rowNumber = 2;

                            foreach (var ruleBasedDelivery in recipientSortedDeliveries[recipient].OrderBy(o => o.FlightInformation.FlightDate))
                            {
                                int flightBeginRowNumber = rowNumber;

                                foreach (var flightInvoiceLineItem in ruleBasedDelivery.DeliveryItems.OrderBy(o => o.Position))
                                {
                                    worksheet.Cells[rowNumber, 1].Value = flightNr;
                                    worksheet.Cells[rowNumber, 2].Value =
                                        ruleBasedDelivery.FlightInformation.PilotName;
                                    worksheet.Cells[rowNumber, 3].Value =
                                        ruleBasedDelivery.FlightInformation.PilotPersonClubMemberNumber;
                                    worksheet.Cells[rowNumber, 4].Value =
                                        ruleBasedDelivery.RecipientDetails.RecipientName;
                                    worksheet.Cells[rowNumber, 5].Value =
                                        ruleBasedDelivery.RecipientDetails.PersonClubMemberNumber;
                                    worksheet.Cells[rowNumber, 6].Value = ruleBasedDelivery.FlightInformation.FlightDate;
                                    //.ToShortDateString();
                                    worksheet.Cells[rowNumber, 7].Value = ruleBasedDelivery.FlightInformation.AircraftImmatriculation;
                                    worksheet.Cells[rowNumber, 8].Value = ruleBasedDelivery.DeliveryInformation;
                                    worksheet.Cells[rowNumber, 9].Value = flightInvoiceLineItem.Position;
                                    worksheet.Cells[rowNumber, 10].Value = flightInvoiceLineItem.ArticleNumber;
                                    worksheet.Cells[rowNumber, 11].Value = flightInvoiceLineItem.ItemText;
                                    worksheet.Cells[rowNumber, 12].Value = flightInvoiceLineItem.Quantity;
                                    worksheet.Cells[rowNumber, 13].Value = flightInvoiceLineItem.UnitType;
                                    worksheet.Cells[rowNumber, 14].Value = ruleBasedDelivery.AdditionalInformation;

                                    if (_addFlightIdColumn) worksheet.Cells[rowNumber, 15].Value = ruleBasedDelivery.FlightInformation.FlightId;
                                    rowNumber++;
                                }

                                flightNr++;

                                if (flightNr%2 == 0 && rowNumber > flightBeginRowNumber)
                                {
                                    using (var range = worksheet.Cells[flightBeginRowNumber, 1, rowNumber - 1, nrOfColumns])
                                    {
                                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                                    }
                                }
                            }

                            if (rowNumber > 2)
                            {
                                worksheet.Cells[2, 1, rowNumber - 1, nrOfColumns].Style.Numberformat.Format = "@"; //Format as text
                                worksheet.Cells[2, 6, rowNumber - 1, 6].Style.Numberformat.Format = "dd.mm.yyyy"; //Format as date
                                worksheet.Cells[2, 9, rowNumber - 1, 10].Style.Numberformat.Format = "0"; //Format as number --> Item position
                                worksheet.Cells[2, 12, rowNumber - 1, 12].Style.Numberformat.Format = "0.00"; //Format as number with 2 decimals --> Quantity
                            }

                            worksheet.Cells.AutoFitColumns(0); //Autofit columns for all cells

                            // set some document properties
                            package.Workbook.Properties.Title = "Rechnung für " + recipient;
                            package.Workbook.Properties.Author = "FLS Server (P. Schuler)";

                            // set some extended property values
                            package.Workbook.Properties.Company = "Flight Logging System";

                            // save our new workbook and we are done!
                            var excelBytes = package.GetAsByteArray();
                            zip.AddEntry(filename, excelBytes.ToMemoryStream());
                        }
                    }

                    zip.Save(memoryStream);
                }

                return memoryStream.ToArray();
            }
        }
        
        public static byte[] GetPersonExcelPackage(List<PersonDetails> personDetailList, string title)
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Personen");
                var headerRow = 4;
                //Add the headers
                worksheet.Cells[1, 1].Value = title;
                worksheet.Cells[3, 1].Value = "Letzte Aktualisierung:";
                worksheet.Cells[3, 2].Value = DateTime.Today;
                worksheet.Cells[3, 2].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                worksheet.Cells[headerRow, 1].Value = "Nachname";
                worksheet.Cells[headerRow, 2].Value = "Vorname";
                worksheet.Cells[headerRow, 3].Value = "Strasse";
                worksheet.Cells[headerRow, 4].Value = "PLZ";
                worksheet.Cells[headerRow, 5].Value = "Ort";
                worksheet.Cells[headerRow, 6].Value = "Tel Privat";
                worksheet.Cells[headerRow, 7].Value = "Tel Geschäft";
                worksheet.Cells[headerRow, 8].Value = "Tel Mobile";
                worksheet.Cells[headerRow, 9].Value = "Email Privat";
                worksheet.Cells[headerRow, 10].Value = "Email Geschäft";

                worksheet.Cells[headerRow, 1, headerRow, 10].Style.Font.Bold = true;

                int rowNumber = headerRow + 1;

                foreach (var personDetails in personDetailList)
                {
                    worksheet.Cells[rowNumber, 1].Value = personDetails.Lastname;
                    worksheet.Cells[rowNumber, 2].Value = personDetails.Firstname;
                    worksheet.Cells[rowNumber, 3].Value = personDetails.AddressLine1;
                    worksheet.Cells[rowNumber, 4].Value = personDetails.ZipCode;
                    worksheet.Cells[rowNumber, 5].Value = personDetails.City;
                    worksheet.Cells[rowNumber, 6].Value = personDetails.PrivatePhoneNumber;
                    worksheet.Cells[rowNumber, 7].Value = personDetails.BusinessPhoneNumber;
                    worksheet.Cells[rowNumber, 8].Value = personDetails.MobilePhoneNumber;
                    worksheet.Cells[rowNumber, 9].Value = personDetails.PrivateEmail;
                    worksheet.Cells[rowNumber, 10].Value = personDetails.BusinessEmail;
                    rowNumber++;

                    if (rowNumber % 2 == 0)
                    {
                        using (var range = worksheet.Cells[rowNumber, 1, rowNumber, 10])
                        {
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        }
                    }
                }

                worksheet.Cells[headerRow, 1, rowNumber - 1, 10].Style.Numberformat.Format = "@";   //Format as text
                worksheet.Cells.AutoFitColumns(0);  //Autofit columns for all cells

                // set some document properties
                package.Workbook.Properties.Title = title;
                package.Workbook.Properties.Author = "Flight Logging System";

                // set some extended property values
                package.Workbook.Properties.Company = "Flight Logging System";

                // save our new workbook and we are done!
                return package.GetAsByteArray();
            }
        }
    }
}
