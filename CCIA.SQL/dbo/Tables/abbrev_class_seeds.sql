CREATE TABLE [dbo].[abbrev_class_seeds] (
    [class_certified_id]    TINYINT      IDENTITY (1, 1) NOT NULL,
    [class_abbrv]           VARCHAR (3)  NULL,
    [class_certified_trans] VARCHAR (50) NULL,
    [sort_order]            TINYINT      NULL
);

