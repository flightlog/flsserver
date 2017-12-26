using System;
using System.Linq;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Person;
using FLS.Server.Service;
using FLS.Server.Service.Identity;
using FLS.Server.TestInfrastructure;
using FLS.Server.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using NLog;
using System.IO;
using AutoMapper;
using FLS.Server.Data.DbEntities;
using System.Collections.Generic;
using AutoMapper.Configuration;
using AutoMapper.Mappers;
using FLS.Server.Data.Mapping;
using FLS.Server.Service.Extensions;
using OfficeOpenXml;

namespace FLS.Server.Tests.ServiceTests
{
    [TestClass]
    public class PersonServiceTest : BaseTest
    {
        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void PersonServiceTestInitialize(TestContext testContext)
        {
            //Stopwatch sw = Stopwatch.StartNew();
            //using (var context = new FLSDataEntities())
            //{
            //    var countries = context.Countries.ToList();
            //}
            //sw.Stop();
            //Console.WriteLine(string.Format("Database connection and loading all countries took: {0} ms", sw.ElapsedMilliseconds));
        }
        
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        [TestMethod]
        [TestCategory("Service")]
        public void ImportPersonExcelFileTest()
        {
            var csvFileName = @"C:\Projects\flsserver\Data\AdressexportFLS 2017-11-27.csv";
            var excelFileName = @"C:\Projects\flsserver\Data\AdressexportFLS 2017-11-27.xls";

            string worksheetsName = "TEST";

            bool firstRowIsHeader = true;

            var format = new ExcelTextFormat();
            format.Delimiter = ';';
            //format.EOL = "\r";              // DEFAULT IS "\r\n";
                                            // format.TextQualifier = '"';

            if (File.Exists(excelFileName))
                File.Delete(excelFileName);

            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFileName)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(worksheetsName);
                worksheet.Cells["A1"].LoadFromText(new FileInfo(csvFileName), format, OfficeOpenXml.Table.TableStyles.Medium27, firstRowIsHeader);
                package.Save();
            }

            var bytes = File.ReadAllBytes(excelFileName);
            PersonService.ImportPersonExcelFile(bytes);
        }

        [TestMethod]
        [TestCategory("Service")]
        public void PersonAutoMapperTest()
        {
            var person = new Person()
            {
                Lastname = "Huber",
                Firstname = "Peter",
                AddressLine1 = "Hauptstrasse",
                Zip = "8000",
                HasGliderTraineeLicence = false,
                HasGliderPilotLicence = true,
                HasGliderInstructorLicence = true,
                HasGliderPAXLicence = true,
                LicenceNumber = "CH.FCL.99999",
                EmailPrivate = "test@test.ch"
            };

            var importPersonDetails = new PersonDetails()
            {
                Lastname = person.Lastname,
                Firstname = person.Firstname,
                AddressLine1 = person.AddressLine1 + " 1",
                ZipCode = person.Zip,
                City = "Zürich",
                Birthday = new DateTime(1978, 12, 25)
            };
            
            var config = new MapperConfiguration(cfg => cfg.CreateMap<PersonDetails, Person>()
            .ForMember(d => d.Lastname, o => o.MapFrom(s => s.Lastname))
            .ForMember("Firstname", o => o.MapFrom(s => s.Firstname))
            .ForMember("AddressLine1", o => o.MapFrom(s => s.AddressLine1))
            .ForMember(d => d.Zip, o => o.MapFrom(s => s.ZipCode))
            .ForMember(d => d.City, o => o.MapFrom(s => s.City))
            .ForMember(d => d.Birthday, o => o.MapFrom(s => s.Birthday))
            .ForAllOtherMembers(opts => opts.Ignore()));
            var mapper = config.CreateMapper();
            mapper.Map<PersonDetails, Person>(importPersonDetails, person);

            Assert.AreEqual(person.Lastname, importPersonDetails.Lastname);
            Assert.AreEqual(person.Firstname, importPersonDetails.Firstname);
            Assert.AreEqual(person.AddressLine1, importPersonDetails.AddressLine1);
            Assert.AreEqual(person.Zip, importPersonDetails.ZipCode);

            Assert.AreEqual(person.LicenceNumber, "CH.FCL.99999");
            Assert.AreEqual(person.HasGliderTraineeLicence, false);
            Assert.AreEqual(person.HasGliderPilotLicence, true);
            Assert.AreEqual(person.HasGliderInstructorLicence, true);
            Assert.AreEqual(person.HasGliderPAXLicence, true);
            Assert.AreEqual(person.EmailPrivate, "test@test.ch");

            Assert.AreEqual(person.City, "Zürich");
            Assert.AreEqual(person.Birthday.Value.Date, new DateTime(1978, 12, 25).Date);
        }

        [TestMethod]
        [TestCategory("Service")]
        public void PersonAutoMapperMappingTest()
        {
            var person = new Person()
            {
                Lastname = "Huber",
                Firstname = "Peter",
                AddressLine1 = "Hauptstrasse",
                Zip = "8000",
                HasGliderTraineeLicence = false,
                HasGliderPilotLicence = true,
                HasGliderInstructorLicence = true,
                HasGliderPAXLicence = true,
                LicenceNumber = "CH.FCL.99999",
                EmailPrivate = "test@test.ch"
            };

            var importPersonDetails = new PersonDetails()
            {
                Lastname = person.Lastname,
                Firstname = person.Firstname,
                AddressLine1 = person.AddressLine1 + " 1",
                ZipCode = person.Zip,
                City = "Zürich",
                Birthday = new DateTime(1978, 12, 25)
            };

            var mapping = new Dictionary<string, string>()
                {
                    {"AdressNrADR", "ClubRelatedPersonDetails.MemberNumber"},
                    {"Adresszeile1", "AddressLine1"},
                    {"EMail", "PrivateEmail"},
                    {"Fax", "FaxNumber"},
                    {"GeburtsDatum", "Birthday"},
                    {"LandPRO", "LicenceNumber"},
                    {"Natel", "MobilePhoneNumber"},
                    {"Name", "Lastname"},
                    {"Ort", "City"},
                    {"PLZ", "ZipCode"},
                    {"KantonPRO", "Region"},
                    {"Strasse", "AddressLine2"},
                    {"TelPrivat", "PrivatePhoneNumber"},
                    {"TelZentrale", "BusinessPhoneNumber"},
                    {"Vorname", "Firstname"},
                    {"AdressGruppen", "PersonPersonCategories"}
                };

            var mappedProperties = mapping.Values.ToList();

            var cfg = new MapperConfigurationExpression();
            var map = cfg.CreateMap<PersonDetails, Person>();
            if (mappedProperties.Contains("Lastname"))
                map.ForMember(d => d.Lastname, o => o.MapFrom(s => s.Lastname));
            if (mappedProperties.Contains("Firstname"))
                map.ForMember(d => d.Firstname, o => o.MapFrom(s => s.Firstname));
            if (mappedProperties.Contains("AddressLine1"))
                map.ForMember(d => d.AddressLine1, o => o.MapFrom(s => s.AddressLine1));
            if (mappedProperties.Contains("ZipCode"))
                map.ForMember(d => d.Zip, o => o.MapFrom(s => s.ZipCode));
            if (mappedProperties.Contains("City"))
                map.ForMember(d => d.City, o => o.MapFrom(s => s.City));
            if (mappedProperties.Contains("Birthday"))
                map.ForMember(d => d.Birthday, o => o.MapFrom(s => s.Birthday));

            map.ForAllOtherMembers(opts => opts.Ignore());

            var mapperConfig = new MapperConfiguration(cfg);
            var mapper = mapperConfig.CreateMapper();

            mapper.Map<PersonDetails, Person>(importPersonDetails, person);

            Assert.AreEqual(person.Lastname, importPersonDetails.Lastname);
            Assert.AreEqual(person.Firstname, importPersonDetails.Firstname);
            Assert.AreEqual(person.AddressLine1, importPersonDetails.AddressLine1);
            Assert.AreEqual(person.Zip, importPersonDetails.ZipCode);

            Assert.AreEqual(person.LicenceNumber, "CH.FCL.99999");
            Assert.AreEqual(person.HasGliderTraineeLicence, false);
            Assert.AreEqual(person.HasGliderPilotLicence, true);
            Assert.AreEqual(person.HasGliderInstructorLicence, true);
            Assert.AreEqual(person.HasGliderPAXLicence, true);
            Assert.AreEqual(person.EmailPrivate, "test@test.ch");

            Assert.AreEqual(person.City, "Zürich");
            Assert.AreEqual(person.Birthday.Value.Date, new DateTime(1978, 12, 25).Date);
        }

    }
}
