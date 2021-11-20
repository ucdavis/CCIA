CREATE TABLE [dbo].[AppCertificates] (
    [CertId] INT            IDENTITY (1, 1) NOT NULL,
    [AppId]  INT            NOT NULL,
    [Name]   NVARCHAR (MAX) NULL,
    [Link]   NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_AppCertificates] PRIMARY KEY CLUSTERED ([CertId] ASC),
    CONSTRAINT [FK_AppCertificates_applications_AppId] FOREIGN KEY ([AppId]) REFERENCES [dbo].[applications] ([app_id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_AppCertificates_AppId]
    ON [dbo].[AppCertificates]([AppId] ASC);

