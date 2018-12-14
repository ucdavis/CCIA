CREATE TABLE [dbo].[duplicateOrgs] (
    [_key_in]              INT            NULL,
    [_key_out]             INT            NULL,
    [_score]               REAL           NULL,
    [org_id]               INT            NULL,
    [org_name]             NVARCHAR (250) NULL,
    [org_name_clean]       NVARCHAR (250) NULL,
    [_Similarity_org_name] REAL           NULL
);

