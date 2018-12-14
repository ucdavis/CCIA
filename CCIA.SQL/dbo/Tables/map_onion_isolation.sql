CREATE TABLE [dbo].[map_onion_isolation] (
    [field_type]       VARCHAR (50)    NOT NULL,
    [yellow]           NUMERIC (10, 1) NOT NULL,
    [onion_yellow_sd]  NUMERIC (10, 1) NOT NULL,
    [onion_yellow_int] NUMERIC (10, 1) NOT NULL,
    [onion_yellow_ld]  NUMERIC (10, 1) NOT NULL,
    [red]              NUMERIC (10, 1) NOT NULL,
    [onion_red_sd]     NUMERIC (10, 1) NOT NULL,
    [onion_red_int]    NUMERIC (10, 1) NOT NULL,
    [onion_red_ld]     NUMERIC (10, 1) NOT NULL,
    [white]            NUMERIC (10, 1) NOT NULL,
    [onion_white_sd]   NUMERIC (10, 1) NOT NULL,
    [onion_white_int]  NUMERIC (10, 1) NOT NULL,
    [onion_white_ld]   NUMERIC (10, 1) NOT NULL,
    [wakegi]           NUMERIC (10, 1) NOT NULL
);

