-- Create stored procedure in SQL Server
CREATE PROCEDURE AddJob
    @job_name VARCHAR(255),
    @clt_code VARCHAR(5)
AS
BEGIN
    -- Insert statement
    INSERT INTO job (job_name, clt_code)
    VALUES (@job_name, @clt_code);
END;