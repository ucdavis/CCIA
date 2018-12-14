CREATE TABLE [dbo].[rates] (
    [rate_id]  BIGINT        IDENTITY (101, 1) NOT NULL,
    [type]     VARCHAR (50)  NOT NULL,
    [item]     VARCHAR (150) NULL,
    [cost]     MONEY         NULL,
    [unit]     VARCHAR (25)  NULL,
    [active]   BIT           CONSTRAINT [DF_rates_active_1] DEFAULT ((0)) NOT NULL,
    [modified] DATETIME      NULL,
    [comments] VARCHAR (255) NULL,
    CONSTRAINT [PK__rates__74794A92] PRIMARY KEY NONCLUSTERED ([rate_id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Is rate currently in use?', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'rates', @level2type = N'COLUMN', @level2name = N'active';

