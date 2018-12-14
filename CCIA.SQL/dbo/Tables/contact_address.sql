CREATE TABLE [dbo].[contact_address] (
    [contadd_id]    INT          IDENTITY (1, 1) NOT NULL,
    [contact_id]    INT          NOT NULL,
    [address_id]    INT          NOT NULL,
    [mailing]       BIT          CONSTRAINT [DF_Contact_Address_mailing] DEFAULT ((0)) NOT NULL,
    [billing]       BIT          CONSTRAINT [DF_Contact_Address_billing] DEFAULT ((0)) NOT NULL,
    [delivery]      BIT          CONSTRAINT [DF_Contact_Address_delivery] DEFAULT ((0)) NOT NULL,
    [physical_loc]  BIT          CONSTRAINT [DF_Contact_Address_physical_loc] DEFAULT ((0)) NOT NULL,
    [user_modified] VARCHAR (50) NULL,
    [date_modified] DATETIME     NULL,
    [active]        BIT          CONSTRAINT [DF_Contact_Address_active] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_contact_address] PRIMARY KEY CLUSTERED ([contact_id] ASC, [address_id] ASC)
);

