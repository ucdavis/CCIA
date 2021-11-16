CREATE TABLE [dbo].[farm_fields] (
    [field_id]    INT           IDENTITY (1, 1) NOT NULL,
    [field_name]  VARCHAR (100) NULL,
    [meridian]    TINYINT       NULL,
    [township]    VARCHAR (10)  NULL,
    [Range]       VARCHAR (10)  NULL,
    [Section]     VARCHAR (10)  NULL,
    [farm_county] INT           NULL,
    CONSTRAINT [PK_fields_1] PRIMARY KEY CLUSTERED ([field_id] ASC),
    CONSTRAINT [FK_farm_field_county] FOREIGN KEY ([farm_county]) REFERENCES [dbo].[county] ([county_id])
);

