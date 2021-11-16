CREATE TABLE [dbo].[organization] (
    [org_id]                     INT              IDENTITY (1, 1) NOT NULL,
    [org_name]                   NVARCHAR (250)   NOT NULL,
    [org_type]                   VARCHAR (50)     NULL,
    [address_id]                 INT              NULL,
    [county_id]                  INT              NULL,
    [main_phone]                 VARCHAR (15)     NULL,
    [main_email]                 VARCHAR (100)    NULL,
    [main_fax]                   VARCHAR (15)     NULL,
    [website]                    VARCHAR (100)    NULL,
    [conditioner]                BIT              CONSTRAINT [DF_Organization_conditioner] DEFAULT ((0)) NOT NULL,
    [conditioner_status]         VARCHAR (2)      CONSTRAINT [DF_organization_conditioner_status] DEFAULT ('O') NOT NULL,
    [conditioner_update]         DATETIME         NULL,
    [allow_pretag]               BIT              CONSTRAINT [DF_organization_allow_pretag] DEFAULT ((0)) NOT NULL,
    [print_series_tags]          BIT              CONSTRAINT [DF_organization_print_series_tags] DEFAULT ((0)) NOT NULL,
    [date_pretag_approved]       DATETIME         NULL,
    [foundation_seed_contractor] BIT              CONSTRAINT [DF_Organization_foundation_seed_contractor] DEFAULT ((0)) NOT NULL,
    [foundation_seed_grower]     BIT              CONSTRAINT [DF_Organization_foundation_seed_grower] DEFAULT ((0)) NOT NULL,
    [diagnostic_lab]             BIT              CONSTRAINT [DF_Organization_diagnostic_lab] DEFAULT ((0)) NOT NULL,
    [diagnostic_lab_init_year]   DATETIME         NULL,
    [germination_lab]            BIT              CONSTRAINT [DF_Organization_germination_lab] DEFAULT ((0)) NOT NULL,
    [aosca_member]               BIT              CONSTRAINT [DF_Organization_aosca_member] DEFAULT ((0)) NOT NULL,
    [ag_comm_off]                BIT              CONSTRAINT [DF_Organization_ag_comm] DEFAULT ((0)) NOT NULL,
    [grower]                     BIT              CONSTRAINT [DF_Organization_grower] DEFAULT ((0)) NOT NULL,
    [oecd_shipper]               BIT              CONSTRAINT [DF_Organization_oecd] DEFAULT ((0)) NOT NULL,
    [district]                   VARCHAR (5)      NULL,
    [ccia_member]                BIT              CONSTRAINT [DF_organization_ccia_member] DEFAULT ((0)) NOT NULL,
    [member_year]                INT              NULL,
    [member_type]                VARCHAR (50)     NULL,
    [last_member_agreement]      DATETIME         NULL,
    [member_since]               DATETIME         NULL,
    [member_rep_id]              INT              NULL,
    [agreement_year]             INT              NULL,
    [active]                     BIT              CONSTRAINT [DF_Organization_active] DEFAULT ((0)) NOT NULL,
    [contact_type]               VARCHAR (50)     NULL,
    [user_modified]              VARCHAR (50)     NULL,
    [date_modified]              DATETIME         NULL,
    [keywords]                   VARCHAR (1000)   NULL,
    [notes]                      VARCHAR (500)    NULL,
    [password]                   VARCHAR (50)     NULL,
    [app_agree_accept]           INT              NULL,
    [allow_alfalfa_gmo_pinning]  BIT              CONSTRAINT [DF_organization_allow_alfalfa_gmo_pinning] DEFAULT ((0)) NOT NULL,
    [ApiKey]                     UNIQUEIDENTIFIER CONSTRAINT [DF_organization_ApiKey_1] DEFAULT (newid()) NOT NULL,
    [lac_agree_accept]           INT              CONSTRAINT [DF_organization_lac_agree_accept] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_organization] PRIMARY KEY CLUSTERED ([org_id] ASC)
);












GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'e.g.: OECD Shipper, Reg. Seed Tech', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'organization', @level2type = N'COLUMN', @level2name = N'notes';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'e.g.: OECD Shipper', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'organization', @level2type = N'COLUMN', @level2name = N'contact_type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'e.g.: I-VIII', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'organization', @level2type = N'COLUMN', @level2name = N'district';


GO


