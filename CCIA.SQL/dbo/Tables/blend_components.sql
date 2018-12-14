CREATE TABLE [dbo].[blend_components] (
    [blend_comp_id] INT            IDENTITY (1, 1) NOT NULL,
    [var_off_id]    INT            NOT NULL,
    [comp_var_id]   INT            NOT NULL,
    [comp_percent]  NUMERIC (7, 2) NOT NULL
);

