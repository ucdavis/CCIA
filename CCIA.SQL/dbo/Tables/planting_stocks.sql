CREATE TABLE [dbo].[planting_stocks] (
    [ps_id]                              INT             IDENTITY (1, 1) NOT NULL,
    [app_id]                             INT             NULL,
    [ps_cert_num]                        VARCHAR (50)    NULL,
    [ps_entered_variety]                 VARCHAR (50)    NULL,
    [official_variety_id]                INT             NULL,
    [pounds_planted]                     DECIMAL (14, 2) NULL,
    [ps_class]                           INT             NULL,
    [AbbrevClassProducedClassProducedId] INT             NULL,
    [ps_accession]                       INT             NULL,
    [state_country_tag_issued]           INT             NULL,
    [state_country_grown]                INT             NULL,
    [seed_purchased_from]                VARCHAR (100)   NULL,
    [winter_test]                        BIT             CONSTRAINT [DF_planting_stocks_winter_test] DEFAULT ((0)) NOT NULL,
    [PVX_test]                           BIT             CONSTRAINT [DF_planting_stocks_PVX_test] DEFAULT ((0)) NOT NULL,
    [date_modified]                      DATETIME        NULL,
    [user_modified]                      VARCHAR (50)    NULL,
    [date_entered]                       DATETIME        NULL,
    [user_creator]                       INT             NULL,
    [thc_percent]                        VARCHAR (10)    NULL,
    [plants_per_acre]                    DECIMAL (14, 2) NULL,
    [user_emp_modified]                  VARCHAR (10)    NULL,
    CONSTRAINT [PK_planting_stocks] PRIMARY KEY CLUSTERED ([ps_id] ASC),
    CONSTRAINT [FK_planting_stocks_abbrev_class_produced_AbbrevClassProducedClassProducedId] FOREIGN KEY ([AbbrevClassProducedClassProducedId]) REFERENCES [dbo].[abbrev_class_produced] ([class_produced_id])
);




GO
CREATE TRIGGER [dbo].[updtPlantingStocks] ON [dbo].[planting_stocks]
AFTER UPDATE NOT FOR REPLICATION AS
BEGIN
	SET NOCOUNT ON
	DECLARE @app_id INT, @submitable bit, @ps# int
	SELECT @app_id = app_id FROM inserted
	SELECT @submitable = app_submitable FROM applications WHERE app_id = @app_id
	SELECT @ps# = row# FROM (SELECT   ROW_NUMBER() OVER(ORDER BY ps_id DESC) AS Row#, ps_id FROM planting_stocks WHERE app_id = @app_id) a WHERE ps_id = (SELECT ps_id FROM inserted)

	IF UPDATE(ps_cert_num) AND @submitable = 0
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'PS#' + cast(@ps# as varchar) + '-cert number', deleted.ps_cert_num, inserted.ps_cert_num, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND inserted.ps_cert_num <> deleted.ps_cert_num AND  ISNULL(inserted.ps_cert_num,-1) <> ISNULL(deleted.ps_cert_num,-1)
		END
	IF UPDATE(ps_entered_variety) AND @submitable = 0
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'PS#' + cast(@ps# as varchar) + '-variety', deleted.ps_entered_variety, inserted.ps_entered_variety, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND inserted.ps_entered_variety <> deleted.ps_entered_variety AND  ISNULL(inserted.ps_entered_variety,-1) <> ISNULL(deleted.ps_entered_variety,-1)
		END
	IF UPDATE(pounds_planted) AND @submitable = 0
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'PS#' + cast(@ps# as varchar) + '-pounds planted', deleted.pounds_planted, inserted.pounds_planted, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND ISNULL(inserted.pounds_planted,-1) <> ISNULL(deleted.pounds_planted,-1)
		END
	IF UPDATE(ps_class) AND @submitable = 0
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'PS#' + cast(@ps# as varchar) + '-class', old.class_abbrv, new.class_abbrv, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND ISNULL(inserted.ps_class,-1) <> ISNULL(deleted.ps_class,-1)
			LEFT OUTER JOIN abbrev_class_produced new ON inserted.ps_class = new.class_produced_id
			LEFT OUTER JOIN abbrev_class_produced old ON deleted.ps_class = old.class_produced_id
		END
	IF UPDATE(ps_accession) AND @submitable = 0
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'PS#' + cast(@ps# as varchar) + '-accession', deleted.ps_accession, inserted.ps_accession, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND ISNULL(inserted.ps_accession,-1) <> ISNULL(deleted.ps_accession,-1)
		END
	IF UPDATE(state_country_tag_issued) AND @submitable = 0
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'PS#' + cast(@ps# as varchar) + '-tag issued', old.StateProvinceName, new.StateProvinceName, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND ISNULL(inserted.state_country_tag_issued,-1) <> ISNULL(deleted.state_country_tag_issued,-1)
			LEFT OUTER JOIN state_province new ON inserted.state_country_tag_issued = new.StateProvinceID
			LEFT OUTER JOIN state_province old ON deleted.state_country_tag_issued = old.StateProvinceID
		END
	IF UPDATE(state_country_grown) AND @submitable = 0
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'PS#' + cast(@ps# as varchar) + '-grown', old.StateProvinceName, new.StateProvinceName, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND ISNULL(inserted.state_country_grown,-1) <> ISNULL(deleted.state_country_grown,-1)
			LEFT OUTER JOIN state_province new ON inserted.state_country_grown = new.StateProvinceID
			LEFT OUTER JOIN state_province old ON deleted.state_country_grown = old.StateProvinceID
		END
	IF UPDATE(seed_purchased_from) AND @submitable = 0
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'PS#' + cast(@ps# as varchar) + '-accession', deleted.seed_purchased_from, inserted.seed_purchased_from, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND ISNULL(inserted.seed_purchased_from,-1) <> ISNULL(deleted.seed_purchased_from,-1)
		END
	IF UPDATE(winter_test) AND @submitable = 0
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'PS#' + cast(@ps# as varchar) + '-winter test', deleted.winter_test, inserted.winter_test, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND inserted.winter_test <> deleted.winter_test
		END
	IF UPDATE(PVX_test) AND @submitable = 0
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'PS#' + cast(@ps# as varchar) + '-PVX test', deleted.PVX_test, inserted.PVX_test, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND inserted.PVX_test <> deleted.PVX_test
		END
	IF UPDATE(thc_percent) AND @submitable = 0
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'PS#' + cast(@ps# as varchar) + '-thc%', deleted.thc_percent, inserted.thc_percent, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND ISNULL(inserted.thc_percent,-1) <> ISNULL(deleted.thc_percent,-1)
		END
	IF UPDATE(plants_per_acre) AND @submitable = 0
		BEGIN
			INSERT INTO application_changes
			(app_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.app_id, 'PS#' + cast(@ps# as varchar) + '-thc%', deleted.plants_per_acre, inserted.plants_per_acre, inserted.user_emp_modified, GETDATE()
			FROM inserted
			JOIN deleted ON inserted.app_id = deleted.app_id AND ISNULL(inserted.plants_per_acre,-1) <> ISNULL(deleted.plants_per_acre,-1)
		END
End