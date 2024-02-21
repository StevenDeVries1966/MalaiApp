CREATE TABLE [dbo].[worked_hours] (
    [entry_id]       INT           IDENTITY (1, 1) NOT NULL,
    [emp_id]         INT           NULL,
    [clt_code]       VARCHAR (5)   NULL,
    [job_name]       VARCHAR (50)  NULL,
    [job_id]         INT           NULL,
    [week]           INT           NULL,
    [month]          INT           NULL,
    [year]           INT           NULL,
    [work_date]      DATE          NULL,
    [notes]          VARCHAR (500) NULL,
    [start_time]     DATETIME      NULL,
    [end_time]       DATETIME      NULL,
    [minutes_worked] INT           NULL,
    [hours_worked]   FLOAT (53)    NULL,
    PRIMARY KEY CLUSTERED ([entry_id] ASC),
    CONSTRAINT [fk_client] FOREIGN KEY ([clt_code]) REFERENCES [dbo].[client] ([clt_code]),
    CONSTRAINT [fk_employee] FOREIGN KEY ([emp_id]) REFERENCES [dbo].[employee] ([emp_id]),
    CONSTRAINT [fk_job] FOREIGN KEY ([job_id]) REFERENCES [dbo].[job] ([job_id])
);

