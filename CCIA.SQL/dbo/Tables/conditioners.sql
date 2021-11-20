CREATE TABLE [dbo].[conditioners] (
    [org_id]              INT           NOT NULL,
    [cond_status]         CHAR (2)      NULL,
    [cond_last_year]      SMALLINT      NULL,
    [cond_first_year]     SMALLINT      NULL,
    [cond_draw_sample]    BIT           NULL,
    [cond_monitor_self]   BIT           NULL,
    [cond_print_oecd_tag] BIT           NULL,
    [cond_print_ccia_tag] BIT           NULL,
    [cond_receive_tags]   BIT           NULL,
    [comments]            VARCHAR (500) NULL,
    [cond_receive_sp]     BIT           NULL,
    [date_update]         DATETIME      NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'was varchar(50)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'conditioners', @level2type = N'COLUMN', @level2name = N'cond_status';

