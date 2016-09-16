SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[_DBVersioning](
	[Id]        [int]            NOT NULL,
	[Filename]  [nvarchar](100)  NOT NULL,
	[Timestamp] [datetimeoffset] NOT NULL,
    CONSTRAINT [PK_DBVersioning] PRIMARY KEY CLUSTERED (
	   [Id] ASC
    ) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT INTO [_DBVersioning] ([Id], [Filename], [Timestamp]) VALUES (0, '000._DBVersioning.sql', SYSDATETIMEOFFSET());

GO