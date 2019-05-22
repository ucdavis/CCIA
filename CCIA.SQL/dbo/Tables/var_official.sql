CREATE TABLE [dbo].[var_official] (
    [var_off_id]            INT            NOT NULL,
    [var_off_name]          VARCHAR (100)  NOT NULL,
    [var_category]          VARCHAR (20)   NULL,
    [var_status]            VARCHAR (50)   NULL,
    [experimental]          BIT            CONSTRAINT [DF_Var_Official_experimental] DEFAULT ((0)) NOT NULL,
    [private_code]          BIT            NULL,
    [oecd]                  BIT            NULL,
    [ccia_certified]        BIT            CONSTRAINT [DF_Var_Official_ccia_certified] DEFAULT ((0)) NOT NULL,
    [ccia_certified_date]   DATETIME       NULL,
    [ccia_certifier]        VARCHAR (50)   NULL,
    [pending_certification] BIT            CONSTRAINT [DF_Var_Official_pending_certification] DEFAULT ((0)) NOT NULL,
    [germplasm_entity]      BIT            CONSTRAINT [DF_var_official_germplasm_entity] DEFAULT ((0)) NOT NULL,
    [description_on_file]   BIT            CONSTRAINT [DF_var_official_description_on_file] DEFAULT ((0)) NULL,
    [crop_id]               INT            NULL,
    [active]                BIT            CONSTRAINT [DF_Var_Official_in_use] DEFAULT ((1)) NOT NULL,
    [reason_for_inactive]   VARCHAR (500)  NULL,
    [historical_name]       VARCHAR (100)  NULL,
    [comments]              VARCHAR (2500) NULL,
    [brief_description]     VARCHAR (1000) NULL,
    [confidential]          BIT            NULL,
    [ctc_approved]          BIT            NULL,
    [ctc_date_approved]     DATETIME       NULL,
    [gen_breeder]           VARCHAR (3)    NULL,
    [gen_certified]         VARCHAR (3)    NULL,
    [gen_foundation]        VARCHAR (3)    NULL,
    [gen_registered]        VARCHAR (3)    NULL,
    [owner_id]              INT            NULL,
    [producer_id]           INT            NULL,
    [plant_patent]          BIT            NULL,
    [plant_patent_num]      INT            NULL,
    [plant_patent_date]     DATETIME       NULL,
    [pvp]                   BIT            NULL,
    [pvp_date]              DATETIME       NULL,
    [pvp_number]            INT            NULL,
    [pvp_exp_date]          DATETIME       NULL,
    [pvp_years]             TINYINT        NULL,
    [title_v]               BIT            NULL,
    [var_review_board]      BIT            NULL,
    [var_review_board_date] DATETIME       NULL,
    [other_state_cert]      VARCHAR (30)   NULL,
    [generation]            VARCHAR (3)    NULL,
    [date_entered]          DATETIME       NULL,
    [user_entered]          VARCHAR (100)  NULL,
    [date_updated]          DATETIME       NULL,
    [user_updated]          VARCHAR (50)   NULL,
    [desc_hyperlink]        VARCHAR (500)  NULL,
    [rice_qa]               BIT            CONSTRAINT [DF_var_official_rice_qa] DEFAULT ((0)) NOT NULL,
    [rice_qa_color]         VARCHAR (50)   NULL,
    [turfgrass]             BIT            CONSTRAINT [DF_var_official_turgrass] DEFAULT ((0)) NOT NULL,
    [ecoregion]             INT            CONSTRAINT [DF_var_official_ecoregion] DEFAULT ((0)) NOT NULL,
    [elevation]             VARCHAR (50)   NULL,
    [county_harvested]      INT            CONSTRAINT [DF_var_official_county_harvested] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Var_Official] PRIMARY KEY CLUSTERED ([var_off_id] ASC),
    CONSTRAINT [FK_Var_Official_Crops] FOREIGN KEY ([crop_id]) REFERENCES [dbo].[crops] ([crop_id])
);




GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Date Certified by the Board', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'var_official', @level2type = N'COLUMN', @level2name = N'ccia_certified_date';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Proprietary; Public: Pending', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'var_official', @level2type = N'COLUMN', @level2name = N'var_category';

