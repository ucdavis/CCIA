CREATE TABLE [dbo].[field_maps] (
    [mappt_id]  INT              IDENTITY (1, 1) NOT NULL,
    [app_id]    INT              NOT NULL,
    [latitude]  NUMERIC (25, 15) NOT NULL,
    [longitude] NUMERIC (25, 15) NOT NULL,
    CONSTRAINT [PK_field_maps] PRIMARY KEY CLUSTERED ([mappt_id] ASC)
);

