CREATE TABLE [dbo].[cond_status] (
    [cond_status_id]            INT         IDENTITY (1, 1) NOT NULL,
    [org_id]                    INT         NOT NULL,
    [cond_year]                 SMALLINT    NOT NULL,
    [cond_status]               VARCHAR (2) NOT NULL,
    [cond_update]               DATETIME    NOT NULL,
    [allow_pretag]              BIT         CONSTRAINT [DF_cond_status_allow_pretag] DEFAULT ((0)) NOT NULL,
    [print_series]              BIT         CONSTRAINT [DF_cond_status_print_series] DEFAULT ((0)) NOT NULL,
    [request_ccia_print_series] BIT         CONSTRAINT [DF_cond_status_request_ccia_print_series] DEFAULT ((0)) NOT NULL,
    [date_pretag_approved]      DATETIME    NULL,
    CONSTRAINT [PK_cond_status_1] PRIMARY KEY CLUSTERED ([cond_status_id] ASC)
);

