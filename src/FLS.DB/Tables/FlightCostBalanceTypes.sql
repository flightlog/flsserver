CREATE TABLE [dbo].[FlightCostBalanceTypes] (
    [FlightCostBalanceTypeId]   INT            IDENTITY (1, 1) NOT NULL,
    [FlightCostBalanceTypeName] NVARCHAR (100) NOT NULL,
    [Comment]                   NVARCHAR (500) NULL,
    [PersonForInvoiceRequired]  BIT            CONSTRAINT [DF__FlightCos__Perso__18B6AB08] DEFAULT ((0)) NOT NULL,
    [IsForGliderFlights]        BIT            CONSTRAINT [DF__FlightCos__IsFor__19AACF41] DEFAULT ((1)) NOT NULL,
    [IsForTowFlights]           BIT            CONSTRAINT [DF__FlightCos__IsFor__1A9EF37A] DEFAULT ((0)) NOT NULL,
    [IsForMotorFlights]         BIT            CONSTRAINT [DF__FlightCos__IsFor__1B9317B3] DEFAULT ((0)) NOT NULL,
    [CreatedOn]                 DATETIME2 (7)  NOT NULL,
    [ModifiedOn]                DATETIME2 (7)  NULL,
    CONSTRAINT [PK_FlightCostBalanceTypes] PRIMARY KEY CLUSTERED ([FlightCostBalanceTypeId] ASC)
);

