CREATE TABLE [dbo].[var_historical_owner] (
    [hist_id]        INT          IDENTITY (101, 1) NOT NULL,
    [var_off_id]     INT          NOT NULL,
    [past_owner_id]  INT          NOT NULL,
    [date_effective] DATETIME     NULL,
    [date_entered]   DATETIME     NULL,
    [user_entered]   VARCHAR (50) NULL,
    CONSTRAINT [PK_var_historical_owner] PRIMARY KEY CLUSTERED ([hist_id] ASC),
    CONSTRAINT [FK_var_historical_owner_Var_Official] FOREIGN KEY ([var_off_id]) REFERENCES [dbo].[var_official] ([var_off_id])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'fk organization.org_id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'var_historical_owner', @level2type = N'COLUMN', @level2name = N'past_owner_id';

