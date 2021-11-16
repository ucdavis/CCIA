CREATE TABLE [dbo].[crops_new] (
    [crop_id]       INT            NOT NULL,
    [anual]         VARCHAR (50)   NULL,
    [genus]         VARCHAR (200)  NULL,
    [species]       VARCHAR (100)  NULL,
    [crop_type]     VARCHAR (50)   NULL,
    [crop_kind]     VARCHAR (50)   NULL,
    [crop]          VARCHAR (50)   NULL,
    [crops_variety] VARCHAR (50)   NULL,
    [current_crop]  BIT            NULL,
    [keep]          BIT            NULL,
    [notes]         VARCHAR (8000) NULL
);

