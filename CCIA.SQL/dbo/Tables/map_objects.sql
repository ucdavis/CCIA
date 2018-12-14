CREATE TABLE [dbo].[map_objects] (
    [obj_id]    INT               IDENTITY (1, 1) NOT NULL,
    [app_id]    INT               NOT NULL,
    [map_title] VARCHAR (50)      NOT NULL,
    [map_desc]  VARCHAR (5000)    NOT NULL,
    [field]     [sys].[geography] NOT NULL,
    [goz_type]  VARCHAR (50)      NULL,
    [active]    BIT               CONSTRAINT [DF_map_objects_active] DEFAULT ((1)) NOT NULL
);

