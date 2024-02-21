CREATE TABLE [dbo].[log] (
    [log_id]       INT           IDENTITY (1, 1) NOT NULL,
    [message]      VARCHAR (100) NULL,
    [stack]        TEXT          NULL,
    [emp_id]       INT           NULL,
    [date_created] DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([log_id] ASC),
    CONSTRAINT [fk_log_employee] FOREIGN KEY ([emp_id]) REFERENCES [dbo].[employee] ([emp_id])
);

