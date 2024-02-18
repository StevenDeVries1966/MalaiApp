-- Create table in SQL Server
CREATE TABLE employee
(
    emp_id INT PRIMARY KEY IDENTITY(1,1),
    emp_code VARCHAR(5),
    first_name VARCHAR(100),
    last_name VARCHAR(100),
    login VARCHAR(100),
    email VARCHAR(100),
    phone VARCHAR(100),
    rate DOUBLE PRECISION
);

-- Insert values into employee table in SQL Server
INSERT INTO employee (emp_code, first_name, last_name, email, phone, rate)
VALUES 
    ('ES001', 'Esther', 'Smoorenburg', 'esther@malai-efficiency.com', '+1(721)522-5107', 50),
    ('AS001', 'Aisha', 'Smoorenburg', 'aisha@malai-efficiency.com', '+1(721)586-5081', 35);


-- Create table in SQL Server
CREATE TABLE client
(
    clt_code VARCHAR(5) PRIMARY KEY,
    clt_name VARCHAR(255),
    address VARCHAR(255),
    postalcode VARCHAR(255),
    city VARCHAR(100),
    country VARCHAR(255),
    email VARCHAR(100),
    phone VARCHAR(100),
    rate_ES001 FLOAT,
    rate_AS001 FLOAT,
    report_type VARCHAR(5)
);

-- Insert values into client table in SQL Server
INSERT INTO client (clt_code, clt_name, rate_ES001, rate_AS001, report_type)
VALUES
    ('CHP', 'Cayhill Pharmacy', 50, 35, 'A'),
    ('CPM', 'Conpromax', 50, 35, 'A'),
    ('IMC', 'IMC Resort Services', 35, 35, 'A'),
    ('ME', 'Malai Efficiency', 50, 35, 'A'),
    ('PHC', 'Pharmasacrum', 50, 35, 'A'),
    ('PHS', 'Pharmastore', 50, 35, 'A'),
    ('RBT', 'Robotech', 50, 35, 'A'),
    ('SBP', 'Simpsonbay Pharmacy', 50, 35, 'A');



-- Create table in SQL Server
CREATE TABLE job
(
    job_id INT PRIMARY KEY IDENTITY(1,1),
    job_name VARCHAR(255),
    clt_code VARCHAR(5),
    FOREIGN KEY (clt_code) REFERENCES client(clt_code)
);

-- Insert values into job table in SQL Server
INSERT INTO job (job_name, clt_code)
VALUES
    ('Accounts Payable', 'CHP'),
    ('Accounts Receivable', 'CHP'),
    ('Cash Sales', 'CHP'),
    ('Bank Statements', 'CHP'),
    ('Month Report', 'CHP'),
    ('Meeting', 'CHP'),
    ('Project Services', 'CHP'),
    ('Payroll', 'CHP'),
    ('Taxes', 'CHP'),
    ('Other', 'CHP'),
    ('Payroll', 'CPM'),
    ('Other', 'CPM'),
    ('Billing', 'ME'),
    ('Flex Time', 'ME'),
    ('General Administration', 'ME'),
    ('Holiday', 'ME'),
    ('Lunch', 'ME'),
    ('Meeting', 'ME'),
    ('Other', 'ME'),
    ('Payroll', 'ME'),
    ('Project Management', 'ME'),
    ('Sick', 'ME'),
    ('Taxes', 'ME'),
    ('Travel', 'ME'),
    ('Vacation', 'ME'),
    ('Accounts Payable', 'PHC'),
    ('Accounts Receivable', 'PHC'),
    ('Cash Sales', 'PHC'),
    ('Bank Statements', 'PHC'),
    ('Month Report', 'PHC'),
    ('Project Services', 'PHC'),
    ('Payroll', 'PHC'),
    ('Taxes', 'PHC'),
    ('Other', 'PHC'),
    ('Accounts Payable', 'PHS'),
    ('Accounts Receivable', 'PHS'),
    ('Cash Sales', 'PHS'),
    ('Bank Statements', 'PHS'),
    ('Month Report', 'PHS'),
    ('Project Services', 'PHS'),
    ('Payroll', 'PHS'),
    ('Other', 'PHS'),
    ('General Administration', 'RBT'),
    ('Payroll', 'RBT'),
    ('Training', 'RBT'),
    ('Other', 'RBT'),
    ('Accounts Payable', 'SBP'),
    ('Accounts Receivable', 'SBP'),
    ('Cash Sales', 'SBP'),
    ('Bank Statements', 'SBP'),
    ('Month Report', 'SBP'),
    ('Project Services', 'SBP'),
    ('Other', 'SBP'),
    ('AP Approvals', 'IMC'),
    ('AP Rejections', 'IMC'),
    ('Manual Check Upload', 'IMC'),
    ('Payroll', 'IMC'),
    ('Training', 'IMC'),
    ('Other', 'IMC');


-- Create table in SQL Server
CREATE TABLE worked_hours
(
    entry_id INT PRIMARY KEY IDENTITY(1,1),
    emp_id INT,
    clt_code VARCHAR(5),
    job_name VARCHAR(50),
    job_id INT,
    week INT,
    month INT,
    year INT,
    work_date DATE,
    notes VARCHAR(500),
    start_time DATETIME,
    end_time DATETIME,
    minutes_worked INT,
    hours_worked FLOAT,
    CONSTRAINT fk_employee
        FOREIGN KEY (emp_id)
        REFERENCES employee(emp_id),
    CONSTRAINT fk_job
        FOREIGN KEY (job_id)
        REFERENCES job(job_id),
    CONSTRAINT fk_client
        FOREIGN KEY (clt_code)
        REFERENCES client(clt_code)
);

-- Create stored procedure in SQL Server
CREATE PROCEDURE InsertWorkedHours
    @emp_id INT,
    @clt_code VARCHAR(5),
    @job_name VARCHAR(50),
    @job_id INT,
    @week INT,
    @month INT,
    @year INT,
    @work_date DATE,
    @notes VARCHAR(500),
    @start_time DATETIME,
    @end_time DATETIME,
    @minutes_worked INT,
    @hours_worked FLOAT
AS
BEGIN
    INSERT INTO worked_hours (emp_id, clt_code, job_name, job_id, week, month, year, work_date, notes, start_time, end_time, minutes_worked, hours_worked)
    VALUES (@emp_id, @clt_code, @job_name, @job_id, @week, @month, @year, @work_date, @notes, @start_time, @end_time, @minutes_worked, @hours_worked);
END;

-- Create table in SQL Server
CREATE TABLE log
(
    log_id INT PRIMARY KEY IDENTITY(1,1),
    message VARCHAR(100),
    stack TEXT,
    emp_id INT,
    date_created DATETIME,
    CONSTRAINT fk_log_employee
        FOREIGN KEY (emp_id)
        REFERENCES employee(emp_id)
);

-- Create stored procedure in SQL Server
CREATE PROCEDURE InsertLog
    @message VARCHAR(100),
    @stack TEXT,
    @emp_id INT,
    @date_created DATETIME
AS
BEGIN
    INSERT INTO log (message, stack, emp_id, date_created)
    VALUES (@message, @stack, @emp_id, @date_created);
END;


