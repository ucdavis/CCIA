CREATE TABLE [dbo].[contact_map] (
    [contact_id]   INT          NOT NULL,
    [map_name]     VARCHAR (50) NOT NULL,
    [allow_access] BIT          CONSTRAINT [DF_contact_map_allow_access] DEFAULT ((0)) NOT NULL
);

