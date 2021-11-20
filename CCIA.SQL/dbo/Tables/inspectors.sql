CREATE TABLE [dbo].[inspectors] (
    [fld_insp_id]     INT          NOT NULL,
    [inspection_type] VARCHAR (50) NULL,
    [fld_tbl_id]      INT          NULL,
    [inspector]       VARCHAR (9)  NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'field_inspect table or field_results table', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'inspectors', @level2type = N'COLUMN', @level2name = N'inspection_type';

