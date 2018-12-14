CREATE TABLE [dbo].[seedlab_purity_lists] (
    [purity_list_id] INT          IDENTITY (1, 1) NOT NULL,
    [lab_id]         INT          NOT NULL,
    [type]           VARCHAR (50) NOT NULL,
    [list_id]        INT          NOT NULL,
    [count]          SMALLINT     NULL,
    [grams]          FLOAT (53)   NULL,
    CONSTRAINT [PK_seedlab_purity_lists_1] PRIMARY KEY NONCLUSTERED ([purity_list_id] ASC),
    CONSTRAINT [IX_seedlab_purity_lists] UNIQUE NONCLUSTERED ([lab_id] ASC, [type] ASC, [list_id] ASC)
);

