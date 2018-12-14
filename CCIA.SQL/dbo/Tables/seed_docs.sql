CREATE TABLE [dbo].[seed_docs] (
    [seed_cert_id] INT           IDENTITY (1, 1) NOT NULL,
    [seeds_id]     INT           NOT NULL,
    [doc_name]     VARCHAR (100) NOT NULL,
    [doc_link]     VARCHAR (100) NOT NULL,
    [doc_type]     TINYINT       NOT NULL
);

