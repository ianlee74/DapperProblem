CREATE DATABASE [DapperProblem]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DapperProblem', FILENAME = N'D:\Databases\MSSQL12.MSSQLSERVER\MSSQL\DATA\DapperProblem.mdf' , SIZE = 4096KB , FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'DapperProblem_log', FILENAME = N'D:\Databases\MSSQL12.MSSQLSERVER\MSSQL\DATA\DapperProblem_log.ldf' , SIZE = 1024KB , FILEGROWTH = 10%)
GO

use DapperProblem;
go

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
    [IsPatientDefault]     BIT            DEFAULT ((0)) NOT NULL,
    [ClientSurveyBundleId] INT            NULL,
    [IsLocked]             BIT            DEFAULT ((0)) NOT NULL,
    [IntroHtml]            NVARCHAR (MAX) NULL,
    [OutroHtml]            NVARCHAR (MAX) NULL,
    [MaxResponses]         INT            DEFAULT ((1)) NOT NULL,
	[Description]		   NVARCHAR(2000) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ClientSurveys_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [dbo].[Clients] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_ClientSurveys_SurveyId] FOREIGN KEY ([SurveyId]) REFERENCES [dbo].[Surveys] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);
go
create unique index SK_ClientSurveys on dbo.ClientSurveys (ClientId, SurveyId, StartDate)
go

insert into dbo.ClientSurveys (ClientId, SurveyId, Name, StartDate, EndDate, IsPatientDefault) values (1, 1, 'Client Survey #1', '1/1/1900', '1/1/1901', 0);
insert into dbo.ClientSurveys (ClientId, SurveyId, Name, StartDate, EndDate, IsPatientDefault) values (1, 1, 'Client Survey #2', '1/1/1901', '1/1/1902', 0);
insert into dbo.ClientSurveys (ClientId, SurveyId, Name, StartDate, EndDate, IsPatientDefault) values (1, 1, 'Client Survey #3', '1/1/1902', '1/1/2016', 1);
go

