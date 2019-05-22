CREATE TABLE [dbo].[renew_fields] (
    [counter]             INT            IDENTITY (1, 1) NOT NULL,
    [app_num]             INT            NOT NULL,
    [renew_year]          INT            NOT NULL,
    [field_name]          VARCHAR (50)   NULL,
    [acres]               NUMERIC (7, 3) NOT NULL,
    [class_produced_char] CHAR (10)      NOT NULL,
    [gen_planted_char]    CHAR (10)      NOT NULL,
    [class_produced]      INT            NULL,
    [gen_planted]         INT            NULL,
    [renew_action]        TINYINT        CONSTRAINT [DF_renew_fields_renew_action] DEFAULT ((0)) NOT NULL,
    [county_id]           INT            NULL,
    [action_date]         DATETIME       NULL
);



