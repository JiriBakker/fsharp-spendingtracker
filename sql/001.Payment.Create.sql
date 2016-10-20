CREATE TABLE [dbo].[Payment] (
    [PaymentId] [int]            NOT NULL IDENTITY (1,1) PRIMARY KEY,
    [Amount]    [decimal](10,3)  NOT NULL,
    [Timestamp] [datetimeoffset] NOT NULL
) ON [PRIMARY]

GO

CREATE NONCLUSTERED INDEX IX_Payment_Timestamp ON [dbo].[Payment] (
    [Timestamp]
)

GO

INSERT INTO [_DBVersioning] ([Id], [Filename], [Timestamp]) VALUES (1, '001.Payment.Create.sql', SYSDATETIMEOFFSET())

GO