CREATE TABLE [dbo].[field_history] (
    [history_id]        INT          IDENTITY (1, 1) NOT NULL,
    [app_id]            INT          NOT NULL,
    [year]              INT          NOT NULL,
    [crop]              INT          NULL,
    [entered_variety]   VARCHAR (50) NULL,
    [app_num]           VARCHAR (50) NULL,
    [date_modified]     DATETIME     NULL,
    [user_emp_modified] VARCHAR (10) NULL
);




GO
CREATE TRIGGER [dbo].[updtFieldHistory] ON [dbo].[field_history]
AFTER UPDATE NOT FOR REPLICATION AS
BEGIN
	SET NOCOUNT ON
	DECLARE @app_id INT, @submitable bit, @history# int
	SELECT @app_id = app_id FROM inserted
	SELECT @submitable = app_submitable FROM applications WHERE app_id = @app_id
	SELECT @history# = row# FROM (SELECT   ROW_NUMBER() OVER(ORDER BY ps_id DESC) AS Row#, ps_id FROM planting_stocks WHERE app_id = @app_id) a WHERE ps_id = (SELECT ps_id FROM inserted)

	IF UPDATE([year]) AND @submitable = 0
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'History#' + cast(@history# as varchar) + '-year', deleted.year, inserted.year, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND inserted.year <> deleted.year
		END
	IF UPDATE(crop) AND @submitable = 0
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'History#' + cast(@history# as varchar) + '-crop', ISNULL(old.crop_kind + ' ' + old.crop,old.crop), ISNULL(new.crop_kind + ' ' + new.crop,new.crop), inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND ISNULL(inserted.crop,-1) <> ISNULL(deleted.crop,-1)
			JOIN crops new ON inserted.crop = new.crop_id
			JOIN crops old ON deleted.crop = old.crop_id
		END
	IF UPDATE(entered_variety) AND @submitable = 0
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'History#' + cast(@history# as varchar) + '-variety', deleted.entered_variety, inserted.entered_variety, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND ISNULL(inserted.entered_variety,-1) <> ISNULL(deleted.entered_variety,-1)
		END	
	IF UPDATE(app_num) AND @submitable = 0
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'History#' + cast(@history# as varchar) + '-app#', deleted.app_num, inserted.app_num, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND ISNULL(inserted.app_num,-1) <> ISNULL(deleted.app_num,-1)
		END	
End