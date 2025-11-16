USE [malai-2026-uat]
GO

select * from [dbo].[Client]
select * from [dbo].[ReportType]
select * from [dbo].[Client_ReportType]

INSERT INTO [dbo].[Client]
           ([Name]
           ,[Email])
     VALUES
           ('Steven'
           ,'steven.d.vries@gmail.com')
		   
INSERT INTO [dbo].[Client]
           ([Name]
           ,[Email])
     VALUES
           ('Steven de Vries'
           ,'steven.devries@arcadis.com')
INSERT INTO [dbo].[ReportType]
           ([Name])
     VALUES
           ('Type_A')
INSERT INTO [dbo].[ReportType]
           ([Name])
     VALUES
           ('Type_B')
INSERT INTO [dbo].[ReportType]
           ([Name])
     VALUES
           ('Type_C')

Select * from [dbo].[Client] as c
join [dbo].[Client_ReportType] as cr on c.Id = cr.ClientId
join [dbo].[ReportType] as r on cr.ReportTypeId = r.Id



