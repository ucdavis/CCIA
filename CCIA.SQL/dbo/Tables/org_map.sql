CREATE TABLE [dbo].[org_map] (
    [id]           INT          IDENTITY (1, 1) NOT NULL,
    [org_id]       INT          NOT NULL,
    [map_name]     VARCHAR (50) NOT NULL,
    [allow_access] BIT          CONSTRAINT [DF_org_map_allow_access] DEFAULT ((0)) NOT NULL
);



