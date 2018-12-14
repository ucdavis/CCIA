CREATE TABLE [dbo].[seedlab_germination_read] (
    [read_id]   INT            IDENTITY (1, 1) NOT NULL,
    [lab_id]    INT            NOT NULL,
    [date_read] DATE           NULL,
    [read_by]   VARCHAR (50)   NULL,
    [comments]  VARCHAR (5000) NULL,
    [final]     BIT            CONSTRAINT [DF_seedlab_germination_read_final] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_seedlab_germination_read] PRIMARY KEY NONCLUSTERED ([read_id] ASC)
);

