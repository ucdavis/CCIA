CREATE TABLE [dbo].[address] (
    [address_id]        INT            IDENTITY (1, 1) NOT NULL,
    [reference_id]      INT            NULL,
    [oc_id]             INT            NULL,
    [address1]          NVARCHAR (100) NOT NULL,
    [address2]          NVARCHAR (100) NULL,
    [address3]          VARCHAR (100)  NULL,
    [city]              NVARCHAR (250) NULL,
    [county_id]         INT            NULL,
    [state_province_id] INT            NOT NULL,
    [postal_code]       VARCHAR (20)   NULL,
    [country_id]        INT            NOT NULL,
    [date_modified]     DATETIME       NULL,
    [user_modified]     VARCHAR (9)    NULL,
    CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED ([address_id] ASC)
);



