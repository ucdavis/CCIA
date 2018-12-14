CREATE TABLE [dbo].[seedlab_dodder] (
    [lab_id]                INT             NOT NULL,
    [weight_working_sample] NUMERIC (18, 4) NULL,
    [dodder]                BIT             CONSTRAINT [DF_seedlab_dodder_dodder] DEFAULT ((0)) NOT NULL,
    [weight_dodder]         NUMERIC (7, 2)  NULL,
    [completed_by]          VARCHAR (50)    NULL,
    [date_completed]        DATE            NULL,
    [comments]              VARCHAR (5000)  NULL,
    CONSTRAINT [PK_seedlab_dodder] PRIMARY KEY NONCLUSTERED ([lab_id] ASC)
);

