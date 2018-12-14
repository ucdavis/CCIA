CREATE TABLE [dbo].[po_cert_history] (
    [app_id]     INT          NOT NULL,
    [prod_year]  SMALLINT     NOT NULL,
    [greenhouse] VARCHAR (50) NULL,
    [field]      VARCHAR (50) NULL,
    [cert_no]    VARCHAR (50) NULL,
    [cert_state] VARCHAR (50) NULL
);

