CREATE TABLE [dbo].[certs] (
    [cert_num]            INT         IDENTITY (1, 1) NOT NULL,
    [official_variety_id] INT         NOT NULL,
    [org_id]              INT         NOT NULL,
    [class_produced]      TINYINT     NOT NULL,
    [class_produced_char] VARCHAR (2) NULL,
    [cert_year]           SMALLINT    NULL,
    [date_entered]        DATETIME    NULL,
    [date_modified]       DATETIME    NULL,
    [cert_num_num]        INT         NULL,
    CONSTRAINT [PK_certs_1] PRIMARY KEY CLUSTERED ([cert_num] ASC)
);



