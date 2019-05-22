CREATE TABLE [dbo].[planting_stocks] (
    [ps_id]                              INT             IDENTITY (1, 1) NOT NULL,
    [app_id]                             INT             NULL,
    [ps_cert_num]                        VARCHAR (50)    NULL,
    [ps_entered_variety]                 VARCHAR (50)    NULL,
    [official_variety_id]                INT             NULL,
    [pounds_planted]                     DECIMAL (14, 2) NULL,
    [ps_class]                           INT             NULL,
    [AbbrevClassProducedClassProducedId] INT             NULL,
    [ps_accession]                       TINYINT         NULL,
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
    CONSTRAINT [PK_planting_stocks] PRIMARY KEY CLUSTERED ([ps_id] ASC),
    CONSTRAINT [FK_planting_stocks_abbrev_class_produced_AbbrevClassProducedClassProducedId] FOREIGN KEY ([AbbrevClassProducedClassProducedId]) REFERENCES [dbo].[abbrev_class_produced] ([class_produced_id])
);



