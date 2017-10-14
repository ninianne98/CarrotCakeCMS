CREATE TABLE [dbo].[tblRegistrations] (
    [RegistrationID]    UNIQUEIDENTIFIER CONSTRAINT [DF_tblRegistrations_RegistrationID] DEFAULT (newid()) NOT NULL,
    [SiteID]            UNIQUEIDENTIFIER NOT NULL,
    [FirstName]         VARCHAR (64)     NULL,
    [LastName]          VARCHAR (64)     NULL,
    [PhoneNbr]          VARCHAR (32)     NULL,
    [Address]           VARCHAR (256)    NULL,
    [NumberRoomsNeeded] INT              NULL,
    [EventYear]         INT              NULL,
    [SubmitDate]        DATETIME         CONSTRAINT [DF_tblRegistrations_EditDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_tblRegistrations] PRIMARY KEY CLUSTERED ([RegistrationID] ASC)
);

