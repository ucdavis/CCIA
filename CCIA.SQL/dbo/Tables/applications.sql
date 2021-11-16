CREATE TABLE [dbo].[applications] (
    [app_id]                   INT               IDENTITY (1, 1) NOT NULL,
    [paper_app_num]            INT               NULL,
    [cert_num]                 INT               NULL,
    [cert_year]                INT               NOT NULL,
    [app_original_cert_year]   INT               NULL,
    [lot_no]                   VARCHAR (50)      NULL,
    [app_type]                 VARCHAR (2)       NULL,
    [applicant_id]             INT               NOT NULL,
    [user_app_dataentry]       INT               NULL,
    [user_app_modifed]         INT               NULL,
    [user_app_mod_dt]          DATETIME          NULL,
    [grower_id]                INT               NULL,
    [crop_id]                  INT               NULL,
    [selected_variety_id]      INT               NULL,
    [entered_variety]          VARCHAR (50)      NULL,
    [class_produced_id]        INT               NULL,
    [class_produced_accession] INT               NULL,
    [app_received]             DATETIME          NULL,
    [app_postmark]             SMALLDATETIME     NULL,
    [app_deadline]             DATETIME          CONSTRAINT [DF_applications_app_late] DEFAULT ((0)) NULL,
    [app_pkg_complete]         BIT               CONSTRAINT [DF_applications_app_pkg_complete] DEFAULT ((0)) NULL,
    [app_submitable]           BIT               CONSTRAINT [DF_applications_app_submitted] DEFAULT ((1)) NULL,
    [app_complete_dt]          DATETIME          NULL,
    [status]                   VARCHAR (50)      NULL,
    [renewal]                  BIT               CONSTRAINT [DF_Applications_renewal] DEFAULT ((0)) NOT NULL,
    [app_approved]             BIT               CONSTRAINT [DF_Applications_app_final] DEFAULT ((0)) NOT NULL,
    [app_approver]             VARCHAR (50)      NULL,
    [app_date_appr]            DATETIME          NULL,
    [trace]                    INT               NULL,
    [warning_flag]             BIT               CONSTRAINT [DF_Applications_warning_flag] DEFAULT ((0)) NOT NULL,
    [applicant_notes]          VARCHAR (500)     NULL,
    [app_denied]               BIT               CONSTRAINT [DF_Applications_app_denied] DEFAULT ((0)) NOT NULL,
    [app_rejector]             VARCHAR (50)      NULL,
    [app_date_denied]          DATETIME          NULL,
    [maps]                     BIT               CONSTRAINT [DF_Applications_app_complete] DEFAULT ((0)) NOT NULL,
    [maps_sub_dt]              DATETIME          NULL,
    [map_center_lat]           NUMERIC (25, 15)  NULL,
    [map_center_long]          NUMERIC (25, 15)  NULL,
    [map_ve]                   BIT               CONSTRAINT [DF_applications_map_ve] DEFAULT ((0)) NULL,
    [map_upload_file]          VARCHAR (100)     NULL,
    [text_field]               VARCHAR (3000)    NULL,
    [geo_text_field]           VARCHAR (5000)    NULL,
    [geo_field]                [sys].[geography] NULL,
    [geo_field_area_sq_m]      AS                ([geo_field].[STArea]()*(0.000247105)),
    [map_zoom]                 INT               NULL,
    [tags]                     BIT               CONSTRAINT [DF_applications_tags] DEFAULT ((0)) NOT NULL,
    [po_lot_num]               VARCHAR (20)      NULL,
    [field_id]                 INT               NULL,
    [field_name]               VARCHAR (100)     NULL,
    [meridian]                 TINYINT           NULL,
    [township]                 VARCHAR (10)      NULL,
    [range]                    VARCHAR (10)      NULL,
    [section]                  VARCHAR (10)      NULL,
    [farm_county]              INT               NULL,
    [date_planted]             DATETIME          NULL,
    [acres_applied]            DECIMAL (14, 2)   NULL,
    [is_square_feet]           BIT               CONSTRAINT [DF_applications_is_square_feet] DEFAULT ((0)) NULL,
    [billable]                 BIT               CONSTRAINT [DF_applications_billable] DEFAULT ((1)) NOT NULL,
    [charged]                  BIT               CONSTRAINT [DF_applications_charged] DEFAULT ((0)) NOT NULL,
    [user_emp_modified]        VARCHAR (9)       NULL,
    [user_emp_date_mod]        DATETIME          NULL,
    [app_cancelled]            BIT               CONSTRAINT [DF_applications_app_cancelled] DEFAULT ((0)) NOT NULL,
    [app_cancelled_by]         VARCHAR (9)       NULL,
    [comments]                 VARCHAR (500)     NULL,
    [app_fee]                  SMALLMONEY        NULL,
    [late_fee]                 SMALLMONEY        NULL,
    [incomplete_fee]           SMALLMONEY        NULL,
    [override_late_fee]        BIT               CONSTRAINT [DF_applications_override_late_fee] DEFAULT ((0)) NOT NULL,
    [fee_cofactor]             DECIMAL (5, 4)    CONSTRAINT [DF_applications_fee_cofactor] DEFAULT ((1)) NOT NULL,
    [notify_needed]            BIT               CONSTRAINT [DF_applications_notify_needed] DEFAULT ((0)) NOT NULL,
    [notify_date]              DATETIME          NULL,
    [date_notified]            DATETIME          NULL,
    [applicant_comments]       VARCHAR (5000)    NULL,
    [pvg_source]               VARCHAR (50)      NULL,
    [pvg_selectionId]          VARCHAR (50)      NULL,
    [field_hardiness]          VARCHAR (50)      NULL,
    [field_elevation]          INT               NULL,
    [ecoregion]                INT               CONSTRAINT [DF_applications_ecoregion] DEFAULT ((0)) NOT NULL,
    [county_permit]            VARCHAR (50)      NULL,
    CONSTRAINT [PK_Applications] PRIMARY KEY CLUSTERED ([app_id] ASC),
    CONSTRAINT [FK_Applications_Applications2] FOREIGN KEY ([trace]) REFERENCES [dbo].[applications] ([app_id]),
    CONSTRAINT [FK_applications_county_farm_county] FOREIGN KEY ([farm_county]) REFERENCES [dbo].[county] ([county_id])
);






GO



GO
CREATE TRIGGER [dbo].[updtApplications] ON [dbo].[applications]
AFTER UPDATE NOT FOR REPLICATION AS
BEGIN
	SET NOCOUNT ON
	DECLARE @app_id INT
	SELECT @app_id = app_id FROM inserted

	IF UPDATE(applicant_id)
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'applicant_id', deleted.applicant_id, inserted.applicant_id, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND inserted.applicant_id <> deleted.applicant_id
			WHERE deleted.app_submitable = 0

			EXEC update_application_from_trigger @app_id
		END
	IF UPDATE([status])
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'status', deleted.[status], inserted.[status], inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND inserted.[status] <> deleted.[status]
			WHERE deleted.app_submitable = 0
			
		END
	IF UPDATE(cert_year)
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'cert_year', deleted.cert_year, inserted.cert_year, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND inserted.cert_year <> deleted.cert_year
			WHERE deleted.app_submitable = 0

			DECLARE @updated bit
			SET @updated = 0

			if EXISTS (SELECT * FROM inserted JOIN deleted ON inserted.app_id = deleted.app_id AND inserted.cert_year <> deleted.cert_year
			WHERE deleted.app_submitable = 0) 
				SET @updated = 1

			UPDATE applications SET app_original_cert_year = cert_year WHERE app_id = @app_id and renewal = 0 AND @updated =1
		END
	IF UPDATE(lot_no)
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'lot_no', deleted.lot_no, inserted.lot_no, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND inserted.lot_no <> deleted.lot_no
			WHERE deleted.app_submitable = 0
		END
	IF UPDATE(grower_id)
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'grower_id', deleted.grower_id, inserted.grower_id, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND inserted.grower_id <> deleted.grower_id
			WHERE deleted.app_submitable = 0
		END
	IF UPDATE(crop_id)
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'crop_id', deleted.crop_id, inserted.crop_id, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND inserted.crop_id <> deleted.crop_id
			WHERE deleted.app_submitable = 0

			EXEC update_application_from_trigger @app_id
		END
	IF UPDATE(selected_variety_id)
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'selected_variety_id', deleted.selected_variety_id, inserted.selected_variety_id, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND inserted.selected_variety_id <> deleted.selected_variety_id
			WHERE deleted.app_submitable = 0

			EXEC update_application_from_trigger @app_id
		END
	IF UPDATE(class_produced_id)
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'class_produced_id', deleted.class_produced_id, inserted.class_produced_id, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND inserted.class_produced_id <> deleted.class_produced_id
			WHERE deleted.app_submitable = 0

			EXEC update_application_from_trigger @app_id
		END
	IF UPDATE(app_postmark)
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'app_postmark', deleted.app_postmark, inserted.app_postmark, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND inserted.app_postmark <> deleted.app_postmark
			WHERE deleted.app_submitable = 0
		END
	IF UPDATE(acres_applied)
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'acres_applied', deleted.acres_applied, inserted.acres_applied, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND inserted.acres_applied <> deleted.acres_applied
			WHERE deleted.app_submitable = 0
			
			EXEC update_application_from_trigger @app_id
		END
	IF UPDATE(po_lot_num)
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'po_lot_num', deleted.po_lot_num, inserted.po_lot_num, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND inserted.po_lot_num <> deleted.po_lot_num
			WHERE deleted.app_submitable = 0	
		END
	IF UPDATE(field_name)
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'field_name', deleted.field_name, inserted.field_name, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND inserted.field_name <> deleted.field_name
			WHERE deleted.app_submitable = 0	
		END
	IF UPDATE(farm_county)
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'farm_county', deleted.farm_county, inserted.farm_county, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND inserted.farm_county <> deleted.farm_county
			WHERE deleted.app_submitable = 0	
		END
	IF UPDATE(township)
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'township', deleted.township, inserted.township, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND inserted.township <> deleted.township
			WHERE deleted.app_submitable = 0	
		END
	IF UPDATE([range])
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'range', deleted.[range], inserted.[range], inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND inserted.[range] <> deleted.[range]
			WHERE deleted.app_submitable = 0	
		END	
	IF UPDATE(section)
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'section', deleted.section, inserted.section, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND inserted.section <> deleted.section
			WHERE deleted.app_submitable = 0	
		END
	IF UPDATE(date_planted)
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'date_planted', deleted.date_planted, inserted.date_planted, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND inserted.date_planted <> deleted.date_planted
			WHERE deleted.app_submitable = 0
			
			EXEC recalculate_application_deadline @app_id
		END
	IF UPDATE(entered_variety)
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'entered_variety', deleted.entered_variety, inserted.entered_variety, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND inserted.entered_variety <> deleted.entered_variety
			WHERE deleted.app_submitable = 0	
		END
	IF UPDATE(pvg_selectionId)
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'pvg_selectionId', deleted.pvg_selectionId, inserted.pvg_selectionId, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND inserted.pvg_selectionId <> deleted.pvg_selectionId
			WHERE deleted.app_submitable = 0	
		END
	IF UPDATE(pvg_source)
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'pvg source', deleted.pvg_source, inserted.pvg_source, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND inserted.pvg_source <> deleted.pvg_source
			WHERE deleted.app_submitable = 0	
		END
	IF UPDATE(field_hardiness)
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'field hardiness', deleted.field_hardiness, inserted.field_hardiness, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND inserted.field_hardiness <> deleted.field_hardiness
			WHERE deleted.app_submitable = 0	
		END
	IF UPDATE(field_elevation)
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'field elevation', deleted.field_elevation, inserted.field_elevation, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND inserted.field_elevation <> deleted.field_elevation AND ISNULL(inserted.field_elevation,-1) <> ISNULL(deleted.field_elevation,-1)
			WHERE deleted.app_submitable = 0	
		END
	IF UPDATE(ecoregion)
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'ecoregion', deleted.ecoregion, inserted.ecoregion, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND inserted.ecoregion <> deleted.ecoregion AND ISNULL(inserted.ecoregion,-1) <> ISNULL(deleted.ecoregion,-1)
			WHERE deleted.app_submitable = 0	
		END
END
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Flags: all foundation apps.; ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'applications', @level2type = N'COLUMN', @level2name = N'warning_flag';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'if renewals receive a new app. no.; otherwise don''t use', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'applications', @level2type = N'COLUMN', @level2name = N'trace';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'org_id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'applications', @level2type = N'COLUMN', @level2name = N'grower_id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'contact id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'applications', @level2type = N'COLUMN', @level2name = N'user_app_dataentry';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'org_id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'applications', @level2type = N'COLUMN', @level2name = N'applicant_id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'WHAT DATATYPE?', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'applications', @level2type = N'COLUMN', @level2name = N'cert_num';


GO
CREATE NONCLUSTERED INDEX [IX_applications_grower_id]
    ON [dbo].[applications]([grower_id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_applications_farm_county]
    ON [dbo].[applications]([farm_county] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_applications_applicant_id]
    ON [dbo].[applications]([applicant_id] ASC);

