CREATE TABLE [dbo].[idaho_carrot_isolation] (
    [diff_color]               NUMERIC (10, 1) NOT NULL,
    [hybrid_vs_openPollinated] NUMERIC (10, 1) NOT NULL,
    [hybrid_diff_type]         NUMERIC (10, 1) NOT NULL,
    [hybrid_same_type]         NUMERIC (10, 1) NOT NULL,
    [openPollinated_diff_type] NUMERIC (10, 1) NOT NULL,
    [openPollinated_same_type] NUMERIC (10, 1) NOT NULL,
    [stock_researchStation]    NUMERIC (10, 1) NOT NULL
);

