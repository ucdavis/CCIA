CREATE TABLE [dbo].[seedlab_list] (
    [list_id]         INT           IDENTITY (1, 1) NOT NULL,
    [genus]           VARCHAR (50)  NOT NULL,
    [species]         VARCHAR (50)  NULL,
    [subspecies]      VARCHAR (50)  NULL,
    [common_name]     VARCHAR (500) NOT NULL,
    [noxious]         BIT           CONSTRAINT [DF_seedlab_list_noxious] DEFAULT ((0)) NOT NULL,
    [noxious_type]    VARCHAR (50)  NULL,
    [list_name]       AS            (((([genus]+isnull(' '+[species],''))+isnull(' '+[subspecies],''))+' | ')+[common_name]) PERSISTED NOT NULL,
    [scientific_name] AS            (([genus]+isnull(' '+[species],''))+isnull(' '+[subspecies],'')) PERSISTED NOT NULL,
    CONSTRAINT [PK_seedlab_list] PRIMARY KEY CLUSTERED ([list_id] ASC),
    CONSTRAINT [IX_seedlab_list] UNIQUE NONCLUSTERED ([genus] ASC, [species] ASC, [subspecies] ASC, [common_name] ASC)
);



