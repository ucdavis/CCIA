CREATE TABLE [dbo].[organization_address] (
    [id]         INT IDENTITY (1, 1) NOT NULL,
    [org_id]     INT NOT NULL,
    [address_id] INT NOT NULL,
    [mailing]    BIT CONSTRAINT [DF_organization_address_mailing] DEFAULT ((0)) NOT NULL,
    [billing]    BIT CONSTRAINT [DF_organization_address_billing] DEFAULT ((0)) NOT NULL,
    [delivery]   BIT CONSTRAINT [DF_organization_address_delivery] DEFAULT ((0)) NOT NULL,
    [physical]   BIT CONSTRAINT [DF_organization_address_physical] DEFAULT ((0)) NOT NULL,
    [active]     BIT CONSTRAINT [DF_organization_address_active] DEFAULT ((1)) NOT NULL,
    [Facility]   BIT CONSTRAINT [DF_organization_address_Facility] DEFAULT ((0)) NOT NULL
);

