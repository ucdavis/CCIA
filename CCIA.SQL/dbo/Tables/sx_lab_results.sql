CREATE TABLE [dbo].[sx_lab_results] (
    [seeds_id]                      INT             NOT NULL,
    [date_complete]                 DATETIME        NULL,
    [germ_percent]                  NUMERIC (8, 7)  NULL,
    [germ_days]                     INT             NULL,
    [germ_results]                  CHAR (1)        NULL,
    [germ_temp]                     VARCHAR (10)    NULL,
    [inert_percent]                 NUMERIC (8, 7)  NULL,
    [inert_comments]                VARCHAR (100)   NULL,
    [other_variety_percent]         NUMERIC (8, 7)  NULL,
    [other_variety_comments]        VARCHAR (100)   NULL,
    [other_variety_count]           INT             NULL,
    [other_crop_percent]            NUMERIC (8, 7)  NULL,
    [other_crop_comments]           VARCHAR (100)   NULL,
    [other_crop_count]              INT             NULL,
    [purity_percent]                NUMERIC (8, 7)  NULL,
    [purity_comments]               VARCHAR (100)   NULL,
    [purity_results]                CHAR (1)        NULL,
    [sample_comments]               VARCHAR (500)   NULL,
    [weed_seed_percent]             NUMERIC (8, 7)  NULL,
    [weed_seed_comments]            VARCHAR (100)   NULL,
    [weed_seed_count]               INT             NULL,
    [noxious_grams]                 NUMERIC (7, 2)  NULL,
    [noxious_comments]              VARCHAR (100)   NULL,
    [noxious_percent]               NUMERIC (8, 7)  NULL,
    [noxious_count]                 INT             NULL,
    [purity_grams]                  NUMERIC (7, 2)  NULL,
    [bushel_weight]                 NUMERIC (7, 2)  NULL,
    [ccia_germ]                     BIT             NULL,
    [germ_hard_seed]                NUMERIC (8, 5)  NULL,
    [assay_test]                    BIT             CONSTRAINT [DF_sx_lab_results_assay_test] DEFAULT ((0)) NOT NULL,
    [assay_results]                 CHAR (1)        NULL,
    [assay_test2]                   BIT             CONSTRAINT [DF_sx_lab_results_assay_test2] DEFAULT ((0)) NOT NULL,
    [assay_results2]                CHAR (1)        NULL,
    [dodder_grams]                  NUMERIC (7, 2)  NULL,
    [lbs_canceled]                  NUMERIC (16, 2) NULL,
    [lbs_passed]                    NUMERIC (16, 2) NULL,
    [lbs_rejected]                  NUMERIC (16, 2) NULL,
    [data_entry_date]               DATETIME        NULL,
    [data_entry_user]               VARCHAR (50)    NULL,
    [update_date]                   DATETIME        NULL,
    [update_user]                   VARCHAR (50)    NULL,
    [private_lab_date]              DATETIME        NULL,
    [private_lab_name]              VARCHAR (50)    NULL,
    [private_lab_id]                INT             NULL,
    [private_lab_number]            VARCHAR (500)   NULL,
    [ccia_confirmed]                BIT             CONSTRAINT [DF_sx_lab_results_ccia_confirmed] DEFAULT ((0)) NOT NULL,
    [confirm_date]                  DATETIME        NULL,
    [confirm_user]                  VARCHAR (50)    NULL,
    [badly_discolored_percent]      NUMERIC (8, 7)  NULL,
    [foreign_material_percent]      NUMERIC (8, 7)  NULL,
    [foreign_materials_comments]    VARCHAR (100)   NULL,
    [splits_and_cracks_percent]     NUMERIC (8, 7)  NULL,
    [chewing_insect_damage_percent] NUMERIC (8, 7)  NULL,
    [other_kind_percent]            NUMERIC (8, 7)  NULL,
    [other_kind_comments]           VARCHAR (100)   NULL,
    CONSTRAINT [PK_sx_lab_results] PRIMARY KEY NONCLUSTERED ([seeds_id] ASC)
);




GO
CREATE TRIGGER [dbo].[updtSxLabResults] ON [dbo].[sx_lab_results]
AFTER UPDATE NOT FOR REPLICATION AS
BEGIN
	SET NOCOUNT ON

	IF UPDATE(date_complete)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Date Complete', deleted.date_complete, inserted.date_complete, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.date_complete,'1/1/1990') <> ISNULL(deleted.date_complete,'1/1/1990') 
			WHERE inserted.update_user IS NOT NULL			
		END	
	IF UPDATE(germ_percent)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Germ Percent', deleted.germ_percent, inserted.germ_percent, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.germ_percent,-1) <> ISNULL(deleted.germ_percent,-1) 
			WHERE inserted.update_user IS NOT NULL			
		END	
	IF UPDATE(germ_days)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Germ Days', deleted.germ_days, inserted.germ_days, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.germ_days,255) <> ISNULL(deleted.germ_days,255) 
			WHERE inserted.update_user IS NOT NULL			
		END
	IF UPDATE(germ_results)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Germ Results', deleted.germ_results, inserted.germ_results, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.germ_results,'Z') <> ISNULL(deleted.germ_results,'Z') 
			WHERE inserted.update_user IS NOT NULL			
		END
	IF UPDATE(germ_temp)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Germ Temp', deleted.germ_temp, inserted.germ_temp, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.germ_temp,'Z') <> ISNULL(deleted.germ_temp,'Z')
			WHERE inserted.update_user IS NOT NULL			
		END
	IF UPDATE(inert_percent)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Inert Percent', deleted.inert_percent, inserted.inert_percent, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.inert_percent,-1) <> ISNULL(deleted.inert_percent,-1) 
			WHERE inserted.update_user IS NOT NULL			
		END
	IF UPDATE(inert_comments)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Inert Comments', deleted.inert_comments, inserted.inert_comments, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.inert_comments,'Z') <> ISNULL(deleted.inert_comments,'Z') 
			WHERE inserted.update_user IS NOT NULL			
		END
	IF UPDATE(other_variety_percent)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Other Variety Percent', deleted.other_variety_percent, inserted.other_variety_percent, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.other_variety_percent,-1) <> ISNULL(deleted.other_variety_percent,-1) 
			WHERE inserted.update_user IS NOT NULL			
		END
	IF UPDATE(other_variety_comments)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Other Variety Comments', deleted.other_Variety_comments, inserted.other_Variety_comments, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.other_Variety_comments,'Z') <> ISNULL(deleted.other_Variety_comments,'Z') 
			WHERE inserted.update_user IS NOT NULL			
		END
	IF UPDATE(other_variety_count)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Other Variety Count', deleted.other_variety_count, inserted.other_variety_count, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.other_variety_count,-1) <> ISNULL(deleted.other_variety_count,-1) 
			WHERE inserted.update_user IS NOT NULL			
		END
	IF UPDATE(other_crop_percent)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Other Crop Percent', deleted.other_crop_percent, inserted.other_crop_percent, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.other_crop_percent,-1) <> ISNULL(deleted.other_crop_percent,-1) 
			WHERE inserted.update_user IS NOT NULL			
		END
	IF UPDATE(other_crop_comments)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Other Crop Comments', deleted.other_crop_comments, inserted.other_crop_comments, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.other_crop_comments,'Z') <> ISNULL(deleted.other_crop_comments,'Z') 
			WHERE inserted.update_user IS NOT NULL			
		END
	IF UPDATE(other_crop_count)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Other Crop Count', deleted.other_crop_count, inserted.other_crop_count, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.other_crop_count,-1) <> ISNULL(deleted.other_crop_count,-1) 
			WHERE inserted.update_user IS NOT NULL			
		END
	IF UPDATE(purity_percent)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Purity Percent', deleted.purity_percent, inserted.purity_percent, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.purity_percent,-1) <> ISNULL(deleted.purity_percent,-1) 
			WHERE inserted.update_user IS NOT NULL			
		END
	IF UPDATE(purity_comments)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Purity Comments', deleted.purity_comments, inserted.purity_comments, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.purity_comments,'Z') <> ISNULL(deleted.purity_comments,'Z') 
			WHERE inserted.update_user IS NOT NULL			
		END
	IF UPDATE(purity_results)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Purity Results', deleted.purity_results, inserted.purity_results, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.purity_results,'Z') <> ISNULL(deleted.purity_results,'Z') 
			WHERE inserted.update_user IS NOT NULL	
		End
	IF UPDATE(sample_comments)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Sample Comments', deleted.sample_comments, inserted.sample_comments, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.sample_comments,'Z') <> ISNULL(deleted.sample_comments,'Z') 
			WHERE inserted.update_user IS NOT NULL			
		END
	IF UPDATE(weed_seed_percent)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Weed Seed Percent', deleted.weed_seed_percent, inserted.weed_seed_percent, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.weed_seed_percent,-1) <> ISNULL(deleted.weed_seed_percent,-1) 
			WHERE inserted.update_user IS NOT NULL			
		END
	IF UPDATE(weed_seed_comments)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Weed Seed Comments', deleted.weed_seed_comments, inserted.weed_seed_comments, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.weed_seed_comments,'Z') <> ISNULL(deleted.weed_seed_comments,'Z') 
			WHERE inserted.update_user IS NOT NULL			
		END
	IF UPDATE(weed_seed_count)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Weed Seed Count', deleted.weed_seed_count, inserted.weed_seed_count, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.weed_seed_count,-1) <> ISNULL(deleted.weed_seed_count,-1) 
			WHERE inserted.update_user IS NOT NULL			
		END
	IF UPDATE(noxious_grams)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Noxious Grams', deleted.noxious_grams, inserted.noxious_grams, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.noxious_grams,-1) <> ISNULL(deleted.noxious_grams,-1) 
			WHERE inserted.update_user IS NOT NULL			
		END
	IF UPDATE(purity_grams)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Purity Grams', deleted.purity_grams, inserted.purity_grams, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.purity_grams,-1) <> ISNULL(deleted.purity_grams,-1) 
			WHERE inserted.update_user IS NOT NULL			
		END
	IF UPDATE(germ_hard_seed)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Germ Hard Seed', deleted.germ_hard_seed, inserted.germ_hard_seed, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.germ_hard_seed,-1) <> ISNULL(deleted.germ_hard_seed,-1) 
			WHERE inserted.update_user IS NOT NULL			
		END
	IF UPDATE(assay_test)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Assay Test', deleted.assay_test, inserted.assay_test, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.assay_test,0) <> ISNULL(deleted.assay_test,0) 
			WHERE inserted.update_user IS NOT NULL			
		END
	IF UPDATE(assay_results)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Assay Result', deleted.assay_results, inserted.assay_results, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.assay_results,'Z') <> ISNULL(deleted.assay_results,'Z') 
			WHERE inserted.update_user IS NOT NULL			
		END

		IF UPDATE(assay_test2)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Assay Test2', deleted.assay_test2, inserted.assay_test2, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.assay_test2,0) <> ISNULL(deleted.assay_test2,0) 
			WHERE inserted.update_user IS NOT NULL			
		END
	IF UPDATE(assay_results2)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Assay Result2', deleted.assay_results2, inserted.assay_results2, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.assay_results2,'Z') <> ISNULL(deleted.assay_results2,'Z') 
			WHERE inserted.update_user IS NOT NULL			
		END


	IF UPDATE(lbs_canceled)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Pounds Canceled', deleted.lbs_canceled, inserted.lbs_canceled, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.lbs_canceled,-1) <> ISNULL(deleted.lbs_canceled,-1) 
			WHERE inserted.update_user IS NOT NULL			
		END
	IF UPDATE(lbs_passed)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Pounds Passed', deleted.lbs_passed, inserted.lbs_passed, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.lbs_passed,-1) <> ISNULL(deleted.lbs_passed,-1) 
			WHERE inserted.update_user IS NOT NULL			
		END
	IF UPDATE(lbs_rejected)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Pounds Rejected', deleted.lbs_rejected, inserted.lbs_rejected, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.lbs_rejected,-1) <> ISNULL(deleted.lbs_rejected,-1) 
			WHERE inserted.update_user IS NOT NULL			
		END
	IF UPDATE(private_lab_date)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Private Lab Date', deleted.private_lab_date, inserted.private_lab_date, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.private_lab_date,'1/1/1990') <> ISNULL(deleted.private_lab_date,'1/1/1990') 
			WHERE inserted.update_user IS NOT NULL			
		END		
	IF UPDATE(private_lab_name)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Private Lab Name', deleted.private_lab_name, inserted.private_lab_name, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.private_lab_name,'Z') <> ISNULL(deleted.private_lab_name,'Z') 
			WHERE inserted.update_user IS NOT NULL			
		END
	IF UPDATE(private_lab_id)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Private Lab ID', deleted.private_lab_id, inserted.private_lab_id, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.private_lab_id,-1) <> ISNULL(deleted.private_lab_id,-1) 
			WHERE inserted.update_user IS NOT NULL			
		END	
	IF UPDATE(private_lab_number)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Private Lab Test Number', deleted.private_lab_number, inserted.private_lab_number, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.private_lab_number,-1) <> ISNULL(deleted.private_lab_number,-1) 
			WHERE inserted.update_user IS NOT NULL			
		END	
	IF UPDATE(badly_discolored_percent)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Badly Discolored Percent', deleted.badly_discolored_percent, inserted.badly_discolored_percent, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.badly_discolored_percent,-1) <> ISNULL(deleted.badly_discolored_percent,-1) 
			WHERE inserted.update_user IS NOT NULL			
		END
	IF UPDATE(splits_and_cracks_percent)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Splits & Cracks Percent', deleted.splits_and_cracks_percent, inserted.splits_and_cracks_percent, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.splits_and_cracks_percent,-1) <> ISNULL(deleted.splits_and_cracks_percent,-1) 
			WHERE inserted.update_user IS NOT NULL			
		END
	IF UPDATE(foreign_material_percent)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Foreign Material Percent', deleted.foreign_material_percent, inserted.foreign_material_percent, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.foreign_material_percent,-1) <> ISNULL(deleted.foreign_material_percent,-1) 
			WHERE inserted.update_user IS NOT NULL			
		END
	IF UPDATE(chewing_insect_damage_percent)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Chewing Insect Damage Percent', deleted.chewing_insect_damage_percent, inserted.chewing_insect_damage_percent, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.chewing_insect_damage_percent,-1) <> ISNULL(deleted.chewing_insect_damage_percent,-1) 
			WHERE inserted.update_user IS NOT NULL			
		END
	IF UPDATE(foreign_materials_comments)
		BEGIN
			INSERT INTO sx_lab_results_changes (seeds_id, column_change, old_value, new_value, user_change, date_change) 
			SELECT inserted.seeds_id, 'Foreign Material Comments', deleted.foreign_materials_comments, inserted.foreign_materials_comments, inserted.update_user, GETDATE()
			FROM inserted JOIN deleted ON inserted.seeds_id = deleted.seeds_id AND ISNULL(inserted.foreign_materials_comments,'Z') <> ISNULL(deleted.foreign_materials_comments,'Z') 
			WHERE inserted.update_user IS NOT NULL			
		END
END