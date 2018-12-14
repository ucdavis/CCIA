CREATE TABLE [dbo].[random_samples] (
    [random_id]              INT          IDENTITY (1, 1) NOT NULL,
    [cond_id]                INT          NOT NULL,
    [crop]                   VARCHAR (50) NULL,
    [cert_year]              SMALLINT     NOT NULL,
    [random_number_selected] SMALLINT     NOT NULL
);

