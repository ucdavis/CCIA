CREATE TABLE [dbo].[lot_blends] (
    [comp_id]  INT             IDENTITY (1, 1) NOT NULL,
    [blend_id] INT             NOT NULL,
    [sid]      INT             NOT NULL,
    [weight]   NUMERIC (16, 2) NOT NULL,
    CONSTRAINT [PK_lot_blends] PRIMARY KEY CLUSTERED ([comp_id] ASC)
);

