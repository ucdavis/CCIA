CREATE TABLE [dbo].[crop_standards] (
    [crop_id] INT NOT NULL,
    [std_id]  INT NOT NULL,
    CONSTRAINT [PK_crop_standards] PRIMARY KEY CLUSTERED ([crop_id] ASC, [std_id] ASC)
);

