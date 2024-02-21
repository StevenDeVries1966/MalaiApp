-- Create stored procedure in SQL Server
CREATE PROCEDURE GetAllWorkedHours
AS
BEGIN
    -- Select statement with INNER JOIN
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
    ORDER BY start_time;
END;