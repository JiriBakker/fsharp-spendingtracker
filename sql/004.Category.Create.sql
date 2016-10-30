CREATE TABLE [dbo].[Category] (
    [CategoryId] [int]           NOT NULL IDENTITY (1,1) PRIMARY KEY,
    [Title]      [nvarchar](100) NOT NULL
) ON [PRIMARY]

GO


INSERT INTO [_DBVersioning] ([Id], [Filename], [Timestamp]) VALUES (4, '004.Category.Create.sql', SYSDATETIMEOFFSET())

GO