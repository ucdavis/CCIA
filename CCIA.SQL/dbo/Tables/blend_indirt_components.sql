CREATE TABLE [dbo].[blend_indirt_components] (
    [comp_id]             INT             IDENTITY (1, 1) NOT NULL,
    [bid]                 INT             NOT NULL,
    [app_id]              INT             NULL,
    [weight]              NUMERIC (16, 2) NOT NULL,
    [applicant_id]        INT             NULL,
    [crop_id]             INT             NULL,
    [official_variety_id] INT             NULL,
    [cert_year]           INT             NULL,
    [country_of_origin]   INT             NULL,
    [state_of_origin]     INT             NULL,
    [cert_number]         VARCHAR (50)    NULL,
    [lot_number]          VARCHAR (50)    NULL,
    [class]               INT             NULL,
    [last_edit_by]        VARCHAR (50)    NULL
);


GO
CREATE TRIGGER [dbo].[updtInDirtBlendComponents]
   ON  dbo.blend_indirt_components
   AFTER UPDATE NOT FOR REPLICATION AS
BEGIN
	SET NOCOUNT ON;
	IF UPDATE(app_id)
		BEGIN
			INSERT INTO blend_components_changes
			(blend_id, comp_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.bid, inserted.comp_id, 'App ID', deleted.app_id, inserted.app_id, inserted.last_edit_by, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.bid = deleted.bid AND inserted.comp_id=deleted.comp_id AND ISNULL(inserted.app_id,-1) <> ISNULL(deleted.app_id,-1)																								
		END
    IF UPDATE([weight])
		BEGIN
			INSERT INTO blend_components_changes
			(blend_id, comp_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.bid, inserted.comp_id, 'Weight', deleted.[weight], inserted.[weight], inserted.last_edit_by, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.bid = deleted.bid AND inserted.comp_id=deleted.comp_id AND inserted.[weight] <> deleted.[weight]
		END
	IF UPDATE(applicant_id)
		BEGIN
			INSERT INTO blend_components_changes
			(blend_id, comp_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.bid, inserted.comp_id, 'Applicant ID', deleted.applicant_id, inserted.applicant_id, inserted.last_edit_by, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.bid = deleted.bid AND inserted.comp_id=deleted.comp_id AND ISNULL(inserted.applicant_id,-1) <> ISNULL(deleted.applicant_id,-1)																								
		END
	IF UPDATE(crop_id)
		BEGIN
			INSERT INTO blend_components_changes
			(blend_id, comp_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.bid, inserted.comp_id, 'Crop ID', deleted.crop_id, inserted.crop_id, inserted.last_edit_by, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.bid = deleted.bid AND inserted.comp_id=deleted.comp_id AND ISNULL(inserted.crop_id,-1) <> ISNULL(deleted.crop_id,-1)																								
		END
	IF UPDATE(official_variety_id)
		BEGIN
			INSERT INTO blend_components_changes
			(blend_id, comp_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.bid, inserted.comp_id, 'Official Variety ID', deleted.official_variety_id, inserted.official_variety_id, inserted.last_edit_by, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.bid = deleted.bid AND inserted.comp_id=deleted.comp_id AND ISNULL(inserted.official_variety_id,-1) <> ISNULL(deleted.official_variety_id,-1)																								
		END
	IF UPDATE(cert_year)
		BEGIN
			INSERT INTO blend_components_changes
			(blend_id, comp_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.bid, inserted.comp_id, 'Cert Year', deleted.cert_year, inserted.cert_year, inserted.last_edit_by, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.bid = deleted.bid AND inserted.comp_id=deleted.comp_id AND ISNULL(inserted.cert_year,-1) <> ISNULL(deleted.cert_year,-1)																								
		END
	IF UPDATE(country_of_origin)
		BEGIN
			INSERT INTO blend_components_changes
			(blend_id, comp_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.bid, inserted.comp_id, 'Country of Origin', deleted.country_of_origin, inserted.country_of_origin, inserted.last_edit_by, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.bid = deleted.bid AND inserted.comp_id=deleted.comp_id AND ISNULL(inserted.country_of_origin,-1) <> ISNULL(deleted.country_of_origin,-1)																								
		END
	IF UPDATE(state_of_origin)
		BEGIN
			INSERT INTO blend_components_changes
			(blend_id, comp_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.bid, inserted.comp_id, 'State of Origin', deleted.state_of_origin, inserted.state_of_origin, inserted.last_edit_by, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.bid = deleted.bid AND inserted.comp_id=deleted.comp_id AND ISNULL(inserted.state_of_origin,-1) <> ISNULL(deleted.state_of_origin,-1)																								
		END
	IF UPDATE(cert_number)
		BEGIN
			INSERT INTO blend_components_changes
			(blend_id, comp_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.bid, inserted.comp_id, 'Cert Number', deleted.cert_number, inserted.cert_number, inserted.last_edit_by, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.bid = deleted.bid AND inserted.comp_id=deleted.comp_id AND ISNULL(inserted.cert_number,-1) <> ISNULL(deleted.cert_number,-1)																								
		END
	IF UPDATE(lot_number)
		BEGIN
			INSERT INTO blend_components_changes
			(blend_id, comp_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.bid, inserted.comp_id, 'Lot Number', deleted.lot_number, inserted.lot_number, inserted.last_edit_by, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.bid = deleted.bid AND inserted.comp_id=deleted.comp_id AND ISNULL(inserted.lot_number,-1) <> ISNULL(deleted.lot_number,-1)																								
		END
	IF UPDATE(class)
		BEGIN
			INSERT INTO blend_components_changes
			(blend_id, comp_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.bid, inserted.comp_id, 'class', deleted.class, inserted.class, inserted.last_edit_by, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.bid = deleted.bid AND inserted.comp_id=deleted.comp_id AND ISNULL(inserted.class,-1) <> ISNULL(deleted.class,-1)																								
		END
END