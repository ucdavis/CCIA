CREATE TABLE [dbo].[cert_rad] (
    [cert_num]  INT NOT NULL,
    [cert_year] INT NOT NULL,
    [rad]       INT NOT NULL,
    CONSTRAINT [PK_cert_rad] PRIMARY KEY NONCLUSTERED ([cert_num] ASC, [cert_year] ASC, [rad] ASC)
);



