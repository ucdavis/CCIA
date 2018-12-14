CREATE TABLE [dbo].[standards] (
    [std_id]       INT            IDENTITY (1, 1) NOT NULL,
    [std_name]     VARCHAR (50)   NOT NULL,
    [std_category] VARCHAR (50)   NOT NULL,
    [std_desc]     VARCHAR (220)  NOT NULL,
    [value]        DECIMAL (8, 4) NULL,
    [min_value]    DECIMAL (8, 5) NULL,
    [max_value]    DECIMAL (8, 5) NULL,
    [value_type]   CHAR (1)       NOT NULL,
    [text_value]   VARCHAR (50)   NULL,
    [neg_msg]      VARCHAR (100)  NULL,
    [pos_msg]      VARCHAR (100)  NULL,
    [program]      VARCHAR (10)   CONSTRAINT [DF_standards_program] DEFAULT ('SD') NOT NULL
);

