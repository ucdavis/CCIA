﻿CREATE TABLE [dbo].[field_history] (
    [history_id]      INT          IDENTITY (1, 1) NOT NULL,
    [app_id]          INT          NOT NULL,
    [year]            SMALLINT     NOT NULL,
    [crop]            INT          NULL,
    [entered_variety] VARCHAR (50) NULL,
    [app_num]         VARCHAR (50) CONSTRAINT [DF_field_history_app_num] DEFAULT ('') NULL
);

