CREATE TABLE [dbo].[blend_docs] (
    [blend_doc_id] INT           IDENTITY (1, 1) NOT NULL,
    [blend_id]     INT           NOT NULL,
    [doc_name]     VARCHAR (100) NOT NULL,
    [doc_link]     VARCHAR (100) NOT NULL
);

