CREATE TABLE [dbo].[seedlab] (
    [lab_id]                   INT            IDENTITY (200, 1) NOT NULL,
    [date_received]            DATE           NOT NULL,
    [weight]                   FLOAT (53)     NULL,
    [insufficient_size_sample] BIT            CONSTRAINT [DF_seedlab_insufficient_size_sample] DEFAULT ((0)) NOT NULL,
    [seeds_id]                 INT            NULL,
    [sample_type]              VARCHAR (50)   NULL,
    [condition]                VARCHAR (50)   NULL,
    [comments]                 VARCHAR (5000) NULL,
    [recorded_by]              VARCHAR (10)   NOT NULL,
    [app_entered_by]           VARCHAR (10)   NOT NULL,
    [app_entered_date]         DATETIME       NOT NULL,
    [app_updated_by]           VARCHAR (10)   NULL,
    [app_updated_date]         DATETIME       NULL,
    [lab_year]                 SMALLINT       NOT NULL,
    [has_sample_form]          BIT            CONSTRAINT [DF_seedlab_has_sample_form] DEFAULT ((0)) NOT NULL,
    [clearly_marked_for_cert]  BIT            CONSTRAINT [DF_seedlab_clearly_marked_for_cert] DEFAULT ((0)) NOT NULL,
    [date_divided]             DATETIME       NULL,
    [divided_by]               VARCHAR (10)   NULL,
    CONSTRAINT [PK_seedlab] PRIMARY KEY NONCLUSTERED ([lab_id] ASC)
);



