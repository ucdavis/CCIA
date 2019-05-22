CREATE TABLE [dbo].[var_family] (
    [var_fam_id]       INT            NOT NULL,
    [var_fam_name]     VARCHAR (100)  NOT NULL,
    [var_off_id]       INT            NULL,
    [orig_official_id] INT            NULL,
    [variety_type]     VARCHAR (50)   NULL,
    [in_use]           BIT            CONSTRAINT [DF_Var_Family_in_use] DEFAULT ((1)) NOT NULL,
    [oecd_country]     INT            NULL,
    [confidential]     BIT            NULL,
    [var_comments]     VARCHAR (1000) NULL,
    [update_comments]  VARCHAR (1000) NULL,
    [experimental]     BIT            NULL,
    [private_code]     BIT            CONSTRAINT [DF_var_family_private_code] DEFAULT ((0)) NOT NULL,
    [oecd]             BIT            NULL,
    [alias]            BIT            NULL,
    [date_entered]     DATETIME       NULL,
    [user_entered]     VARCHAR (50)   NULL,
    [date_updated]     DATETIME       NULL,
    [user_updated]     VARCHAR (50)   NULL,
    [desc_hyperlink]   VARCHAR (500)  NULL,
    CONSTRAINT [PK_Var_Family] PRIMARY KEY CLUSTERED ([var_fam_id] ASC),
    CONSTRAINT [FK_Var_Family_Var_Official] FOREIGN KEY ([var_off_id]) REFERENCES [dbo].[var_official] ([var_off_id])
);


GO
ALTER TABLE [dbo].[var_family] NOCHECK CONSTRAINT [FK_Var_Family_Var_Official];




GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'alias', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'var_family', @level2type = N'COLUMN', @level2name = N'variety_type';

