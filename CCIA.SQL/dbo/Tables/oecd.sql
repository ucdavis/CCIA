CREATE TABLE [dbo].[oecd] (
    [file_num]         INT            IDENTITY (1, 1) NOT NULL,
    [seeds_id]         INT            NULL,
    [variety_id]       INT            NULL,
    [tag_id_link]      INT            NULL,
    [lbs_oecd]         INT            NULL,
    [cert_num]         VARCHAR (50)   NULL,
    [oecd_num]         VARCHAR (100)  NULL,
    [class]            TINYINT        NULL,
    [close_date]       DATETIME       NULL,
    [conditioner_id]   INT            NULL,
    [country]          SMALLINT       NULL,
    [issue_date]       DATETIME       NULL,
    [lot_num]          VARCHAR (50)   NULL,
    [shipper_id]       INT            NULL,
    [sample_form_num]  INT            NULL,
    [date_requested]   DATETIME       NULL,
    [total_fee]        SMALLMONEY     NULL,
    [not_cert]         BIT            NULL,
    [data_entry_date]  DATETIME       NULL,
    [data_entry_year]  AS             (case when datepart(month,[data_entry_date])=(12) OR datepart(month,[data_entry_date])=(11) OR datepart(month,[data_entry_date])=(10) then CONVERT([smallint],datepart(year,[data_entry_date])+(1),(0)) else CONVERT([smallint],datepart(year,[data_entry_date]),(0)) end) PERSISTED,
    [data_entry_user]  VARCHAR (50)   NULL,
    [update_date]      DATETIME       NULL,
    [update_user]      VARCHAR (50)   NULL,
    [domestic_origin]  BIT            NULL,
    [canceled]         BIT            NULL,
    [comments]         VARCHAR (5000) NULL,
    [admin_comments]   VARCHAR (5000) NULL,
    [date_printed]     DATETIME       NULL,
    [ref_num]          VARCHAR (100)  NULL,
    [billable]         BIT            CONSTRAINT [DF_oecd_billable] DEFAULT ((1)) NOT NULL,
    [charged]          BIT            CONSTRAINT [DF_oecd_charged] DEFAULT ((0)) NOT NULL,
    [usda_reported]    BIT            CONSTRAINT [DF_oecd_usda_reported] DEFAULT ((0)) NOT NULL,
    [usda_report_date] DATETIME       NULL,
    [tags_requested]   INT            CONSTRAINT [DF_oecd_tags_requested] DEFAULT ((0)) NOT NULL,
    [certificate_fee]  SMALLMONEY     NULL,
    [oecd_fee]         SMALLMONEY     NULL,
    [nfc_fee]          SMALLMONEY     NULL,
    [client_notified]  BIT            CONSTRAINT [DF_oecd_client_notified] DEFAULT ((0)) NOT NULL
);


GO
CREATE TRIGGER [dbo].[updtOECD] ON [dbo].[oecd]
AFTER UPDATE NOT FOR REPLICATION AS
BEGIN
	SET NOCOUNT ON

	IF UPDATE(seeds_id)
		BEGIN
			INSERT INTO oecd_changes (file_num, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.file_num, 'SID', deleted.seeds_id, inserted.seeds_id, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.file_num = deleted.file_num AND ISNULL(inserted.seeds_id,-1) <> ISNULL(deleted.seeds_id,-1)
			WHERE inserted.update_user IS NOT NULL
		END
	IF UPDATE(variety_id)
		BEGIN
			INSERT INTO oecd_changes (file_num, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.file_num, 'Variety', deleted.variety_id, inserted.variety_id, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.file_num = deleted.file_num AND ISNULL(inserted.variety_id,-1) <> ISNULL(deleted.variety_id,-1)
			WHERE inserted.update_user IS NOT NULL
		END
	IF UPDATE(lbs_oecd)
		BEGIN
			INSERT INTO oecd_changes (file_num, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.file_num, 'Lbs OECD', deleted.lbs_oecd, inserted.lbs_oecd, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.file_num = deleted.file_num AND ISNULL(inserted.lbs_oecd,-1) <> ISNULL(deleted.lbs_oecd,-1)
			WHERE inserted.update_user IS NOT NULL
		END
		IF UPDATE(cert_num)
		BEGIN
			INSERT INTO oecd_changes (file_num, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.file_num, 'Cert #', deleted.cert_num, inserted.cert_num, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.file_num = deleted.file_num AND ISNULL(inserted.cert_num,-1) <> ISNULL(deleted.cert_num,-1)
			WHERE inserted.update_user IS NOT NULL
		END
		IF UPDATE(oecd_num)
		BEGIN
			INSERT INTO oecd_changes (file_num, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.file_num, 'OECD #', deleted.oecd_num, inserted.oecd_num, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.file_num = deleted.file_num AND ISNULL(inserted.oecd_num,-1) <> ISNULL(deleted.oecd_num,-1)
			WHERE inserted.update_user IS NOT NULL
		END
		IF UPDATE(class)
		BEGIN
			INSERT INTO oecd_changes (file_num, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.file_num, 'Class', deleted.class, inserted.class, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.file_num = deleted.file_num AND ISNULL(inserted.class,-1) <> ISNULL(deleted.class,-1)
			WHERE inserted.update_user IS NOT NULL
		END
		IF UPDATE(close_date)
		BEGIN
			INSERT INTO oecd_changes (file_num, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.file_num, 'Close date', deleted.close_date, inserted.close_date, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.file_num = deleted.file_num AND ISNULL(inserted.close_date,-1) <> ISNULL(deleted.close_date,-1)
			WHERE inserted.update_user IS NOT NULL
		END
		IF UPDATE(conditioner_id)
		BEGIN
			INSERT INTO oecd_changes (file_num, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.file_num, 'Conditioner ID', deleted.conditioner_id, inserted.conditioner_id, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.file_num = deleted.file_num AND ISNULL(inserted.conditioner_id,-1) <> ISNULL(deleted.conditioner_id,-1)
			WHERE inserted.update_user IS NOT NULL
		END
		IF UPDATE(country)
		BEGIN
			INSERT INTO oecd_changes (file_num, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.file_num, 'Country', deleted.country, inserted.country, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.file_num = deleted.file_num AND ISNULL(inserted.country,-1) <> ISNULL(deleted.country,-1)
			WHERE inserted.update_user IS NOT NULL
		END
		IF UPDATE(issue_date)
		BEGIN
			INSERT INTO oecd_changes (file_num, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.file_num, 'Issue date', deleted.issue_date, inserted.issue_date, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.file_num = deleted.file_num AND ISNULL(inserted.issue_date,-1) <> ISNULL(deleted.issue_date,-1)
			WHERE inserted.update_user IS NOT NULL
		END
		IF UPDATE(lot_num)
		BEGIN
			INSERT INTO oecd_changes (file_num, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.file_num, 'Conditioner ID', deleted.lot_num, inserted.lot_num, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.file_num = deleted.file_num AND ISNULL(inserted.lot_num,-1) <> ISNULL(deleted.lot_num,-1)
			WHERE inserted.update_user IS NOT NULL
		END
		IF UPDATE(shipper_id)
		BEGIN
			INSERT INTO oecd_changes (file_num, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.file_num, 'Conditioner ID', deleted.shipper_id, inserted.shipper_id, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.file_num = deleted.file_num AND ISNULL(inserted.shipper_id,-1) <> ISNULL(deleted.shipper_id,-1)
			WHERE inserted.update_user IS NOT NULL
		END
		IF UPDATE(sample_form_num)
		BEGIN
			INSERT INTO oecd_changes (file_num, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.file_num, 'Sample form #', deleted.conditioner_id, inserted.sample_form_num, inserted.sample_form_num, GETDATE()
			FROM inserted JOIN deleted ON inserted.file_num = deleted.file_num AND ISNULL(inserted.sample_form_num,-1) <> ISNULL(deleted.sample_form_num,-1)
			WHERE inserted.update_user IS NOT NULL
		END
		IF UPDATE(not_cert)
		BEGIN
			INSERT INTO oecd_changes (file_num, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.file_num, 'Not certified', deleted.not_cert, inserted.not_cert, inserted.sample_form_num, GETDATE()
			FROM inserted JOIN deleted ON inserted.file_num = deleted.file_num AND ISNULL(inserted.not_cert,-1) <> ISNULL(deleted.not_cert,-1)
			WHERE inserted.update_user IS NOT NULL
		END
		IF UPDATE(domestic_origin)
		BEGIN
			INSERT INTO oecd_changes (file_num, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.file_num, 'Domestic origin', deleted.domestic_origin, inserted.domestic_origin, inserted.sample_form_num, GETDATE()
			FROM inserted JOIN deleted ON inserted.file_num = deleted.file_num AND ISNULL(inserted.domestic_origin,-1) <> ISNULL(deleted.domestic_origin,-1)
			WHERE inserted.update_user IS NOT NULL
		END
		IF UPDATE(canceled)
		BEGIN
			INSERT INTO oecd_changes (file_num, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.file_num, 'Canceled', deleted.canceled, inserted.canceled, inserted.sample_form_num, GETDATE()
			FROM inserted JOIN deleted ON inserted.file_num = deleted.file_num AND ISNULL(inserted.canceled,-1) <> ISNULL(deleted.canceled,-1)
			WHERE inserted.update_user IS NOT NULL
		END
		IF UPDATE(comments)
		BEGIN
			INSERT INTO oecd_changes (file_num, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.file_num, 'Comments', deleted.comments, inserted.comments, inserted.sample_form_num, GETDATE()
			FROM inserted JOIN deleted ON inserted.file_num = deleted.file_num AND ISNULL(inserted.comments,-1) <> ISNULL(deleted.comments,-1)
			WHERE inserted.update_user IS NOT NULL
		END
		IF UPDATE(date_printed)
		BEGIN
			INSERT INTO oecd_changes (file_num, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.file_num, 'Date printed', deleted.date_printed, inserted.date_printed, inserted.sample_form_num, GETDATE()
			FROM inserted JOIN deleted ON inserted.file_num = deleted.file_num AND ISNULL(inserted.date_printed,-1) <> ISNULL(deleted.date_printed,-1)
			WHERE inserted.update_user IS NOT NULL
		END
		IF UPDATE(ref_num)
		BEGIN
			INSERT INTO oecd_changes (file_num, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.file_num, 'ref #', deleted.ref_num, inserted.ref_num, inserted.sample_form_num, GETDATE()
			FROM inserted JOIN deleted ON inserted.file_num = deleted.file_num AND ISNULL(inserted.ref_num,-1) <> ISNULL(deleted.ref_num,-1)
			WHERE inserted.update_user IS NOT NULL
		END
		IF UPDATE(tags_requested)
		BEGIN
			INSERT INTO oecd_changes (file_num, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.file_num, 'tags requested', deleted.tags_requested, inserted.tags_requested, inserted.sample_form_num, GETDATE()
			FROM inserted JOIN deleted ON inserted.file_num = deleted.file_num AND ISNULL(inserted.tags_requested,-1) <> ISNULL(deleted.tags_requested,-1)
			WHERE inserted.update_user IS NOT NULL
		END
END