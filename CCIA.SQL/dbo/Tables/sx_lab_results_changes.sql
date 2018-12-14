CREATE TABLE [dbo].[sx_lab_results_changes] (
    [seeds_id]      INT           NOT NULL,
    [column_change] VARCHAR (50)  NOT NULL,
    [old_value]     VARCHAR (500) NULL,
    [new_value]     VARCHAR (500) NULL,
    [user_change]   VARCHAR (50)  NULL,
    [date_change]   DATETIME      NULL
);

