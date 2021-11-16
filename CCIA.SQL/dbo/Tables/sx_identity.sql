CREATE TABLE [dbo].[sx_identity] (
    [sx_indentity_id] INT          NOT NULL,
    [sx_type]         VARCHAR (25) NULL,
    [form_no]         INT          NULL,
    [test_date]       DATETIME     NULL,
    [laboratory]      INT          NULL,
    CONSTRAINT [PK_sx_identity] PRIMARY KEY CLUSTERED ([sx_indentity_id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'field_results.fld_res_id or seeds.seed_id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'sx_identity', @level2type = N'COLUMN', @level2name = N'form_no';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'e.g.: seed, field', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'sx_identity', @level2type = N'COLUMN', @level2name = N'sx_type';

