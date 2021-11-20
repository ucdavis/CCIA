CREATE TABLE [dbo].[online] (
    [contact_id]     INT       IDENTITY (1, 1) NOT NULL,
    [loginid]        CHAR (10) NULL,
    [password]       CHAR (10) NULL,
    [contactname]    CHAR (10) NULL,
    [keyword]        CHAR (10) NULL,
    [timesloggedin]  INT       NULL,
    [dataentry_date] DATETIME  NULL,
    [dataentry_user] CHAR (10) NULL,
    [update_date]    DATETIME  NULL
);



