CREATE TABLE [dbo].[notifications] (
    [Id]      INT            IDENTITY (1, 1) NOT NULL,
    [email]   VARCHAR (100)  NULL,
    [appId]   INT            NULL,
    [sid]     INT            NULL,
    [blendId] INT            NULL,
    [tagId]   INT            NULL,
    [orgId]   INT            NULL,
    [oecdId]  INT            NULL,
    [message] VARCHAR (5000) NULL,
    [pending] BIT            CONSTRAINT [DF_notifications_pending] DEFAULT ((1)) NOT NULL,
    [created] DATETIME       NULL,
    [sent]    DATETIME       NULL
);

