using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using FLS.Common.Extensions;
using FLS.Common.Utils;
using FLS.Common.Validators;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Aircraft;
using FLS.Data.WebApi.AircraftReservation;
using FLS.Data.WebApi.Articles;
using FLS.Data.WebApi.Audit;
using FLS.Data.WebApi.Club;
using FLS.Data.WebApi.Dashboard;
using FLS.Data.WebApi.Emails;
using FLS.Data.WebApi.Flight;
using FLS.Data.WebApi.Globalization;
using FLS.Data.WebApi.Location;
using FLS.Data.WebApi.Person;
using FLS.Data.WebApi.PlanningDay;
using FLS.Data.WebApi.System;
using FLS.Data.WebApi.User;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
using NLog;
using TrackerEnabledDbContext.Common.Models;

namespace FLS.Server.Data.Mapping
{
    public static class MappingExtensions
    {
        #region Metadata
        public static void MapMetaData(this IFLSMetaData source, IFLSMetaData destination)
        {
            source.ArgumentNotNull("source");
            destination.ArgumentNotNull("destination");

            destination.SetPropertyValue("Id", source.Id);
            destination.SetPropertyValue("CreatedByUserId", source.CreatedByUserId);
            destination.SetPropertyValue("CreatedOn", source.CreatedOn.SetAsUtc());
            destination.SetPropertyValue("DeletedByUserId", source.DeletedByUserId);
            destination.SetPropertyValue("DeletedOn", source.DeletedOn.SetAsUtc());
            destination.SetPropertyValue("ModifiedByUserId", source.ModifiedByUserId);
            destination.SetPropertyValue("ModifiedOn", source.ModifiedOn.SetAsUtc());
            destination.SetPropertyValue("OwnerId", source.OwnerId);
            destination.SetPropertyValue("OwnershipType", source.OwnershipType);
            destination.SetPropertyValue("RecordState", source.RecordState);

            if (destination.CreatedOn.Kind == DateTimeKind.Unspecified)
            {
                destination.SetPropertyValue("CreatedOn", source.CreatedOn.SetAsUtc());
            }

            if (destination.ModifiedOn.HasValue && destination.ModifiedOn.Value.Kind == DateTimeKind.Unspecified)
            {
                destination.SetPropertyValue("ModifiedOn", source.ModifiedOn.SetAsUtc());
            }

            if (destination.DeletedOn.HasValue && destination.DeletedOn.Value.Kind == DateTimeKind.Unspecified)
            {
                destination.SetPropertyValue("DeletedOn", source.DeletedOn.SetAsUtc());
            }
        }

        public static void MapTimeStampsMetaData(this IFLSMetaData source, IFLSMetaData destination)
        {
            source.ArgumentNotNull("source");
            destination.ArgumentNotNull("destination");

            destination.SetPropertyValue("CreatedOn", source.CreatedOn.SetAsUtc());
            destination.SetPropertyValue("DeletedOn", source.DeletedOn.SetAsUtc());
            destination.SetPropertyValue("ModifiedOn", source.ModifiedOn.SetAsUtc());

            if (destination.CreatedOn.Kind == DateTimeKind.Unspecified)
            {
                destination.SetPropertyValue("CreatedOn", source.CreatedOn.SetAsUtc());
            }

            if (destination.ModifiedOn.HasValue && destination.ModifiedOn.Value.Kind == DateTimeKind.Unspecified)
            {
                destination.SetPropertyValue("ModifiedOn", source.ModifiedOn.SetAsUtc());
            }

            if (destination.DeletedOn.HasValue && destination.DeletedOn.Value.Kind == DateTimeKind.Unspecified)
            {
                destination.SetPropertyValue("DeletedOn", source.DeletedOn.SetAsUtc());
            }
        }
        #endregion Metadata

        #region Aircraft
        public static AircraftListItem ToAircraftListItem(this Aircraft entity, AircraftListItem listItem = null)
        {
            entity.ArgumentNotNull("entity");

            if (listItem == null)
            {
                listItem = new AircraftListItem();
            }

            listItem.AircraftId = entity.AircraftId;
            listItem.CompetitionSign = entity.CompetitionSign;
            listItem.Immatriculation = entity.Immatriculation;
            listItem.IsTowingOrWinchRequired = entity.IsTowingOrWinchRequired;
            listItem.IsTowingstartAllowed = entity.IsTowingstartAllowed;
            listItem.IsWinchstartAllowed = entity.IsWinchstartAllowed;
            listItem.NrOfSeats = entity.NrOfSeats;
            listItem.HasEngine = entity.HasEngine;
            //listItem.EngineOperatorCounterPrecision = entity.EngineOperatorCounterPrecision;

            if (entity.CurrentAircraftAircraftState == null)
            {
                listItem.CurrentAircraftState = 0;
            }
            else
            {
                listItem.CurrentAircraftState = entity.CurrentAircraftAircraftState.AircraftStateId;
            }

            return listItem;
        }

        public static AircraftOverview ToAircraftOverview(this Aircraft entity, AircraftOverview overview = null)
        {
            entity.ArgumentNotNull("entity");

            if (overview == null)
            {
                overview = new AircraftOverview();
            }

            overview.AircraftId = entity.AircraftId;
            overview.AircraftModel = entity.AircraftModel;
            overview.CompetitionSign = entity.CompetitionSign;
            overview.Immatriculation = entity.Immatriculation;
            overview.IsTowingAircraft = entity.IsTowingAircraft;
            overview.IsTowingOrWinchRequired = entity.IsTowingOrWinchRequired;
            overview.IsTowingstartAllowed = entity.IsTowingstartAllowed;
            overview.IsWinchstartAllowed = entity.IsWinchstartAllowed;
            overview.ManufacturerName = entity.ManufacturerName;
            overview.NrOfSeats = entity.NrOfSeats;
            overview.HasEngine = entity.HasEngine;
            //overview.EngineOperatorCounterPrecision = entity.EngineOperatorCounterPrecision;

            if (entity.CurrentAircraftAircraftState == null)
            {
                overview.CurrentAircraftState = 0;
            }
            else
            {
                overview.CurrentAircraftState = entity.CurrentAircraftAircraftState.AircraftStateId;
            }

            if (entity.AircraftOwnerClub != null)
            {
                overview.AircraftOwnerName = entity.AircraftOwnerClub.Clubname;
            }
            else if (entity.AircraftOwnerPerson != null)
            {
                overview.AircraftOwnerName = entity.AircraftOwnerPerson.DisplayName;
            }

            if (entity.AircraftType != null)
            {
                overview.AircraftTypeName = entity.AircraftType.AircraftTypeName;
            }

            return overview;
        }

        public static AircraftDetails ToAircraftDetails(this Aircraft entity, AircraftDetails details = null)
        {
            entity.ArgumentNotNull("entity");

            if (details == null)
            {
                details = new AircraftDetails();
            }

            details.AircraftId = entity.AircraftId;

            details.AircraftModel = entity.AircraftModel;
            details.AircraftType = entity.AircraftTypeId;
            details.CompetitionSign = entity.CompetitionSign;
            details.Immatriculation = entity.Immatriculation;
            details.IsTowingAircraft = entity.IsTowingAircraft;
            details.IsTowingOrWinchRequired = entity.IsTowingOrWinchRequired;
            details.IsTowingstartAllowed = entity.IsTowingstartAllowed;
            details.IsWinchstartAllowed = entity.IsWinchstartAllowed;
            details.ManufacturerName = entity.ManufacturerName;
            details.NrOfSeats = entity.NrOfSeats;
            details.Comment = entity.Comment;
            details.DaecIndex = entity.DaecIndex;
            details.FLARMId = entity.FLARMId;
            details.AircraftSerialNumber = entity.AircraftSerialNumber;
            details.YearOfManufacture = entity.YearOfManufacture.SetAsUtc();
            details.NoiseClass = entity.NoiseClass;
            details.NoiseLevel = entity.NoiseLevel;
            details.MTOM = entity.MTOM;
            details.AircraftOwnerClubId = entity.AircraftOwnerClubId;
            details.AircraftOwnerPersonId = entity.AircraftOwnerPersonId;
            details.FlightOperatingCounterUnitTypeId = entity.FlightOperatingCounterUnitTypeId;
            details.EngineOperatingCounterUnitTypeId = entity.EngineOperatingCounterUnitTypeId;
            details.SpotLink = entity.SpotLink;

            if (entity.CurrentAircraftAircraftState != null)
            {
                if (details.AircraftStateData == null)
                {
                    details.AircraftStateData = new AircraftStateData();
                }

                var currentState = entity.CurrentAircraftAircraftState;
                var detailState = details.AircraftStateData;

                detailState.AircraftId = entity.AircraftId;
                detailState.AircraftState = currentState.AircraftStateId;
                detailState.NoticedByPersonId = currentState.NoticedByPersonId;
                detailState.ValidFrom = currentState.ValidFrom.SetAsUtc();
                detailState.ValidTo = currentState.ValidTo.SetAsUtc();
                detailState.Remarks = currentState.Remarks;
            }
      
            return details;
        }

        public static Aircraft ToAircraft(this AircraftDetails details, Aircraft entity = null, bool overwriteAircraftReservationId = false)
        {
            details.ArgumentNotNull("details");

            if (entity == null)
            {
                entity = new Aircraft();
            }

            if (overwriteAircraftReservationId) entity.AircraftId = details.AircraftId;

            entity.AircraftModel = details.AircraftModel;
            entity.AircraftTypeId = details.AircraftType;
            entity.CompetitionSign = details.CompetitionSign;
            entity.Immatriculation = details.Immatriculation;
            entity.IsTowingAircraft = details.IsTowingAircraft;
            entity.IsTowingOrWinchRequired = details.IsTowingOrWinchRequired;
            entity.IsTowingstartAllowed = details.IsTowingstartAllowed;
            entity.IsWinchstartAllowed = details.IsWinchstartAllowed;
            entity.ManufacturerName = details.ManufacturerName;
            entity.NrOfSeats = details.NrOfSeats;
            entity.Comment = details.Comment;
            entity.DaecIndex = details.DaecIndex;
            entity.FLARMId = details.FLARMId;
            entity.AircraftSerialNumber = details.AircraftSerialNumber;
            entity.YearOfManufacture = details.YearOfManufacture;
            entity.NoiseClass = details.NoiseClass;
            entity.NoiseLevel = details.NoiseLevel;
            entity.MTOM = details.MTOM;
            entity.AircraftOwnerClubId = details.AircraftOwnerClubId;
            entity.AircraftOwnerPersonId = details.AircraftOwnerPersonId;
            entity.FlightOperatingCounterUnitTypeId = details.FlightOperatingCounterUnitTypeId;
            entity.EngineOperatingCounterUnitTypeId = details.EngineOperatingCounterUnitTypeId;
            entity.SpotLink = details.SpotLink;

            //Check if aircraft state has changes and update it if required
            if (entity.HasAircraftStateChanges(details))
            {
                if (entity.CurrentAircraftAircraftState != null)
                {
                    //update validTo field on current aircraft state
                    entity.CurrentAircraftAircraftState.ValidTo = details.AircraftStateData.ValidFrom;
                }

                //create new aircraft state object (required for history)
                var aircraftAircraftState = new AircraftAircraftState();
                aircraftAircraftState.AircraftStateId = details.AircraftStateData.AircraftState;
                aircraftAircraftState.AircraftId = details.AircraftId;
                aircraftAircraftState.NoticedByPersonId = details.AircraftStateData.NoticedByPersonId;
                aircraftAircraftState.Remarks = details.AircraftStateData.Remarks;
                aircraftAircraftState.ValidFrom = details.AircraftStateData.ValidFrom;
                aircraftAircraftState.ValidTo = details.AircraftStateData.ValidTo;
                
                entity.AircraftAircraftStates.Add(aircraftAircraftState);
            }

            return entity;
        }
        #endregion Aircraft

        #region AircraftReservation
        public static AircraftReservationOverview ToAircraftReservationOverview(this AircraftReservation entity, AircraftReservationOverview overview = null)
        {
            entity.ArgumentNotNull("entity");

            if (overview == null)
            {
                overview = new AircraftReservationOverview();
            }

            overview.AircraftReservationId = entity.AircraftReservationId;

            overview.Start = entity.Start.SetAsUtc();
            overview.End = entity.End.SetAsUtc();
            overview.IsAllDayReservation = entity.IsAllDayReservation;
            overview.Remarks = entity.Remarks;

            if (entity.Aircraft != null)
            {
                overview.Immatriculation = entity.Aircraft.Immatriculation;
            }

            if (entity.PilotPerson != null)
            {
                overview.PilotName = entity.PilotPerson.DisplayName;
            }

            if (entity.Location != null)
            {
                overview.LocationName = entity.Location.LocationName;
            }

            if (entity.InstructorPerson != null)
            {
                overview.InstructorName = entity.InstructorPerson.DisplayName;
            }

            if (entity.ReservationType != null)
            {
                overview.ReservationTypeName = entity.ReservationType.AircraftReservationTypeName;
            }

            return overview;
        }

        public static AircraftReservationDetails ToAircraftReservationDetails(this AircraftReservation entity, AircraftReservationDetails details = null)
        {
            entity.ArgumentNotNull("entity");

            if (details == null)
            {
                details = new AircraftReservationDetails();
            }

            details.AircraftReservationId = entity.AircraftReservationId;

            details.Start = entity.Start.SetAsUtc();
            details.End = entity.End.SetAsUtc();
            details.IsAllDayReservation = entity.IsAllDayReservation;
            details.AircraftId = entity.AircraftId;
            details.PilotPersonId = entity.PilotPersonId;
            details.LocationId = entity.LocationId;
            details.InstructorPersonId = entity.InstructorPersonId;
            details.ReservationTypeId = entity.ReservationTypeId;
            details.Remarks = entity.Remarks;

            return details;
        }

        public static AircraftReservation ToAircraftReservation(this AircraftReservationDetails details, AircraftReservation entity = null, bool overwriteAircraftReservationId = false)
        {
            details.ArgumentNotNull("details");

            if (entity == null)
            {
                entity = new AircraftReservation();
            }

            if (overwriteAircraftReservationId) entity.AircraftReservationId = details.AircraftReservationId;

            entity.Start = details.Start.SetAsUtc();
            entity.End = details.End.SetAsUtc();
            entity.IsAllDayReservation = details.IsAllDayReservation;
            entity.AircraftId = details.AircraftId;
            entity.PilotPersonId = details.PilotPersonId;
            entity.LocationId = details.LocationId;
            entity.InstructorPersonId = details.InstructorPersonId;
            entity.ReservationTypeId = details.ReservationTypeId;
            entity.Remarks = details.Remarks;

            return entity;
        }
        #endregion AircraftReservation

        #region AircraftReservationType
        public static AircraftReservationTypeListItem ToAircraftReservationTypeListItem(this AircraftReservationType entity, AircraftReservationTypeListItem listItem = null)
        {
            entity.ArgumentNotNull("entity");

            if (listItem == null)
            {
                listItem = new AircraftReservationTypeListItem();
            }

            listItem.AircraftReservationTypeId = entity.AircraftReservationTypeId;
            listItem.AircraftReservationTypeName = entity.AircraftReservationTypeName;
            listItem.Remarks = entity.Remarks;
            listItem.IsInstructorRequired = entity.IsInstructorRequired;

            return listItem;
        }
        #endregion AircraftReservationType

        #region AircraftState
        public static AircraftStateListItem ToAircraftStateListItem(this AircraftState entity, AircraftStateListItem listItem = null)
        {
            entity.ArgumentNotNull("entity");

            if (listItem == null)
            {
                listItem = new AircraftStateListItem();
            }

            listItem.AircraftStateId = entity.AircraftStateId;

            listItem.AircraftStateName = entity.AircraftStateName;
            listItem.IsAircraftFlyable = entity.IsAircraftFlyable;
            return listItem;
        }
        #endregion AircraftState

        #region AircraftType
        public static AircraftTypeListItem ToAircraftTypeListItem(this DbEntities.AircraftType entity, AircraftTypeListItem listItem = null)
        {
            entity.ArgumentNotNull("entity");

            if (listItem == null)
            {
                listItem = new AircraftTypeListItem();
            }

            listItem.AircraftTypeId = entity.AircraftTypeId;

            listItem.AircraftTypeName = entity.AircraftTypeName;
            listItem.HasEngine = entity.HasEngine;
            listItem.RequiresTowingInfo = entity.RequiresTowingInfo;
            listItem.MayBeTowingAircraft = entity.MayBeTowingAircraft;

            return listItem;
        }
        #endregion AircraftType


        #region Article
        
        public static ArticleDetails ToArticleDetails(this Article entity, ArticleDetails details = null)
        {
            entity.ArgumentNotNull("entity");

            if (details == null)
            {
                details = new ArticleDetails();
            }

            details.ArticleId = entity.ArticleId;
            details.ArticleNumber = entity.ArticleNumber;
            details.ArticleName = entity.ArticleName;
            details.ArticleInfo = entity.ArticleInfo;
            details.Description = entity.Description;
            details.IsActive = entity.IsActive;

            return details;
        }

        public static Article ToArticle(this ArticleDetails details, Guid clubId, Article entity = null, bool overwriteArticleId = false)
        {
            details.ArgumentNotNull("details");

            if (entity == null)
            {
                entity = new Article();
            }

            entity.ClubId = clubId;

            if (overwriteArticleId) entity.ArticleId = details.ArticleId;
            entity.ArticleNumber = details.ArticleNumber;
            entity.ArticleName = details.ArticleName;
            entity.ArticleInfo = details.ArticleInfo;
            entity.Description = details.Description;
            entity.IsActive = details.IsActive;

            return entity;
        }

        #endregion Article

        #region AssemblyInfo
        public static AssemblyInfo ToAssemblyInfo(this AssemblyBuildInfo assemblyBuildInfo, AssemblyInfo assemblyInfo = null)
        {
            assemblyBuildInfo.ArgumentNotNull("assemblyBuildInfo");

            if (assemblyInfo == null)
            {
                assemblyInfo = new AssemblyInfo();
            }

            assemblyInfo.AssemblyFullName = assemblyBuildInfo.AssemblyFullName;
            assemblyInfo.AssemblyName = assemblyBuildInfo.AssemblyName;
            assemblyInfo.BuildDateTime = assemblyBuildInfo.BuildDateTime;
            assemblyInfo.FileVersion = assemblyBuildInfo.FileVersion;
            assemblyInfo.ManifestModule = assemblyBuildInfo.ManifestModule;
            assemblyInfo.Version = assemblyBuildInfo.Version;

            return assemblyInfo;
        }
        #endregion AssemblyInfo

        #region AuditLog
        public static AuditLogOverview ToAuditLogOverview(this AuditLog entity)
        {
            entity.ArgumentNotNull("entity");

            var overview = new AuditLogOverview();
            overview.AuditLogId = entity.AuditLogId;
            overview.EventDateTime = entity.EventDateUTC;
            overview.UserName = entity.UserName;
            overview.EventTypeName = entity.EventType.ToEventTypeName();
            overview.EntityName = entity.TypeFullName.ToEntityName();
            overview.RecordId = entity.RecordId;
            overview.PropertyChanges = new List<PropertyChangeLogDetails>();

            foreach (var auditLogDetail in entity.LogDetails)
            {
                var propertyChange = new PropertyChangeLogDetails
                {
                    PropertyName = auditLogDetail.PropertyName,
                    OriginalValue = auditLogDetail.OriginalValue,
                    NewValue = auditLogDetail.NewValue
                };
                overview.PropertyChanges.Add(propertyChange);
            }

            return overview;
        }

        public static string ToEventTypeName(this EventType eventType)
        {
            string name = string.Empty;

            switch (eventType)
            {
                case EventType.Added:
                    name = "Added";
                    break;
                case EventType.Deleted:
                    name = "Deleted";
                    break;
                case EventType.Modified:
                    name = "Modified";
                    break;
                case EventType.SoftDeleted:
                    name = "SoftDeleted";
                    break;
                case EventType.UnDeleted:
                    name = "UnDeleted";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null);
            }

            return name;
        }

        public static string ToEntityName(this string typeFullName)
        {
            if (string.IsNullOrEmpty(typeFullName)) return typeFullName;

            return typeFullName.Replace("FLS.Server.Data.DbEntities.", "");
        }

        public static string ToTypeFullName(this string entityName)
        {
            if (string.IsNullOrEmpty(entityName)) return entityName;

            return "FLS.Server.Data.DbEntities." + entityName;
        }
        #endregion AuditLog

        #region Club

        public static ClubOverview ToClubOverview(this Club entity, ClubOverview overview = null)
        {
            entity.ArgumentNotNull("entity");

            if (overview == null)
            {
                overview = new ClubOverview();
            }

            overview.ClubId = entity.ClubId;
            overview.Address = entity.Address;
            overview.City = entity.City;
            overview.ClubName = entity.Clubname;
            overview.CountryName = entity.Country.CountryName;
            overview.EmailAddress = entity.Email;
            overview.HomebaseName = entity.Homebase != null ? entity.Homebase.LocationName : null;
            overview.PhoneNumber = entity.Phone;
            overview.WebPage = entity.WebPage;
            overview.ZipCode = entity.Zip;

            return overview;
        }

        public static ClubDetails ToClubDetails(this Club entity, ClubDetails details = null)
        {
            entity.ArgumentNotNull("entity");

            if (details == null)
            {
                details = new ClubDetails();
            }

            details.ClubId = entity.ClubId;

            details.Address = entity.Address;
            details.City = entity.City;
            details.ClubName = entity.Clubname;
            details.ClubKey = entity.ClubKey;
            details.ContactName = entity.Contact;
            details.CountryId = entity.CountryId;
            details.DefaultGliderFlightTypeId = entity.DefaultGliderFlightTypeId;
            details.DefaultStartType = entity.DefaultStartTypeId;
            details.DefaultTowFlightTypeId = entity.DefaultTowFlightTypeId;
            details.DefaultMotorFlightTypeId = entity.DefaultMotorFlightTypeId;
            details.EmailAddress = entity.Email;
            details.FaxNumber = entity.FaxNumber;
            details.HomebaseId = entity.HomebaseId;
            details.PhoneNumber = entity.Phone;
            details.WebPage = entity.WebPage;
            details.ZipCode = entity.Zip;
            details.SendAircraftStatisticReportTo = entity.SendAircraftStatisticReportTo;
            details.SendPlanningDayInfoMailTo = entity.SendPlanningDayInfoMailTo;
            details.SendInvoiceReportsTo = entity.SendInvoiceReportsTo;

            details.LastArticleSynchronisationOn = entity.LastArticleSynchronisationOn.SetAsUtc();
            details.LastInvoiceExportOn = entity.LastInvoiceExportOn.SetAsUtc();
            details.LastPersonSynchronisationOn = entity.LastPersonSynchronisationOn.SetAsUtc();

            return details;
        }

        public static Club ToClub(this ClubDetails details, Club entity = null, bool overwriteClubId = false)
        {
            details.ArgumentNotNull("details");

            if (entity == null)
            {
                entity = new Club();
            }

            if (overwriteClubId) entity.ClubId = details.ClubId;
            entity.Address = details.Address;
            entity.City = details.City;
            entity.Clubname = details.ClubName;
            entity.ClubKey = details.ClubKey;
            entity.Contact = details.ContactName;
            entity.CountryId = details.CountryId;
            entity.DefaultStartTypeId = details.DefaultStartType;
            entity.DefaultGliderFlightTypeId = details.DefaultGliderFlightTypeId;
            entity.DefaultTowFlightTypeId = details.DefaultTowFlightTypeId;
            entity.DefaultMotorFlightTypeId = details.DefaultMotorFlightTypeId;
            entity.Email = details.EmailAddress;
            entity.FaxNumber = details.FaxNumber;
            entity.HomebaseId = details.HomebaseId;
            entity.Phone = details.PhoneNumber;
            entity.WebPage = details.WebPage;
            entity.Zip = details.ZipCode;

            entity.SendAircraftStatisticReportTo = details.SendAircraftStatisticReportTo;
            entity.SendPlanningDayInfoMailTo = details.SendPlanningDayInfoMailTo;
            entity.SendInvoiceReportsTo = details.SendInvoiceReportsTo;

            entity.ClubStateId = (int) FLS.Data.WebApi.Club.ClubState.Active;

            return entity;
        }

        #endregion Club

        #region Country

        public static CountryListItem ToCountryListItem(this Country entity, CountryListItem listItem = null)
        {
            entity.ArgumentNotNull("entity");

            if (listItem == null)
            {
                listItem = new CountryListItem();
            }

            listItem.CountryId = entity.CountryId;
            listItem.CountryName = entity.CountryName;

            return listItem;
        }

        public static CountryOverview ToCountryOverview(this Country entity, CountryOverview overview = null)
        {
            entity.ArgumentNotNull("entity");

            if (overview == null)
            {
                overview = new CountryOverview();
            }

            overview.CountryId = entity.CountryId;
            overview.CountryName = entity.CountryName;
            overview.CountryCode = entity.CountryCodeIso2;
            overview.CountryIdIso = entity.CountryIdIso;

            return overview;
        }

        #endregion Country

        #region ElevationUnitType

        public static ElevationUnitTypeListItem ToElevationUnitTypeListItem(this DbEntities.ElevationUnitType entity, ElevationUnitTypeListItem listItem = null)
        {
            entity.ArgumentNotNull("entity");

            if (listItem == null)
            {
                listItem = new ElevationUnitTypeListItem();
            }

            listItem.ElevationUnitTypeId = entity.ElevationUnitTypeId;
            listItem.ElevationUnitTypeName = entity.ElevationUnitTypeName;
            listItem.ElevationUnitTypeShortName = entity.ElevationUnitTypeShortName;

            return listItem;
        }

        #endregion ElevationUnitType

        #region EmailTemplate

        public static EmailTemplateOverview ToEmailTemplateOverview(this EmailTemplate entity, EmailTemplateOverview overview = null)
        {
            entity.ArgumentNotNull("entity");

            if (overview == null)
            {
                overview = new EmailTemplateOverview();
            }

            overview.EmailTemplateId = entity.EmailTemplateId;

            overview.EmailTemplateName = entity.EmailTemplateName;
            overview.Description = entity.Description;
            overview.IsSystemTemplate = entity.IsSystemTemplate;
            overview.IsCustomizable = entity.IsCustomizable;

            return overview;
        }

        public static EmailTemplateDetails ToEmailTemplateDetails(this EmailTemplate entity, EmailTemplateDetails details = null)
        {
            entity.ArgumentNotNull("entity");

            if (details == null)
            {
                details = new EmailTemplateDetails();
            }

            details.EmailTemplateId = entity.EmailTemplateId;
            details.EmailTemplateName = entity.EmailTemplateName;
            details.EmailTemplateKeyName = entity.EmailTemplateKeyName;
            details.Description = entity.Description;
            details.FromAddress = entity.FromAddress;
            details.ReplyToAddresses = entity.ReplyToAddresses;
            details.Subject = entity.Subject;
            details.HtmlBody = entity.HtmlBody;
            details.TextBody = entity.TextBody;

            return details;
        }

        public static EmailTemplate ToEmailTemplate(this EmailTemplateDetails details, EmailTemplate entity = null, bool overwriteEmailTemplateId = false)
        {
            details.ArgumentNotNull("details");

            if (entity == null)
            {
                entity = new EmailTemplate();
                //only set key name when no original entity was found otherwise service set the correct key name
                entity.EmailTemplateKeyName = details.EmailTemplateKeyName;
            }

            if (overwriteEmailTemplateId) entity.EmailTemplateId = details.EmailTemplateId;
            entity.EmailTemplateName = details.EmailTemplateName;
            entity.Description = details.Description;
            entity.FromAddress = details.FromAddress;
            entity.ReplyToAddresses = details.ReplyToAddresses;
            entity.Subject = details.Subject;
            entity.HtmlBody = details.HtmlBody;
            entity.TextBody = details.TextBody;

            return entity;
        }

        #endregion EmailTemplate

        #region Flight

        public static FlightOverview ToFlightOverview(this Flight entity, FlightOverview overview = null)
        {
            entity.ArgumentNotNull("entity");

            if (overview == null)
            {
                overview = new FlightOverview();
            }

            overview.FlightId = entity.FlightId;

            overview.FlightComment = entity.Comment;
            overview.IsSoloFlight = entity.IsSoloFlight;

            if (entity.Pilot != null)
            {
                overview.PilotName = entity.PilotDisplayName;
            }

            if (entity.Instructor != null)
            {
                overview.SecondCrewName = entity.InstructorDisplayName;
            }
            else if (entity.CoPilot != null)
            {
                overview.SecondCrewName = entity.CoPilotDisplayName;
            }
            else if (entity.Passenger != null)
            {
                overview.SecondCrewName = entity.PassengerDisplayName;
            }

            if (entity.FlightType != null)
            {
                overview.FlightCode = entity.FlightType.FlightCode;
            }

            overview.FlightState = entity.FlightStateId;

            if (entity.Aircraft != null)
            {
                overview.Immatriculation = entity.Aircraft.Immatriculation;
            }

            if (entity.StartType != null) overview.StartType = entity.StartTypeId;

            if (entity.StartDateTime.HasValue)
            {
                overview.StartDateTime = entity.StartDateTime.Value.SetAsUtc();
            }

            if (entity.LdgDateTime.HasValue)
            {
                overview.LdgDateTime = entity.LdgDateTime.Value.SetAsUtc();
            }

            if (entity.StartLocationId.HasValue && entity.StartLocation != null)
            {
                overview.StartLocation = entity.StartLocation.LocationName;
            }

            if (entity.LdgLocationId.HasValue && entity.LdgLocation != null)
            {
                overview.LdgLocation = entity.LdgLocation.LocationName;
            }

            return overview;
        }

        public static GliderFlightOverview ToGliderFlightOverview(this Flight entity, GliderFlightOverview overview = null)
        {
            entity.ArgumentNotNull("entity");

            if (overview == null)
            {
                overview = new GliderFlightOverview();
            }

            overview.FlightId = entity.FlightId;

            overview.FlightComment = entity.Comment;
            overview.IsSoloFlight = entity.IsSoloFlight;

            if (entity.Pilot != null)
            {
                overview.PilotName = entity.PilotDisplayName;
            }

            if (entity.Instructor != null)
            {
                overview.SecondCrewName = entity.InstructorDisplayName;
            }
            else if (entity.CoPilot != null)
            {
                overview.SecondCrewName = entity.CoPilotDisplayName;
            }
            else if (entity.Passenger != null)
            {
                overview.SecondCrewName = entity.PassengerDisplayName;
            }

            if (entity.FlightType != null)
            {
                overview.FlightCode = entity.FlightType.FlightCode;
            }

            overview.FlightState = entity.FlightStateId;

            if (entity.Aircraft != null)
            {
                overview.Immatriculation = entity.Aircraft.Immatriculation;
            }

            if (entity.StartType != null) overview.StartType = entity.StartTypeId;

            if (entity.StartDateTime.HasValue)
            {
                overview.StartDateTime = entity.StartDateTime.Value.SetAsUtc();
            }

            if (entity.LdgDateTime.HasValue)
            {
                overview.LdgDateTime = entity.LdgDateTime.Value.SetAsUtc();
            }

            if (entity.StartLocationId.HasValue && entity.StartLocation != null)
            {
                overview.StartLocation = entity.StartLocation.LocationName;
            }

            if (entity.LdgLocationId.HasValue && entity.LdgLocation != null)
            {
                overview.LdgLocation = entity.LdgLocation.LocationName;
            }

            overview.TowFlightId = entity.TowFlightId;

            if (entity.TowFlight != null)
            {
                if (entity.TowFlight.StartLocation != null) overview.TowFlightStartLocation = entity.TowFlight.StartLocation.LocationName;
                if (entity.TowFlight.LdgLocation != null) overview.TowFlightLdgLocation = entity.TowFlight.LdgLocation.LocationName;
                overview.TowAircraftImmatriculation = entity.TowFlight.AircraftImmatriculation;
                overview.TowFlightState = entity.TowFlight.FlightStateId;

                if (entity.TowFlight.StartDateTime.HasValue)
                {
                    overview.TowFlightStartDateTime = entity.TowFlight.StartDateTime.Value.SetAsUtc();
                }

                if (entity.TowFlight.LdgDateTime.HasValue)
                {
                    overview.TowFlightLdgDateTime = entity.TowFlight.LdgDateTime.Value.SetAsUtc();
                }

                if (entity.TowFlight.Pilot != null)
                {
                    overview.TowPilotName = entity.TowFlight.PilotDisplayName;
                }
            }

            return overview;
        }

        public static FlightDetails ToFlightDetails(this Flight entity, FlightDetails details = null)
        {
            entity.ArgumentNotNull("entity");

            if (details == null)
            {
                details = new FlightDetails();
            }

            details.FlightId = entity.FlightId;
            details.StartType = entity.StartTypeId;

            if (entity.IsGliderFlight)
            {
                var gliderFlightDetailsData = new GliderFlightDetailsData();

                entity.ToGliderFlightDetailsData(gliderFlightDetailsData);

                details.GliderFlightDetailsData = gliderFlightDetailsData;

                if (entity.IsTowed.HasValue && entity.IsTowed.Value)
                {
                    var towFlightDetailsData = new TowFlightDetailsData();
                    if (entity.TowFlight != null)
                    {
                        entity.TowFlight.ToTowFlightDetailsData(towFlightDetailsData);
                    }
                    details.TowFlightDetailsData = towFlightDetailsData;
                }
            }
            else if (entity.FlightAircraftType == (int) FlightAircraftTypeValue.TowFlight)
            {
                var towFlightDetailsData = new TowFlightDetailsData();
                entity.ToTowFlightDetailsData(towFlightDetailsData);

                details.TowFlightDetailsData = towFlightDetailsData;
            }
            else if (entity.FlightAircraftType == (int) FlightAircraftTypeValue.MotorFlight)
            {
                var motorFlightDetailsData = new MotorFlightDetailsData();
                entity.ToMotorFlightDetailsData(motorFlightDetailsData);

                details.MotorFlightDetailsData = motorFlightDetailsData;
            }

            return details;
        }

        public static GliderFlightDetailsData ToGliderFlightDetailsData(this Flight flight, GliderFlightDetailsData gliderFlightDetailsData = null)
        {
            flight.ArgumentNotNull("flight");

            if (gliderFlightDetailsData == null)
            {
                gliderFlightDetailsData = new GliderFlightDetailsData();
            }

            flight.ToFlightDetailsData(gliderFlightDetailsData);

            if (flight.Passenger != null) gliderFlightDetailsData.PassengerPersonId = flight.Passenger.PersonId;

            gliderFlightDetailsData.CouponNumber = flight.CouponNumber;

            return gliderFlightDetailsData;
        }

        public static TowFlightDetailsData ToTowFlightDetailsData(this Flight flight, TowFlightDetailsData towFlightDetailsData = null)
        {
            flight.ArgumentNotNull("flight");

            if (towFlightDetailsData == null)
            {
                towFlightDetailsData = new TowFlightDetailsData();
            }

            flight.ToFlightDetailsData(towFlightDetailsData);

            return towFlightDetailsData;
        }

        public static MotorFlightDetailsData ToMotorFlightDetailsData(this Flight flight, MotorFlightDetailsData motorFlightDetailsData = null)
        {
            flight.ArgumentNotNull("flight");

            if (motorFlightDetailsData == null)
            {
                motorFlightDetailsData = new MotorFlightDetailsData();
            }

            motorFlightDetailsData.BlockStartDateTime = flight.BlockStartDateTime.SetAsUtc();
            motorFlightDetailsData.BlockEndDateTime = flight.BlockEndDateTime.SetAsUtc();
            motorFlightDetailsData.NrOfLdgsOnStartLocation = flight.NrOfLdgsOnStartLocation;
            motorFlightDetailsData.CouponNumber = flight.CouponNumber;

            flight.ToFlightDetailsData(motorFlightDetailsData);

            motorFlightDetailsData.PassengerPersonIds = new List<Guid>();

            if (flight.Passengers != null && flight.Passengers.Any())
            {
                foreach (var flightCrew in flight.Passengers)
                {
                    motorFlightDetailsData.PassengerPersonIds.Add(flightCrew.PersonId);
                }
            }

            motorFlightDetailsData.NrOfPassengers = flight.NrOfPassengers;

            return motorFlightDetailsData;
        }

        public static FlightDetailsData ToFlightDetailsData(this Flight flight, FlightDetailsData flightDetailsData = null)
        {
            flight.ArgumentNotNull("flight");

            if (flightDetailsData == null)
            {
                flightDetailsData = new FlightDetailsData();
            }

            flightDetailsData.FlightId = flight.FlightId;
            flightDetailsData.AircraftId = flight.AircraftId;

            flightDetailsData.EngineStartOperatingCounter = flight.EngineStartOperatingCounter;
            flightDetailsData.EngineEndOperatingCounter = flight.EngineEndOperatingCounter;
            flightDetailsData.EngineOperatingCounterUnitTypeId = flight.EngineOperatingCounterUnitTypeId;

            flightDetailsData.FlightState = flight.FlightStateId;
            flightDetailsData.FlightTypeId = flight.FlightTypeId;
            flightDetailsData.StartLocationId = flight.StartLocationId;
            flightDetailsData.LdgLocationId = flight.LdgLocationId;
            flightDetailsData.NrOfLdgs = flight.NrOfLdgs;
            flightDetailsData.IsSoloFlight = flight.IsSoloFlight;
            flightDetailsData.OutboundRoute = flight.OutboundRoute;
            flightDetailsData.InboundRoute = flight.InboundRoute;
            flightDetailsData.NoStartTimeInformation = flight.NoStartTimeInformation;
            flightDetailsData.NoLdgTimeInformation = flight.NoLdgTimeInformation;

            flightDetailsData.FlightComment = flight.Comment;

            flightDetailsData.FlightCostBalanceType = flight.FlightCostBalanceTypeId;

            if (flight.StartDateTime.HasValue)
            {
                flightDetailsData.StartDateTime = flight.StartDateTime.Value.SetAsUtc();
            }

            if (flight.LdgDateTime.HasValue)
            {
                flightDetailsData.LdgDateTime = flight.LdgDateTime.Value.SetAsUtc();
            }

            if (flight.LdgDateTime.HasValue && flight.StartDateTime.HasValue)
            {
                flightDetailsData.FlightDuration = flight.LdgDateTime.Value - flight.StartDateTime.Value;
            }

            if (flight.Pilot != null) flightDetailsData.PilotPersonId = flight.Pilot.PersonId;
            if (flight.CoPilot != null) flightDetailsData.CoPilotPersonId = flight.CoPilot.PersonId;
            if (flight.Instructor != null) flightDetailsData.InstructorPersonId = flight.Instructor.PersonId;
            if (flight.InvoiceRecipient != null) flightDetailsData.InvoiceRecipientPersonId = flight.InvoiceRecipient.PersonId;
            if (flight.ObserverPerson != null) flightDetailsData.ObserverPersonId = flight.ObserverPerson.PersonId;

            return flightDetailsData;
        }

        public static Flight ToFlight(this FlightDetails details, Flight entity = null, List<Aircraft> detailsRelatedAircrafts = null, List<FlightType> detailsRelatedFlightTypes = null, bool overwriteFlightId = false)
        {
            details.ArgumentNotNull("details");

            if (entity == null)
            {
                entity = new Flight();
            }

            Aircraft relatedAircraft = null;
            FlightType relatedFlightType = null;

            if (overwriteFlightId) entity.FlightId = details.FlightId;
            entity.StartTypeId = details.StartType;

            if (details.GliderFlightDetailsData != null && (details.StartType.HasValue == false || details.StartType.Value == (int) AircraftStartType.TowingByAircraft || details.StartType.Value == (int) AircraftStartType.WinchLaunch || details.StartType.Value == (int) AircraftStartType.SelfStart || details.StartType.Value == (int) AircraftStartType.ExternalStart))
            {
                if (details.TowFlightDetailsData != null && (details.StartType.HasValue == false || details.StartType.Value == (int) AircraftStartType.TowingByAircraft))
                {
                    //it is a towed glider flight
                    entity.StartTypeId = (int) AircraftStartType.TowingByAircraft;
                    if (entity.TowFlight == null) entity.TowFlight = new Flight();

                    relatedAircraft = GetRelatedAircraft(detailsRelatedAircrafts, details.TowFlightDetailsData);
                    relatedFlightType = GetRelatedFlightType(detailsRelatedFlightTypes, details.TowFlightDetailsData);

                    //check if start locations are equal
                    if (details.TowFlightDetailsData.StartLocationId.HasValue == false && details.GliderFlightDetailsData.StartLocationId.HasValue)
                    {
                        //set towing flight start location to the same location as the glider flight
                        details.TowFlightDetailsData.StartLocationId = details.GliderFlightDetailsData.StartLocationId;
                    }
                    else if (details.GliderFlightDetailsData.StartLocationId.HasValue == false && details.TowFlightDetailsData.StartLocationId.HasValue)
                    {
                        //set glider flight start location to the same location as towing flight
                        details.GliderFlightDetailsData.StartLocationId = details.TowFlightDetailsData.StartLocationId;
                    }
                    else if (details.GliderFlightDetailsData.StartLocationId != details.TowFlightDetailsData.StartLocationId)
                    {
                        throw new InvalidDataException("Start location is not equal between glider flight and towing flight!");
                    }

                    //set start time to the same time as the glider flight (if required)
                    if (details.TowFlightDetailsData.StartDateTime.HasValue == false && details.GliderFlightDetailsData.StartDateTime.HasValue)
                    {
                        details.TowFlightDetailsData.StartDateTime = details.GliderFlightDetailsData.StartDateTime;
                    }

                    //check if start time are equal
                    if (details.TowFlightDetailsData.StartDateTime.HasValue == false && details.GliderFlightDetailsData.StartDateTime.HasValue)
                    {
                        //set towing flight start time to the same time as the glider flight
                        details.TowFlightDetailsData.StartDateTime = details.GliderFlightDetailsData.StartDateTime;
                    }
                    else if (details.GliderFlightDetailsData.StartDateTime.HasValue == false && details.TowFlightDetailsData.StartDateTime.HasValue)
                    {
                        //set glider flight start time to the same time as towing flight
                        details.GliderFlightDetailsData.StartDateTime = details.TowFlightDetailsData.StartDateTime;
                    }
                    else if (details.GliderFlightDetailsData.StartDateTime != details.TowFlightDetailsData.StartDateTime)
                    {
                        throw new InvalidDataException("Start time is not equal between glider flight and towing flight!");
                    }

                    details.TowFlightDetailsData.ToFlight(entity.TowFlight, relatedAircraft, relatedFlightType);
                }
                else
                {
                    //remove any tow flight, it is NO towed glider flight
                    entity.TowFlight = null;

                    if (details.GliderFlightDetailsData.WinchOperatorPersonId.HasValue && (details.StartType.HasValue == false || details.StartType.Value == (int) AircraftStartType.WinchLaunch))
                    {
                        //it is a winch launch
                        entity.StartTypeId = (int) AircraftStartType.WinchLaunch;
                    }
                    else
                    {
                        //it is an external start or self start
                    }
                }

                //first priority has the glider flight
                relatedAircraft = GetRelatedAircraft(detailsRelatedAircrafts, details.GliderFlightDetailsData);
                relatedFlightType = GetRelatedFlightType(detailsRelatedFlightTypes, details.GliderFlightDetailsData);

                details.GliderFlightDetailsData.ToFlight(entity, (AircraftStartType?) entity.StartTypeId, relatedAircraft, relatedFlightType);
            }
            else if (details.MotorFlightDetailsData != null)
            {
                //second priority is the motorflight

                if (entity.StartTypeId.HasValue == false) entity.StartTypeId = (int) AircraftStartType.MotorFlightStart;
                relatedAircraft = GetRelatedAircraft(detailsRelatedAircrafts, details.MotorFlightDetailsData);
                relatedFlightType = GetRelatedFlightType(detailsRelatedFlightTypes, details.MotorFlightDetailsData);
                details.MotorFlightDetailsData.ToFlight(entity, relatedAircraft, relatedFlightType);
            }
            else if (details.TowFlightDetailsData != null)
            {
                //towing flight which should be catched by the glider flight data
                throw new InvalidDataException("Towflight without glider flight is not valid and will not be saved.");
            }

            return entity;
        }

        public static Flight ToFlight(this FlightDetailsData details, Flight entity = null, AircraftStartType? startType = null, Aircraft detailsRelatedAircraft = null, FlightType detailsRelatedFlightType = null, bool overwriteFlightId = false)
        {
            details.ArgumentNotNull("details");

            if (detailsRelatedAircraft != null && detailsRelatedAircraft.AircraftId != details.AircraftId)
            {
                throw new InvalidConstraintException("FlightDetailsData.AircraftId is not equals to detailsRelatedAircraft.AircraftId");
            }

            if (detailsRelatedFlightType != null && detailsRelatedFlightType.FlightTypeId != details.FlightTypeId)
            {
                throw new InvalidConstraintException("FlightDetailsData.FlightTypeId is not equals to detailsRelatedFlightType.FlightTypeId");
            }

            if (entity == null)
            {
                entity = new Flight();
            }

            if (overwriteFlightId) entity.FlightId = details.FlightId;
            if (startType.HasValue) entity.StartTypeId = (int) startType;

            entity.AircraftId = details.AircraftId;
            
            entity.EngineStartOperatingCounter = details.EngineStartOperatingCounter;
            entity.EngineEndOperatingCounter = details.EngineEndOperatingCounter;
            entity.EngineOperatingCounterUnitTypeId = details.EngineOperatingCounterUnitTypeId;

            entity.Comment = details.FlightComment;
            entity.FlightStateId = details.FlightState;
            entity.FlightTypeId = details.FlightTypeId.GetNullableGuid();
            entity.LdgDateTime = details.LdgDateTime;
            entity.LdgLocationId = details.LdgLocationId.GetNullableGuid();
            entity.StartDateTime = details.StartDateTime;
            entity.StartLocationId = details.StartLocationId.GetNullableGuid();
            entity.NrOfLdgs = details.NrOfLdgs;
            entity.FlightCostBalanceTypeId = details.FlightCostBalanceType;
            entity.NoStartTimeInformation = details.NoStartTimeInformation;
            entity.NoLdgTimeInformation = details.NoLdgTimeInformation;

            entity.IsSoloFlight = details.IsSoloFlight;

            //override IsSoloFlight flag if aircraft is one seat
            if (detailsRelatedAircraft != null && detailsRelatedAircraft.NrOfSeats.HasValue && detailsRelatedAircraft.NrOfSeats == 1)
            {
                entity.IsSoloFlight = true;
            }

            entity.OutboundRoute = details.OutboundRoute;
            entity.InboundRoute = details.InboundRoute;

            //if no FlightCostBalanceType is set, set default
            if (entity.FlightCostBalanceType == null && entity.FlightCostBalanceTypeId.HasValue == false && details.FlightCostBalanceType.HasValue == false)
            {
                entity.FlightCostBalanceTypeId = (int) FLS.Data.WebApi.Flight.FlightCostBalanceType.PilotPaysAllCosts;
            }

            //may override FlightState (depending on Start or Ldg date time
            entity.FlightStateId = entity.GetCalculatedFlightStateId();

            //may override NrOfLdgs (depending on start or ldg date time
            entity.NrOfLdgs = entity.GetCalculatedNrOfLandings(detailsRelatedAircraft.IsTowingOrWinchRequired);

            //may override IsSoloFlight (depending on NrOfSeats of Aircraft, or depending on FlightType)
            var isSoloFlight = entity.GetCalculatedIsSoloFlight(detailsRelatedAircraft, detailsRelatedFlightType);
            if (isSoloFlight.HasValue) entity.IsSoloFlight = isSoloFlight.Value;

            //Convert Flightcrews
            details.ToFlightCrewsInFlight(entity, startType, detailsRelatedAircraft, detailsRelatedFlightType);

            return entity;
        }

        public static Flight ToFlight(this GliderFlightDetailsData details, Flight entity = null, AircraftStartType? startType = null, Aircraft detailsRelatedAircraft = null, FlightType detailsRelatedFlightType = null, bool overwriteFlightId = false)
        {
            details.ArgumentNotNull("details");

            if (entity == null)
            {
                entity = new Flight();
            }

            if (overwriteFlightId) entity.FlightId = details.FlightId;
            entity.FlightAircraftType = (int) FlightAircraftTypeValue.GliderFlight;

            if (detailsRelatedFlightType == null ||
                (detailsRelatedFlightType != null && detailsRelatedFlightType.IsCouponNumberRequired))
            {
                //only set coupon number when flight type requires coupon number
                entity.CouponNumber = details.CouponNumber;
            }

            entity.StartPosition = details.StartPosition;

            //converts the base flight stuff to the flight entity
            ((FlightDetailsData) details).ToFlight(entity, startType, detailsRelatedAircraft, detailsRelatedFlightType);

            //Convert Flightcrews
            details.ToFlightCrewsInFlight(entity, startType, detailsRelatedAircraft, detailsRelatedFlightType);

            return entity;
        }

        public static Flight ToFlight(this MotorFlightDetailsData details, Flight entity = null, Aircraft detailsRelatedAircraft = null, FlightType detailsRelatedFlightType = null, bool overwriteFlightId = false)
        {
            details.ArgumentNotNull("details");

            if (entity == null)
            {
                entity = new Flight();
            }

            if (overwriteFlightId) entity.FlightId = details.FlightId;
            entity.FlightAircraftType = (int) FlightAircraftTypeValue.MotorFlight;
            entity.BlockStartDateTime = details.BlockStartDateTime;
            entity.BlockEndDateTime = details.BlockEndDateTime;
            entity.NrOfPassengers = details.NrOfPassengers;
            entity.CouponNumber = details.CouponNumber;
            entity.NrOfLdgsOnStartLocation = details.NrOfLdgsOnStartLocation;

            //converts the base flight stuff to the flight entity
            ((FlightDetailsData) details).ToFlight(entity, AircraftStartType.MotorFlightStart, detailsRelatedAircraft, detailsRelatedFlightType);

            //Convert Flightcrews
            details.ToFlightCrewsInFlight(entity, AircraftStartType.MotorFlightStart, detailsRelatedAircraft, detailsRelatedFlightType);

            return entity;
        }

        public static Flight ToFlight(this TowFlightDetailsData details, Flight entity = null, Aircraft detailsRelatedAircraft = null, FlightType detailsRelatedFlightType = null, bool overwriteFlightId = false)
        {
            details.ArgumentNotNull("details");

            if (entity == null)
            {
                entity = new Flight();
            }

            if (overwriteFlightId) entity.FlightId = details.FlightId;
            entity.FlightAircraftType = (int) FlightAircraftTypeValue.TowFlight;

            //converts the base flight stuff to the flight entity
            ((FlightDetailsData) details).ToFlight(entity, AircraftStartType.TowingByAircraft, detailsRelatedAircraft, detailsRelatedFlightType);

            if (detailsRelatedFlightType != null && detailsRelatedFlightType.InstructorRequired == false)
            {
                entity.IsSoloFlight = true;
            }

            //Convert Flightcrews
            details.ToFlightCrewsInFlight(entity, AircraftStartType.TowingByAircraft, detailsRelatedAircraft, detailsRelatedFlightType);

            return entity;
        }

        #endregion Flight

        #region FlightCrew

        public static Flight ToFlightCrewsInFlight(this FlightDetailsData details, Flight entity = null, AircraftStartType? startType = null, Aircraft detailsRelatedAircraft = null, FlightType detailsRelatedFlightType = null, bool overwriteFlightId = false)
        {
            details.ArgumentNotNull("details");

            if (detailsRelatedAircraft != null && detailsRelatedAircraft.AircraftId != details.AircraftId)
            {
                throw new InvalidConstraintException("FlightDetailsData.AircraftId is not equals to detailsRelatedAircraft.AircraftId");
            }

            if (detailsRelatedFlightType != null && detailsRelatedFlightType.FlightTypeId != details.FlightTypeId)
            {
                throw new InvalidConstraintException("FlightDetailsData.FlightTypeId is not equals to detailsRelatedFlightType.FlightTypeId");
            }

            if (entity == null)
            {
                entity = new Flight();
            }

            FlightCrew flightCrew = null;

            if (details.PilotPersonId.IsValid())
            {
                if (entity.Pilot != null)
                {
                    entity.Pilot.PersonId = details.PilotPersonId;
                }
                else
                {
                    flightCrew = new FlightCrew();
                    flightCrew.FlightId = entity.FlightId;
                    flightCrew.PersonId = details.PilotPersonId;
                    flightCrew.FlightCrewTypeId = (int) FLS.Data.WebApi.Flight.FlightCrewType.PilotOrStudent;
                    entity.FlightCrews.Add(flightCrew);
                }

                if (entity.Pilot != null)
                {
                    //TODO: Handle flight time splitting
                    entity.Pilot.BeginFlightDateTime = entity.StartDateTime;
                    entity.Pilot.EndFlightDateTime = entity.LdgDateTime;
                    entity.Pilot.NrOfLdgs = entity.NrOfLdgs;
                    //entity.Pilot.NrOfStarts = entity.N
                }
            }
            else if (entity.Pilot != null)
            {
                //remove old selected pilot crew, as current flight has no pilot
                entity.FlightCrews.Remove(entity.Pilot);
            }

            if ((detailsRelatedAircraft != null && detailsRelatedAircraft.NrOfSeats.HasValue && detailsRelatedAircraft.NrOfSeats.Value < 2) || (detailsRelatedFlightType != null && detailsRelatedFlightType.InstructorRequired && details.CoPilotPersonId.HasValue && details.CoPilotPersonId.Value.IsValid()) || (detailsRelatedFlightType != null && detailsRelatedFlightType.ObserverPilotOrInstructorRequired && details.ObserverPersonId.HasValue && details.ObserverPersonId.Value.IsValid()))
            {
                //we only have a one seat aircraft
                //or we have an instructor or observer required flight
                //passenger flight must be handled outside
                //--> delete copilot person
                details.CoPilotPersonId = null;

                if (entity.CoPilot != null)
                {
                    //remove old copilot crew, as current flight has no copilot
                    entity.FlightCrews.Remove(entity.CoPilot);
                }
            }
            else if (details.CoPilotPersonId.HasValue && details.CoPilotPersonId.Value.IsValid())
            {
                if (entity.CoPilot != null)
                {
                    entity.CoPilot.PersonId = details.CoPilotPersonId.Value;
                }
                else
                {
                    flightCrew = new FlightCrew();
                    flightCrew.FlightId = entity.FlightId;
                    flightCrew.PersonId = details.CoPilotPersonId.Value;
                    flightCrew.FlightCrewTypeId = (int) FLS.Data.WebApi.Flight.FlightCrewType.CoPilot;
                    entity.FlightCrews.Add(flightCrew);
                }
            }
            else if (entity.CoPilot != null)
            {
                //remove old copilot crew, as current flight has no copilot
                entity.FlightCrews.Remove(entity.CoPilot);
            }


            if (detailsRelatedFlightType != null && detailsRelatedFlightType.InstructorRequired && details.InstructorPersonId.HasValue && details.InstructorPersonId.Value.IsValid())
            {
                if (entity.Instructor != null)
                {
                    entity.Instructor.PersonId = details.InstructorPersonId.Value;
                }
                else
                {
                    flightCrew = new FlightCrew();
                    flightCrew.FlightId = entity.FlightId;
                    flightCrew.PersonId = details.InstructorPersonId.Value;
                    flightCrew.FlightCrewTypeId = (int) FLS.Data.WebApi.Flight.FlightCrewType.FlightInstructor;
                    entity.FlightCrews.Add(flightCrew);
                }

                if (entity.Instructor != null)
                {
                    //TODO: Handle instructor time (glider and motor flight)
                    entity.Instructor.BeginFlightDateTime = entity.StartDateTime;
                    entity.Instructor.EndFlightDateTime = entity.LdgDateTime;
                    entity.Instructor.BeginInstructionDateTime = entity.StartDateTime;
                    entity.Instructor.EndInstructionDateTime = entity.LdgDateTime;
                    entity.Instructor.NrOfLdgs = entity.NrOfLdgs;
                }
            }
            else
            {
                //we don't need an instructor
                details.InstructorPersonId = null;

                if (entity.Instructor != null)
                {
                    //remove old Instructor crew, as current flight has no Instructor
                    entity.FlightCrews.Remove(entity.Instructor);
                }
            }

            if (detailsRelatedFlightType != null && detailsRelatedFlightType.ObserverPilotOrInstructorRequired && details.ObserverPersonId.HasValue && details.ObserverPersonId.Value.IsValid())
            {
                if (entity.ObserverPerson != null)
                {
                    entity.ObserverPerson.PersonId = details.ObserverPersonId.Value;
                }
                else
                {
                    flightCrew = new FlightCrew();
                    flightCrew.FlightId = entity.FlightId;
                    flightCrew.PersonId = details.ObserverPersonId.Value;
                    flightCrew.FlightCrewTypeId = (int) FLS.Data.WebApi.Flight.FlightCrewType.Observer;
                    entity.FlightCrews.Add(flightCrew);
                }
            }
            else
            {
                //we don't need an observer pilot
                details.ObserverPersonId = null;

                if (entity.ObserverPerson != null)
                {
                    //remove old observer crew, as current flight has no observer
                    entity.FlightCrews.Remove(entity.ObserverPerson);
                }
            }


            if (details.InvoiceRecipientPersonId.HasValue && details.InvoiceRecipientPersonId.Value.IsValid())
            {
                if (entity.InvoiceRecipient != null)
                {
                    entity.InvoiceRecipient.PersonId = details.InvoiceRecipientPersonId.Value;
                }
                else
                {
                    flightCrew = new FlightCrew();
                    flightCrew.FlightId = entity.FlightId;
                    flightCrew.PersonId = details.InvoiceRecipientPersonId.Value;
                    flightCrew.FlightCrewTypeId = (int) FLS.Data.WebApi.Flight.FlightCrewType.FlightCostInvoiceRecipient;
                    entity.FlightCrews.Add(flightCrew);
                }
            }
            else
            {
                if (entity.InvoiceRecipient != null)
                {
                    entity.FlightCrews.Remove(entity.InvoiceRecipient);
                }
            }

            return entity;
        }

        public static Flight ToFlightCrewsInFlight(this GliderFlightDetailsData details, Flight entity = null, AircraftStartType? startType = null, Aircraft detailsRelatedAircraft = null, FlightType detailsRelatedFlightType = null, bool overwriteFlightId = false)
        {
            details.ArgumentNotNull("details");

            if (detailsRelatedAircraft != null && detailsRelatedAircraft.AircraftId != details.AircraftId)
            {
                throw new InvalidConstraintException("FlightDetailsData.AircraftId is not equals to detailsRelatedAircraft.AircraftId");
            }

            if (detailsRelatedFlightType != null && detailsRelatedFlightType.FlightTypeId != details.FlightTypeId)
            {
                throw new InvalidConstraintException("FlightDetailsData.FlightTypeId is not equals to detailsRelatedFlightType.FlightTypeId");
            }

            if (entity == null)
            {
                entity = new Flight();
            }

            FlightCrew flightCrew = null;

            ((FlightDetailsData) details).ToFlightCrewsInFlight(entity, startType, detailsRelatedAircraft, detailsRelatedFlightType);

            //handle winch operator
            if (startType.HasValue && startType.Value == AircraftStartType.WinchLaunch && details.WinchOperatorPersonId.HasValue && details.WinchOperatorPersonId.Value.IsValid())
            {
                if (entity.WinchOperator != null)
                {
                    entity.WinchOperator.PersonId = details.WinchOperatorPersonId.Value;
                }
                else
                {
                    flightCrew = new FlightCrew();
                    flightCrew.FlightId = entity.FlightId;
                    flightCrew.PersonId = details.WinchOperatorPersonId.Value;
                    flightCrew.FlightCrewTypeId = (int) FLS.Data.WebApi.Flight.FlightCrewType.WinchOperator;
                    entity.FlightCrews.Add(flightCrew);
                }
            }
            else
            {
                //remove old winch operator if it exists as we don't have winch start now
                details.WinchOperatorPersonId = null;

                if (entity.WinchOperator != null)
                {
                    entity.FlightCrews.Remove(entity.WinchOperator);
                }
            }

            //handle glider passenger
            if (detailsRelatedFlightType != null && detailsRelatedFlightType.IsPassengerFlight && detailsRelatedAircraft != null && detailsRelatedAircraft.NrOfSeats == 2 && details.PassengerPersonId.HasValue)
            {
                //is passenger flight

                if (details.CoPilotPersonId.HasValue && details.CoPilotPersonId.Value.IsValid())
                {
                    //we have to remove copilot as it is a passenger flight
                    details.CoPilotPersonId = null;

                    if (entity.CoPilot != null)
                    {
                        entity.FlightCrews.Remove(entity.CoPilot);
                    }
                }

                if (details.InstructorPersonId.HasValue && details.InstructorPersonId.Value.IsValid())
                {
                    //we have to remove Instructor as it is a passenger flight
                    details.InstructorPersonId = null;

                    if (entity.Instructor != null)
                    {
                        entity.FlightCrews.Remove(entity.Instructor);
                    }
                }

                if (details.ObserverPersonId.HasValue && details.ObserverPersonId.Value.IsValid())
                {
                    //we have to remove ObserverPerson as it is a passenger flight
                    details.ObserverPersonId = null;

                    if (entity.ObserverPerson != null)
                    {
                        entity.FlightCrews.Remove(entity.ObserverPerson);
                    }
                }

                if (entity.Passenger != null)
                {
                    entity.Passenger.PersonId = details.PassengerPersonId.Value;
                }
                else
                {
                    flightCrew = new FlightCrew();
                    flightCrew.FlightId = entity.FlightId;
                    flightCrew.PersonId = details.PassengerPersonId.Value;
                    flightCrew.FlightCrewTypeId = (int) FLS.Data.WebApi.Flight.FlightCrewType.Passenger;
                    entity.FlightCrews.Add(flightCrew);
                }
            }
            else
            {
                details.PassengerPersonId = null;

                if (entity.Passenger != null)
                {
                    entity.FlightCrews.Remove(entity.Passenger);
                }
            }

            return entity;
        }

        public static Flight ToFlightCrewsInFlight(this MotorFlightDetailsData details, Flight entity = null, AircraftStartType? startType = null, Aircraft detailsRelatedAircraft = null, FlightType detailsRelatedFlightType = null, bool overwriteFlightId = false)
        {
            details.ArgumentNotNull("details");

            if (detailsRelatedAircraft != null && detailsRelatedAircraft.AircraftId != details.AircraftId)
            {
                throw new InvalidConstraintException("FlightDetailsData.AircraftId is not equals to detailsRelatedAircraft.AircraftId");
            }

            if (detailsRelatedFlightType != null && detailsRelatedFlightType.FlightTypeId != details.FlightTypeId)
            {
                throw new InvalidConstraintException("FlightDetailsData.FlightTypeId is not equals to detailsRelatedFlightType.FlightTypeId");
            }

            if (entity == null)
            {
                entity = new Flight();
            }

            ((FlightDetailsData) details).ToFlightCrewsInFlight(entity, startType, detailsRelatedAircraft, detailsRelatedFlightType);

            //handle motor flight passengers
            if (detailsRelatedAircraft != null && detailsRelatedAircraft.NrOfSeats > 2 && detailsRelatedFlightType != null && detailsRelatedFlightType.IsPassengerFlight && details.PassengerPersonIds != null && details.PassengerPersonIds.Any())
            {
                //it is a passenger flight and we have some passengers
                foreach (var passengerPersonId in details.PassengerPersonIds)
                {
                    if (entity.Passengers.Any(flightPassenger => flightPassenger.PersonId == passengerPersonId) == false)
                    {
                        //passenger not found, add it
                        var flightCrew = new FlightCrew();
                        flightCrew.FlightId = entity.FlightId;
                        flightCrew.PersonId = passengerPersonId;
                        flightCrew.FlightCrewTypeId = (int) FLS.Data.WebApi.Flight.FlightCrewType.Passenger;
                        entity.FlightCrews.Add(flightCrew);
                    }
                }

                //remove all flight crews which are not found in current flight
                foreach (var passenger in entity.Passengers.ToList())
                {
                    if (details.PassengerPersonIds.Any(passengerPersonId => passengerPersonId == passenger.PersonId) == false)
                    {
                        entity.FlightCrews.Remove(passenger);
                    }
                }
            }
            else
            {
                //we haven't any selected passengers in the current flight, so remove all passengers from the stored flight
                foreach (var passenger in entity.Passengers.ToList())
                {
                    entity.FlightCrews.Remove(passenger);
                }
            }

            return entity;
        }

        #endregion FlightCrew

        #region FlightCostBalanceType

        public static FlightCostBalanceTypeListItem ToFlightCostBalanceTypeListItem(this DbEntities.FlightCostBalanceType entity, FlightCostBalanceTypeListItem listItem = null)
        {
            entity.ArgumentNotNull("entity");

            if (listItem == null)
            {
                listItem = new FlightCostBalanceTypeListItem();
            }

            listItem.FlightCostBalanceTypeId = entity.FlightCostBalanceTypeId;

            listItem.FlightCostBalanceTypeName = entity.FlightCostBalanceTypeName;
            listItem.Comment = entity.Comment;
            listItem.PersonForInvoiceRequired = entity.PersonForInvoiceRequired;

            return listItem;
        }

        #endregion FlightCostBalanceType

        #region FlightState

        public static FlightStateListItem ToFlightStateListItem(this DbEntities.FlightState entity, FlightStateListItem listItem = null)
        {
            entity.ArgumentNotNull("entity");

            if (listItem == null)
            {
                listItem = new FlightStateListItem();
            }

            listItem.FlightStateId = entity.FlightStateId;

            listItem.FlightState = entity.FlightStateName;

            return listItem;
        }

        public static string ToFlightStateName(this FLS.Data.WebApi.Flight.FlightState flightStateEnum)
        {
            var flightStateString = "Schleppstart";

            switch (flightStateEnum)
            {
                case FLS.Data.WebApi.Flight.FlightState.New:
                    flightStateString = "Neuer Flug";
                    break;
                case FLS.Data.WebApi.Flight.FlightState.FlightPlanOpen:
                    flightStateString = "Flugplan eröffnet";
                    break;
                case FLS.Data.WebApi.Flight.FlightState.Started:
                    flightStateString = "Gestartet";
                    break;
                case FLS.Data.WebApi.Flight.FlightState.Landed:
                    flightStateString = "Gelandet";
                    break;
                case FLS.Data.WebApi.Flight.FlightState.FlightPlanClosed:
                    flightStateString = "Flugplan geschlossen";
                    break;
                case FLS.Data.WebApi.Flight.FlightState.Invalid:
                    flightStateString = "Ungültige Angaben";
                    break;
                case FLS.Data.WebApi.Flight.FlightState.Valid:
                    flightStateString = "Gültig";
                    break;
                case FLS.Data.WebApi.Flight.FlightState.Locked:
                    flightStateString = "Gesperrt";
                    break;
                case FLS.Data.WebApi.Flight.FlightState.Invoiced:
                    flightStateString = "Verrechnet";
                    break;
                case FLS.Data.WebApi.Flight.FlightState.PartialPaid:
                    flightStateString = "Teilweise bezahlt";
                    break;
                case FLS.Data.WebApi.Flight.FlightState.Paid:
                    flightStateString = "Bezahlt";
                    break;
            }

            return flightStateString;
        }

        #endregion FlightState

        #region FlightType

        public static FlightTypeOverview ToFlightTypeOverview(this FlightType entity, FlightTypeOverview overview = null)
        {
            entity.ArgumentNotNull("entity");

            if (overview == null)
            {
                overview = new FlightTypeOverview();
            }

            overview.FlightTypeId = entity.FlightTypeId;

            overview.FlightCode = entity.FlightCode;
            overview.FlightTypeId = entity.FlightTypeId;
            overview.FlightTypeName = entity.FlightTypeName;
            overview.InstructorRequired = entity.InstructorRequired;
            overview.IsCheckFlight = entity.IsCheckFlight;
            overview.IsForGliderFlights = entity.IsForGliderFlights;
            overview.IsForTowFlights = entity.IsForTowFlights;
            overview.IsForMotorFlights = entity.IsForMotorFlights;
            overview.IsPassengerFlight = entity.IsPassengerFlight;
            overview.ObserverPilotOrInstructorRequired = entity.ObserverPilotOrInstructorRequired;
            overview.IsFlightCostBalanceSelectable = entity.IsFlightCostBalanceSelectable;
            overview.IsSoloFlight = entity.IsSoloFlight;
            overview.IsCouponNumberRequired = entity.IsCouponNumberRequired;

            if (entity.MinNrOfAircraftSeatsRequired.HasValue == false)
            {
                overview.MinNrOfAircraftSeatsRequired = 0;
            }
            else
            {
                overview.MinNrOfAircraftSeatsRequired = entity.MinNrOfAircraftSeatsRequired.Value;
            }

            return overview;
        }

        public static FlightTypeDetails ToFlightTypeDetails(this FlightType entity, FlightTypeDetails details = null)
        {
            entity.ArgumentNotNull("entity");

            if (details == null)
            {
                details = new FlightTypeDetails();
            }

            details.FlightTypeId = entity.FlightTypeId;

            details.FlightCode = entity.FlightCode;
            details.FlightTypeName = entity.FlightTypeName;
            details.InstructorRequired = entity.InstructorRequired;
            details.IsCheckFlight = entity.IsCheckFlight;
            details.IsForGliderFlights = entity.IsForGliderFlights;
            details.IsForTowFlights = entity.IsForTowFlights;
            details.IsForMotorFlights = entity.IsForMotorFlights;
            details.IsPassengerFlight = entity.IsPassengerFlight;
            details.ObserverPilotOrInstructorRequired = entity.ObserverPilotOrInstructorRequired;
            details.IsFlightCostBalanceSelectable = entity.IsFlightCostBalanceSelectable;
            details.IsSoloFlight = entity.IsSoloFlight;
            details.IsCouponNumberRequired = entity.IsCouponNumberRequired;

            if (entity.MinNrOfAircraftSeatsRequired.HasValue == false)
            {
                details.MinNrOfAircraftSeatsRequired = 0;
            }
            else
            {
                details.MinNrOfAircraftSeatsRequired = entity.MinNrOfAircraftSeatsRequired.Value;
            }

            return details;
        }

        public static FlightType ToFlightType(this FlightTypeDetails details, Guid clubId, FlightType entity = null, bool overwriteFlightTypeId = false)
        {
            details.ArgumentNotNull("details");

            if (entity == null)
            {
                entity = new FlightType();
            }

            if (overwriteFlightTypeId) entity.FlightTypeId = details.FlightTypeId;
            entity.ClubId = clubId;
            entity.FlightCode = details.FlightCode;
            entity.FlightTypeName = details.FlightTypeName;
            entity.InstructorRequired = details.InstructorRequired;
            entity.IsCheckFlight = details.IsCheckFlight;
            entity.IsForGliderFlights = details.IsForGliderFlights;
            entity.IsForTowFlights = details.IsForTowFlights;
            entity.IsForMotorFlights = details.IsForMotorFlights;
            entity.IsPassengerFlight = details.IsPassengerFlight;
            entity.ObserverPilotOrInstructorRequired = details.ObserverPilotOrInstructorRequired;
            entity.IsFlightCostBalanceSelectable = details.IsFlightCostBalanceSelectable;
            entity.IsSoloFlight = details.IsSoloFlight;
            entity.IsCouponNumberRequired = details.IsCouponNumberRequired;
            entity.MinNrOfAircraftSeatsRequired = details.MinNrOfAircraftSeatsRequired;

            return entity;
        }

        #endregion FlightType

        #region Language

        public static LanguageListItem ToLanguageListItem(this DbEntities.Language entity, LanguageListItem listItem = null)
        {
            entity.ArgumentNotNull("entity");

            if (listItem == null)
            {
                listItem = new LanguageListItem();
            }

            listItem.LanguageId = entity.LanguageId;
            listItem.LanguageName = entity.LanguageName;
            listItem.LanguageKey = entity.LanguageKey;

            return listItem;
        }

        #endregion Language

        #region LengthUnitType

        public static LengthUnitTypeListItem ToLengthUnitTypeListItem(this DbEntities.LengthUnitType entity, LengthUnitTypeListItem listItem = null)
        {
            entity.ArgumentNotNull("entity");

            if (listItem == null)
            {
                listItem = new LengthUnitTypeListItem();
            }

            listItem.LengthUnitTypeId = entity.LengthUnitTypeId;
            listItem.LengthUnitTypeName = entity.LengthUnitTypeName;
            listItem.LengthUnitTypeShortName = entity.LengthUnitTypeShortName;

            return listItem;
        }

        #endregion LengthUnitType

        #region Location

        public static LocationListItem ToLocationListItem(this Location entity, LocationListItem listItem = null)
        {
            entity.ArgumentNotNull("entity");

            if (listItem == null)
            {
                listItem = new LocationListItem();
            }

            listItem.LocationId = entity.LocationId;

            listItem.LocationName = entity.LocationName;
            listItem.IcaoCode = entity.IcaoCode;

            if (entity.Country != null)
            {
                listItem.CountryCode = entity.Country.CountryCodeIso2;
            }

            if (entity.LocationType != null)
            {
                listItem.IsAirfield = entity.LocationType.IsAirfield;
            }

            listItem.IsInboundRouteRequired = entity.IsInboundRouteRequired;
            listItem.IsOutboundRouteRequired = entity.IsOutboundRouteRequired;
            return listItem;
        }

        public static LocationOverview ToLocationOverview(this Location entity, LocationOverview overview = null)
        {
            entity.ArgumentNotNull("entity");

            if (overview == null)
            {
                overview = new LocationOverview();
            }

            overview.LocationId = entity.LocationId;

            overview.AirportFrequency = entity.AirportFrequency;
            overview.LocationName = entity.LocationName;
            overview.LocationShortName = entity.LocationShortName;
            overview.IcaoCode = entity.IcaoCode;
            overview.RunwayDirection = entity.RunwayDirection;
            overview.Description = entity.Description;
            overview.IsInboundRouteRequired = entity.IsInboundRouteRequired;
            overview.IsOutboundRouteRequired = entity.IsOutboundRouteRequired;

            if (entity.LocationType != null)
            {
                overview.LocationTypeName = entity.LocationType.LocationTypeName;
            }

            if (entity.Country != null)
            {
                overview.CountryName = entity.Country.CountryName;
            }

            if (entity.Elevation.HasValue)
            {
                var unit = "";

                if (entity.ElevationUnitType != null)
                {
                    unit = entity.ElevationUnitType.ElevationUnitTypeShortName;
                }

                overview.Elevation = string.Format("{0} {1}", entity.Elevation, unit);
            }

            if (entity.RunwayLength.HasValue)
            {
                var unit = "";

                if (entity.LengthUnitType != null)
                {
                    unit = entity.LengthUnitType.LengthUnitTypeShortName;
                }

                overview.RunwayLength = string.Format("{0} {1}", entity.RunwayLength, unit);
            }

            return overview;
        }

        public static LocationDetails ToLocationDetails(this Location entity, LocationDetails details = null)
        {
            entity.ArgumentNotNull("entity");

            if (details == null)
            {
                details = new LocationDetails();
            }

            details.LocationId = entity.LocationId;
            details.AirportFrequency = entity.AirportFrequency;
            details.LocationName = entity.LocationName;
            details.LocationShortName = entity.LocationShortName;
            details.Latitude = entity.Latitude;
            details.Longitude = entity.Longitude;
            details.Elevation = entity.Elevation;
            details.ElevationUnitType = entity.ElevationUnitTypeId;
            details.IcaoCode = entity.IcaoCode;
            details.RunwayDirection = entity.RunwayDirection;
            details.RunwayLength = entity.RunwayLength;
            details.RunwayLengthUnitType = entity.RunwayLengthUnitType;
            details.CountryId = entity.CountryId;
            details.LocationTypeId = entity.LocationTypeId;
            details.Description = entity.Description;
            details.IsInboundRouteRequired = entity.IsInboundRouteRequired;
            details.IsOutboundRouteRequired = entity.IsOutboundRouteRequired;

            return details;
        }

        public static Location ToLocation(this LocationDetails details, Location entity = null, bool overwriteLocationId = false)
        {
            details.ArgumentNotNull("details");

            if (entity == null)
            {
                entity = new Location();
            }

            if (overwriteLocationId) entity.LocationId = details.LocationId;
            entity.AirportFrequency = details.AirportFrequency;
            entity.LocationName = details.LocationName;
            entity.LocationShortName = details.LocationShortName;
            entity.Latitude = details.Latitude;
            entity.Longitude = details.Longitude;
            entity.Elevation = details.Elevation;
            entity.ElevationUnitTypeId = details.ElevationUnitType;
            entity.IcaoCode = details.IcaoCode;
            entity.RunwayDirection = details.RunwayDirection;
            entity.RunwayLength = details.RunwayLength;
            entity.RunwayLengthUnitType = details.RunwayLengthUnitType;
            entity.CountryId = details.CountryId;
            entity.LocationTypeId = details.LocationTypeId;
            entity.Description = details.Description;
            entity.IsInboundRouteRequired = details.IsInboundRouteRequired;
            entity.IsOutboundRouteRequired = details.IsOutboundRouteRequired;

            return entity;
        }

        #endregion Location

        #region LocationType

        public static LocationTypeListItem ToLocationTypeListItem(this DbEntities.LocationType entity, LocationTypeListItem listItem = null)
        {
            entity.ArgumentNotNull("entity");

            if (listItem == null)
            {
                listItem = new LocationTypeListItem();
            }

            listItem.LocationTypeId = entity.LocationTypeId;

            listItem.LocationTypeName = entity.LocationTypeName;
            listItem.CupWaypointId = entity.LocationTypeCupId;
            listItem.IsAirfield = entity.IsAirfield;
            return listItem;
        }

        #endregion LocationType

        #region MemberState

        public static MemberStateOverview ToMemberStateOverview(this MemberState entity, MemberStateOverview overview = null)
        {
            entity.ArgumentNotNull("entity");

            if (overview == null)
            {
                overview = new MemberStateOverview();
            }

            overview.MemberStateId = entity.MemberStateId;
            overview.MemberStateName = entity.MemberStateName;

            return overview;
        }

        public static MemberStateDetails ToMemberStateDetails(this MemberState entity, MemberStateDetails details = null)
        {
            entity.ArgumentNotNull("entity");

            if (details == null)
            {
                details = new MemberStateDetails();
            }

            details.MemberStateId = entity.MemberStateId;
            details.MemberStateName = entity.MemberStateName;
            details.Remarks = entity.Remarks;

            return details;
        }

        public static MemberState ToMemberState(this MemberStateDetails details, Guid clubId, MemberState entity = null, bool overwriteMemberStateId = false)
        {
            details.ArgumentNotNull("details");

            if (entity == null)
            {
                entity = new MemberState();
            }

            if (overwriteMemberStateId) entity.MemberStateId = details.MemberStateId;

            entity.ClubId = clubId;
            entity.MemberStateName = details.MemberStateName;
            entity.Remarks = details.Remarks;

            return entity;
        }

        #endregion MemberState

        #region Person, PilotPerson, Passenger, FullPersonDetails

        public static PersonListItem ToPersonListItem(this Person entity, PersonListItem listItem = null)
        {
            entity.ArgumentNotNull("entity");

            if (listItem == null)
            {
                listItem = new PersonListItem();
            }

            listItem.PersonId = entity.PersonId;

            listItem.Firstname = entity.Firstname;
            listItem.Lastname = entity.Lastname;
            listItem.City = entity.City;

            return listItem;
        }

        public static PilotPersonListItem ToPilotPersonListItem(this Person entity, PilotPersonListItem listItem = null)
        {
            entity.ArgumentNotNull("entity");

            if (listItem == null)
            {
                listItem = new PilotPersonListItem();
            }

            entity.ToPersonListItem(listItem);

            return listItem;
        }

        public static PassengerListItem ToPassengerListItem(this Person entity, PassengerListItem listItem = null)
        {
            entity.ArgumentNotNull("entity");

            if (listItem == null)
            {
                listItem = new PassengerListItem();
            }

            entity.ToPersonListItem(listItem);

            return listItem;
        }

        public static PersonOverview ToPersonOverview(this Person entity, Guid clubId, PersonOverview overview = null)
        {
            entity.ArgumentNotNull("entity");

            if (overview == null)
            {
                overview = new PersonOverview();
            }

            overview.PersonId = entity.PersonId;

            overview.Firstname = entity.Firstname;
            overview.Lastname = entity.Lastname;
            overview.AddressLine = entity.AddressLine1;
            overview.City = entity.City;
            overview.ZipCode = entity.Zip;

            if (entity.Country != null)
            {
                overview.CountryName = entity.Country.CountryName;
            }

            overview.MobilePhoneNumber = entity.MobilePhone;
            overview.PrivateEmail = entity.EmailPrivate;

            var personClub = entity.PersonClubs.FirstOrDefault(e => e.ClubId == clubId);

            if (personClub != null)
            {
                if (personClub.MemberState != null)
                {
                    overview.MemberStateName = personClub.MemberState.MemberStateName;
                }
            }

            return overview;
        }

        public static PilotPersonOverview ToPilotPersonOverview(this Person entity, Guid clubId, PilotPersonOverview overview = null)
        {
            entity.ArgumentNotNull("entity");

            if (overview == null)
            {
                overview = new PilotPersonOverview();
            }

            entity.ToPersonOverview(clubId, overview);

            overview.HasGliderInstructorLicence = entity.HasGliderInstructorLicence;
            overview.HasGliderPassengerLicence = entity.HasGliderPAXLicence;
            overview.HasGliderPilotLicence = entity.HasGliderPilotLicence;
            overview.HasGliderTraineeLicence = entity.HasGliderTraineeLicence;
            overview.HasMotorPilotLicence = entity.HasMotorPilotLicence;
            overview.HasTowPilotLicence = entity.HasTowPilotLicence;
            overview.HasWinchOperatorLicence = entity.HasWinchOperatorLicence;
            overview.HasMotorInstructorLicence = entity.HasMotorInstructorLicence;
            overview.HasTMGLicence = entity.HasTMGLicence;
            overview.LicenceNumber = entity.LicenceNumber;

            return overview;
        }

        public static PassengerOverview ToPassengerOverview(this Person entity, Guid clubId, PassengerOverview overview = null)
        {
            entity.ArgumentNotNull("entity");

            if (overview == null)
            {
                overview = new PassengerOverview();
            }

            entity.ToPersonOverview(clubId, overview);

            return overview;
        }

        public static PersonDetails ToPersonDetails(this Person entity, Guid clubId, PersonDetails details = null)
        {
            entity.ArgumentNotNull("entity");

            if (details == null)
            {
                details = new PersonDetails();
            }

            details.PersonId = entity.PersonId;

            details.AddressLine1 = entity.AddressLine1;
            details.AddressLine2 = entity.AddressLine2;
            details.Birthday = entity.Birthday.SetAsUtc();
            details.BusinessPhoneNumber = entity.BusinessPhone;
            details.City = entity.City;
            details.CompanyName = entity.CompanyName;
            details.CountryId = entity.CountryId;
            details.BusinessEmail = entity.EmailBusiness;
            details.PrivateEmail = entity.EmailPrivate;
            details.FaxNumber = entity.FaxNumber;
            details.Firstname = entity.Firstname;
            details.Lastname = entity.Lastname;
            details.Midname = entity.Midname;
            details.MobilePhoneNumber = entity.MobilePhone;
            details.PrivatePhoneNumber = entity.PrivatePhone;
            details.Region = entity.Region;
            details.ZipCode = entity.Zip;
            details.ReceiveOwnedAircraftStatisticReports = entity.ReceiveOwnedAircraftStatisticReports;
            details.EnableAddress = entity.EnableAddress;

            var personClub = entity.PersonClubs.FirstOrDefault(e => e.ClubId == clubId);

            if (personClub != null)
            {
                if (details.ClubRelatedPersonDetails == null)
                {
                    details.ClubRelatedPersonDetails = new ClubRelatedPersonDetails();
                }

                details.ClubRelatedPersonDetails.MemberStateId = personClub.MemberStateId;
                details.ClubRelatedPersonDetails.MemberNumber = personClub.MemberNumber;
                details.ClubRelatedPersonDetails.MemberKey = personClub.MemberKey;
                details.ClubRelatedPersonDetails.IsGliderInstructor = personClub.IsGliderInstructor;
                details.ClubRelatedPersonDetails.IsGliderPilot = personClub.IsGliderPilot;
                details.ClubRelatedPersonDetails.IsGliderTrainee = personClub.IsGliderTrainee;
                details.ClubRelatedPersonDetails.IsMotorPilot = personClub.IsMotorPilot;
                details.ClubRelatedPersonDetails.IsPassenger = personClub.IsPassenger;
                details.ClubRelatedPersonDetails.IsTowPilot = personClub.IsTowPilot;
                details.ClubRelatedPersonDetails.IsWinchOperator = personClub.IsWinchOperator;
                details.ClubRelatedPersonDetails.IsMotorInstructor = personClub.IsMotorInstructor;
                details.ClubRelatedPersonDetails.ReceiveFlightReports = personClub.ReceiveFlightReports;
                details.ClubRelatedPersonDetails.ReceiveAircraftReservationNotifications = personClub.ReceiveAircraftReservationNotifications;
                details.ClubRelatedPersonDetails.ReceivePlanningDayRoleReminder = personClub.ReceivePlanningDayRoleReminder;

                foreach (var entityPersonCategory in entity.PersonPersonCategories.Where(entityPersonCategory => entityPersonCategory.PersonCategory.ClubId == clubId))
                {
                    details.ClubRelatedPersonDetails.PersonCategoryIds.Add(entityPersonCategory.PersonCategoryId);
                }
            }

            return details;
        }

        public static PilotPersonDetails ToPilotPersonDetails(this Person entity, Guid clubId, PilotPersonDetails details = null)
        {
            entity.ArgumentNotNull("entity");

            if (details == null)
            {
                details = new PilotPersonDetails();
            }

            entity.ToPersonDetails(clubId, details);

            details.HasGliderInstructorLicence = entity.HasGliderInstructorLicence;
            details.HasGliderPilotLicence = entity.HasGliderPilotLicence;
            details.HasGliderTraineeLicence = entity.HasGliderTraineeLicence;
            details.HasMotorPilotLicence = entity.HasMotorPilotLicence;
            details.HasTowPilotLicence = entity.HasTowPilotLicence;
            details.HasGliderPassengerLicence = entity.HasGliderPAXLicence;
            details.HasTMGLicence = entity.HasTMGLicence;
            details.HasWinchOperatorLicence = entity.HasWinchOperatorLicence;
            details.HasMotorInstructorLicence = entity.HasMotorInstructorLicence;
            details.LicenceNumber = entity.LicenceNumber;
            details.GliderInstructorLicenceExpireDate = entity.GliderInstructorLicenceExpireDate.SetAsUtc();
            details.MedicalClass1ExpireDate = entity.MedicalClass1ExpireDate.SetAsUtc();
            details.MedicalClass2ExpireDate = entity.MedicalClass2ExpireDate.SetAsUtc();
            details.MedicalLaplExpireDate = entity.MedicalLaplExpireDate.SetAsUtc();
            details.HasGliderTowingStartPermission = entity.HasGliderTowingStartPermission;
            details.HasGliderSelfStartPermission = entity.HasGliderSelfStartPermission;
            details.HasGliderWinchStartPermission = entity.HasGliderWinchStartPermission;

            details.ReceiveOwnedAircraftStatisticReports = entity.ReceiveOwnedAircraftStatisticReports;
            details.SpotLink = entity.SpotLink;

            return details;
        }

        public static PassengerDetails ToPassengerDetails(this Person entity, Guid clubId, PassengerDetails details = null)
        {
            entity.ArgumentNotNull("entity");

            if (details == null)
            {
                details = new PassengerDetails();
            }

            entity.ToPersonDetails(clubId, details);

            return details;
        }

        public static Person ToPerson(this PersonDetails details, Guid clubId, Person entity = null, bool overwritePersonId = false)
        {
            details.ArgumentNotNull("details");

            if (entity == null)
            {
                entity = new Person();
            }

            if (overwritePersonId) entity.PersonId = details.PersonId;

            entity.AddressLine1 = details.AddressLine1;
            entity.AddressLine2 = details.AddressLine2;
            entity.BusinessPhone = details.BusinessPhoneNumber;
            entity.City = details.City;
            entity.CompanyName = details.CompanyName;
            entity.CountryId = details.CountryId;
            entity.EmailBusiness = details.BusinessEmail;
            entity.EmailPrivate = details.PrivateEmail;
            entity.Firstname = details.Firstname;
            entity.Lastname = details.Lastname;
            entity.Midname = details.Midname;
            entity.MobilePhone = details.MobilePhoneNumber;
            entity.PrivatePhone = details.PrivatePhoneNumber;
            entity.Region = details.Region;
            entity.Zip = details.ZipCode;

            entity.Birthday = details.Birthday;
            entity.FaxNumber = details.FaxNumber;
            entity.ReceiveOwnedAircraftStatisticReports = details.ReceiveOwnedAircraftStatisticReports;
            entity.EnableAddress = details.EnableAddress;

            if (details.ClubRelatedPersonDetails != null)
            {
                var personClub = entity.PersonClubs.FirstOrDefault(pc => pc.ClubId == clubId);

                if (personClub == null)
                {
                    personClub = new PersonClub {ClubId = clubId};
                    entity.PersonClubs.Add(personClub);
                }

                personClub.MemberNumber = details.ClubRelatedPersonDetails.MemberNumber;
                personClub.MemberKey = details.ClubRelatedPersonDetails.MemberKey;
                personClub.MemberStateId = details.ClubRelatedPersonDetails.MemberStateId;
                personClub.IsGliderInstructor = details.ClubRelatedPersonDetails.IsGliderInstructor;
                personClub.IsGliderPilot = details.ClubRelatedPersonDetails.IsGliderPilot;
                personClub.IsGliderTrainee = details.ClubRelatedPersonDetails.IsGliderTrainee;
                personClub.IsMotorPilot = details.ClubRelatedPersonDetails.IsMotorPilot;
                personClub.IsPassenger = details.ClubRelatedPersonDetails.IsPassenger;
                personClub.IsTowPilot = details.ClubRelatedPersonDetails.IsTowPilot;
                personClub.IsWinchOperator = details.ClubRelatedPersonDetails.IsWinchOperator;
                personClub.IsMotorInstructor = details.ClubRelatedPersonDetails.IsMotorInstructor;
                personClub.ReceiveFlightReports = details.ClubRelatedPersonDetails.ReceiveFlightReports;
                personClub.ReceiveAircraftReservationNotifications = details.ClubRelatedPersonDetails.ReceiveAircraftReservationNotifications;
                personClub.ReceivePlanningDayRoleReminder = details.ClubRelatedPersonDetails.ReceivePlanningDayRoleReminder;

                //Handle person categories
                if (details.ClubRelatedPersonDetails.PersonCategoryIds != null && details.ClubRelatedPersonDetails.PersonCategoryIds.Any())
                {
                    //we have some personCategories
                    foreach (var personCategoryId in details.ClubRelatedPersonDetails.PersonCategoryIds)
                    {
                        if (entity.PersonPersonCategories.Any(ppc => ppc.PersonCategoryId == personCategoryId) == false)
                        {
                            //personCategory not found, add it
                            var personPersonCategory = new PersonPersonCategory();
                            personPersonCategory.PersonId = entity.PersonId;
                            personPersonCategory.PersonCategoryId = personCategoryId;
                            entity.PersonPersonCategories.Add(personPersonCategory);
                        }
                    }

                    //remove all personPersonCategories which are not found in current list
                    foreach (var personPersonCategory in entity.PersonPersonCategories.ToList())
                    {
                        if (details.ClubRelatedPersonDetails.PersonCategoryIds.Any(id => id == personPersonCategory.PersonCategoryId) == false)
                        {
                            entity.PersonPersonCategories.Remove(personPersonCategory);
                        }
                    }
                }
                else
                {
                    //we haven't any selected personCategoryIds, so remove all from database
                    foreach (var personPersonCategory in entity.PersonPersonCategories.ToList())
                    {
                        entity.PersonPersonCategories.Remove(personPersonCategory);
                    }
                }
            }
            else
            {
                //we require minimum one PersonClub relation to have the relation to our club
                var personClub = entity.PersonClubs.FirstOrDefault(pc => pc.ClubId == clubId);

                if (personClub == null)
                {
                    personClub = new PersonClub {ClubId = clubId};
                    entity.PersonClubs.Add(personClub);
                }
            }

            return entity;
        }

        public static Person ToPerson(this PilotPersonDetails details, Guid clubId, Person entity = null, bool overwritePersonId = false)
        {
            details.ArgumentNotNull("details");

            if (entity == null)
            {
                entity = new Person();
            }

            ((PersonDetails) details).ToPerson(clubId, entity, overwritePersonId);

            entity.HasGliderInstructorLicence = details.HasGliderInstructorLicence;
            entity.HasGliderPilotLicence = details.HasGliderPilotLicence;
            entity.HasGliderTraineeLicence = details.HasGliderTraineeLicence;
            entity.HasMotorPilotLicence = details.HasMotorPilotLicence;
            entity.HasTowPilotLicence = details.HasTowPilotLicence;
            entity.HasGliderPAXLicence = details.HasGliderPassengerLicence;
            entity.HasTMGLicence = details.HasTMGLicence;
            entity.HasWinchOperatorLicence = details.HasWinchOperatorLicence;
            entity.LicenceNumber = details.LicenceNumber;
            entity.GliderInstructorLicenceExpireDate = details.GliderInstructorLicenceExpireDate;
            entity.MedicalClass1ExpireDate = details.MedicalClass1ExpireDate;
            entity.MedicalClass2ExpireDate = details.MedicalClass2ExpireDate;
            entity.MedicalLaplExpireDate = details.MedicalLaplExpireDate;
            entity.HasGliderTowingStartPermission = details.HasGliderTowingStartPermission;
            entity.HasGliderSelfStartPermission = details.HasGliderSelfStartPermission;
            entity.HasGliderWinchStartPermission = details.HasGliderWinchStartPermission;
            entity.HasMotorInstructorLicence = details.HasMotorInstructorLicence;

            entity.SpotLink = details.SpotLink;
            entity.ReceiveOwnedAircraftStatisticReports = details.ReceiveOwnedAircraftStatisticReports;

            return entity;
        }

        public static Person ToPerson(this PassengerDetails details, Guid clubId, Person entity = null, bool overwritePersonId = false)
        {
            details.ArgumentNotNull("details");

            if (entity == null)
            {
                entity = new Person();
            }

            ((PersonDetails) details).ToPerson(clubId, entity, overwritePersonId);

            return entity;
        }

        public static Person ToPerson(this PilotPersonFullDetails details, Guid clubId, Person entity = null, bool overwritePersonId = false)
        {
            details.ArgumentNotNull("details");

            if (entity == null)
            {
                entity = new Person();
            }

            ((PilotPersonDetails) details).ToPerson(clubId, entity, overwritePersonId);

            //only map timestamps back to the server data entity
            details.MapTimeStampsMetaData(entity);
            entity.DoNotUpdateTimeStampsInMetaData = true;

            var personClub = entity.PersonClubs.FirstOrDefault(pc => pc.ClubId == clubId);

            if (personClub == null)
            {
                throw new NullReferenceException("No PersonClub in Person found!");
            }

            //map person details time stamps back to person club entity
            details.MapTimeStampsMetaData(personClub);
            personClub.DoNotUpdateTimeStampsInMetaData = true;

            return entity;
        }

        public static PilotPersonFullDetails ToPilotPersonFullDetails(this Person entity, Guid clubId, PilotPersonFullDetails details = null)
        {
            entity.ArgumentNotNull("entity");

            if (details == null)
            {
                details = new PilotPersonFullDetails();
            }

            entity.ToPilotPersonDetails(clubId, details);

            entity.MapMetaData(details);

            //handle metadata for club related person details
            if (details.ClubRelatedPersonDetails != null)
            {
                var personClub = entity.PersonClubs.FirstOrDefault(e => e.ClubId == clubId);

                if (personClub != null)
                {
                    //create metadata based club related person full details
                    var fullDetails = new ClubRelatedPersonFullDetails(details.ClubRelatedPersonDetails);

                    fullDetails.CreatedOn = personClub.CreatedOn.SetAsUtc();
                    fullDetails.CreatedByUserId = personClub.CreatedByUserId;
                    fullDetails.DeletedOn = personClub.DeletedOn.SetAsUtc();
                    fullDetails.DeletedByUserId = personClub.DeletedByUserId;
                    fullDetails.ModifiedOn = personClub.ModifiedOn.SetAsUtc();
                    fullDetails.ModifiedByUserId = personClub.ModifiedByUserId;
                    fullDetails.OwnerId = personClub.OwnerId;
                    fullDetails.OwnershipType = personClub.OwnershipType;
                    fullDetails.RecordState = personClub.RecordState;

                    details.ClubRelatedPersonDetails = fullDetails;
                }
            }

            return details;
        }

        public static PersonDashboardDetails ToPersonDashboardDetails(this Person entity, PersonDashboardDetails details = null)
        {
            entity.ArgumentNotNull("entity");

            if (details == null)
            {
                details = new PersonDashboardDetails();
            }

            details.LicenceNumber = entity.LicenceNumber;
            details.MedicalClass1ExpireDate = entity.MedicalClass1ExpireDate.SetAsUtc();
            details.MedicalClass2ExpireDate = entity.MedicalClass2ExpireDate.SetAsUtc();
            details.MedicalLaplExpireDate = entity.MedicalLaplExpireDate.SetAsUtc();
            details.GliderInstructorLicenceExpireDate = entity.GliderInstructorLicenceExpireDate.SetAsUtc();
            details.HasGliderTowingStartPermission = entity.HasGliderTowingStartPermission;
            details.HasGliderSelfStartPermission = entity.HasGliderSelfStartPermission;
            details.HasGliderWinchStartPermission = entity.HasGliderWinchStartPermission;
            details.HasGliderInstructorLicence = entity.HasGliderInstructorLicence;
            details.HasGliderPilotLicence = entity.HasGliderPilotLicence;
            details.HasGliderTraineeLicence = entity.HasGliderTraineeLicence;
            details.HasMotorPilotLicence = entity.HasMotorPilotLicence;
            details.HasTowPilotLicence = entity.HasTowPilotLicence;
            details.HasGliderPassengerLicence = entity.HasGliderPAXLicence;
            details.HasTMGLicence = entity.HasTMGLicence;

            return details;
        }

        #endregion Person, PilotPerson, Passenger, FullPersonDetails

        #region PersonCategory

        public static PersonCategoryOverview ToPersonCategoryOverview(this PersonCategory entity, PersonCategoryOverview overview = null)
        {
            entity.ArgumentNotNull("entity");

            if (overview == null)
            {
                overview = new PersonCategoryOverview();
            }

            overview.PersonCategoryId = entity.PersonCategoryId;

            overview.CategoryName = entity.CategoryName;
            overview.ParentPersonCategoryId = entity.ParentPersonCategoryId;

            return overview;
        }

        public static PersonCategoryDetails ToPersonCategoryDetails(this PersonCategory entity, PersonCategoryDetails details = null)
        {
            entity.ArgumentNotNull("entity");

            if (details == null)
            {
                details = new PersonCategoryDetails();
            }

            details.PersonCategoryId = entity.PersonCategoryId;
            details.CategoryName = entity.CategoryName;
            details.ParentPersonCategoryId = entity.ParentPersonCategoryId;
            details.Remarks = entity.Remarks;

            return details;
        }

        public static PersonCategory ToPersonCategory(this PersonCategoryDetails details, Guid clubId, PersonCategory entity = null, bool overwritePersonCategoryId = false)
        {
            details.ArgumentNotNull("details");

            if (entity == null)
            {
                entity = new PersonCategory();
            }

            if (overwritePersonCategoryId) entity.PersonCategoryId = details.PersonCategoryId;

            entity.ClubId = clubId;
            entity.CategoryName = details.CategoryName;
            entity.ParentPersonCategoryId = details.ParentPersonCategoryId;
            entity.Remarks = details.Remarks;

            return entity;
        }

        #endregion PersonCategory

        #region PlanningDay

        public static PlanningDayOverview ToPlanningDayOverview(this PlanningDay entity, PlanningDayOverview overview = null)
        {
            entity.ArgumentNotNull("entity");

            if (overview == null)
            {
                overview = new PlanningDayOverview();
            }

            overview.PlanningDayId = entity.PlanningDayId;

            overview.Day = entity.Day.SetAsUtc();
            overview.Remarks = entity.Remarks;

            if (entity.Location != null)
            {
                overview.LocationName = entity.Location.LocationName;
            }

            if (entity.PlanningDayAssignments != null)
            {
                foreach (var planningDayAssignment in entity.PlanningDayAssignments.Where(planningDayAssignment => planningDayAssignment.AssignmentType != null))
                {
                    //TODO: remove hack with generic implementation
                    if (planningDayAssignment.AssignmentType.AssignmentTypeName.ToLower() == "schlepppilot")
                    {
                        if (planningDayAssignment.AssignedPerson != null)
                        {
                            overview.TowingPilotName = planningDayAssignment.AssignedPerson.DisplayName;
                        }
                    }

                    if (planningDayAssignment.AssignmentType.AssignmentTypeName.ToLower() == "segelflugleiter")
                    {
                        if (planningDayAssignment.AssignedPerson != null)
                        {
                            overview.FlightOperatorName = planningDayAssignment.AssignedPerson.DisplayName;
                        }
                    }

                    if (planningDayAssignment.AssignmentType.AssignmentTypeName.ToLower() == "fluglehrer")
                    {
                        if (planningDayAssignment.AssignedPerson != null)
                        {
                            overview.InstructorName = planningDayAssignment.AssignedPerson.DisplayName;
                        }
                    }
                }
            }

            return overview;
        }

        public static PlanningDayDetails ToPlanningDayDetails(this PlanningDay entity, PlanningDayDetails details = null)
        {
            entity.ArgumentNotNull("entity");

            if (details == null)
            {
                details = new PlanningDayDetails();
            }

            details.PlanningDayId = entity.PlanningDayId;
            details.Day = entity.Day.SetAsUtc();
            details.LocationId = entity.LocationId;
            details.Remarks = entity.Remarks;

            if (entity.PlanningDayAssignments != null)
            {
                foreach (var planningDayAssignment in entity.PlanningDayAssignments.Where(planningDayAssignment => planningDayAssignment.AssignmentType != null))
                {
                    //TODO: remove hack with generic implementation
                    if (planningDayAssignment.AssignmentType.AssignmentTypeName.ToLower() == "schlepppilot")
                    {
                        if (planningDayAssignment.AssignedPerson != null)
                        {
                            details.TowingPilotPersonId = planningDayAssignment.AssignedPersonId;
                        }
                    }

                    if (planningDayAssignment.AssignmentType.AssignmentTypeName.ToLower() == "segelflugleiter")
                    {
                        if (planningDayAssignment.AssignedPerson != null)
                        {
                            details.FlightOperatorPersonId = planningDayAssignment.AssignedPersonId;
                        }
                    }

                    if (planningDayAssignment.AssignmentType.AssignmentTypeName.ToLower() == "fluglehrer")
                    {
                        if (planningDayAssignment.AssignedPerson != null)
                        {
                            details.InstructorPersonId = planningDayAssignment.AssignedPersonId;
                        }
                    }
                }
            }

            return details;
        }

        public static PlanningDay ToPlanningDay(this PlanningDayDetails details, Guid clubId, List<PlanningDayAssignmentType> planningDayAssignmentTypes, PlanningDay entity = null, bool overwritePlanningDayId = false)
        {
            details.ArgumentNotNull("details");

            if (entity == null)
            {
                entity = new PlanningDay();
            }

            if (overwritePlanningDayId) entity.PlanningDayId = details.PlanningDayId;
            entity.Day = details.Day;
            entity.LocationId = details.LocationId;
            entity.Remarks = details.Remarks;
            entity.ClubId = clubId;

            //handle flight operator assignments
            var flightOperatorAssignmentTypeId = planningDayAssignmentTypes.First(q => q.AssignmentTypeName.ToLower() == "segelflugleiter").PlanningDayAssignmentTypeId;
            var flightOperatorAssignment = entity.PlanningDayAssignments.FirstOrDefault(q => q.AssignmentTypeId == flightOperatorAssignmentTypeId);

            if (details.FlightOperatorPersonId.HasValue && details.FlightOperatorPersonId.Value.IsValid())
            {
                if (flightOperatorAssignment == null)
                {
                    var assignment = new PlanningDayAssignment();
                    assignment.AssignmentTypeId = flightOperatorAssignmentTypeId;
                    assignment.AssignedPersonId = details.FlightOperatorPersonId.Value;
                    entity.PlanningDayAssignments.Add(assignment);
                }
                else
                {
                    flightOperatorAssignment.AssignedPersonId = details.FlightOperatorPersonId.Value;
                }
            }
            else
            {
                if (flightOperatorAssignment != null) entity.PlanningDayAssignments.Remove(flightOperatorAssignment);
            }

            //handle tow pilot assignments
            var towingPilotAssignmentTypeId = planningDayAssignmentTypes.First(q => q.AssignmentTypeName.ToLower() == "schlepppilot").PlanningDayAssignmentTypeId;
            var towingPilotAssignment = entity.PlanningDayAssignments.FirstOrDefault(q => q.AssignmentTypeId == towingPilotAssignmentTypeId);

            if (details.TowingPilotPersonId.HasValue && details.TowingPilotPersonId.Value.IsValid())
            {
                if (towingPilotAssignment == null)
                {
                    var assignment = new PlanningDayAssignment();
                    assignment.AssignmentTypeId = towingPilotAssignmentTypeId;
                    assignment.AssignedPersonId = details.TowingPilotPersonId.Value;
                    entity.PlanningDayAssignments.Add(assignment);
                }
                else
                {
                    towingPilotAssignment.AssignedPersonId = details.TowingPilotPersonId.Value;
                }
            }
            else
            {
                if (towingPilotAssignment != null) entity.PlanningDayAssignments.Remove(towingPilotAssignment);
            }

            //handle instructor assignments
            var assignmentType = planningDayAssignmentTypes.FirstOrDefault(q => q.AssignmentTypeName.ToLower() == "fluglehrer");

            if (assignmentType != null)
            {
                var instructorAssignmentTypeId = assignmentType.PlanningDayAssignmentTypeId;
                var instructorAssignment =
                    entity.PlanningDayAssignments.FirstOrDefault(q => q.AssignmentTypeId == instructorAssignmentTypeId);

                if (details.InstructorPersonId.HasValue && details.InstructorPersonId.Value.IsValid())
                {
                    if (instructorAssignment == null)
                    {
                        var assignment = new PlanningDayAssignment();
                        assignment.AssignmentTypeId = instructorAssignmentTypeId;
                        assignment.AssignedPersonId = details.InstructorPersonId.Value;
                        entity.PlanningDayAssignments.Add(assignment);
                    }
                    else
                    {
                        instructorAssignment.AssignedPersonId = details.InstructorPersonId.Value;
                    }
                }
                else
                {
                    if (instructorAssignment != null) entity.PlanningDayAssignments.Remove(instructorAssignment);
                }
            }

            return entity;
        }

        #endregion PlanningDay

        #region Role

        public static RoleOverview ToRoleOverview(this DbEntities.Role entity, RoleOverview overview = null)
        {
            entity.ArgumentNotNull("entity");

            if (overview == null)
            {
                overview = new RoleOverview();
            }

            overview.RoleId = entity.RoleId;
            overview.RoleApplicationKeyString = entity.RoleApplicationKeyString;
            overview.RoleName = entity.RoleName;

            return overview;
        }

        public static Role ToRole(this Role sourceEntity, Role entity = null, bool overwriteUserId = false)
        {
            sourceEntity.ArgumentNotNull("sourceEntity");

            if (entity == null)
            {
                entity = new Role();
            }

            if (overwriteUserId) entity.RoleId = sourceEntity.RoleId;

            entity.RoleName = sourceEntity.RoleName;
            entity.RoleApplicationKeyString = sourceEntity.RoleApplicationKeyString;

            return entity;
        }

        #endregion Role

        #region StartType

        public static StartTypeListItem ToStartTypeListItem(this StartType entity, StartTypeListItem listItem = null)
        {
            entity.ArgumentNotNull("entity");

            if (listItem == null)
            {
                listItem = new StartTypeListItem();
            }

            listItem.StartTypeId = entity.StartTypeId;

            listItem.StartTypeName = entity.StartTypeName;
            listItem.IsForGliderFlights = entity.IsForGliderFlights;
            listItem.IsForTowFlights = entity.IsForTowFlights;
            listItem.IsForMotorFlights = entity.IsForMotorFlights;

            if (entity.StartTypeId == (int) AircraftStartType.WinchLaunch)
            {
                listItem.IsWinchStart = true;
            }

            return listItem;
        }

        #endregion StartType

        #region SystemData

        public static SystemDataDetails ToSystemDataDetails(this SystemData entity, SystemDataDetails details = null)
        {
            entity.ArgumentNotNull("entity");

            if (details == null)
            {
                details = new SystemDataDetails();
            }

            details.SystemDataId = entity.SystemId;
            details.BaseURL = entity.BaseURL;
            details.ReportSenderEmailAddress = entity.ReportSenderEmailAddress;
            details.SystemSenderEmailAddress = entity.SystemSenderEmailAddress;
            details.UseSmtpAuthentication = entity.UseSmtpAuthentication;
            details.UseSSLforSmtpConnection = entity.UseSSLforSmtpConnection;
            details.SmtpUsername = entity.SmtpUsername;
            details.SmtpPassword = entity.SmtpPassword;
            details.SmtpServer = entity.SmtpServer;
            details.SmtpPort = entity.SmtpPort;
            details.MaxUserLoginAttempts = entity.MaxUserLoginAttempts;
            details.Testmode = entity.Testmode;
            details.TestmodeEmailPickupDirectory = entity.TestmodeEmailPickupDirectory;
            details.DebugMode = entity.DebugMode;
            details.SendToBccRecipients = entity.SendToBccRecipients;
            details.BccRecipientEmailAddresses = entity.BccRecipientEmailAddresses;

            return details;
        }

        public static SystemData ToSystemData(this SystemDataDetails details, SystemData entity = null, bool overwriteSystemDataId = false)
        {
            details.ArgumentNotNull("details");

            if (entity == null)
            {
                entity = new SystemData();
            }

            if (overwriteSystemDataId) entity.SystemId = details.SystemDataId;
            entity.BaseURL = details.BaseURL;
            entity.ReportSenderEmailAddress = details.ReportSenderEmailAddress;
            entity.SystemSenderEmailAddress = details.SystemSenderEmailAddress;
            entity.UseSmtpAuthentication = details.UseSmtpAuthentication;
            entity.UseSSLforSmtpConnection = details.UseSSLforSmtpConnection;
            entity.SmtpUsername = details.SmtpUsername;
            entity.SmtpPassword = details.SmtpPassword;
            entity.SmtpServer = details.SmtpServer;
            entity.SmtpPort = details.SmtpPort;
            entity.MaxUserLoginAttempts = details.MaxUserLoginAttempts;
            entity.Testmode = details.Testmode;
            entity.TestmodeEmailPickupDirectory = details.TestmodeEmailPickupDirectory;
            entity.DebugMode = details.DebugMode;
            entity.SendToBccRecipients = details.SendToBccRecipients;
            entity.BccRecipientEmailAddresses = details.BccRecipientEmailAddresses;

            return entity;
        }

        #endregion SystemData

        #region SystemLog

        public static SystemLogOverview ToSystemLogOverview(this SystemLog entity, SystemLogOverview overview = null)
        {
            entity.ArgumentNotNull("entity");

            if (overview == null)
            {
                overview = new SystemLogOverview();
            }

            overview.LogId = entity.LogId;

            overview.EventDateTime = entity.EventDateTime.SetAsUtc();
            overview.LogLevel = entity.LogLevel;
            overview.EventType = entity.EventType;
            overview.Logger = entity.Logger;
            overview.Message = entity.Message;
            overview.UserName = entity.UserName;

            return overview;
        }

        public static SystemLogDetails ToSystemLogDetails(this SystemLog entity, SystemLogDetails details = null)
        {
            entity.ArgumentNotNull("entity");

            if (details == null)
            {
                details = new SystemLogDetails();
            }

            details.LogId = entity.LogId;

            details.EventDateTime = entity.EventDateTime.SetAsUtc();
            details.LogLevel = entity.LogLevel;
            details.EventType = entity.EventType;
            details.Logger = entity.Logger;
            details.Message = entity.Message;
            details.UserName = entity.UserName;

            details.Application = entity.Application;
            details.ComputerName = entity.ComputerName;
            details.CallSite = entity.CallSite;
            details.Thread = entity.Thread;
            details.Exception = entity.Exception;
            details.Stacktrace = entity.Stacktrace;

            return details;
        }

        #endregion SystemLog

        #region SystemVersionInfoOverview

        public static SystemVersionInfoOverview ToSystemVersionInfoOverview(this SystemVersionInfoDetails systemVersionInfoDetails, SystemVersionInfoOverview systemVersionInfoOverview = null)
        {
            var mostRecentAssembly = systemVersionInfoDetails.AssembliesInfo.OrderByDescending(q => q.BuildDateTime).FirstOrDefault();

            if (systemVersionInfoOverview == null) systemVersionInfoOverview = new SystemVersionInfoOverview();

            if (mostRecentAssembly != null)
            {
                systemVersionInfoOverview.BuildDateTime = mostRecentAssembly.BuildDateTime;
                systemVersionInfoOverview.Version = mostRecentAssembly.Version;
            }

            systemVersionInfoOverview.DatabaseSchemaVersion = systemVersionInfoDetails.DatabaseSchemaVersion;

            return systemVersionInfoOverview;
        }

        #endregion

        #region User

        public static UserOverview ToUserOverview(this User entity, UserOverview overview = null)
        {
            entity.ArgumentNotNull("entity");

            if (overview == null)
            {
                overview = new UserOverview();
            }

            overview.UserId = entity.UserId;

            overview.FriendlyName = entity.FriendlyName;
            overview.NotificationEmail = entity.NotificationEmail;
            overview.UserName = entity.UserName;

            overview.UserRoles = entity.UserRoles.Where(userRole => userRole.Role != null).OrderBy(userRole => userRole.RoleId).Aggregate("", (current, userRole) => current + (userRole.Role.RoleName + ", "));

            if (overview.UserRoles.Length > 2)
            {
                overview.UserRoles = overview.UserRoles.Remove(overview.UserRoles.Length - 2);
            }

            if (entity.Club != null)
            {
                overview.ClubName = entity.Club.Clubname;
            }
            else
            {
                LogManager.GetCurrentClassLogger().Error($"User: {entity.UserName} has no relation to club or club might be set as deleted!");
            }

            overview.AccountState = entity.GetUserAccountStateString();

            if (entity.Person != null)
            {
                overview.PersonName = entity.Person.DisplayName;
            }

            return overview;
        }

        public static UserDetails ToUserDetails(this User entity, UserDetails details = null)
        {
            entity.ArgumentNotNull("entity");

            if (details == null)
            {
                details = new UserDetails();
            }

            details.UserId = entity.UserId;
            details.UserName = entity.UserName;
            details.AccountState = entity.AccountState;
            details.ClubId = entity.ClubId;
            details.FriendlyName = entity.FriendlyName;
            details.NotificationEmail = entity.NotificationEmail;
            details.PersonId = entity.PersonId;
            details.Remarks = entity.Remarks;

            details.LastPasswordChangeOn = entity.LastPasswordChangeOn;
            details.ForcePasswordChangeNextLogon = entity.ForcePasswordChangeNextLogon;

            details.AccountState = entity.AccountState;

            foreach (var userRole in entity.UserRoles)
            {
                details.UserRoleIds.Add(userRole.RoleId);
            }

            return details;
        }

        public static User ToUser(this UserDetails details, User entity = null, bool overwriteUserId = false)
        {
            details.ArgumentNotNull("details");

            if (entity == null)
            {
                entity = new User();
            }

            if (overwriteUserId) entity.UserId = details.UserId;

            entity.UserName = details.UserName;
            entity.ClubId = details.ClubId;
            entity.FriendlyName = details.FriendlyName;
            entity.NotificationEmail = details.NotificationEmail;
            entity.PersonId = details.PersonId;
            entity.Remarks = details.Remarks;

            //set by server
            //entity.LastPasswordChangeOn = details.LastPasswordChangeOn;
            entity.ForcePasswordChangeNextLogon = details.ForcePasswordChangeNextLogon;

            entity.AccountState = details.AccountState;

            return entity;
        }

        public static User ToUser(this UserRegistrationDetails details, Guid clubId, FLS.Data.WebApi.User.UserAccountState userAccountState, User entity = null)
        {
            details.ArgumentNotNull("details");

            if (entity == null)
            {
                entity = new User();
            }

            entity.UserName = details.UserName;
            entity.ClubId = clubId;
            entity.FriendlyName = details.FriendlyName;
            entity.NotificationEmail = details.NotificationEmail;
            entity.PersonId = details.PersonId;
            entity.Remarks = details.Remarks;

            //set by server
            //entity.LastPasswordChangeOn = details.LastPasswordChangeOn;
            entity.ForcePasswordChangeNextLogon = false;

            entity.AccountState = (int)userAccountState;

            return entity;
        }

        public static User ToUser(this User sourceEntity, User entity = null, bool overwriteUserId = false)
        {
            sourceEntity.ArgumentNotNull("sourceEntity");

            if (entity == null)
            {
                entity = new User();
            }

            if (overwriteUserId) entity.UserId = sourceEntity.UserId;

            entity.UserName = sourceEntity.UserName;
            entity.ClubId = sourceEntity.ClubId;
            entity.FriendlyName = sourceEntity.FriendlyName;
            entity.NotificationEmail = sourceEntity.NotificationEmail;
            entity.PersonId = sourceEntity.PersonId;
            entity.Remarks = sourceEntity.Remarks;

            //set by server
            //entity.LastPasswordChangeOn = details.LastPasswordChangeOn;
            entity.ForcePasswordChangeNextLogon = sourceEntity.ForcePasswordChangeNextLogon;

            entity.AccountState = sourceEntity.AccountState;
            entity.SecurityStamp = sourceEntity.SecurityStamp;
            entity.AccessFailedCount = sourceEntity.AccessFailedCount;
            entity.EmailConfirmed = sourceEntity.EmailConfirmed;
            entity.LockoutEnabled = sourceEntity.LockoutEnabled;
            entity.LockoutEndDateUtc = sourceEntity.LockoutEndDateUtc;
            entity.DoNotUpdateMetaData = sourceEntity.DoNotUpdateMetaData;
            
            return entity;
        }

        #endregion User

        #region UserAccountState

        public static UserAccountStateListItem ToUserAccountStateListItem(this DbEntities.UserAccountState entity, UserAccountStateListItem listItem = null)
        {
            entity.ArgumentNotNull("entity");

            if (listItem == null)
            {
                listItem = new UserAccountStateListItem();
            }

            listItem.UserAccountStateId = entity.UserAccountStateId;
            listItem.AccountStateName = entity.UserAccountStateName;

            return listItem;
        }

        #endregion UserAccountState

        #region Helper Methods

        private static Aircraft GetRelatedAircraft(List<Aircraft> relatedAircrafts, FlightDetailsData flightDetailsData)
        {
            if (relatedAircrafts != null)
            {
                return relatedAircrafts.FirstOrDefault(a => a.AircraftId == flightDetailsData.AircraftId);
            }

            return null;
        }

        private static FlightType GetRelatedFlightType(List<FlightType> relatedFlightTypes, FlightDetailsData flightDetailsData)
        {
            if (relatedFlightTypes != null && flightDetailsData.FlightTypeId.HasValue)
            {
                return relatedFlightTypes.FirstOrDefault(a => a.FlightTypeId == flightDetailsData.FlightTypeId.Value);
            }

            return null;
        }

        #endregion Helper Methods
    }
}