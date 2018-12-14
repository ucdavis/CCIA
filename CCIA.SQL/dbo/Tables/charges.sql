CREATE TABLE [dbo].[charges] (
    [chg_id]       INT             IDENTITY (1, 1) NOT NULL,
    [link_id]      INT             NOT NULL,
    [link_type]    VARCHAR (50)    NOT NULL,
    [org_id]       INT             NOT NULL,
    [batchno]      VARCHAR (15)    NULL,
    [chg_category] VARCHAR (50)    NOT NULL,
    [charge_amt]   DECIMAL (10, 2) NULL,
    [description]  VARCHAR (250)   NULL,
    [date_entered] DATETIME        NULL,
    [date_applied] DATETIME        NULL,
    [holdchk]      BIT             CONSTRAINT [DF_charges_holdchk] DEFAULT ((0)) NOT NULL,
    [holdinit]     VARCHAR (50)    NULL,
    [holddt]       DATETIME        NULL,
    [delcharge]    INT             CONSTRAINT [DF_Charges_delcharge] DEFAULT ((0)) NOT NULL,
    [correction]   BIT             CONSTRAINT [DF_Charges_dnr] DEFAULT ((0)) NOT NULL,
    [approval]     BIT             CONSTRAINT [DF_Charges_approval] DEFAULT ((0)) NOT NULL,
    [approver]     VARCHAR (50)    NULL,
    [note]         VARCHAR (250)   NULL,
    CONSTRAINT [PK_Charges] PRIMARY KEY CLUSTERED ([chg_id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'application, field,seeds, etc.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'charges', @level2type = N'COLUMN', @level2name = N'chg_category';

