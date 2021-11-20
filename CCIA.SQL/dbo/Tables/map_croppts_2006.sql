CREATE TABLE [dbo].[map_croppts_2006] (
    [company]       VARCHAR (50) NULL,
    [type]          VARCHAR (50) NULL,
    [date_plant]    DATETIME     NULL,
    [crop]          VARCHAR (50) NULL,
    [genus]         VARCHAR (50) NULL,
    [variety]       VARCHAR (50) NULL,
    [date_entered]  DATETIME     NULL,
    [status]        VARCHAR (10) NULL,
    [xcoord]        FLOAT (53)   NULL,
    [ycoord]        FLOAT (53)   NULL,
    [acres]         FLOAT (53)   NULL,
    [crop_pk]       INT          IDENTITY (1, 1) NOT NULL,
    [loginid]       VARCHAR (50) NULL,
    [date_inactive] DATETIME     NULL,
    CONSTRAINT [PK_map_croppts_2006_1__18] PRIMARY KEY CLUSTERED ([crop_pk] ASC) WITH (FILLFACTOR = 1)
);



