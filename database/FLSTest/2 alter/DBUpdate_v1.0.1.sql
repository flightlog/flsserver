USE [FLSTest]
GO
/****** Object:  Table [dbo].[AircraftReservations]    Script Date: 28.12.2014 13:32:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AircraftReservations](
	[AircraftReservationId] [uniqueidentifier] NOT NULL,
	[Start] [datetime2](7) NOT NULL,
	[End] [datetime2](7) NOT NULL,
	[IsAllDayReservation] [bit] NOT NULL,
	[AircraftId] [uniqueidentifier] NOT NULL,
	[PilotPersonId] [uniqueidentifier] NOT NULL,
	[InstructorPersonId] [uniqueidentifier] NULL,
	[ReservationTypeId] [uniqueidentifier] NOT NULL,
	[Remarks] [nvarchar](max) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedByUserId] [uniqueidentifier] NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedByUserId] [uniqueidentifier] NULL,
	[RecordState] [int] NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[OwnershipType] [int] NOT NULL,
	[PilotPerson_PersonId] [uniqueidentifier] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.AircraftReservations] PRIMARY KEY CLUSTERED 
(
	[AircraftReservationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AircraftReservationTypes]    Script Date: 28.12.2014 13:32:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AircraftReservationTypes](
	[AircraftReservationTypeId] [uniqueidentifier] NOT NULL,
	[AircraftReservationTypeName] [nvarchar](50) NOT NULL,
	[Remarks] [nvarchar](max) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
 CONSTRAINT [PK_dbo.AircraftReservationTypes] PRIMARY KEY CLUSTERED 
(
	[AircraftReservationTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PlanningDayAssignments]    Script Date: 28.12.2014 13:32:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlanningDayAssignments](
	[PlanningDayAssignmentId] [uniqueidentifier] NOT NULL,
	[AssignedPlanningDayId] [uniqueidentifier] NOT NULL,
	[AssignedPersonId] [uniqueidentifier] NOT NULL,
	[AssignmentTypeId] [uniqueidentifier] NOT NULL,
	[Remarks] [nvarchar](max) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedByUserId] [uniqueidentifier] NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedByUserId] [uniqueidentifier] NULL,
	[RecordState] [int] NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[OwnershipType] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.PlanningDayAssignments] PRIMARY KEY CLUSTERED 
(
	[PlanningDayAssignmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PlanningDayAssignmentTypes]    Script Date: 28.12.2014 13:32:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlanningDayAssignmentTypes](
	[PlanningDayAssignmentTypeId] [uniqueidentifier] NOT NULL,
	[AssignmentTypeName] [nvarchar](max) NULL,
	[ClubId] [uniqueidentifier] NOT NULL,
	[RequiredNrOfPlanningDayAssignments] [int] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedByUserId] [uniqueidentifier] NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedByUserId] [uniqueidentifier] NULL,
	[RecordState] [int] NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[OwnershipType] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.PlanningDayAssignmentTypes] PRIMARY KEY CLUSTERED 
(
	[PlanningDayAssignmentTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PlanningDays]    Script Date: 28.12.2014 13:32:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlanningDays](
	[PlanningDayId] [uniqueidentifier] NOT NULL,
	[ClubId] [uniqueidentifier] NOT NULL,
	[Day] [date] NOT NULL,
	[LocationId] [uniqueidentifier] NOT NULL,
	[Remarks] [nvarchar](max) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedByUserId] [uniqueidentifier] NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedByUserId] [uniqueidentifier] NULL,
	[RecordState] [int] NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[OwnershipType] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.PlanningDays] PRIMARY KEY CLUSTERED 
(
	[PlanningDayId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[AircraftReservations]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AircraftReservations_dbo.AircraftReservationTypes_ReservationTypeId] FOREIGN KEY([ReservationTypeId])
REFERENCES [dbo].[AircraftReservationTypes] ([AircraftReservationTypeId])
GO
ALTER TABLE [dbo].[AircraftReservations] CHECK CONSTRAINT [FK_dbo.AircraftReservations_dbo.AircraftReservationTypes_ReservationTypeId]
GO
ALTER TABLE [dbo].[AircraftReservations]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AircraftReservations_dbo.Aircrafts_AircraftId] FOREIGN KEY([AircraftId])
REFERENCES [dbo].[Aircrafts] ([AircraftId])
GO
ALTER TABLE [dbo].[AircraftReservations] CHECK CONSTRAINT [FK_dbo.AircraftReservations_dbo.Aircrafts_AircraftId]
GO
ALTER TABLE [dbo].[AircraftReservations]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AircraftReservations_dbo.Persons_InstructorPersonId] FOREIGN KEY([InstructorPersonId])
REFERENCES [dbo].[Persons] ([PersonId])
GO
ALTER TABLE [dbo].[AircraftReservations] CHECK CONSTRAINT [FK_dbo.AircraftReservations_dbo.Persons_InstructorPersonId]
GO
ALTER TABLE [dbo].[AircraftReservations]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AircraftReservations_dbo.Persons_PilotPerson_PersonId] FOREIGN KEY([PilotPerson_PersonId])
REFERENCES [dbo].[Persons] ([PersonId])
GO
ALTER TABLE [dbo].[AircraftReservations] CHECK CONSTRAINT [FK_dbo.AircraftReservations_dbo.Persons_PilotPerson_PersonId]
GO

ALTER TABLE [dbo].[PlanningDayAssignments]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PlanningDayAssignments_dbo.Persons_AssignedPersonId] FOREIGN KEY([AssignedPersonId])
REFERENCES [dbo].[Persons] ([PersonId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PlanningDayAssignments] CHECK CONSTRAINT [FK_dbo.PlanningDayAssignments_dbo.Persons_AssignedPersonId]
GO
ALTER TABLE [dbo].[PlanningDayAssignments]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PlanningDayAssignments_dbo.PlanningDayAssignmentTypes_AssignmentTypeId] FOREIGN KEY([AssignmentTypeId])
REFERENCES [dbo].[PlanningDayAssignmentTypes] ([PlanningDayAssignmentTypeId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PlanningDayAssignments] CHECK CONSTRAINT [FK_dbo.PlanningDayAssignments_dbo.PlanningDayAssignmentTypes_AssignmentTypeId]
GO
ALTER TABLE [dbo].[PlanningDayAssignments]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PlanningDayAssignments_dbo.PlanningDays_AssignedPlanningDayId] FOREIGN KEY([AssignedPlanningDayId])
REFERENCES [dbo].[PlanningDays] ([PlanningDayId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PlanningDayAssignments] CHECK CONSTRAINT [FK_dbo.PlanningDayAssignments_dbo.PlanningDays_AssignedPlanningDayId]
GO
ALTER TABLE [dbo].[PlanningDayAssignmentTypes]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PlanningDayAssignmentTypes_dbo.Clubs_ClubId] FOREIGN KEY([ClubId])
REFERENCES [dbo].[Clubs] ([ClubId])
GO
ALTER TABLE [dbo].[PlanningDayAssignmentTypes] CHECK CONSTRAINT [FK_dbo.PlanningDayAssignmentTypes_dbo.Clubs_ClubId]
GO
ALTER TABLE [dbo].[PlanningDays]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PlanningDays_dbo.Clubs_ClubId] FOREIGN KEY([ClubId])
REFERENCES [dbo].[Clubs] ([ClubId])
GO
ALTER TABLE [dbo].[PlanningDays] CHECK CONSTRAINT [FK_dbo.PlanningDays_dbo.Clubs_ClubId]
GO
ALTER TABLE [dbo].[PlanningDays]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PlanningDays_dbo.Locations_LocationId] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Locations] ([LocationId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PlanningDays] CHECK CONSTRAINT [FK_dbo.PlanningDays_dbo.Locations_LocationId]
GO
