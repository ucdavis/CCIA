CREATE TABLE [dbo].[board_of_dir] (
    [org_id]           INT          NOT NULL,
    [contact_id]       INT          NOT NULL,
    [status]           CHAR (1)     NULL,
    [dal]              BIT          CONSTRAINT [DF_board_of_dir_dal] DEFAULT ((0)) NOT NULL,
    [adal]             BIT          CONSTRAINT [DF_board_of_dir_adal] DEFAULT ((0)) NOT NULL,
    [president]        BIT          CONSTRAINT [DF_board_of_dir_president] DEFAULT ((0)) NOT NULL,
    [active_board_mem] BIT          CONSTRAINT [DF_board_of_dir_active_board_mem] DEFAULT ((0)) NOT NULL,
    [active]           BIT          CONSTRAINT [DF_board_of_dir_active] DEFAULT ((0)) NOT NULL,
    [user_modified]    VARCHAR (50) NULL,
    [date_modified]    DATETIME     NULL,
    CONSTRAINT [PK_board_of_dir] PRIMARY KEY CLUSTERED ([org_id] ASC, [contact_id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'associate director at large', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'board_of_dir', @level2type = N'COLUMN', @level2name = N'adal';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Director At Large', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'board_of_dir', @level2type = N'COLUMN', @level2name = N'dal';

