CREATE TABLE [dbo].[seed_transfers] (
    [stid]                       INT             IDENTITY (1, 1) NOT NULL,
    [transfer_type]              VARCHAR (50)    NULL,
    [originating_org]            INT             NOT NULL,
    [originating_county]         INT             NULL,
    [app_id]                     INT             NULL,
    [sid]                        INT             NULL,
    [bid]                        INT             NULL,
    [certificate_date]           DATETIME        NOT NULL,
    [created_by]                 INT             NOT NULL,
    [created_on]                 DATETIME        NULL,
    [transfer_lbs]               NUMERIC (16, 2) NOT NULL,
    [transfer_class]             INT             NULL,
    [seedstock_lot_numbers]      VARCHAR (500)   NULL,
    [submitted_for_analysis]     BIT             CONSTRAINT [DF_seed_transfers_submitted_for_analysis] DEFAULT ((0)) NOT NULL,
    [destination_org]            INT             NULL,
    [purch_name]                 VARCHAR (100)   NULL,
    [purch_address_line_1]       VARCHAR (100)   NULL,
    [purch_address_line_2]       VARCHAR (100)   NULL,
    [purch_city]                 VARCHAR (50)    NULL,
    [purch_county]               INT             NULL,
    [purch_state_id]             INT             NULL,
    [purch_country]              INT             NULL,
    [purch_zip]                  VARCHAR (15)    NULL,
    [purch_phone]                VARCHAR (50)    NULL,
    [purch_email]                VARCHAR (50)    NULL,
    [stage_indirt]               BIT             CONSTRAINT [DF_seed_transfers_stage_indirt] DEFAULT ((0)) NOT NULL,
    [stage_from_field]           BIT             CONSTRAINT [DF_Table_1_state_from_field] DEFAULT ((0)) NOT NULL,
    [stage_from_field_num_acres] NUMERIC (8, 2)  NULL,
    [stage_from_storage]         BIT             CONSTRAINT [DF_seed_transfers_stage_from_storage] DEFAULT ((0)) NOT NULL,
    [stage_conditioned]          BIT             CONSTRAINT [DF_seed_transfers_stage_conditioned] DEFAULT ((0)) NOT NULL,
    [stage_nfc]                  BIT             CONSTRAINT [DF_seed_transfers_stage_nfc] DEFAULT ((0)) NOT NULL,
    [stage_certified_seed]       BIT             CONSTRAINT [DF_seed_transfers_stage_certified_seed] DEFAULT ((0)) NOT NULL,
    [stage_treatment]            BIT             CONSTRAINT [DF_seed_transfers_stage_treatment] DEFAULT ((0)) NOT NULL,
    [stage_bagging]              BIT             CONSTRAINT [DF_seed_transfers_stage_bagging] DEFAULT ((0)) NOT NULL,
    [stage_tagging]              BIT             CONSTRAINT [DF_seed_transfers_stage_tagging] DEFAULT ((0)) NOT NULL,
    [stage_blending]             BIT             CONSTRAINT [DF_seed_transfers_stage_blending] DEFAULT ((0)) NOT NULL,
    [stage_storage]              BIT             CONSTRAINT [DF_seed_transfers_stage_storage] DEFAULT ((0)) NOT NULL,
    [stage_other]                BIT             CONSTRAINT [DF_seed_transfers_stage_other] DEFAULT ((0)) NOT NULL,
    [stage_other_value]          VARCHAR (50)    NULL,
    [type_retail]                BIT             CONSTRAINT [DF_seed_transfers_type_retail] DEFAULT ((0)) NOT NULL,
    [type_tote]                  BIT             CONSTRAINT [DF_seed_transfers_type_tote] DEFAULT ((0)) NOT NULL,
    [type_bulk]                  BIT             CONSTRAINT [DF_seed_transfers_type_bulk] DEFAULT ((0)) NOT NULL,
    [number_of_trucks]           INT             NULL,
    [ag_comm_accurate]           BIT             CONSTRAINT [DF_seed_transfers_ag_comm_accurate] DEFAULT ((0)) NOT NULL,
    [ag_comm_inaccurate]         BIT             CONSTRAINT [DF_seed_transfers_ag_comm_inaccurate] DEFAULT ((0)) NOT NULL,
    [ag_comm_approve]            BIT             CONSTRAINT [DF_seed_transfers_ag_comm_approve] DEFAULT ((0)) NOT NULL,
    [ag_comm_date_respond]       DATETIME        NULL,
    [contact_respond]            INT             NULL,
    [employee_update_date]       DATETIME        NULL,
    [employee_update_id]         VARCHAR (10)    NULL,
    [update_by_admin]            BIT             CONSTRAINT [DF_seed_transfers_update_by_admin] DEFAULT ((0)) NOT NULL
);




GO
CREATE TRIGGER [dbo].[updtSeedTransfers] ON [dbo].[seed_transfers]
AFTER UPDATE NOT FOR REPLICATION AS
BEGIN
	SET NOCOUNT ON
	
	IF UPDATE([transfer_type])
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'transfer_type', deleted.[transfer_type], inserted.[transfer_type], inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.transfer_type <> deleted.transfer_type			
		END
	IF UPDATE(originating_org)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'originating_org', deleted.originating_org, inserted.originating_org, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.originating_org <> deleted.originating_org			
		END
	IF UPDATE(originating_county)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'originating_county', deleted.originating_county, inserted.originating_county, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.originating_county <> deleted.originating_county			
		END	
	IF UPDATE(app_id)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'app_id', deleted.app_id, inserted.app_id, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND (inserted.app_id <> deleted.app_id OR (deleted.app_id IS NULL AND inserted.app_id IS NOT NULL))			
		END
	IF UPDATE(bid)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'BID', deleted.bid, inserted.bid, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND (inserted.bid <> deleted.bid OR (deleted.bid IS NULL AND inserted.bid IS NOT NULL))			
		END
	IF UPDATE([sid])
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, '[sid]', deleted.[sid], inserted.[sid], inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND (inserted.[sid] <> deleted.[sid] OR (deleted.[sid] IS NULL AND inserted.[sid] IS NOT NULL))	
		END	
	IF UPDATE(certificate_date)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'certificate_date', deleted.certificate_date, inserted.certificate_date, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.certificate_date <> deleted.certificate_date			
		END
	IF UPDATE(transfer_lbs)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'transfer_lbs', deleted.transfer_lbs, inserted.transfer_lbs, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.transfer_lbs <> deleted.transfer_lbs			
		END
	IF UPDATE(transfer_class)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'transfer_class', deleted.transfer_class, inserted.transfer_class, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.transfer_class <> deleted.transfer_class			
		END
	IF UPDATE(seedstock_lot_numbers)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'seedstock_lot_numbers', deleted.seedstock_lot_numbers, inserted.seedstock_lot_numbers, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.seedstock_lot_numbers <> deleted.seedstock_lot_numbers			
		END
	IF UPDATE(submitted_for_analysis)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'submitted_for_analysis', deleted.submitted_for_analysis, inserted.submitted_for_analysis, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.submitted_for_analysis <> deleted.submitted_for_analysis			
		END
	IF UPDATE(destination_org)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'destination_org', deleted.destination_org, inserted.destination_org, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.destination_org <> deleted.destination_org			
		END
	IF UPDATE(purch_name)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'purch_name', deleted.purch_name, inserted.purch_name, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.purch_name <> deleted.purch_name			
		END
	IF UPDATE(purch_address_line_1)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'purch_address_line_1', deleted.purch_address_line_1, inserted.purch_address_line_1, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.purch_address_line_1 <> deleted.purch_address_line_1			
		END
	IF UPDATE(purch_address_line_2)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'purch_address_line_2', deleted.purch_address_line_2, inserted.purch_address_line_2, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.purch_address_line_2 <> deleted.purch_address_line_2			
		END
	IF UPDATE(purch_city)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'purch_city', deleted.purch_city, inserted.purch_city, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.purch_city <> deleted.purch_city			
		END
	IF UPDATE(purch_county)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'purch_county', deleted.purch_county, inserted.purch_county, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.purch_county <> deleted.purch_county			
		END
	IF UPDATE(purch_state_id)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'purch_state_id', deleted.purch_state_id, inserted.purch_state_id, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.purch_state_id <> deleted.purch_state_id			
		END
	IF UPDATE(purch_country)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'purch_country', deleted.purch_country, inserted.purch_country, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.purch_country <> deleted.purch_country			
		END
	IF UPDATE(purch_zip)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'purch_zip', deleted.purch_zip, inserted.purch_zip, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.purch_zip <> deleted.purch_zip			
		END
	IF UPDATE(purch_phone)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'purch_phone', deleted.purch_phone, inserted.purch_phone, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.purch_phone <> deleted.purch_phone			
		END
	IF UPDATE(purch_email)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'purch_email', deleted.purch_email, inserted.purch_email, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.purch_email <> deleted.purch_email			
		END
	IF UPDATE(stage_indirt)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'stage_indirt', deleted.stage_indirt, inserted.stage_indirt, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.stage_indirt <> deleted.stage_indirt			
		END
	IF UPDATE(stage_from_field)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'stage_from_field', deleted.stage_from_field, inserted.stage_from_field, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.stage_from_field <> deleted.stage_from_field			
		END
	IF UPDATE(stage_from_field_num_acres)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'stage_from_field_num_acres', deleted.stage_from_field_num_acres, inserted.stage_from_field_num_acres, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND (inserted.stage_from_field_num_acres <> deleted.stage_from_field_num_acres	OR (deleted.stage_from_field_num_acres IS NULL AND inserted.stage_from_field_num_acres IS NOT NULL))
		END
	IF UPDATE(stage_from_storage)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'stage_from_storage', deleted.stage_from_storage, inserted.stage_from_storage, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.stage_from_storage <> deleted.stage_from_storage			
		END
	IF UPDATE(stage_conditioned)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'stage_conditioned', deleted.stage_conditioned, inserted.stage_conditioned, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.stage_conditioned <> deleted.stage_conditioned			
		END
	IF UPDATE(stage_nfc)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'stage_nfc', deleted.stage_nfc, inserted.stage_nfc, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.stage_nfc <> deleted.stage_nfc			
		END
	IF UPDATE(stage_certified_seed)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'stage_certified_seed', deleted.stage_certified_seed, inserted.stage_certified_seed, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.stage_certified_seed <> deleted.stage_certified_seed			
		END
	IF UPDATE(stage_treatment)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'stage_treatment', deleted.stage_treatment, inserted.stage_treatment, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.stage_treatment <> deleted.stage_treatment			
		END
	IF UPDATE(stage_bagging)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'stage_bagging', deleted.stage_bagging, inserted.stage_bagging, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.stage_bagging <> deleted.stage_bagging			
		END
	IF UPDATE(stage_tagging)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'stage_tagging', deleted.stage_tagging, inserted.stage_tagging, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.stage_tagging <> deleted.stage_tagging			
		END
	IF UPDATE(stage_blending)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'stage_blending', deleted.stage_blending, inserted.stage_blending, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.stage_blending <> deleted.stage_blending			
		END
	IF UPDATE(stage_storage)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'stage_storage', deleted.stage_storage, inserted.stage_storage, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.stage_storage <> deleted.stage_storage			
		END
	IF UPDATE(stage_other)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'stage_other', deleted.stage_other, inserted.stage_other, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.stage_other <> deleted.stage_other			
		END
	IF UPDATE(stage_other_value)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'stage_other_value', deleted.stage_other_value, inserted.stage_other_value, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND (inserted.stage_other_value <> deleted.stage_other_value OR (deleted.stage_other_value IS NULL AND inserted.stage_other_value IS NOT NULL))
		END
	IF UPDATE(type_retail)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'type_retail', deleted.type_retail, inserted.type_retail, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.type_retail <> deleted.type_retail			
		END
	IF UPDATE(type_tote)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'type_tote', deleted.type_tote, inserted.type_tote, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.type_tote <> deleted.type_tote			
		END
	IF UPDATE(type_bulk)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'type_bulk', deleted.type_bulk, inserted.type_bulk, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.type_bulk <> deleted.type_bulk			
		END
	IF UPDATE(number_of_trucks)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'number_of_trucks', deleted.number_of_trucks, inserted.number_of_trucks, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.number_of_trucks <> deleted.number_of_trucks			
		END
	IF UPDATE(ag_comm_accurate)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'ag_comm_accurate', deleted.ag_comm_accurate, inserted.ag_comm_accurate, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.ag_comm_accurate <> deleted.ag_comm_accurate			
		END
	IF UPDATE(ag_comm_inaccurate)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'ag_comm_inaccurate', deleted.ag_comm_inaccurate, inserted.ag_comm_inaccurate, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.ag_comm_inaccurate <> deleted.ag_comm_inaccurate			
		END
	IF UPDATE(ag_comm_approve)
		BEGIN
			INSERT INTO seed_transfer_changes
			(stid, column_change, old_value, new_value, user_change, date_change, user_admin) 
			SELECT inserted.stid, 'ag_comm_approve', deleted.ag_comm_approve, inserted.ag_comm_approve, inserted.employee_update_id, GETDATE(), inserted.update_by_admin
			FROM inserted
			JOIN deleted ON inserted.stid = deleted.stid AND inserted.ag_comm_approve <> deleted.ag_comm_approve			
		END
END