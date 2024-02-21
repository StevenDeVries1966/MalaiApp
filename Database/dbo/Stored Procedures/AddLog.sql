-- Create stored procedure in SQL Server
CREATE PROCEDURE AddLog
    @message VARCHAR(100),
    @stack TEXT,
    @emp_id INT,
    @date_created DATETIME
AS
BEGIN
    -- Insert statement
    INSERT INTO log (message, stack, emp_id, date_created)
    VALUES (@message, @stack, @emp_id, @date_created);
END;