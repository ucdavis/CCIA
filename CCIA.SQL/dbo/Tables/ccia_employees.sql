﻿CREATE TABLE [dbo].[ccia_employees] (
    [employee_id]          VARCHAR (9)  NOT NULL,
    [first_name]           VARCHAR (30) NOT NULL,
    [last_name]            VARCHAR (30) NOT NULL,
    [ucd_mailid]           VARCHAR (32) NULL,
    [campus_room]          VARCHAR (10) NULL,
    [campus_bldg]          VARCHAR (10) NULL,
    [campus_phone]         VARCHAR (15) NULL,
    [cell_phone]           VARCHAR (15) NULL,
    [kerberos_id]          VARCHAR (32) NULL,
    [current]              BIT          NULL,
    [ccia_access]          BIT          NULL,
    [core_staff]           BIT          CONSTRAINT [DF_ccia_employees_core_staff] DEFAULT ((0)) NOT NULL,
    [field_inspect]        BIT          CONSTRAINT [DF_ccia_employees_field_inspect] DEFAULT ((0)) NULL,
    [seed_lot_inform]      BIT          CONSTRAINT [DF_ccia_employees_seed_lot_inform] DEFAULT ((0)) NOT NULL,
    [edit_varieties]       BIT          CONSTRAINT [DF_ccia_employees_edit_varieties] DEFAULT ((0)) NOT NULL,
    [billing_access]       BIT          CONSTRAINT [DF_ccia_employees_billing_access] DEFAULT ((0)) NOT NULL,
    [seed_lab]             BIT          CONSTRAINT [DF_ccia_employees_seed_lab] DEFAULT ((0)) NOT NULL,
    [seasonal_employee]    BIT          CONSTRAINT [DF_ccia_employees_seasonal_employee] DEFAULT ((1)) NOT NULL,
    [new_tag]              BIT          CONSTRAINT [DF_ccia_employees_new_tag] DEFAULT ((0)) NOT NULL,
    [tag_print]            BIT          CONSTRAINT [DF_ccia_employees_tag_print] DEFAULT ((0)) NOT NULL,
    [heritage_grain_qa]    BIT          CONSTRAINT [DF_ccia_employees_heritage_grain_qa] DEFAULT ((0)) NOT NULL,
    [prevariety_germplasm] BIT          CONSTRAINT [DF_ccia_employees_prevariety_germplams] DEFAULT ((0)) NOT NULL,
    [oecd_invoice_printer] BIT          CONSTRAINT [DF_ccia_employees_oecd_inoice_printer] DEFAULT ((0)) NOT NULL
);



