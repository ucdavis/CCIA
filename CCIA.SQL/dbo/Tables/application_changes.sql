﻿CREATE TABLE [dbo].[application_changes] (
    [id]            INT          IDENTITY (1, 1) NOT NULL,
    [app_id]        INT          NOT NULL,
    [column_change] VARCHAR (50) NULL,
    [old_value]     VARCHAR (50) NULL,
    [new_value]     VARCHAR (50) NULL,
    [user_change]   VARCHAR (50) NULL,
    [date_change]   DATETIME     NULL
);



