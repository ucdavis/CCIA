CREATE TABLE [dbo].[fir_docs] (
    [doc_id]   INT           IDENTITY (1, 1) NOT NULL,
    [app_id]   INT           NOT NULL,
    [doc_name] VARCHAR (100) NOT NULL,
    [doc_link] VARCHAR (100) NOT NULL
);

