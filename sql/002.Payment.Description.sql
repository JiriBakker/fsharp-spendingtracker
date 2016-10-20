ALTER TABLE [dbo].[Payment] 
    ADD [Description] nvarchar(2048) NULL;

GO

INSERT INTO [_DBVersioning] ([Id], [Filename], [Timestamp]) VALUES (2, '002.Payment.Description.sql', SYSDATETIMEOFFSET())

GO