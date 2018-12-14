CREATE TABLE [dbo].[seedlab_impurity] (
    [impurity_list_id] INT          IDENTITY (1, 1) NOT NULL,
    [lab_id]           INT          NOT NULL,
    [list_id]          INT          NOT NULL,
    [impurity_type]    VARCHAR (50) NOT NULL,
    [number_found]     SMALLINT     NOT NULL,
    [report_rate]      FLOAT (53)   NULL,
    [fraction]         VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_seedlab_impurity] PRIMARY KEY NONCLUSTERED ([impurity_list_id] ASC)
);

