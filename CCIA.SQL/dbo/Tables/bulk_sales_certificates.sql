CREATE TABLE [dbo].[bulk_sales_certificates] (
    [Id]                   INT           IDENTITY (1, 1) NOT NULL,
    [cond_org]             INT           NOT NULL,
    [date]                 DATE          NOT NULL,
    [sid]                  INT           NULL,
    [bid]                  INT           NULL,
    [cert_program]         VARCHAR (10)  NULL,
    [purch_name]           VARCHAR (100) NULL,
    [purch_address_line_1] VARCHAR (100) NULL,
    [purch_address_line_2] VARCHAR (100) NULL,
    [purch_city]           VARCHAR (50)  NULL,
    [purch_state_id]       INT           NULL,
    [purch_country]        INT           NULL,
    [purch_zip]            VARCHAR (15)  NULL,
    [purch_phone]          VARCHAR (50)  NULL,
    [purch_email]          VARCHAR (50)  NULL,
    [cert_lbs]             INT           NOT NULL,
    [class]                INT           NOT NULL,
    [created_by]           INT           NOT NULL,
    [created_on]           DATE          NOT NULL,
    [admin_updated]        VARCHAR (10)  NULL,
    [admin_update_date]    DATETIME      NULL,
    [notification_sent]    BIT           CONSTRAINT [DF_bulk_sales_certificates_notification_sent] DEFAULT ((0)) NOT NULL
);




GO
CREATE TRIGGER [dbo].[updtBulkSalesCertificates] ON [dbo].[bulk_sales_certificates]
AFTER UPDATE NOT FOR REPLICATION AS
BEGIN
	SET NOCOUNT ON
	
	IF UPDATE([sid])
		BEGIN
			INSERT INTO bulk_sales_certificates_changes
			(bsc_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.id, 'sid', deleted.[sid], inserted.[sid], inserted.admin_updated, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.id = deleted.id AND inserted.[sid] <> deleted.[sid]			
		END
	IF UPDATE([date])
	BEGIN
		INSERT INTO bulk_sales_certificates_changes
		(bsc_id, column_change, old_value, new_value, user_change, date_change) 
		SELECT inserted.id, 'Date', deleted.[date], inserted.[date], inserted.admin_updated, GETDATE()
		FROM inserted
		JOIN deleted ON inserted.id = deleted.id AND inserted.[date] <> deleted.[date]			
	END
	IF UPDATE(class)
	BEGIN
		INSERT INTO bulk_sales_certificates_changes
		(bsc_id, column_change, old_value, new_value, user_change, date_change) 
		SELECT inserted.id, 'Class', deleted.class, inserted.class, inserted.admin_updated, GETDATE()
		FROM inserted
		JOIN deleted ON inserted.id = deleted.id AND inserted.class <> deleted.class			
	END
	IF UPDATE(cert_lbs)
	BEGIN
		INSERT INTO bulk_sales_certificates_changes
		(bsc_id, column_change, old_value, new_value, user_change, date_change) 
		SELECT inserted.id, 'Pounds', deleted.cert_lbs, inserted.cert_lbs, inserted.admin_updated, GETDATE()
		FROM inserted
		JOIN deleted ON inserted.id = deleted.id AND inserted.cert_lbs <> deleted.cert_lbs			
	END
	IF UPDATE(purch_name)
	BEGIN
		INSERT INTO bulk_sales_certificates_changes
		(bsc_id, column_change, old_value, new_value, user_change, date_change) 
		SELECT inserted.id, 'Purch Name', deleted.purch_name, inserted.purch_name, inserted.admin_updated, GETDATE()
		FROM inserted
		JOIN deleted ON inserted.id = deleted.id AND inserted.purch_name <> deleted.purch_name			
	END
	IF UPDATE(purch_address_line_1)
	BEGIN
		INSERT INTO bulk_sales_certificates_changes
		(bsc_id, column_change, old_value, new_value, user_change, date_change) 
		SELECT inserted.id, 'Purch Address1', deleted.purch_address_line_1, inserted.purch_address_line_1, inserted.admin_updated, GETDATE()
		FROM inserted
		JOIN deleted ON inserted.id = deleted.id AND inserted.purch_address_line_1 <> deleted.purch_address_line_1			
	END
	IF UPDATE(purch_address_line_2)
	BEGIN
		INSERT INTO bulk_sales_certificates_changes
		(bsc_id, column_change, old_value, new_value, user_change, date_change) 
		SELECT inserted.id, 'Purch Address2', deleted.purch_address_line_2, inserted.purch_address_line_2, inserted.admin_updated, GETDATE()
		FROM inserted
		JOIN deleted ON inserted.id = deleted.id AND inserted.purch_address_line_2 <> deleted.purch_address_line_2			
	END
	IF UPDATE(purch_city)
	BEGIN
		INSERT INTO bulk_sales_certificates_changes
		(bsc_id, column_change, old_value, new_value, user_change, date_change) 
		SELECT inserted.id, 'Purch City', deleted.purch_city, inserted.purch_city, inserted.admin_updated, GETDATE()
		FROM inserted
		JOIN deleted ON inserted.id = deleted.id AND inserted.purch_city <> deleted.purch_city			
	END
	IF UPDATE(purch_state_id)
	BEGIN
		INSERT INTO bulk_sales_certificates_changes
		(bsc_id, column_change, old_value, new_value, user_change, date_change) 
		SELECT inserted.id, 'Purch State', deleted.purch_state_id, inserted.purch_state_id, inserted.admin_updated, GETDATE()
		FROM inserted
		JOIN deleted ON inserted.id = deleted.id AND inserted.purch_state_id <> deleted.purch_state_id			
	END
	IF UPDATE(purch_country)
	BEGIN
		INSERT INTO bulk_sales_certificates_changes
		(bsc_id, column_change, old_value, new_value, user_change, date_change) 
		SELECT inserted.id, 'Purch Country', deleted.purch_country, inserted.purch_country, inserted.admin_updated, GETDATE()
		FROM inserted
		JOIN deleted ON inserted.id = deleted.id AND inserted.purch_country <> deleted.purch_country			
	END
	IF UPDATE(purch_zip)
	BEGIN
		INSERT INTO bulk_sales_certificates_changes
		(bsc_id, column_change, old_value, new_value, user_change, date_change) 
		SELECT inserted.id, 'Purch Zip', deleted.purch_country, inserted.purch_country, inserted.admin_updated, GETDATE()
		FROM inserted
		JOIN deleted ON inserted.id = deleted.id AND inserted.purch_country <> deleted.purch_country			
	END
END