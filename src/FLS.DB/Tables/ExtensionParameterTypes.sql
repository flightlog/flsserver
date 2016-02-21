CREATE TABLE [dbo].[ExtensionParameterTypes] (
    [ExtensionParameterTypeId]   INT            IDENTITY (1, 1) NOT NULL,
    [ExtensionParameterTypeName] NVARCHAR (50)  NOT NULL,
    [StoreValuesAsBinaryData]    BIT            NOT NULL,
    [Comment]                    NVARCHAR (MAX) NULL,
    [CreatedOn]                  DATETIME2 (7)  NOT NULL,
    [ModifiedOn]                 DATETIME2 (7)  NULL,
    CONSTRAINT [PK_ExtensionParameterTypes] PRIMARY KEY CLUSTERED ([ExtensionParameterTypeId] ASC)
);

