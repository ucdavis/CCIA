CREATE TABLE [dbo].[app_errors] (
    [error_id]       INT            NOT NULL,
    [app_id]         INT            NULL,
    [error_desc]     VARCHAR (100)  NOT NULL,
    [error_comments] VARCHAR (1000) NULL,
    [error_fixed]    BIT            CONSTRAINT [DF_app_errors_error_fixed] DEFAULT ((0)) NOT NULL,
    [date_initiated] DATETIME       NULL,
    [date_cleared]   DATETIME       NULL,
    [user_cleared]   VARCHAR (50)   NULL
);

