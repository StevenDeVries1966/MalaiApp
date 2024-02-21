-- Create stored procedure in SQL Server
CREATE PROCEDURE AddClient
    @clt_code VARCHAR(5),
    @clt_name VARCHAR(255),
    @address VARCHAR(255),
    @postalcode VARCHAR(255),
    @city VARCHAR(100),
    @country VARCHAR(255),
    @email VARCHAR(100),
    @phone VARCHAR(100)
AS
BEGIN
    -- Insert statement
    INSERT INTO client (clt_code, clt_name, address, postalcode, city, country, email, phone)
    VALUES (@clt_code, @clt_name, @address, @postalcode, @city, @country, @email, @phone);
END;