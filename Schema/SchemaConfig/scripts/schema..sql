
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#tmpErrors')) DROP TABLE #tmpErrors
GO
CREATE TABLE #tmpErrors (Error int)
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
PRINT N'Creating [dbo].[tblConfig_Setting]'
GO
CREATE TABLE [dbo].[tblConfig_Setting]
(
[SettingId] [int] NOT NULL IDENTITY(1, 1),
[SettingName] [varchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
[SettingGroupId] [int] NOT NULL,
[SettingTypeId] [int] NOT NULL,
[SettingClientCanEdit] [bit] NOT NULL CONSTRAINT [DF_tblConfig_Setting_SettingClientCanEdit] DEFAULT (1),
[SettingValueBoolean] [bit] NULL,
[SettingValueString] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL,
[SettingValueInteger] [int] NULL,
[SettingValueDouble] [float] NULL,
[SettingValueDate] [datetime] NULL,
[SettingValueMoney] [decimal] (16, 2) NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_tblConfig_Setting] on [dbo].[tblConfig_Setting]'
GO
ALTER TABLE [dbo].[tblConfig_Setting] ADD CONSTRAINT [PK_tblConfig_Setting] PRIMARY KEY CLUSTERED ([SettingId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[tblConfig_Group]'
GO
CREATE TABLE [dbo].[tblConfig_Group]
(
[GroupId] [int] NOT NULL IDENTITY(1, 1),
[GroupName] [varchar] (50) COLLATE Latin1_General_CI_AS NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_tblConfig_Group] on [dbo].[tblConfig_Group]'
GO
ALTER TABLE [dbo].[tblConfig_Group] ADD CONSTRAINT [PK_tblConfig_Group] PRIMARY KEY CLUSTERED ([GroupId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[tblConfig_Type]'
GO
CREATE TABLE [dbo].[tblConfig_Type]
(
[TypeId] [int] NOT NULL IDENTITY(1, 1),
[TypeName] [varchar] (100) COLLATE Latin1_General_CI_AS NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_tblConfig_Type] on [dbo].[tblConfig_Type]'
GO
ALTER TABLE [dbo].[tblConfig_Type] ADD CONSTRAINT [PK_tblConfig_Type] PRIMARY KEY CLUSTERED ([TypeId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding constraints to [dbo].[tblConfig_Setting]'
GO
ALTER TABLE [dbo].[tblConfig_Setting] ADD CONSTRAINT [IX_tblConfig_Setting] UNIQUE NONCLUSTERED ([SettingName])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[tblConfig_Setting]'
GO
ALTER TABLE [dbo].[tblConfig_Setting] WITH NOCHECK ADD
CONSTRAINT [FK_tblConfig_Setting_tblConfig_Group] FOREIGN KEY ([SettingGroupId]) REFERENCES [dbo].[tblConfig_Group] ([GroupId]),
CONSTRAINT [FK_tblConfig_Setting_tblConfig_Type] FOREIGN KEY ([SettingTypeId]) REFERENCES [dbo].[tblConfig_Type] ([TypeId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
IF EXISTS (SELECT * FROM #tmpErrors) ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT>0 BEGIN
PRINT 'The database update succeeded'
COMMIT TRANSACTION
END
ELSE PRINT 'The database update failed'
GO
DROP TABLE #tmpErrors
GO


ALTER TABLE dbo.tblConfig_Setting ADD	SettingSortOrder int NULL
ALTER TABLE dbo.tblConfig_Group ADD	GroupSortOrder int NULL



CREATE TABLE [dbo].[tblConfig_List](
	[ListId] [int] IDENTITY(1,1) NOT NULL,
	[ListName] [nvarchar](max) NOT NULL,
	[ListIsExternal] [bit] NOT NULL,
	[ListExternalConnectionString] [varchar](max) NULL,
	[ListExternalTable] [varchar](max) NULL,
	[ListExteralPrimaryKey] [varchar](max) NULL,
	[ListExteralNameColumn] [varchar](50) NULL,
	[ListModified] [datetime] NULL,
	[ListIsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_SS_ConfigList] PRIMARY KEY CLUSTERED 
(
	[ListId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [dbo].[tblConfig_List] ADD  CONSTRAINT [DF_SS_ConfigList_ListIsExternal]  DEFAULT ((0)) FOR [ListIsExternal]
ALTER TABLE [dbo].[tblConfig_List] ADD  CONSTRAINT [DF_SS_ConfigList_ListIsDeleted]  DEFAULT ((0)) FOR [ListIsDeleted]





CREATE TABLE [dbo].[tblConfig_Item](
	[ItemId] [int] IDENTITY(1,1) NOT NULL,
	[ItemListId] [int] NOT NULL,
	[ItemName] [nvarchar](max) NOT NULL,
	[ItemSortOrder] [int] NOT NULL,
	[ItemIsDeleted] [bit] NOT NULL,
	[ItemModified] [datetime] NOT NULL,
 CONSTRAINT [PK_tblConfig_Item] PRIMARY KEY CLUSTERED 
(
	[ItemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]


ALTER TABLE [dbo].[tblConfig_Item]  WITH CHECK ADD  CONSTRAINT [FK_ItemListId] FOREIGN KEY([ItemListId])
REFERENCES [dbo].[tblConfig_List] ([ListId])

ALTER TABLE [dbo].[tblConfig_Item] CHECK CONSTRAINT [FK_ItemListId]

ALTER TABLE [dbo].[tblConfig_Item] ADD  CONSTRAINT [DF_tblConfig_Item_ItemSortOrder]  DEFAULT ((0)) FOR [ItemSortOrder]

ALTER TABLE [dbo].[tblConfig_Item] ADD  CONSTRAINT [DF_tblConfig_Item_ItemIsDeleted]  DEFAULT ((0)) FOR [ItemIsDeleted]

ALTER TABLE [dbo].[tblConfig_Item] ADD  CONSTRAINT [DF_tblConfig_Item_ItemModified]  DEFAULT (getdate()) FOR [ItemModified]


