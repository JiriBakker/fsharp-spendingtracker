CREATE TABLE [dbo].[CategoryDescription] (

    [CategoryDescriptionId] [int] NOT NULL IDENTITY (1,1) PRIMARY KEY,
    [CategoryId]            [int] NOT NULL,
    [Description]           [nvarchar](2048) NOT NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[CategoryDescription]
    ADD FOREIGN KEY ([CategoryId])
    REFERENCES [dbo].[Category]([CategoryId])

GO

INSERT INTO [_DBVersioning] ([Id], [Filename], [Timestamp]) VALUES (5, '005.CategoryDescription.Create.sql', SYSDATETIMEOFFSET())

GO