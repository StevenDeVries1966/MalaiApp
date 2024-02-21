-- Create stored procedure in SQL Server
CREATE PROCEDURE [dbo].[GetDataClientMonth]
    @month INT,
    @year INT,
    @clt_code VARCHAR(5)
AS
BEGIN
    -- Select statement with INNER JOIN and WHERE clause
    SELECT
        entry_id,
        wh.emp_id,
        e.emp_code,
        clt_code,
        job_name,
        job_id,
        week,
        month,
        year,
        work_date,
        notes,
        start_time,
        end_time,
        minutes_worked,
        hours_worked
    FROM worked_hours wh
    INNER JOIN employee e ON wh.emp_id = e.emp_id
    WHERE MONTH(start_time) = @month AND YEAR(start_time) = @year AND @clt_code = clt_code
    ORDER BY start_time;
END;