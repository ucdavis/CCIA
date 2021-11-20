CREATE TABLE [dbo].[samples] (
    [sx_form_id]      INT      NOT NULL,
    [sx_form_no]      INT      NULL,
    [applicant_id]    INT      NULL,
    [date_submitted]  DATETIME NULL,
    [app_id]          INT      NULL,
    [cert_num]        INT      NULL,
    [state_of_origin] INT      NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'pnts to org_id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'samples', @level2type = N'COLUMN', @level2name = N'date_submitted';

