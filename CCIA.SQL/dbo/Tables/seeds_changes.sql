CREATE TABLE [dbo].[seeds_changes] (
    [seeds_id]      INT          NOT NULL,
    [column_change] VARCHAR (50) NOT NULL,
    [old_value]     VARCHAR (50) NULL,
    [new_value]     VARCHAR (50) NULL,
    [user_change]   VARCHAR (50) NULL,
    [date_change]   DATETIME     NULL
);

