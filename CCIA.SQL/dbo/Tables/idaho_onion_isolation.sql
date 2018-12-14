CREATE TABLE [dbo].[idaho_onion_isolation] (
    [field_type]                       VARCHAR (50)    NOT NULL,
    [diff_color]                       NUMERIC (10, 1) NOT NULL,
    [diff_shape]                       NUMERIC (10, 1) NOT NULL,
    [diff_crop]                        NUMERIC (10, 1) NULL,
    [same_crop]                        NUMERIC (10, 1) NULL,
    [stock_researchStation]            NUMERIC (10, 1) NOT NULL,
    [diff_type]                        NUMERIC (10, 1) NOT NULL,
    [hybrid_vs_hybrid]                 NUMERIC (10, 1) NOT NULL,
    [hybrid_vs_openPollinated]         NUMERIC (10, 1) NOT NULL,
    [openPollinated_vs_openPollinated] NUMERIC (10, 1) NOT NULL
);

