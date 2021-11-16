CREATE TABLE [dbo].[schedules] (
    [sched_id]      INT            NOT NULL,
    [event_type]    VARCHAR (50)   NULL,
    [event_descr]   VARCHAR (250)  NULL,
    [deadline]      DATETIME       NULL,
    [range]         BIT            CONSTRAINT [DF_Schedules_range] DEFAULT ((0)) NOT NULL,
    [beg_date]      DATETIME       NULL,
    [end_date]      DATETIME       NULL,
    [date_entered]  DATETIME       NULL,
    [user_entereed] VARBINARY (50) NULL,
    [date_modified] DATETIME       NULL,
    [user_modified] VARBINARY (50) NULL,
    CONSTRAINT [PK_Schedules] PRIMARY KEY CLUSTERED ([sched_id] ASC)
);

