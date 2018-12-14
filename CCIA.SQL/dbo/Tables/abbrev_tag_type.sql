CREATE TABLE [dbo].[abbrev_tag_type] (
    [tag_type_id]       TINYINT      IDENTITY (1, 1) NOT NULL,
    [tag_type_trans]    VARCHAR (50) NULL,
    [sort_order]        TINYINT      NULL,
    [standard_tag_form] BIT          CONSTRAINT [DF_abbrev_tag_type_standard_form] DEFAULT ((0)) NOT NULL,
    [oecd]              BIT          CONSTRAINT [DF_abbrev_tag_type_oecd] DEFAULT ((0)) NOT NULL,
    [po_tag]            BIT          CONSTRAINT [DF_abbrev_tag_type_po_tag] DEFAULT ((0)) NOT NULL
);

