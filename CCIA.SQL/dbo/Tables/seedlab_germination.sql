CREATE TABLE [dbo].[seedlab_germination] (
    [lab_id]                 INT            NOT NULL,
    [date_planted]           DATE           NULL,
    [pre_chill]              BIT            CONSTRAINT [DF_seedlab_germination_pre_chill] DEFAULT ((0)) NOT NULL,
    [pre_chill_days]         TINYINT        NULL,
    [num_seeds_planted]      TINYINT        NULL,
    [substrate]              VARCHAR (50)   NULL,
    [replicates]             TINYINT        NULL,
    [temperature]            VARCHAR (10)   NULL,
    [comments]               VARCHAR (5000) NULL,
    [started_by]             VARCHAR (50)   NULL,
    [report_germ]            TINYINT        NULL,
    [report_hard]            TINYINT        NULL,
    [insufficient_size_germ] BIT            CONSTRAINT [DF_seedlab_germination_insufficient_size_germ] DEFAULT ((0)) NOT NULL,
    [calc_germ]              TINYINT        NULL,
    [calc_abnormal]          TINYINT        NULL,
    [calc_hard]              TINYINT        NULL,
    [calc_dormant]           TINYINT        NULL,
    [calc_dead]              TINYINT        NULL,
    CONSTRAINT [PK_seedlab_germination_1] PRIMARY KEY NONCLUSTERED ([lab_id] ASC)
);

