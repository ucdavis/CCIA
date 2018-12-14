CREATE TABLE [dbo].[app_certificates] (
    [cert_id]   INT           IDENTITY (1, 1) NOT NULL,
    [app_id]    INT           NOT NULL,
    [cert_name] VARCHAR (100) NULL,
    [cert_link] VARCHAR (100) NOT NULL
);

