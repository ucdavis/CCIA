CREATE TABLE [dbo].[my_customers] (
    [id]                  INT           IDENTITY (1, 1) NOT NULL,
    [org_id]              INT           NOT NULL,
    [cust_name]           VARCHAR (100) NULL,
    [cust_address_line_1] VARCHAR (100) NULL,
    [cust_address_line_2] VARCHAR (100) NULL,
    [cust_city]           VARCHAR (100) NULL,
    [cust_county]         INT           NULL,
    [cust_state_id]       INT           NULL,
    [cust_country]        INT           NULL,
    [cust_zip]            VARCHAR (50)  NULL,
    [cust_phone]          VARCHAR (50)  NULL,
    [cust_email]          VARCHAR (50)  NULL
);

