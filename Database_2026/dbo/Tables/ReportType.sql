CREATE TABLE [dbo].[ReportType] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (255) NULL,
    CONSTRAINT [PK_ReportType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

