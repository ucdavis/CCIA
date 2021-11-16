CREATE TABLE [dbo].[tags] (
    [tag_id]               INT             IDENTITY (1, 1) NOT NULL,
    [seeds_id]             INT             NULL,
    [blend_id]             INT             NULL,
    [pot_app_id]           INT             NULL,
    [oecd_file_num]        INT             NULL,
    [tag_class]            INT             NULL,
    [date_requested]       DATETIME        NULL,
    [date_needed]          DATETIME        NULL,
    [date_run]             DATETIME        NULL,
    [lot_weight_bagged]    NUMERIC (16, 2) NULL,
    [coating_percent]      NUMERIC (3, 2)  NULL,
    [lot_weight_requested] AS              (([count_requested]-[tag_extras_overrun])*case when [weight_unit]='L' then [bag_size] else [bag_size]*(2.20462262) end) PERSISTED,
    [count_requested]      INT             NULL,
    [count_used]           INT             NULL,
    [tag_type]             INT             CONSTRAINT [DF_tags_tag_type] DEFAULT ((2)) NULL,
    [tag_extras_overrun]   INT             CONSTRAINT [DF_tags_tag_extras_overrun] DEFAULT ((0)) NOT NULL,
    [tag_statement]        VARCHAR (1000)  NULL,
    [bag_size]             NUMERIC (8, 2)  NULL,
    [weight_unit]          CHAR (1)        NULL,
    [bag_size_lbs]         AS              (case when [weight_unit]='L' then [bag_size] else [bag_size]*(2.20462262) end) PERSISTED,
    [comments]             VARCHAR (5000)  NULL,
    [order_contact]        INT             NULL,
    [user_printed]         VARCHAR (50)    NULL,
    [user_entered]         INT             NULL,
    [date_entered]         DATETIME        NULL,
    [user_modified]        VARCHAR (50)    NULL,
    [date_modified]        DATETIME        NULL,
    [pretagging]           BIT             CONSTRAINT [DF_tags_pretagging] DEFAULT ((0)) NOT NULL,
    [series_numbered]      BIT             CONSTRAINT [DF_tags_series_numbered] DEFAULT ((0)) NULL,
    [bulk_request]         BIT             CONSTRAINT [DF_tags_bulk_request] DEFAULT ((0)) NOT NULL,
    [analysis_request]     BIT             CONSTRAINT [DF_tags_analysis_request] DEFAULT ((0)) NOT NULL,
    [how_deliver]          VARCHAR (50)    NULL,
    [tracking_number]      VARCHAR (50)    NULL,
    [tagging_org]          INT             NULL,
    [stage]                VARCHAR (50)    NULL,
    [user_apporved]        VARCHAR (50)    NULL,
    [approved_date]        DATETIME        NULL,
    [printed_date]         DATETIME        NULL,
    [requested_alias]      VARCHAR (50)    NULL,
    [oecd_request]         BIT             CONSTRAINT [DF_tags_oecd_request] DEFAULT ((0)) NULL,
    [ps_number]            VARCHAR (50)    NULL,
    [oecd_tag_type]        INT             NULL,
    [date_sealed]          DATE            NULL,
    [oecd_country]         INT             NULL,
    [admin_comments]       VARCHAR (8000)  NULL,
    [series_request]       BIT             CONSTRAINT [DF_tags_series_request] DEFAULT ((0)) NOT NULL,
    [bulk_crop_id]         INT             NULL,
    [bulk_var_off_id]      INT             NULL
);




GO
CREATE TRIGGER [dbo].[updtTags] ON [dbo].[tags]
   AFTER UPDATE NOT FOR REPLICATION 
AS 
BEGIN
	SET NOCOUNT ON;

    IF UPDATE(seeds_id)
		BEGIN
			INSERT INTO tags_changes (tag_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.tag_id, 'SID', deleted.seeds_id, inserted.seeds_id, inserted.user_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.tag_id = deleted.tag_id AND ISNULL(inserted.seeds_id,-1) <> ISNULL(deleted.seeds_id,-1)
			WHERE inserted.user_modified IS NOT NULL
		END
		
    IF UPDATE(blend_id)
		BEGIN
			INSERT INTO tags_changes (tag_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.tag_id, 'BID', deleted.blend_id, inserted.blend_id, inserted.user_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.tag_id = deleted.tag_id AND ISNULL(inserted.blend_id,-1) <> ISNULL(deleted.blend_id,-1)
			WHERE inserted.user_modified IS NOT NULL
		END
   IF UPDATE(pot_app_id)
		BEGIN
			INSERT INTO tags_changes (tag_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.tag_id, 'Pot App ID', deleted.pot_app_id, inserted.pot_app_id, inserted.user_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.tag_id = deleted.tag_id AND ISNULL(inserted.pot_app_id,-1) <> ISNULL(deleted.pot_app_id,-1)
			WHERE inserted.user_modified IS NOT NULL
		END
    IF UPDATE(oecd_file_num)
		BEGIN
			INSERT INTO tags_changes (tag_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.tag_id, 'OECD File Number', deleted.oecd_file_num, inserted.oecd_file_num, inserted.user_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.tag_id = deleted.tag_id AND ISNULL(inserted.oecd_file_num,-1) <> ISNULL(deleted.oecd_file_num,-1)
			WHERE inserted.user_modified IS NOT NULL
		END
	IF UPDATE(tag_class)
		BEGIN
			INSERT INTO tags_changes (tag_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.tag_id, 'Tag Class', deleted.tag_class, inserted.tag_class, inserted.user_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.tag_id = deleted.tag_id AND ISNULL(inserted.tag_class,255) <> ISNULL(deleted.tag_class,255)
			WHERE inserted.user_modified IS NOT NULL
		END
	IF UPDATE(lot_weight_bagged)
		BEGIN
			INSERT INTO tags_changes (tag_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.tag_id, 'Lot Weight Bagged', deleted.lot_weight_bagged, inserted.lot_weight_bagged, inserted.user_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.tag_id = deleted.tag_id AND ISNULL(inserted.lot_weight_bagged,-1) <> ISNULL(deleted.lot_weight_bagged,-1)
			WHERE inserted.user_modified IS NOT NULL
		END
	IF UPDATE(coating_percent)
		BEGIN
			INSERT INTO tags_changes (tag_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.tag_id, 'Coating percent', deleted.coating_percent, inserted.coating_percent, inserted.user_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.tag_id = deleted.tag_id AND ISNULL(inserted.coating_percent,-1) <> ISNULL(deleted.coating_percent,-1)
			WHERE inserted.user_modified IS NOT NULL
		END
	IF UPDATE(count_requested)
		BEGIN
			INSERT INTO tags_changes (tag_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.tag_id, 'Tags Requested', deleted.count_requested, inserted.count_requested, inserted.user_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.tag_id = deleted.tag_id AND ISNULL(inserted.count_requested,-1) <> ISNULL(deleted.count_requested,-1)
			WHERE inserted.user_modified IS NOT NULL
		END
	IF UPDATE(count_used)
		BEGIN
			INSERT INTO tags_changes (tag_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.tag_id, 'Tags Used', deleted.count_used, inserted.count_used, inserted.user_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.tag_id = deleted.tag_id AND ISNULL(inserted.count_used,-1) <> ISNULL(deleted.count_used,-1)
			WHERE inserted.user_modified IS NOT NULL
		END
	IF UPDATE(tag_type)
		BEGIN
			INSERT INTO tags_changes (tag_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.tag_id, 'Tags Type', deleted.tag_type, inserted.tag_type, inserted.user_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.tag_id = deleted.tag_id AND ISNULL(inserted.tag_type,0) <> ISNULL(deleted.tag_type,0)
			WHERE inserted.user_modified IS NOT NULL
		END
	IF UPDATE(tag_extras_overrun)
		BEGIN
			INSERT INTO tags_changes (tag_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.tag_id, 'Extras Overrun', deleted.tag_extras_overrun, inserted.tag_extras_overrun, inserted.user_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.tag_id = deleted.tag_id AND ISNULL(inserted.tag_extras_overrun,-1) <> ISNULL(deleted.tag_extras_overrun,-1)
			WHERE inserted.user_modified IS NOT NULL
		END
	IF UPDATE(tag_statement)
		BEGIN
			INSERT INTO tags_changes (tag_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.tag_id, 'Tag Statement', deleted.tag_statement, inserted.tag_statement, inserted.user_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.tag_id = deleted.tag_id AND ISNULL(inserted.tag_statement,-1) <> ISNULL(deleted.tag_statement,-1)
			WHERE inserted.user_modified IS NOT NULL
		END
	IF UPDATE(bag_size)
		BEGIN
			INSERT INTO tags_changes (tag_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.tag_id, 'Bag size', deleted.bag_size, inserted.bag_size, inserted.user_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.tag_id = deleted.tag_id AND ISNULL(inserted.bag_size,-1) <> ISNULL(deleted.bag_size,-1)
			WHERE inserted.user_modified IS NOT NULL
		END
	IF UPDATE(weight_unit)
		BEGIN
			INSERT INTO tags_changes (tag_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.tag_id, 'Weight unit', deleted.weight_unit, inserted.weight_unit, inserted.user_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.tag_id = deleted.tag_id AND ISNULL(inserted.weight_unit,-1) <> ISNULL(deleted.weight_unit,-1)
			WHERE inserted.user_modified IS NOT NULL
		END
	IF UPDATE(order_contact)
		BEGIN
			INSERT INTO tags_changes (tag_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.tag_id, 'Order contact', deleted.order_contact, inserted.order_contact, inserted.user_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.tag_id = deleted.tag_id AND ISNULL(inserted.order_contact,-1) <> ISNULL(deleted.order_contact,-1)
			WHERE inserted.user_modified IS NOT NULL
		END
	IF UPDATE(pretagging)
		BEGIN
			INSERT INTO tags_changes (tag_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.tag_id, 'Pretagging', deleted.pretagging, inserted.pretagging, inserted.user_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.tag_id = deleted.tag_id AND ISNULL(inserted.pretagging,-1) <> ISNULL(deleted.pretagging,-1)
			WHERE inserted.user_modified IS NOT NULL
		END
	IF UPDATE(how_deliver)
		BEGIN
			INSERT INTO tags_changes (tag_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.tag_id, 'Delivery', deleted.how_deliver, inserted.how_deliver, inserted.user_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.tag_id = deleted.tag_id AND ISNULL(inserted.how_deliver,-1) <> ISNULL(deleted.how_deliver,-1)
			WHERE inserted.user_modified IS NOT NULL
		END
	IF UPDATE(tagging_org)
		BEGIN
			INSERT INTO tags_changes (tag_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.tag_id, 'Tagging Org', deleted.tagging_org, inserted.tagging_org, inserted.user_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.tag_id = deleted.tag_id AND ISNULL(inserted.tagging_org,-1) <> ISNULL(deleted.tagging_org,-1)
			WHERE inserted.user_modified IS NOT NULL
		END
	IF UPDATE(requested_alias)
		BEGIN
			INSERT INTO tags_changes (tag_id, column_change, old_value, new_value, user_change, date_change)
			SELECT inserted.tag_id, 'Alias', deleted.requested_alias, inserted.requested_alias, inserted.user_modified, GETDATE()
			FROM inserted JOIN deleted ON inserted.tag_id = deleted.tag_id AND ISNULL(inserted.requested_alias,-1) <> ISNULL(deleted.requested_alias,-1)
			WHERE inserted.user_modified IS NOT NULL
		END
END
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'pnts to seeds.seeds_id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tags', @level2type = N'COLUMN', @level2name = N'seeds_id';

