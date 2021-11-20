CREATE TABLE [dbo].[sx_results] (
    [sx_reults_id]   INT           NOT NULL,
    [sx_identity_id] INT           NULL,
    [testgrp_id]     INT           NULL,
    [sx_testname_id] INT           NULL,
    [test_result]    INT           NULL,
    [test_descr]     VARCHAR (50)  NULL,
    [test_unit]      VARCHAR (50)  NULL,
    [comments]       VARCHAR (250) NULL,
    [date_lab_run]   DATETIME      NULL,
    [lab_sx_id]      VARCHAR (50)  NULL,
    [date_entered]   DATETIME      NULL,
    [user_entered]   VARCHAR (9)   NULL,
    CONSTRAINT [FK_sx_results_sx_identity] FOREIGN KEY ([sx_identity_id]) REFERENCES [dbo].[sx_identity] ([sx_indentity_id]),
    CONSTRAINT [FK_sx_results_sx_testname] FOREIGN KEY ([test_result]) REFERENCES [dbo].[sx_testname] ([sx_testname_id])
);

