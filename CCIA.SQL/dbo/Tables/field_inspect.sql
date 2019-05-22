CREATE TABLE [dbo].[field_inspect] (
    [fldinsp_id]           INT             IDENTITY (1, 1) NOT NULL,
    [app_id]               INT             NULL,
    [date_fld_rpt]         DATETIME        NULL,
    [acres_fld]            DECIMAL (14, 2) CONSTRAINT [DF_field_inspect_acres_fld] DEFAULT ((0)) NULL,
    [acres_insp_only]      DECIMAL (14, 2) CONSTRAINT [DF_field_inspect_acres_insp_only] DEFAULT ((0)) NULL,
    [acres_approved]       DECIMAL (14, 2) CONSTRAINT [DF_field_inspect_acres_approved] DEFAULT ((0)) NULL,
    [acres_cancelled]      DECIMAL (14, 2) CONSTRAINT [DF_field_inspect_acres_cancelled] DEFAULT ((0)) NULL,
    [acres_growout]        DECIMAL (14, 2) CONSTRAINT [DF_field_inspect_acres_growout] DEFAULT ((0)) NULL,
    [acres_refund]         DECIMAL (14, 2) CONSTRAINT [DF_field_inspect_acres_refund] DEFAULT ((0)) NULL,
    [acres_rejected]       DECIMAL (14, 2) CONSTRAINT [DF_field_inspect_acres_rejected] DEFAULT ((0)) NULL,
    [acres_no_crop]        DECIMAL (14, 2) CONSTRAINT [DF_field_inspect_acres_no_crop] DEFAULT ((0)) NULL,
    [date_entered]         DATETIME        NULL,
    [user_entered]         VARCHAR (9)     NULL,
    [date_modified]        DATETIME        NULL,
    [user_modified]        VARCHAR (9)     NULL,
    [complete]             BIT             CONSTRAINT [DF_Field_Inspect_complete] DEFAULT ((0)) NOT NULL,
    [date_complete]        DATE            NULL,
    [complete_by]          VARCHAR (10)    NULL,
    [passed]               BIT             CONSTRAINT [DF_field_inspect_passed] DEFAULT ((0)) NOT NULL,
    [pass_class]           TINYINT         CONSTRAINT [DF_field_inspect_pass_class] DEFAULT ((0)) NOT NULL,
    [report_generated]     BIT             CONSTRAINT [DF_Field_Inspect_report_generated] DEFAULT ((0)) NOT NULL,
    [report_gen_dt]        DATETIME        NULL,
    [charged]              BIT             CONSTRAINT [DF_field_inspect_charged] DEFAULT ((0)) NOT NULL,
    [old_field_name]       VARCHAR (50)    NULL,
    [old_county_id]        INT             NULL,
    [fld_inspect_comments] VARCHAR (2000)  NULL,
    [path_date]            DATETIME        NULL,
    [path_num_samples]     INT             NULL,
    [path_num_plants]      INT             NULL,
    [path_lab]             VARCHAR (50)    NULL,
    [path_sent_via]        VARCHAR (50)    NULL,
    [path_cms]             TINYINT         CONSTRAINT [DF_field_inspect_path_cms] DEFAULT ((255)) NOT NULL,
    [path_erw]             TINYINT         CONSTRAINT [DF_field_inspect_path_erw] DEFAULT ((255)) NOT NULL,
    [path_pstvd]           TINYINT         CONSTRAINT [DF_field_inspect_path_pstvd] DEFAULT ((255)) NOT NULL,
    [path_pva]             TINYINT         CONSTRAINT [DF_field_inspect_path_pva] DEFAULT ((255)) NOT NULL,
    [path_pvm]             TINYINT         CONSTRAINT [DF_field_inspect_path_pvm] DEFAULT ((255)) NOT NULL,
    [path_plrv]            TINYINT         CONSTRAINT [DF_field_inspect_path_plrv] DEFAULT ((255)) NOT NULL,
    [path_pvs]             TINYINT         CONSTRAINT [DF_field_inspect_path_pvs] DEFAULT ((255)) NOT NULL,
    [path_pvx]             TINYINT         CONSTRAINT [DF_field_inspect_path_pvx] DEFAULT ((255)) NOT NULL,
    [path_pvy]             TINYINT         CONSTRAINT [DF_field_inspect_path_pvy] DEFAULT ((255)) NOT NULL,
    [path_comments]        VARCHAR (1000)  NULL,
    [thc_percent]          NUMERIC (7, 4)  NULL,
    CONSTRAINT [PK_Fields] PRIMARY KEY CLUSTERED ([fldinsp_id] ASC)
);




GO


