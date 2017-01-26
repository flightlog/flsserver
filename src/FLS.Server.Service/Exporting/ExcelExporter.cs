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
using Ionic.Zip;
using NLog;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace FLS.Server.Service.Exporting
{
    /// <summary>
    /// Excel exporter helper class which uses EPPlus library.
    /// </summary>
    public static class ExcelExporter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static byte[] ExportInvoicesToExcel(List<DeliveryDetails> flightInvoiceDetailList)
        {
            var list = new List<string>();
            foreach (var ruleBasedDelivery in flightInvoiceDetailList)
            {
                //check if there are some line items in the invoice, if not, check next invoice
                if (ruleBasedDelivery.DeliveryItems.Any() == false) continue;

                if (list.Contains(ruleBasedDelivery.RecipientDetails.RecipientName) == false)
                {
                    list.Add(ruleBasedDelivery.RecipientDetails.RecipientName);
                }
            }

            using (var memoryStream = new MemoryStream())
            {
                using (ZipFile zip = new ZipFile())
                {
                    foreach (var recipient in list)
                    {
                        var filename = $"Rechnung {DateTime.Now.Date.ToString("yyyy-MM-dd")} {recipient}.xlsx";
                        filename = filename.SanitizeFilename();


                        using (ExcelPackage package = new ExcelPackage())
                        {
                            // add a new worksheet to the empty workbook
                            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Tabelle");
                            //Add the headers
                            worksheet.Cells[1, 1].Value = "Flugnr";
                            worksheet.Cells[1, 2].Value = "Pilot";
                            worksheet.Cells[1, 3].Value = "Mitgliedernummer";
                            worksheet.Cells[1, 4].Value = "Flugdatum";
                            worksheet.Cells[1, 5].Value = "Immatrikulation";
                            worksheet.Cells[1, 6].Value = "Flugart";
                            worksheet.Cells[1, 7].Value = "Position";
                            worksheet.Cells[1, 8].Value = "Produkt";
                            worksheet.Cells[1, 9].Value = "Produktbezeichnung";
                            worksheet.Cells[1, 10].Value = "Anzahl";
                            worksheet.Cells[1, 11].Value = "Einheit";
                            worksheet.Cells[1, 12].Value = "Schulung";

                            worksheet.Cells[1, 1, 1, 12].Style.Font.Bold = true;

                            int flightNr = 1;
                            int rowNumber = 2;

                            foreach (var ruleBasedDelivery in flightInvoiceDetailList)
                            {
                                if (ruleBasedDelivery.RecipientDetails.RecipientName != recipient)
                                {
                                    continue;
                                }

                                int flightBeginRowNumber = rowNumber;

                                foreach (var flightInvoiceLineItem in ruleBasedDelivery.DeliveryItems)
                                {
                                    worksheet.Cells[rowNumber, 1].Value = flightNr;
                                    worksheet.Cells[rowNumber, 2].Value =
                                        ruleBasedDelivery.RecipientDetails.RecipientName;
                                    worksheet.Cells[rowNumber, 3].Value =
                                        ruleBasedDelivery.RecipientDetails.PersonClubMemberNumber;
                                    worksheet.Cells[rowNumber, 4].Value = ruleBasedDelivery.FlightInformation.FlightDate;
                                    //.ToShortDateString();
                                    worksheet.Cells[rowNumber, 5].Value = ruleBasedDelivery.FlightInformation.AircraftImmatriculation;
                                    worksheet.Cells[rowNumber, 6].Value = ruleBasedDelivery.DeliveryInformation;
                                    worksheet.Cells[rowNumber, 7].Value = flightInvoiceLineItem.Position;
                                    worksheet.Cells[rowNumber, 8].Value = flightInvoiceLineItem.ArticleNumber;
                                    worksheet.Cells[rowNumber, 9].Value = flightInvoiceLineItem.ItemText;
                                    worksheet.Cells[rowNumber, 10].Value = flightInvoiceLineItem.Quantity;
                                    worksheet.Cells[rowNumber, 11].Value = flightInvoiceLineItem.UnitType;
                                    worksheet.Cells[rowNumber, 12].Value = ruleBasedDelivery.AdditionalInformation;
                                    rowNumber++;
                                }

                                flightNr++;

                                if (flightNr%2 == 0 && rowNumber > flightBeginRowNumber)
                                {
                                    using (var range = worksheet.Cells[flightBeginRowNumber, 1, rowNumber - 1, 12])
                                    {
                                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                                    }
                                }
                            }

                            if (rowNumber > 2)
                            {
                                worksheet.Cells[2, 1, rowNumber - 1, 12].Style.Numberformat.Format = "@";
                                    //Format as text
                                worksheet.Cells[2, 4, rowNumber - 1, 4].Style.Numberformat.Format = "dd.mm.yyyy";
                                //Format as date
                                worksheet.Cells[2, 7, rowNumber - 1, 8].Style.Numberformat.Format = "0";
                                    //Format as number
                                worksheet.Cells[2, 10, rowNumber - 1, 10].Style.Numberformat.Format = "0";
                            }

                            //Format as number
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

        public static void ExportFlightReportToExcel(FlightReportData flightReportData,
                                                string exportDirectory, string filename)
        {
            if (Directory.Exists(exportDirectory) == false)
            {
                Directory.CreateDirectory(exportDirectory);
            }

            DirectoryInfo outputDir = new DirectoryInfo(exportDirectory);

            if (filename.EndsWith(".xlsx") == false)
            {
                filename += ".xlsx";
            }

            FileInfo newFile = new FileInfo(outputDir.FullName + @"\" + filename);

            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(outputDir.FullName + @"\" + filename);
            }

            using (ExcelPackage package = new ExcelPackage(newFile))
            {

                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Statistik");
                //Add the headers
                worksheet.Cells[1, 1].Value = "Statistik vom " + flightReportData.StatisticStartDateTime.ToShortDateString() + " bis " + flightReportData.StatisticEndDateTime.ToShortDateString();
                worksheet.Cells[3, 1].Value = "Monat";
                worksheet.Cells[3, 2].Value = "Flugzeit";
                worksheet.Cells[3, 3].Value = "Flugzeit in Min.";
                worksheet.Cells[3, 4].Value = "Anzahl Landungen";

                worksheet.Cells[1, 1, 1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1, 1, 1].Style.Font.Size = 14;

                worksheet.Cells[3, 1, 3, 4].Style.Font.Bold = true;

                int rowNumber = 2;
                int numberOfLandings = 0;
                TimeSpan totalFlightDuration = new TimeSpan(0);

                for (int month = 1; month <= flightReportData.StatisticEndDateTime.Month; month++)
                {
                    foreach (
                        var flightDetails in
                            flightReportData.FlightDetailsReportDatas.AsQueryable().Where(s => s.StartDateTime.Value.Month == month).OrderBy(r => r.StartDateTime.Value))
                    {
                        if (flightDetails.NrOfLdgs.HasValue)
                        {
                            numberOfLandings += flightDetails.NrOfLdgs.Value;
                        }

                        totalFlightDuration += flightDetails.FlightDuration;

                    }

                    worksheet.Cells[month + 3, 1].Value =
                        new DateTime(flightReportData.StatisticStartDateTime.Year, 1, 1).AddMonths(month - 1);
                    worksheet.Cells[month + 3, 2].Value = totalFlightDuration;
                    worksheet.Cells[month + 3, 3].Value = totalFlightDuration.TotalMinutes;
                    worksheet.Cells[month + 3, 4].Value = numberOfLandings;

                    numberOfLandings = 0;
                    totalFlightDuration = new TimeSpan(0);
                }

                int totalRowNr = flightReportData.StatisticEndDateTime.Month + 4;
                worksheet.Cells[totalRowNr, 1].Value = "Total:";

                worksheet.Cells[totalRowNr, 2].Formula = string.Format("Sum({0})", new ExcelAddress(4, 2, totalRowNr - 1, 2).Address);
                worksheet.Cells[totalRowNr, 2].Style.Font.Bold = true;
                worksheet.Cells[totalRowNr, 2].Style.Numberformat.Format = "[hh]:mm";

                worksheet.Cells[totalRowNr, 3].Formula = string.Format("Sum({0})", new ExcelAddress(4, 3, totalRowNr - 1, 3).Address);
                worksheet.Cells[totalRowNr, 3].Style.Font.Bold = true;
                worksheet.Cells[totalRowNr, 3].Style.Numberformat.Format = "0";

                worksheet.Cells[totalRowNr, 4].Formula = string.Format("Sum({0})", new ExcelAddress(4, 4, totalRowNr - 1, 4).Address);
                worksheet.Cells[totalRowNr, 4].Style.Font.Bold = true;
                worksheet.Cells[totalRowNr, 4].Style.Numberformat.Format = "0";

                worksheet.Cells[3, 1, totalRowNr - 1, 1].Style.Numberformat.Format = "MMMM yyyy";   //Format as date
                worksheet.Cells[3, 2, totalRowNr - 1, 2].Style.Numberformat.Format = "[hh]:mm";   //Format as flighttime
                worksheet.Cells[3, 3, totalRowNr - 1, 4].Style.Numberformat.Format = "0";   //Format as number

                worksheet.Cells[1, 1, 1, 4].Merge = true;

                worksheet.Cells.AutoFitColumns(0);  //Autofit columns for all cells



                // add a new worksheet to the empty workbook
                worksheet = package.Workbook.Worksheets.Add("Details");
                //Add the headers
                worksheet.Cells[1, 1].Value = "Flugdatum";
                worksheet.Cells[1, 2].Value = "Pilot";
                worksheet.Cells[1, 3].Value = "Immatrikulation";
                worksheet.Cells[1, 4].Value = "Flugart";
                worksheet.Cells[1, 5].Value = "Startzeit (UTC)";
                worksheet.Cells[1, 6].Value = "Landezeit (UTC)";
                worksheet.Cells[1, 7].Value = "Flugzeit";
                worksheet.Cells[1, 8].Value = "Anz. Landungen";
                worksheet.Cells[1, 9].Value = "Startort";
                worksheet.Cells[1, 10].Value = "Landeort";
                worksheet.Cells[1, 11].Value = "Soloflug";
                worksheet.Cells[1, 12].Value = "Weitere Crewmitglieder";
                worksheet.Cells[1, 13].Value = "Motorlaufzeit";

                worksheet.Cells[1, 1, 1, 13].Style.Font.Bold = true;

                rowNumber = 2;

                foreach (var flightDetails in flightReportData.FlightDetailsReportDatas)
                {
                    worksheet.Cells[rowNumber, 1].Value = flightDetails.StartDateTime?.Date;
                    worksheet.Cells[rowNumber, 2].Value = flightDetails.PilotPersonName;
                    worksheet.Cells[rowNumber, 3].Value = flightDetails.AircraftImmatriculation;
                    worksheet.Cells[rowNumber, 4].Value = flightDetails.FlightType;
                    worksheet.Cells[rowNumber, 5].Value = flightDetails.StartDateTime?.TimeOfDay;
                    worksheet.Cells[rowNumber, 6].Value = flightDetails.LdgDateTime?.TimeOfDay;
                    worksheet.Cells[rowNumber, 7].Value = flightDetails.FlightDuration;
                    worksheet.Cells[rowNumber, 8].Value = flightDetails.NrOfLdgs;
                    worksheet.Cells[rowNumber, 9].Value = flightDetails.StartLocation;
                    worksheet.Cells[rowNumber, 10].Value = flightDetails.LdgLocation;

                    if (flightDetails.IsSoloFlight)
                    {
                        worksheet.Cells[rowNumber, 11].Value = "X";
                    }
                    else
                    {
                        worksheet.Cells[rowNumber, 11].Value = "";
                    }

                    worksheet.Cells[rowNumber, 12].Value = flightDetails.AdditionalFlightCrewMembers;

                    //if (flightDetails.EngineTime.HasValue)
                    //{
                    //    worksheet.Cells[rowNumber, 13].Value = flightDetails.EngineTime.Value.TimeOfDay;
                    //}

                    rowNumber++;

                    if (rowNumber % 2 == 0)
                    {
                        using (var range = worksheet.Cells[rowNumber - 1, 1, rowNumber - 1, 13])
                        {
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        }
                    }
                }

                if (rowNumber > 2)
                {
                    worksheet.Cells[2, 2, rowNumber - 1, 13].Style.Numberformat.Format = "@"; //Format as text
                    worksheet.Cells[2, 1, rowNumber - 1, 1].Style.Numberformat.Format = "dd.mm.yyyy"; //Format as date
                    worksheet.Cells[2, 5, rowNumber - 1, 6].Style.Numberformat.Format = "hh:mm";
                    worksheet.Cells[2, 7, rowNumber - 1, 7].Style.Numberformat.Format = "[hh]:mm";
                        //Format as flighttime
                    worksheet.Cells[2, 8, rowNumber - 1, 8].Style.Numberformat.Format = "0"; //Format as number
                    worksheet.Cells.AutoFitColumns(0); //Autofit columns for all cells
                }



                // set some document properties
                package.Workbook.Properties.Title = "Statistik";
                package.Workbook.Properties.Author = "FLS Server (P. Schuler)";

                // set some extended property values
                package.Workbook.Properties.Company = "Flight Logging System";

                // save our new workbook and we are done!
                package.Save();

                Logger.Debug(string.Format("Excel file {0} written.", newFile.FullName));
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
