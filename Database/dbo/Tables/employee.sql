CREATE TABLE [dbo].[employee] (
    [emp_id]     INT           IDENTITY (1, 1) NOT NULL,
    [emp_code]   VARCHAR (5)   NULL,
    [first_name] VARCHAR (100) NULL,
    [last_name]  VARCHAR (100) NULL,
    [login]      VARCHAR (100) NULL,
    [email]      VARCHAR (100) NULL,
    [phone]      VARCHAR (100) NULL,
    [rate]       FLOAT (53)    NULL,
    PRIMARY KEY CLUSTERED ([emp_id] ASC)
);

