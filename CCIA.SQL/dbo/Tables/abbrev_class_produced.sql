CREATE TABLE [dbo].[abbrev_class_produced] (
    [class_produced_id]    INT           IDENTITY (1, 1) NOT NULL,
    [class_abbrv]          VARCHAR (3)   NULL,
    [class_produced_trans] VARCHAR (100) NULL,
    [sort_order]           INT           NULL,
    [app_type]             INT           NULL,
    [hemp_producing]       VARCHAR (50)  NULL,
    [hemp_planted]         BIT           CONSTRAINT [DF_abbrev_class_produced_hemp_planted_1] DEFAULT ((0)) NULL,
    [hemp_produced]        BIT           CONSTRAINT [DF_abbrev_class_produced_hemp_produced_1] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_abbrev_class_produced] PRIMARY KEY CLUSTERED ([class_produced_id] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Potato (PN;N;G1;G2;G3;G4;G5) Seed(Breeder;Found.;Reg.;Cert.)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'abbrev_class_produced', @level2type = N'COLUMN', @level2name = N'class_produced_trans';

