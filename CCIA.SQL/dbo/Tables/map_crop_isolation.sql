CREATE TABLE [dbo].[map_crop_isolation] (
    [crop_id]    INT             NOT NULL,
    [foundation] NUMERIC (10, 3) CONSTRAINT [DF_map_crop_isolation_foundation] DEFAULT ((0)) NOT NULL,
    [registered] NUMERIC (10, 3) CONSTRAINT [DF_map_crop_isolation_registered] DEFAULT ((0)) NOT NULL,
    [certified]  NUMERIC (10, 3) CONSTRAINT [DF_map_crop_isolation_certified] DEFAULT ((0)) NOT NULL
);

