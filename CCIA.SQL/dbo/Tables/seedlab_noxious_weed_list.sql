CREATE TABLE [dbo].[seedlab_noxious_weed_list] (
    [noxious_list_id] INT        IDENTITY (1, 1) NOT NULL,
    [lab_id]          INT        NOT NULL,
    [list_id]         INT        NOT NULL,
    [number_found]    INT        NOT NULL,
    [report_rate]     FLOAT (53) NULL,
    CONSTRAINT [PK_seedlab_noxious_list] PRIMARY KEY NONCLUSTERED ([noxious_list_id] ASC)
);

