CREATE TABLE [dbo].[map_crop_access] (
    [access_id] INT IDENTITY (1, 1) NOT NULL,
    [org_id]    INT NOT NULL,
    [crop_id]   INT NOT NULL,
    [access]    BIT CONSTRAINT [DF_map_crop_access_access] DEFAULT ((1)) NOT NULL
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_map_crop_access]
    ON [dbo].[map_crop_access]([crop_id] ASC, [org_id] ASC);

