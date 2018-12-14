CREATE TABLE [dbo].[map_alfalfa_gefree_isolation] (
    [field_type]        VARCHAR (50)    NOT NULL,
    [seed_conventional] NUMERIC (10, 3) NOT NULL,
    [seed_aps]          NUMERIC (10, 3) NOT NULL,
    [seed_organic]      NUMERIC (10, 3) NOT NULL,
    [seed_ge]           NUMERIC (10, 3) NOT NULL,
    [seed_rr]           NUMERIC (10, 3) NULL,
    [seed_rrrl]         NUMERIC (10, 3) NULL,
    [hay_conventional]  NUMERIC (10, 3) NOT NULL,
    [hay_aps]           NUMERIC (10, 3) NOT NULL,
    [hay_organic]       NUMERIC (10, 3) NOT NULL,
    [hay_ge]            NUMERIC (10, 3) NOT NULL,
    [hay_rr]            NUMERIC (10, 3) NULL,
    [hay_rrrl]          NUMERIC (10, 3) NULL,
    [seed_foundation]   NUMERIC (10, 3) NULL,
    [seed_certified]    NUMERIC (10, 3) NULL
);

