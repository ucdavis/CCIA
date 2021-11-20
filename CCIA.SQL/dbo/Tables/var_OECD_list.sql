CREATE TABLE [dbo].[var_OECD_list] (
    [var_oecd_id]   INT           NOT NULL,
    [oecd_var_name] VARCHAR (100) NOT NULL,
    [var_fam_id]    INT           NULL,
    [var_off_id]    INT           NULL,
    [country_id]    INT           NOT NULL,
    CONSTRAINT [PK_OECD_Var_List] PRIMARY KEY CLUSTERED ([var_oecd_id] ASC),
    CONSTRAINT [FK_OECD_Var_List_Countries] FOREIGN KEY ([country_id]) REFERENCES [dbo].[countries] ([country_id]),
    CONSTRAINT [FK_OECD_Var_List_OECD_Var_Official] FOREIGN KEY ([var_off_id]) REFERENCES [dbo].[var_official] ([var_off_id]),
    CONSTRAINT [FK_OECD_Var_List_Var_Family] FOREIGN KEY ([var_fam_id]) REFERENCES [dbo].[var_family] ([var_fam_id])
);

