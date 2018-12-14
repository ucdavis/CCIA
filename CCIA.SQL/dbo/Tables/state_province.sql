CREATE TABLE [dbo].[state_province] (
    [StateProvinceID]   INT            IDENTITY (101, 1) NOT NULL,
    [StateProvinceCode] NVARCHAR (2)   NULL,
    [country_id]        SMALLINT       NOT NULL,
    [StateProvinceName] NVARCHAR (250) NOT NULL,
    [date_modified]     DATETIME       NOT NULL,
    CONSTRAINT [PK_StateProvince] PRIMARY KEY CLUSTERED ([StateProvinceID] ASC)
);

