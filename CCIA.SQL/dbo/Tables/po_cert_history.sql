CREATE TABLE [dbo].[po_cert_history] (
    [id]         INT          IDENTITY (1, 1) NOT NULL,
    [app_id]     INT          NOT NULL,
    [prod_year]  INT          NOT NULL,
    [greenhouse] VARCHAR (50) NULL,
    [field]      VARCHAR (50) NULL,
    [cert_no]    VARCHAR (50) NULL,
    [cert_state] VARCHAR (50) NULL
);



