CREATE TABLE [dbo].[tag_docs] (
    [tag_doc_id] INT           IDENTITY (1, 1) NOT NULL,
    [tag_id]     INT           NOT NULL,
    [doc_link]   VARCHAR (100) NOT NULL
);

