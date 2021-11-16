CREATE TABLE [dbo].[seeds] (
    [seeds_id]                   INT             IDENTITY (1, 1) NOT NULL,
    [cert_program]               VARCHAR (10)    CONSTRAINT [DF_seeds_cert_program] DEFAULT ('SD') NOT NULL,
    [app_id]                     INT             NULL,
    [sx_form_no]                 INT             NULL,
    [sx_form_date]               DATETIME        NULL,
    [sx_form_cert_no]            VARCHAR (50)    NULL,
    [sx_form_rad]                INT             NULL,
    [cert_year]                  INT             NULL,
    [applicant_id]               INT             NULL,
    [conditioner_id]             INT             NULL,
    [sx_form_variety_id]         INT             NULL,
    [official_variety_id]        INT             NULL,
    [lot_num]                    VARCHAR (50)    NULL,
    [lbs_lot]                    NUMERIC (16, 2) CONSTRAINT [DF_seeds_lbs_lot] DEFAULT ((0)) NOT NULL,
    [class]                      INT             NULL,
    [class_produced_accession]   INT             NULL,
    [status]                     VARCHAR (50)    NULL,
    [county_drawn]               INT             NULL,
    [origin_state]               INT             NULL,
    [origin_country]             INT             NULL,
    [lot_country_origin]         CHAR (3)        NULL,
    [sx_bulk]                    BIT             CONSTRAINT [DF_seeds_bulk] DEFAULT ((0)) NOT NULL,
    [original_run]               BIT             CONSTRAINT [DF_seeds_original_run] DEFAULT ((0)) NOT NULL,
    [remill]                     BIT             CONSTRAINT [DF_seeds_remill] DEFAULT ((0)) NOT NULL,
    [treated]                    BIT             CONSTRAINT [DF_seeds_treated] DEFAULT ((0)) NOT NULL,
    [oecd_test_req]              BIT             CONSTRAINT [DF_seeds_oecd_test_reg] DEFAULT ((0)) NOT NULL,
    [resample]                   BIT             CONSTRAINT [DF_seeds_resample] DEFAULT ((0)) NOT NULL,
    [ccia_auth]                  VARCHAR (50)    NULL,
    [remarks]                    VARCHAR (500)   NULL,
    [sx_drawn_by]                VARCHAR (100)   NULL,
    [sampler_id]                 VARCHAR (50)    NULL,
    [cert_id]                    INT             NULL,
    [sample_id]                  INT             NULL,
    [oecd_lot]                   BIT             CONSTRAINT [DF_seeds_oecd_lot] DEFAULT ((0)) NOT NULL,
    [rush]                       BIT             CONSTRAINT [DF_seeds_rush] DEFAULT ((0)) NOT NULL,
    [in_dirt]                    BIT             CONSTRAINT [DF_seeds_in_dirt] DEFAULT ((0)) NOT NULL,
    [blend_num]                  INT             NULL,
    [date_sample_recd]           DATETIME        NULL,
    [crop_fee]                   MONEY           NULL,
    [cert_fee]                   MONEY           NULL,
    [research_fee]               MONEY           NULL,
    [min_fee]                    MONEY           NULL,
    [lot_prec]                   VARCHAR (50)    NULL,
    [lot_succ]                   VARCHAR (50)    NULL,
    [bill_tbl]                   BIT             CONSTRAINT [DF_seeds_bill_tbl] DEFAULT ((0)) NOT NULL,
    [lot_cert_cert_ok]           BIT             CONSTRAINT [DF_seeds_lot_cert_cert_ok] DEFAULT ((0)) NOT NULL,
    [user_entered]               INT             NULL,
    [submitted]                  BIT             CONSTRAINT [DF_seeds_submitted] DEFAULT ((0)) NOT NULL,
    [confirmed]                  BIT             CONSTRAINT [DF_seeds_confirmed] DEFAULT ((0)) NULL,
    [date_confirmed]             DATE            NULL,
    [year_confirmed]             AS              (case when [date_confirmed] IS NULL AND [date_sample_recd] IS NULL then (1900) when [date_confirmed] IS NULL AND datepart(month,[date_sample_recd])=(12) OR datepart(month,[date_sample_recd])=(11) OR datepart(month,[date_sample_recd])=(10) then CONVERT([int],datepart(year,[date_sample_recd])+(1),(0)) when [date_confirmed] IS NULL then CONVERT([int],datepart(year,[date_sample_recd]),(0)) when datepart(month,[date_confirmed])=(12) OR datepart(month,[date_confirmed])=(11) OR datepart(month,[date_confirmed])=(10) then CONVERT([int],datepart(year,[DATE_confirmed])+(1),(0)) else CONVERT([int],datepart(year,[date_confirmed]),(0)) end) PERSISTED,
    [docs]                       BIT             CONSTRAINT [DF_seeds_docs] DEFAULT ((0)) NOT NULL,
    [emp_modified]               VARCHAR (50)    NULL,
    [audit_preflag]              BIT             CONSTRAINT [DF_seeds_audit_preflag] DEFAULT ((0)) NOT NULL,
    [audit_sample]               BIT             CONSTRAINT [DF_seeds_audit_sample] DEFAULT ((0)) NOT NULL,
    [audit_notified]             BIT             CONSTRAINT [DF_seeds_audit_notified] DEFAULT ((0)) NOT NULL,
    [audit_reminder_conditioner] BIT             CONSTRAINT [DF_seeds_audit_reminder_conditioner] DEFAULT ((0)) NOT NULL,
    [audit_reminder_lab]         BIT             CONSTRAINT [DF_seeds_audit_reminder_lab] DEFAULT ((0)) NOT NULL,
    [audit_date_notified]        DATETIME        NULL,
    [not_finally_certified]      BIT             CONSTRAINT [DF_seeds_not_finally_certified] DEFAULT ((0)) NOT NULL,
    [charge_full_fees]           BIT             CONSTRAINT [DF_seeds_charge_full_fees] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_seeds] PRIMARY KEY CLUSTERED ([seeds_id] ASC),
    CONSTRAINT [FK_seeds_abbrev_class_produced] FOREIGN KEY ([class]) REFERENCES [dbo].[abbrev_class_produced] ([class_produced_id])
);




GO
CREATE TRIGGER [dbo].[updtSeeds] ON dbo.seeds
AFTER UPDATE NOT FOR REPLICATION AS
BEGIN
	SET NOCOUNT ON

	IF UPDATE(sx_form_no)
		BEGIN
			INSERT INTO seeds_changes (seeds_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.seeds_id, 'Sample Form Number', deleted.sx_form_no, inserted.sx_form_no, inserted.emp_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.sx_form_no,-1) <> ISNULL(deleted.sx_form_no,-1)
			WHERE inserted.emp_modified IS NOT NULL
		END
	IF UPDATE(sx_form_date)
		BEGIN
			INSERT INTO seeds_changes (seeds_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.seeds_id, 'Sample Form Date', deleted.sx_form_date, inserted.sx_form_date, inserted.emp_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.sx_form_date,'1/1/1990') <> ISNULL(deleted.sx_form_date,'1/1/1990')
			WHERE inserted.emp_modified IS NOT NULL
		END
	IF UPDATE(sx_form_cert_no)
		BEGIN
			INSERT INTO seeds_changes (seeds_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.seeds_id, 'Sample Form Cert Number', deleted.sx_form_cert_no, inserted.sx_form_cert_no, inserted.emp_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.sx_form_cert_no,'') <> ISNULL(deleted.sx_form_cert_no,'')
			WHERE inserted.emp_modified IS NOT NULL
		END
	IF UPDATE(sx_form_rad)
		BEGIN
			INSERT INTO seeds_changes (seeds_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.seeds_id, 'Sample Form RAD', deleted.sx_form_rad, inserted.sx_form_rad, inserted.emp_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.sx_form_rad,-1) <> ISNULL(deleted.sx_form_rad,-1)
			WHERE inserted.emp_modified IS NOT NULL
		END
	IF UPDATE(cert_year)
		BEGIN
			INSERT INTO seeds_changes (seeds_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.seeds_id, 'Cert Year', deleted.cert_year, inserted.cert_year, inserted.emp_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.cert_year,-1) <> ISNULL(deleted.cert_year,-1)
			WHERE inserted.emp_modified IS NOT NULL
		END
	IF UPDATE(applicant_id)
		BEGIN
			INSERT INTO seeds_changes (seeds_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.seeds_id, 'Applicant ID', deleted.applicant_id, inserted.applicant_id, inserted.emp_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.applicant_id,-1) <> ISNULL(deleted.applicant_id,-1)
			WHERE inserted.emp_modified IS NOT NULL
		END
	IF UPDATE(conditioner_id)
		BEGIN
			INSERT INTO seeds_changes (seeds_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.seeds_id, 'Conditioner ID', deleted.conditioner_id, inserted.conditioner_id, inserted.emp_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.conditioner_id,-1) <> ISNULL(deleted.conditioner_id,-1)
			WHERE inserted.emp_modified IS NOT NULL
		END
	IF UPDATE(sx_form_variety_id)
		BEGIN
			INSERT INTO seeds_changes (seeds_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.seeds_id, 'Sample Form Variety ID', deleted.sx_form_variety_id, inserted.sx_form_variety_id, inserted.emp_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.sx_form_variety_id,-1) <> ISNULL(deleted.sx_form_variety_id,-1)
			WHERE inserted.emp_modified IS NOT NULL
		END
	IF UPDATE(official_variety_id)
		BEGIN
			INSERT INTO seeds_changes (seeds_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.seeds_id, 'Official Variety ID', deleted.official_variety_id, inserted.official_variety_id, inserted.emp_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.official_variety_id,-1) <> ISNULL(deleted.official_variety_id,-1)
			WHERE inserted.emp_modified IS NOT NULL
		END
	IF UPDATE(lot_num)
		BEGIN
			INSERT INTO seeds_changes (seeds_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.seeds_id, 'Lot Number', deleted.lot_num, inserted.lot_num, inserted.emp_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.lot_num,'') <> ISNULL(deleted.lot_num,'')
			WHERE inserted.emp_modified IS NOT NULL
		END
	IF UPDATE(lbs_lot)
		BEGIN
			INSERT INTO seeds_changes (seeds_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.seeds_id, 'Pounds in lot', deleted.lbs_lot, inserted.lbs_lot, inserted.emp_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.lbs_lot,-1) <> ISNULL(deleted.lbs_lot,-1)
			WHERE inserted.emp_modified IS NOT NULL
		END
	IF UPDATE(class)
		BEGIN
			INSERT INTO seeds_changes (seeds_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.seeds_id, 'Class', deleted.class, inserted.class, inserted.emp_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.class,255) <> ISNULL(deleted.class,255)
			WHERE inserted.emp_modified IS NOT NULL
		END
	IF UPDATE(origin_state)
		BEGIN
			INSERT INTO seeds_changes (seeds_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.seeds_id, 'Origin State', deleted.origin_state, inserted.origin_state, inserted.emp_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.origin_state,-1) <> ISNULL(deleted.origin_state,-1)
			WHERE inserted.emp_modified IS NOT NULL
		END
	IF UPDATE(origin_country)
		BEGIN
			INSERT INTO seeds_changes (seeds_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.seeds_id, 'Origin Country', deleted.origin_country, inserted.origin_country, inserted.emp_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.origin_country,-1) <> ISNULL(deleted.origin_country,-1)
			WHERE inserted.emp_modified IS NOT NULL
		END
	IF UPDATE(sx_bulk)
		BEGIN
			INSERT INTO seeds_changes (seeds_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.seeds_id, 'Sample Bulk', deleted.sx_bulk, inserted.sx_bulk, inserted.emp_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.sx_bulk,0) <> ISNULL(deleted.sx_bulk,0)
			WHERE inserted.emp_modified IS NOT NULL
		END
	IF UPDATE(original_run)
		BEGIN
			INSERT INTO seeds_changes (seeds_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.seeds_id, 'Original Run', deleted.original_run, inserted.original_run, inserted.emp_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.original_run,0) <> ISNULL(deleted.original_run,0)
			WHERE inserted.emp_modified IS NOT NULL
		END
	IF UPDATE(remill)
		BEGIN
			INSERT INTO seeds_changes (seeds_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.seeds_id, 'Remill', deleted.remill, inserted.remill, inserted.emp_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.remill,0) <> ISNULL(deleted.remill,0)
			WHERE inserted.emp_modified IS NOT NULL
		END
	IF UPDATE(treated)
		BEGIN
			INSERT INTO seeds_changes (seeds_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.seeds_id, 'Treated', deleted.treated, inserted.treated, inserted.emp_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.treated,0) <> ISNULL(deleted.treated,0)
			WHERE inserted.emp_modified IS NOT NULL
		END
	IF UPDATE(oecd_test_req)
		BEGIN
			INSERT INTO seeds_changes (seeds_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.seeds_id, 'OECD Test Required', deleted.oecd_test_req, inserted.oecd_test_req, inserted.emp_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.oecd_test_req,0) <> ISNULL(deleted.oecd_test_req,0)
			WHERE inserted.emp_modified IS NOT NULL
		END
	IF UPDATE([resample])
		BEGIN
			INSERT INTO seeds_changes (seeds_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.seeds_id, 'Resample', deleted.resample, inserted.resample, inserted.emp_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.resample,0) <> ISNULL(deleted.resample,0)
			WHERE inserted.emp_modified IS NOT NULL
		END
	IF UPDATE(ccia_auth)
		BEGIN
			INSERT INTO seeds_changes (seeds_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.seeds_id, 'CCIA Auth', deleted.ccia_auth, inserted.ccia_auth, inserted.emp_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.ccia_auth,'') <> ISNULL(deleted.ccia_auth,'')
			WHERE inserted.emp_modified IS NOT NULL
		END
	IF UPDATE(sx_drawn_by)
		BEGIN
			INSERT INTO seeds_changes (seeds_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.seeds_id, 'Sample drawn by', deleted.sx_drawn_by, inserted.sx_drawn_by, inserted.emp_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.sx_drawn_by,'') <> ISNULL(deleted.sx_drawn_by,'')
			WHERE inserted.emp_modified IS NOT NULL
		END
	IF UPDATE(sample_id)
		BEGIN
			INSERT INTO seeds_changes (seeds_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.seeds_id, 'Sample ID', deleted.sample_id, inserted.sample_id, inserted.emp_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.sample_id,-1) <> ISNULL(deleted.sample_id,-1)
			WHERE inserted.emp_modified IS NOT NULL
		END
	IF UPDATE(oecd_lot)
		BEGIN
			INSERT INTO seeds_changes (seeds_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.seeds_id, 'OECD Lot', deleted.oecd_lot, inserted.oecd_lot, inserted.emp_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.oecd_lot,0) <> ISNULL(deleted.oecd_lot,0)
			WHERE inserted.emp_modified IS NOT NULL
		END
	IF UPDATE(rush)
		BEGIN
			INSERT INTO seeds_changes (seeds_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.seeds_id, 'Rush', deleted.rush, inserted.rush, inserted.emp_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.rush,0) <> ISNULL(deleted.rush,0)
			WHERE inserted.emp_modified IS NOT NULL
		END	
END
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'pnts to avvrev_class_produced', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'seeds', @level2type = N'COLUMN', @level2name = N'class';

