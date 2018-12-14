CREATE TABLE [dbo].[crops_rates] (
    [crop_rate_id] INT          IDENTITY (1, 1) NOT NULL,
    [crop_id]      INT          NOT NULL,
    [sub_type]     VARCHAR (50) NULL,
    [rate_id]      INT          NOT NULL,
    [date_set]     DATETIME     NULL,
    CONSTRAINT [PK_crops_rates] PRIMARY KEY CLUSTERED ([crop_rate_id] ASC)
);

