-- Create stored procedure in SQL Server
CREATE PROCEDURE [dbo].[GetAllWorkedHours]
AS
BEGIN
    -- Select statement with INNER JOIN
    SELECT
        entry_id,
        wh.emp_id,
        wh.clt_code,
        wh.job_name,
        wh.job_id,
        week,
        month,
        year,
        work_date,
        notes,
        start_time,
        end_time,
        minutes_worked,
        hours_worked,
		e.*,
		c.*,
		j.*
    FROM worked_hours wh
    INNER JOIN employee e ON wh.emp_id = e.emp_id
    INNER JOIN client c ON wh.clt_code= c.clt_code
    INNER JOIN job j ON wh.job_id = j.job_id
    ORDER BY wh.start_time;
END;