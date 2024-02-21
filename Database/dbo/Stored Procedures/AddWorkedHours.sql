-- Create stored procedure in SQL Server
CREATE PROCEDURE [dbo].[AddWorkedHours]
    @emp_code VARCHAR(5),
    @clt_code VARCHAR(5),
    @clt_job_code VARCHAR(100), -- e.g., 'IMC - AP Rejections' => "{_clt_code} - {_job_name}"
    @notes VARCHAR(500),
    @start_time DATETIME,
    @end_time DATETIME
AS
BEGIN
    -- Insert statement
    DECLARE @emp_id INT;
    DECLARE @job_id INT;
    DECLARE @job_name VARCHAR(50);
    DECLARE @week INT;
    DECLARE @month INT;
    DECLARE @year INT;
    DECLARE @minutes_worked FLOAT; -- Using FLOAT for minutes
    DECLARE @hours_worked FLOAT;

    SET @emp_id = (SELECT emp_id FROM employee WHERE emp_code = @emp_code);
    --SET @job_name = REPLACE(REPLACE(@clt_job_code, @clt_code, ''), ' - ', '');
	SET @job_name = SUBSTRING(@clt_job_code, CHARINDEX(' - ', @clt_job_code) + 3, LEN(@clt_job_code))
    SET @job_id = (SELECT top(1)job_id FROM job WHERE LOWER(RTRIM(LTRIM(clt_code))) = LOWER(RTRIM(LTRIM(@clt_code))) AND LOWER(RTRIM(LTRIM(job_name))) = LOWER(RTRIM(LTRIM(@job_name))));

    SET @week = DATEPART(WEEK, @start_time);
    SET @month = MONTH(@start_time);
    SET @year = YEAR(@start_time);

    SET @minutes_worked = DATEDIFF(MINUTE, @start_time, @end_time);
    IF @minutes_worked < 0
        SET @minutes_worked = @minutes_worked + (24 * 60);

    SET @hours_worked = CAST(@minutes_worked / 60.0 AS FLOAT);
    IF @hours_worked < 0
        SET @hours_worked = @hours_worked + 24;

    INSERT INTO worked_hours (emp_id, clt_code, job_name, job_id, week, month, year, work_date, notes, start_time, end_time, minutes_worked, hours_worked)
    VALUES (@emp_id, @clt_code, @job_name, @job_id, @week, @month, @year, CAST(@start_time AS DATE), @notes, @start_time, @end_time, @minutes_worked, @hours_worked);
END;