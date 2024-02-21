CREATE TABLE [dbo].[client] (
    [clt_code]        VARCHAR (5)   NOT NULL,
    [clt_name]        VARCHAR (255) NULL,
    [address]         VARCHAR (255) NULL,
    [postalcode]      VARCHAR (255) NULL,
    [city]            VARCHAR (100) NULL,
    [country]         VARCHAR (255) NULL,
    [email]           VARCHAR (100) NULL,
    [phone]           VARCHAR (100) NULL,
    [rate_ES001]      FLOAT (53)    NULL,
    [rate_AS001]      FLOAT (53)    NULL,
    [report_type]     VARCHAR (5)   NULL,
    [retainer_ES0001] FLOAT (53)    NULL,
    [retainer_AS0001] FLOAT (53)    NULL,
    PRIMARY KEY CLUSTERED ([clt_code] ASC)
);

