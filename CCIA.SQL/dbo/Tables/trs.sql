CREATE TABLE [dbo].[trs] (
    [trs_id]       INT          NOT NULL,
    [full_descr]   VARCHAR (50) NULL,
    [state]        VARCHAR (5)  NULL,
    [meridian]     TINYINT      NULL,
    [township]     VARCHAR (10) NULL,
    [t_dir]        VARCHAR (1)  NULL,
    [t_fract]      TINYINT      NULL,
    [range]        VARCHAR (10) NULL,
    [r_dir]        VARCHAR (1)  NULL,
    [r_fract]      TINYINT      NULL,
    [township_dup] TINYINT      NULL,
    [section]      TINYINT      NULL,
    [lat]          FLOAT (53)   NULL,
    [long]         FLOAT (53)   NULL
);

