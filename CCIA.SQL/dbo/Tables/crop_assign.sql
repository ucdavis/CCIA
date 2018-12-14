CREATE TABLE [dbo].[crop_assign] (
    [crop_assign_id] INT          IDENTITY (1, 1) NOT NULL,
    [crop_id]        INT          NOT NULL,
    [assign_id]      VARCHAR (50) NOT NULL,
    [assign_level]   TINYINT      NOT NULL
);

