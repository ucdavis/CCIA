CREATE TABLE [dbo].[Jobs] (
    [jobID]            INT          IDENTITY (1, 1) NOT NULL,
    [JobTitle]         VARCHAR (50) NULL,
    [JobInterval]      SMALLINT     NULL,
    [JobTime]          TIME (0)     NULL,
    [DateLastJobRan]   DATETIME     NULL,
    [DateNextJobStart] DATETIME     NULL,
    [Section]          VARCHAR (50) NULL
);

