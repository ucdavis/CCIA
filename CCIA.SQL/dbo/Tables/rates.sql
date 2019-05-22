CREATE TABLE [dbo].[rates] (
    [rate_id]  BIGINT        IDENTITY (1, 1) NOT NULL,
    [type]     VARCHAR (50)  NOT NULL,
    [item]     VARCHAR (150) NULL,
    [cost]     MONEY         NULL,
    [unit]     VARCHAR (25)  NULL,
    [active]   BIT           NOT NULL,
    [modified] DATETIME      NULL,
    [comments] VARCHAR (255) NULL
);




GO


