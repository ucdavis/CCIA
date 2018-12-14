CREATE TABLE [dbo].[contact_to_org] (
    [cont_org_id] INT          IDENTITY (1, 1) NOT NULL,
    [contact_id]  INT          NULL,
    [org_id]      NCHAR (10)   NULL,
    [comments]    VARCHAR (50) NULL,
    CONSTRAINT [PK_contact_to_org] PRIMARY KEY CLUSTERED ([cont_org_id] ASC)
);

