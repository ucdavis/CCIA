CREATE TABLE [dbo].[district_county] (
    [dist_county_id] INT           IDENTITY (1, 1) NOT NULL,
    [dist_id]        INT           NULL,
    [county_id]      INT           NULL,
    [comments]       VARCHAR (250) NULL,
    [date_entered]   DATETIME      NULL,
    [user_modified]  VARCHAR (9)   NULL,
    CONSTRAINT [PK_district_county] PRIMARY KEY CLUSTERED ([dist_county_id] ASC)
);



