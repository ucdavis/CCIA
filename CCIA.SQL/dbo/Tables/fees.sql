CREATE TABLE [dbo].[fees] (
    [fee_id]        INT            NOT NULL,
    [item]          VARCHAR (150)  NULL,
    [fee_amount]    MONEY          NOT NULL,
    [unit]          DECIMAL (8, 2) NULL,
    [fee_category]  VARCHAR (50)   NULL,
    [active]        BIT            CONSTRAINT [DF_rates_active] DEFAULT ((0)) NOT NULL,
    [user_modified] VARCHAR (50)   NULL,
    [date_modified] DATETIME       NULL,
    CONSTRAINT [PK_Fees] PRIMARY KEY CLUSTERED ([fee_id] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'application, inspection, laboratory', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'fees', @level2type = N'COLUMN', @level2name = N'fee_category';

