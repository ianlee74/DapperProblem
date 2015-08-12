-- Step #1:  Create an Azure Database.
-- Step #2:  Run the following against the new database.
CREATE TABLE [dbo].[Clients] (
    [Id]             INT             IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (256)  NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Name] ASC),
);
go

insert into dbo.Clients (Name) values ('DapperTest');
go

CREATE TABLE [dbo].[Surveys] (
    [Id]                  INT            IDENTITY (1, 1) NOT NULL,
    [Name]                NVARCHAR (512) NOT NULL,
    [IsActive]            BIT            DEFAULT (1) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Name] ASC)
);
go

insert into dbo.Surveys (Name, IsActive) values ('Some Survey', 1);
go

CREATE TABLE [dbo].[ClientSurveys] (
    [Id]                   INT            IDENTITY (1, 1) NOT NULL,
    [ClientId]             INT            NOT NULL,
    [SurveyId]             INT            NOT NULL,
    [Name]                 NVARCHAR (512) NOT NULL,
    [StartDate]            DATETIME       NULL,
    [EndDate]              DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ClientSurveys_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [dbo].[Clients] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_ClientSurveys_SurveyId] FOREIGN KEY ([SurveyId]) REFERENCES [dbo].[Surveys] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);
go
create unique index SK_ClientSurveys on dbo.ClientSurveys (ClientId, SurveyId, StartDate)
go

insert into dbo.ClientSurveys (ClientId, SurveyId, Name, StartDate, EndDate) values (1, 1, 'Client Survey #1', '1/1/1900', '1/1/1901');
insert into dbo.ClientSurveys (ClientId, SurveyId, Name, StartDate, EndDate) values (1, 1, 'Client Survey #2', '1/1/1901', '1/1/1902');
insert into dbo.ClientSurveys (ClientId, SurveyId, Name, StartDate, EndDate) values (1, 1, 'Client Survey #3', '1/1/1902', '1/1/2016');
go

CREATE TABLE [dbo].[Patients] (
    [Id]             INT             IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (256)  NOT NULL,
	[SSN]			 nvarchar(9) not null,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Name] ASC),
);
go

insert into dbo.Patients (Name, SSN) values ('DOE, JOHN', '123456789');
insert into dbo.Patients (Name, SSN) values ('DOE, JANE', '234567890');
go

-- Step #3: Enable Dynamic Data Masking and add a mask on Patients.SSN.
-- Step #4: Test using mydatabase.database.windows.net vs mydatabase.database.secure.windows.net connections.