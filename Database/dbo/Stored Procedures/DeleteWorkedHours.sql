-- Create stored procedure in SQL Server
CREATE PROCEDURE [dbo].[DeleteWorkedHours]
    @month int,
    @year int	
	AS
	BEGIN
	delete [dbo].[worked_hours]
	where [month] = @month and [year] = @year
	
	return @@ROWCOUNT;
END;