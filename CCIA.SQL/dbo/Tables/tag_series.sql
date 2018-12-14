CREATE TABLE [dbo].[tag_series] (
    [seriesId]    INT         IDENTITY (1, 1) NOT NULL,
    [tag_id]      INT         NOT NULL,
    [letter]      VARCHAR (4) NOT NULL,
    [start_value] INT         NOT NULL,
    [end_value]   INT         NOT NULL,
    [void]        BIT         CONSTRAINT [DF_tag_series_void] DEFAULT ((0)) NOT NULL,
    [row_count]   AS          (([end_value]-[start_value])+(1))
);

