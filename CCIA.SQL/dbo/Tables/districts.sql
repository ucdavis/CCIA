CREATE TABLE [dbo].[districts] (
    [dist_id]     INT          IDENTITY (1, 1) NOT NULL,
    [dist_code]   VARCHAR (5)  NULL,
    [dist_name]   VARCHAR (50) NULL,
    [dist_leader] INT          NULL,
    [dist_office] INT          NULL,
    [comments]    VARCHAR (25) NULL,
    CONSTRAINT [PK_Districts] PRIMARY KEY CLUSTERED ([dist_id] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'org_id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'districts', @level2type = N'COLUMN', @level2name = N'dist_office';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'contact id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'districts', @level2type = N'COLUMN', @level2name = N'dist_leader';

