CREATE TABLE [dbo].[cond_reps] (
    [cond_rep_id] INT          IDENTITY (1, 1) NOT NULL,
    [org_id]      INT          NOT NULL,
    [rep_name]    VARCHAR (50) NOT NULL,
    [rep_current] BIT          NOT NULL
);

