CREATE TABLE [dbo].[seedlab_seeds] (
    [lab_id]                INT             NOT NULL,
    [cond_id]               INT             NULL,
    [cond_text]             VARCHAR (500)   NULL,
    [crop_id]               INT             NOT NULL,
    [var_off_id]            INT             NULL,
    [variety_name]          VARCHAR (50)    NULL,
    [lot_number]            VARCHAR (50)    NULL,
    [treated]               BIT             CONSTRAINT [DF_seedlab_seeds_treated] DEFAULT ((0)) NULL,
    [lot_size]              NUMERIC (16, 2) NULL,
    [seed_lab]              VARCHAR (50)    NULL,
    [cert_number]           VARCHAR (50)    NULL,
    [submitting_lab_number] VARCHAR (50)    NULL,
    CONSTRAINT [PK_seedlab_seeds] PRIMARY KEY NONCLUSTERED ([lab_id] ASC)
);

