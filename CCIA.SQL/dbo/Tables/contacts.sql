CREATE TABLE [dbo].[contacts] (
    [contact_id]                          INT           IDENTITY (1, 1) NOT NULL,
    [org_id]                              INT           NULL,
    [title]                               VARCHAR (50)  NULL,
    [form_of_addr]                        VARCHAR (25)  NULL,
    [first_name]                          VARCHAR (50)  NOT NULL,
    [mi]                                  CHAR (1)      NULL,
    [last_name]                           VARCHAR (50)  NOT NULL,
    [suffix]                              VARCHAR (50)  NULL,
    [bus_phone]                           VARCHAR (30)  NULL,
    [bus_phone_ext]                       VARCHAR (50)  NULL,
    [mobile_phone]                        VARCHAR (30)  NULL,
    [fax_no]                              VARCHAR (30)  NULL,
    [home_phone]                          VARCHAR (30)  NULL,
    [pager_no]                            VARCHAR (30)  NULL,
    [email_addr]                          VARCHAR (100) NULL,
    [password]                            VARCHAR (50)  NULL,
    [contact_type]                        VARCHAR (50)  NULL,
    [ccia_member]                         BIT           CONSTRAINT [DF_contacts_ccia_member] DEFAULT ((0)) NOT NULL,
    [ccia_member_year]                    INT           NULL,
    [board_member]                        BIT           CONSTRAINT [DF_Contacts_board_member] DEFAULT ((0)) NOT NULL,
    [board_title]                         VARCHAR (50)  NULL,
    [board_represent]                     VARCHAR (50)  NULL,
    [board_active]                        BIT           CONSTRAINT [DF_contacts_board_active] DEFAULT ((0)) NOT NULL,
    [ag_commissioner]                     BIT           CONSTRAINT [DF_Contacts_ag_comm] DEFAULT ((0)) NOT NULL,
    [deputy_commissioner]                 BIT           CONSTRAINT [DF_Contacts_deputy_commissioner] DEFAULT ((0)) NOT NULL,
    [farm_advisor]                        BIT           CONSTRAINT [DF_Contacts_farm_advisor] DEFAULT ((0)) NOT NULL,
    [lab_contact]                         BIT           CONSTRAINT [DF_Contacts_lab_contact] DEFAULT ((0)) NOT NULL,
    [certified_seed_sx]                   BIT           CONSTRAINT [DF_Contacts_certified_seed_sx] DEFAULT ((0)) NOT NULL,
    [certified_seed_sx_no]                VARCHAR (50)  NULL,
    [mail_list_gr_book]                   BIT           CONSTRAINT [DF_Contacts_mail_list_gr_book] DEFAULT ((0)) NOT NULL,
    [mail_list_seednotes]                 BIT           CONSTRAINT [DF_Contacts_mail_list_seednotes] DEFAULT ((0)) NOT NULL,
    [active]                              BIT           CONSTRAINT [DF_Contacts_active] DEFAULT ((0)) NOT NULL,
    [member_since]                        DATETIME      NULL,
    [create_apps]                         BIT           CONSTRAINT [DF_Contacts_create_apps] DEFAULT ((0)) NOT NULL,
    [date_added]                          DATETIME      NULL,
    [user_adding]                         INT           NULL,
    [user_modified]                       INT           NULL,
    [date_modified]                       DATETIME      NULL,
    [current_year_review]                 BIT           CONSTRAINT [DF_Contacts_contact_updated] DEFAULT ((0)) NOT NULL,
    [user_emp_modified]                   VARCHAR (9)   NULL,
    [user_emp_mod_dt]                     DATETIME      NULL,
    [comments]                            VARCHAR (500) NULL,
    [allow_pinning]                       BIT           CONSTRAINT [DF_contacts_allow_pinning] DEFAULT ((0)) NOT NULL,
    [allow_apps]                          BIT           CONSTRAINT [DF_contacts_allow_apps] DEFAULT ((0)) NOT NULL,
    [allow_seeds]                         BIT           CONSTRAINT [DF_contacts_allow_seeds] DEFAULT ((0)) NOT NULL,
    [audit_notify]                        BIT           CONSTRAINT [DF_contacts_audit_notify] DEFAULT ((0)) NOT NULL,
    [alfalfa_last_year_agreement]         INT           NULL,
    [sweet_corn_last_year_agreement]      INT           NULL,
    [idaho_vegetable_last_year_agreement] INT           NULL,
    CONSTRAINT [PK_Contacts] PRIMARY KEY CLUSTERED ([contact_id] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'updated this year', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'contacts', @level2type = N'COLUMN', @level2name = N'current_year_review';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'contact id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'contacts', @level2type = N'COLUMN', @level2name = N'user_modified';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'contact id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'contacts', @level2type = N'COLUMN', @level2name = N'user_adding';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'contacts', @level2type = N'COLUMN', @level2name = N'contact_type';

