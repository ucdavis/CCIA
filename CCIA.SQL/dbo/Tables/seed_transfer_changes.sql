CREATE TABLE [dbo].[seed_transfer_changes] (
    [stid]          INT          NOT NULL,
    [column_change] VARCHAR (50) NULL,
    [old_value]     VARCHAR (50) NULL,
    [new_value]     VARCHAR (50) NULL,
    [user_change]   VARCHAR (50) NULL,
    [user_admin]    BIT          CONSTRAINT [DF_seed_transfer_changes_used_admin] DEFAULT ((0)) NOT NULL,
    [date_change]   DATETIME     NULL
);



