CREATE TABLE [dbo].[blend_requests] (
    [blend_id]            INT             IDENTITY (1, 1) NOT NULL,
    [blend_type]          VARCHAR (50)    NOT NULL,
    [request_started]     DATETIME        CONSTRAINT [DF_blend_requests_request_started] DEFAULT (getdate()) NOT NULL,
    [conditioner_id]      INT             NOT NULL,
    [user_entered]        INT             NOT NULL,
    [lbs_lot]             NUMERIC (16, 2) NULL,
    [class]               TINYINT         NULL,
    [status]              VARCHAR (50)    NULL,
    [tag_count_requested] INT             NULL,
    [tag_type]            TINYINT         NULL,
    [variety]             INT             NULL,
    [date_needed]         DATETIME        NULL,
    [how_deliver]         VARCHAR (50)    NULL,
    [delivery_address]    VARCHAR (5000)  NULL,
    [comments]            VARCHAR (5000)  NULL,
    [date_submitted]      DATETIME        NULL,
    [submitted]           BIT             CONSTRAINT [DF_blend_requests_submitted] DEFAULT ((0)) NOT NULL,
    [approved]            BIT             CONSTRAINT [DF_blend_requests_approved] DEFAULT ((0)) NOT NULL,
    [approve_date]        DATETIME        NULL,
    [approved_by]         VARCHAR (9)     NULL,
    CONSTRAINT [PK_blend_requests] PRIMARY KEY CLUSTERED ([blend_id] ASC)
);



