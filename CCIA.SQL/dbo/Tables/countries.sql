CREATE TABLE [dbo].[countries] (
    [country_id]    SMALLINT      IDENTITY (1, 1) NOT NULL,
    [country_code]  VARCHAR (3)   NOT NULL,
    [country_name]  VARCHAR (100) NOT NULL,
    [oecd_member]   BIT           CONSTRAINT [DF_countries_oecd] DEFAULT ((0)) NOT NULL,
    [date_modified] DATETIME      NULL,
    [user_modified] VARCHAR (9)   NULL,
    CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED ([country_id] ASC)
);

