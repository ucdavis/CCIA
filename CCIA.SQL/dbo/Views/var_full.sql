CREATE VIEW dbo.var_full
AS
SELECT        var_off_id AS id, var_off_name AS var_name, 'official' AS var_type, crop_id, var_category, var_status, 'official' AS tblname, ccia_certified, ccia_certified_date AS date_certified, rice_qa, rice_qa_color, 
                         var_off_id AS parent_id, turfgrass, CASE WHEN var_off_id IN (SELECT distinct var_off_id FROM blend_components) THEN cast(1 as bit) ELSE cast(0 as bit) END as blend,
						 county_harvested, ecoregion, elevation, number_of_generations_permitted
FROM            dbo.var_official
UNION
SELECT        dbo.var_family.var_fam_id AS id, dbo.var_family.var_fam_name AS var_name, dbo.var_family.variety_type AS var_type, var_official.crop_id, var_official.var_category, var_official.var_status, 
                         'family' AS tblname, var_official.ccia_certified, ccia_certified_date AS date_certified, rice_qa, rice_qa_color, var_family.var_off_id AS parent_id, turfgrass,
						 CASE WHEN var_family.var_off_id IN  (SELECT distinct var_off_id FROM blend_components) THEN cast(1 as bit) ELSE cast(0 as bit) END as blend,
						 var_official.county_harvested, var_official.ecoregion, var_official.elevation, var_official.number_of_generations_permitted
FROM            dbo.var_family INNER JOIN
                         dbo.var_official AS var_official ON dbo.var_family.var_off_id = var_official.var_off_id
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'var_full';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4[30] 2[40] 3) )"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 3
   End
   Begin DiagramPane = 
      PaneHidden = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'var_full';

