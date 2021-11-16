CREATE TABLE [dbo].[apps_to_certs] (
    [app_id]        INT           NOT NULL,
    [cert_id]       INT           NOT NULL,
    [app_cert_year] CHAR (4)      NOT NULL,
    [comments]      VARCHAR (500) NULL,
    [date_entered]  DATETIME      NULL,
    [user_entered]  VARCHAR (9)   NULL,
    CONSTRAINT [PK_Apps_to_Certs] PRIMARY KEY CLUSTERED ([app_id] ASC, [cert_id] ASC, [app_cert_year] ASC)
);

