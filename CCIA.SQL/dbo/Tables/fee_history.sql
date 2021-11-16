CREATE TABLE [dbo].[fee_history] (
    [rate_id]      INT      NOT NULL,
    [rate_amount]  MONEY    NOT NULL,
    [date_int]     DATETIME NOT NULL,
    [date_retired] DATETIME NOT NULL
);

