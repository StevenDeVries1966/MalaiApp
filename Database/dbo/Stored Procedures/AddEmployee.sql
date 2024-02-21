-- Create stored procedure in SQL Server
CREATE PROCEDURE AddEmployee
    @emp_code VARCHAR(5),
    @first_name VARCHAR(100),
    @last_name VARCHAR(100),
    @email VARCHAR(100),
    @phone VARCHAR(100)
AS
BEGIN
    -- Insert statement
    INSERT INTO employee (emp_code, first_name, last_name, email, phone)
    VALUES (@emp_code, @first_name, @last_name, @email, @phone);
END;