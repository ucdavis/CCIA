CREATE TABLE [dbo].[field_results] (
    [fld_res_id]              INT            IDENTITY (1, 1) NOT NULL,
    [app_id]                  INT            NULL,
    [date_inspected]          DATETIME       NULL,
    [inspector]               VARCHAR (9)    NULL,
    [applicant_contacted]     BIT            CONSTRAINT [DF_field_results_applicant_contacted] DEFAULT ((0)) NOT NULL,
    [applicant_present]       BIT            CONSTRAINT [DF_field_results_applicant_present] DEFAULT ((0)) NOT NULL,
    [total_plants_insp]       INT            NULL,
    [PVX_tested]              BIT            CONSTRAINT [DF_field_results_PVX_tested] DEFAULT ((0)) NOT NULL,
    [stage_grow]              SMALLINT       NULL,
    [other_varieties]         TINYINT        NULL,
    [mosaic]                  TINYINT        NULL,
    [leafroll]                TINYINT        NULL,
    [blackleg]                TINYINT        NULL,
    [other_disease]           TINYINT        NULL,
    [calico]                  TINYINT        NULL,
    [insp_comments]           VARCHAR (5000) NULL,
    [field_condition]         VARCHAR (50)   NULL,
    [plants_acre]             SMALLINT       NULL,
    [stand]                   VARCHAR (50)   NULL,
    [weeds]                   VARCHAR (50)   NULL,
    [insects]                 VARCHAR (50)   NULL,
    [pathogen_comments]       VARCHAR (100)  NULL,
    [maturity_comment]        VARCHAR (5000) NULL,
    [isolation_comment]       VARCHAR (5000) NULL,
    [estimated_yield_comment] VARCHAR (5000) NULL,
    [other_varieties_comment] VARCHAR (5000) NULL,
    [other_crops_comment]     VARCHAR (5000) NULL,
    [weed_comment]            VARCHAR (5000) NULL,
    [disease_comment]         VARCHAR (5000) NULL,
    [appearance_comment]      VARCHAR (5000) NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_field_results_app_id]
    ON [dbo].[field_results]([app_id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_field_results]
    ON [dbo].[field_results]([fld_res_id] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ex: worms, aphids...', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'field_results', @level2type = N'COLUMN', @level2name = N'insects';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'traces,light,heavy,clean,medium', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'field_results', @level2type = N'COLUMN', @level2name = N'weeds';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'good; fair; poor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'field_results', @level2type = N'COLUMN', @level2name = N'stand';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Percentage', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'field_results', @level2type = N'COLUMN', @level2name = N'calico';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Percentage', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'field_results', @level2type = N'COLUMN', @level2name = N'other_disease';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Percentage', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'field_results', @level2type = N'COLUMN', @level2name = N'blackleg';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'aka Virus_PLRV; percentage', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'field_results', @level2type = N'COLUMN', @level2name = N'leafroll';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'aka Virus_PVY; percentage', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'field_results', @level2type = N'COLUMN', @level2name = N'mosaic';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Percentage of the no. of other varieties.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'field_results', @level2type = N'COLUMN', @level2name = N'other_varieties';

