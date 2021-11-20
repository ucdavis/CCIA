CREATE TABLE [dbo].[sx_testname] (
    [sx_testname_id] INT           NOT NULL,
    [testname]       VARCHAR (100) NULL,
    [test_unit]      VARCHAR (25)  NULL,
    [test_grp]       VARCHAR (100) NULL,
    CONSTRAINT [PK_sx_testname] PRIMARY KEY CLUSTERED ([sx_testname_id] ASC)
);

