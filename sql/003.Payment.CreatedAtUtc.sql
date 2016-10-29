ALTER TABLE [dbo].[Payment] 
    ADD [CreatedAtUtc] datetime2(3) NOT NULL DEFAULT SYSUTCDATETIME();

GO

INSERT INTO [_DBVersioning] ([Id], [Filename], [Timestamp]) VALUES (3, '003.Payment.CreatedAtUtc.sql', SYSDATETIMEOFFSET())

GO