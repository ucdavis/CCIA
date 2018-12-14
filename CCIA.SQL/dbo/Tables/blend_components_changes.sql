CREATE TABLE [dbo].[blend_components_changes] (
    [blend_id]      INT            NOT NULL,
    [comp_id]       INT            NOT NULL,
    [column_change] VARCHAR (50)   NULL,
    [old_value]     VARCHAR (5000) NULL,
    [new_value]     VARCHAR (5000) NULL,
    [user_change]   VARCHAR (50)   NULL,
    [date_change]   DATETIME       NULL
);

