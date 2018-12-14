CREATE TABLE [dbo].[oecd_changes] (
    [file_num]      INT          NOT NULL,
    [column_change] VARCHAR (50) NOT NULL,
    [old_value]     VARCHAR (50) NULL,
    [new_value]     VARCHAR (50) NULL,
    [user_change]   VARCHAR (50) NULL,
    [date_change]   DATETIME     NULL
);

