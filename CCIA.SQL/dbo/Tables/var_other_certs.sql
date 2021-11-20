CREATE TABLE [dbo].[var_other_certs] (
    [voc_id]          INT            NOT NULL,
    [voc_types]       VARCHAR (100)  CONSTRAINT [DF_Var_Other_Cert_voc_code] DEFAULT ('unknown') NOT NULL,
    [var_id]          INT            NOT NULL,
    [org_id]          INT            NULL,
    [cert_date]       DATETIME       NULL,
    [cert_info]       VARCHAR (1000) NULL,
    [cert_doc_no]     VARCHAR (250)  NULL,
    [cert_exp_date]   DATETIME       NULL,
    [StateProvinceID] INT            NULL,
    [comments]        VARCHAR (1000) NULL,
    CONSTRAINT [PK_Var_Other_Cert] PRIMARY KEY CLUSTERED ([voc_id] ASC),
    CONSTRAINT [FK_Var_Other_Certs_StateProvince] FOREIGN KEY ([StateProvinceID]) REFERENCES [dbo].[state_province] ([StateProvinceID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codes: NVRB; PVP;CTC; PLANT PATENT; TITLE V;Other State', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'var_other_certs', @level2type = N'COLUMN', @level2name = N'voc_types';

