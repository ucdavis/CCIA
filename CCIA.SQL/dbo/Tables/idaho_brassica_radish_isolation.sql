CREATE TABLE [dbo].[idaho_brassica_radish_isolation] (
    [genus]                VARCHAR (50)    NOT NULL,
    [same_crop_same_color] NUMERIC (10, 1) NOT NULL,
    [dif_crop_same_color]  NUMERIC (10, 1) NOT NULL,
    [same_crop_dif_color]  NUMERIC (10, 1) NOT NULL,
    [dif_crop_dif_color]   NUMERIC (10, 1) NOT NULL,
    [dif_genus]            NUMERIC (10, 1) NOT NULL
);

