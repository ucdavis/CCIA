CREATE TABLE [dbo].[map_croppts_app] (
    [croppt_id]         INT               IDENTITY (1, 1) NOT NULL,
    [app_id]            INT               NOT NULL,
    [cert_year]         SMALLINT          NOT NULL,
    [org_id]            INT               NOT NULL,
    [contact_id]        INT               NOT NULL,
    [cultivation]       VARCHAR (50)      NULL,
    [alfalfa_crop]      VARCHAR (50)      NULL,
    [type]              VARCHAR (50)      NOT NULL,
    [class]             VARCHAR (50)      NULL,
    [date_planted]      DATE              NULL,
    [crop_id]           INT               NOT NULL,
    [variety]           VARCHAR (50)      NOT NULL,
    [date_entered]      DATETIME          NOT NULL,
    [date_last_action]  DATETIME          NOT NULL,
    [status]            VARCHAR (50)      NOT NULL,
    [field]             [sys].[geography] NOT NULL,
    [acres]             DECIMAL (7, 2)    NULL,
    [date_inactive]     DATETIME          NULL,
    [charged]           BIT               CONSTRAINT [DF_map_croppts_app_charged] DEFAULT ((0)) NOT NULL,
    [deleted]           BIT               CONSTRAINT [DF_map_croppts_app_deleted] DEFAULT ((0)) NOT NULL,
    [last_harvest_year] SMALLINT          NULL,
    [comments]          VARCHAR (5000)    NULL,
    [renewed]           BIT               CONSTRAINT [DF_map_croppts_app_renewed] DEFAULT ((0)) NOT NULL,
    [orig_pin_year]     SMALLINT          NULL,
    [previous_pin]      INT               NULL
);



