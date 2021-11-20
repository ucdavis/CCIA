CREATE TABLE [dbo].[county] (
    [county_id]       INT          IDENTITY (1, 1) NOT NULL,
    [county_name]     VARCHAR (50) NOT NULL,
    [district]        VARCHAR (5)  NULL,
    [StateProvinceID] INT          NOT NULL,
    [date_modified]   DATETIME     NULL,
    [user_modified]   VARCHAR (9)  NULL,
    [fips]            VARCHAR (10) NULL,
    [ag_comm_org]     INT          NULL,
    CONSTRAINT [PK_County] PRIMARY KEY CLUSTERED ([county_id] ASC)
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'e.g.: I-VIII', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'county', @level2type = N'COLUMN', @level2name = N'county_id';

