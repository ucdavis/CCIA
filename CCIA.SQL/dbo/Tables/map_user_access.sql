CREATE TABLE [dbo].[map_user_access] (
    [access_id]   INT IDENTITY (1, 1) NOT NULL,
    [contact_id]  INT NOT NULL,
    [crop_id]     INT NOT NULL,
    [view_access] BIT NOT NULL,
    [pin_access]  BIT NOT NULL
);

