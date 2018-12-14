CREATE TABLE [dbo].[change_requests] (
    [change_id]      INT            IDENTITY (1, 1) NOT NULL,
    [title]          VARCHAR (50)   NULL,
    [text]           VARCHAR (5000) NULL,
    [priority]       TINYINT        NULL,
    [submit_date]    DATETIME       NULL,
    [completed]      BIT            CONSTRAINT [DF_change_requests_completed] DEFAULT ((0)) NULL,
    [completed_date] DATETIME       NULL,
    CONSTRAINT [PK_change_requests] PRIMARY KEY NONCLUSTERED ([change_id] ASC)
);

