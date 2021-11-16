CREATE TABLE [dbo].[po_health_cert] (
    [app_id]                   INT            NOT NULL,
    [lot_orig_cult]            BIT            NOT NULL,
    [yr_micropropagated]       INT            NULL,
    [micropropagated_by]       VARCHAR (100)  NULL,
    [num_yrs_produced]         INT            NULL,
    [ph_location]              VARCHAR (50)   NULL,
    [ph_leafroll]              INT            NULL,
    [ph_mosaic]                INT            NULL,
    [ph_other_varieties]       INT            NULL,
    [ph_sample_no]             INT            NULL,
    [ph_plant_count]           INT            NULL,
    [percent_pvy]              DECIMAL (7, 2) NULL,
    [percent_pvx]              DECIMAL (7, 2) NULL,
    [bact_ring_rot]            INT            NOT NULL,
    [golden_nematode]          INT            NOT NULL,
    [late_blight]              INT            NOT NULL,
    [root_knot_nematode]       INT            NOT NULL,
    [pot_rot_nematode]         INT            NOT NULL,
    [pot_wart]                 INT            NOT NULL,
    [powder_scab]              INT            NOT NULL,
    [pot_spindle_tuber_viroid] INT            NOT NULL,
    [corky_ring_spots]         INT            NOT NULL,
    [notes]                    VARCHAR (1000) NULL
);



