CREATE TABLE [dbo].[seed_doc_types] (
    [doc_type]    TINYINT      IDENTITY (1, 1) NOT NULL,
    [type_trans]  VARCHAR (50) NOT NULL,
    [type_order]  TINYINT      NOT NULL,
    [folder_name] VARCHAR (50) NOT NULL
);



