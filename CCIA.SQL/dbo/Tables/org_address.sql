CREATE TABLE [dbo].[org_address] (
    [org_id]        SMALLINT     NOT NULL,
    [address_id]    INT          NOT NULL,
    [mailing]       BIT          CONSTRAINT [DF_Org_Address_mailing] DEFAULT ((0)) NOT NULL,
    [billing]       BIT          CONSTRAINT [DF_Org_Address_billing] DEFAULT ((0)) NOT NULL,
    [delivery]      BIT          CONSTRAINT [DF_Org_Address_delivery] DEFAULT ((0)) NOT NULL,
    [physical_loc]  BIT          CONSTRAINT [DF_Org_Address_physical] DEFAULT ((0)) NOT NULL,
    [user_modified] VARCHAR (50) NULL,
    [date_modified] DATETIME     NULL,
    [active]        BIT          CONSTRAINT [DF_Org_Address_active] DEFAULT ((1)) NOT NULL
);

