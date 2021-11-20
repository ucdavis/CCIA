CREATE TABLE [dbo].[seedlab_noxious_weed] (
    [lab_id]                    INT            NOT NULL,
    [weight_working_sample]     FLOAT (53)     NULL,
    [date_completed]            DATE           NULL,
    [completed_by]              VARCHAR (50)   NULL,
    [comments]                  VARCHAR (5000) NULL,
    [insufficient_size_noxious] BIT            CONSTRAINT [DF_seedlab_noxious_weed_insufficient_size_noxious] DEFAULT ((0)) NOT NULL,
    [weight_dodder]             FLOAT (53)     NULL,
    [date_dodder]               DATE           NULL,
    [dodder_completed_by]       VARCHAR (50)   NULL,
    [dodder_result]             VARCHAR (50)   NULL,
    [dodder_comments]           VARCHAR (5000) NULL,
    [dodder_count]              INT            NULL,
    [insufficient_size_dodder]  BIT            CONSTRAINT [DF_seedlab_noxious_weed_insufficient_size_dodder] DEFAULT ((0)) NOT NULL,
    [redrice_weight]            FLOAT (53)     NULL,
    [redrice_date]              DATE           NULL,
    [redrice_completed_by]      VARCHAR (50)   NULL,
    [redrice_result]            VARCHAR (50)   NULL,
    [redrice_comments]          VARCHAR (5000) NULL,
    [redrice_insufficient_size] BIT            CONSTRAINT [DF_seedlab_noxious_weed_redrice_insufficient_size] DEFAULT ((0)) NOT NULL,
    [redrice_count]             INT            NULL,
    CONSTRAINT [PK_seedlab_noxious_weed] PRIMARY KEY NONCLUSTERED ([lab_id] ASC)
);



