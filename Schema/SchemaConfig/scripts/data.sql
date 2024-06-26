
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON
GO
-- Pointer used for text / image updates. This might not be needed, but is declared here just in case
DECLARE @pv binary(16)

BEGIN TRANSACTION

-- Drop constraint FK_tblConfig_Setting_tblConfig_Type from [dbo].[tblConfig_Setting]
ALTER TABLE [dbo].[tblConfig_Setting] DROP CONSTRAINT [FK_tblConfig_Setting_tblConfig_Type]

-- Add rows to [dbo].[tblConfig_Type]
SET IDENTITY_INSERT [dbo].[tblConfig_Type] ON
INSERT INTO [dbo].[tblConfig_Type] ([TypeId], [TypeName]) VALUES (1, 'Boolean')
INSERT INTO [dbo].[tblConfig_Type] ([TypeId], [TypeName]) VALUES (2, 'String')
INSERT INTO [dbo].[tblConfig_Type] ([TypeId], [TypeName]) VALUES (3, 'Double')
INSERT INTO [dbo].[tblConfig_Type] ([TypeId], [TypeName]) VALUES (4, 'Int')
INSERT INTO [dbo].[tblConfig_Type] ([TypeId], [TypeName]) VALUES (5, 'Date')
INSERT INTO [dbo].[tblConfig_Type] ([TypeId], [TypeName]) VALUES (7, 'Money')
SET IDENTITY_INSERT [dbo].[tblConfig_Type] OFF

-- Add constraint FK_tblConfig_Setting_tblConfig_Type to [dbo].[tblConfig_Setting]
ALTER TABLE [dbo].[tblConfig_Setting] WITH NOCHECK ADD CONSTRAINT [FK_tblConfig_Setting_tblConfig_Type] FOREIGN KEY ([SettingTypeId]) REFERENCES [dbo].[tblConfig_Type] ([TypeId])

COMMIT TRANSACTION
GO

-- Reseed identity on [dbo].[tblConfig_Type]
DBCC CHECKIDENT('[dbo].[tblConfig_Type]', RESEED, 9)
GO
