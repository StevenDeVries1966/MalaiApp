CREATE TABLE [dbo].[job] (
    [job_id]   INT           IDENTITY (1, 1) NOT NULL,
    [job_name] VARCHAR (255) NULL,
    [clt_code] VARCHAR (5)   NULL,
    PRIMARY KEY CLUSTERED ([job_id] ASC),
    FOREIGN KEY ([clt_code]) REFERENCES [dbo].[client] ([clt_code])
);

