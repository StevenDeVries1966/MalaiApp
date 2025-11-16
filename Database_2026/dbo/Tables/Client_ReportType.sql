CREATE TABLE [dbo].[Client_ReportType] (
    [Id]           INT IDENTITY (1, 1) NOT NULL,
    [ClientId]     INT NULL,
    [ReportTypeId] INT NULL,
    CONSTRAINT [PK_Client_ReportType] PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_Client_ReportType_Client] FOREIGN KEY ([ClientId]) REFERENCES Client([Id]), 
    CONSTRAINT [FK_Client_ReportType_ReportType] FOREIGN KEY ([ReportTypeId]) REFERENCES [ReportType]([Id])
);

