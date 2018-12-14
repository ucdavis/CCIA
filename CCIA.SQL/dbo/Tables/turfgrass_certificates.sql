CREATE TABLE [dbo].[turfgrass_certificates] (
    [certificate_id]  INT          IDENTITY (1, 1) NOT NULL,
    [app_id]          INT          NOT NULL,
    [sprigs]          INT          NULL,
    [sod]             INT          NULL,
    [billing_invoice] VARCHAR (50) NOT NULL,
    [harvest_date]    DATE         NOT NULL,
    [harvest_number]  INT          NOT NULL
);

