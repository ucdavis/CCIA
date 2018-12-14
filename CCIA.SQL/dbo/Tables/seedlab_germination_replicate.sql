CREATE TABLE [dbo].[seedlab_germination_replicate] (
    [rep_id]        INT     IDENTITY (1, 1) NOT NULL,
    [read_id]       INT     NOT NULL,
    [rep_num]       TINYINT NOT NULL,
    [germ_count]    TINYINT NULL,
    [hard_count]    TINYINT NULL,
    [dormant_fresh] TINYINT NULL,
    [dead_seed]     TINYINT NULL,
    [abnormal_seed] TINYINT NULL,
    [remainder]     TINYINT NULL,
    CONSTRAINT [PK_seedlab_germination_replicate] PRIMARY KEY NONCLUSTERED ([rep_id] ASC)
);

