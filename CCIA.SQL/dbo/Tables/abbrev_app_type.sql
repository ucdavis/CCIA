CREATE TABLE [dbo].[abbrev_app_type] (
    [app_type_id]              INT           NOT NULL,
    [app_type_trans]           VARCHAR (100) NULL,
    [abbreviation]             VARCHAR (2)   NOT NULL,
    [certificate_title]        VARCHAR (50)  NULL,
    [number_title]             VARCHAR (50)  NULL,
    [sir_title]                VARCHAR (50)  NULL,
    [process_title]            VARCHAR (500) NULL,
    [species_or_crop]          VARCHAR (50)  NULL,
    [produced_title]           VARCHAR (50)  NULL,
    [variety_title]            VARCHAR (50)  NULL,
    [show_type]                BIT           NULL,
    [grower_same_as_applicant] BIT           CONSTRAINT [DF_abbrev_app_type_grower_same_as_applicant] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_abbrev_app_type] PRIMARY KEY CLUSTERED ([app_type_id] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SD;PO;IP;OR;DI;WC', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'abbrev_app_type', @level2type = N'COLUMN', @level2name = N'app_type_trans';

