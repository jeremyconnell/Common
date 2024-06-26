/****** Object:  Table [dbo].[tblAudit_Error]    Script Date: 07/09/2010 03:22:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblAudit_Error](
	[ErrorID] [int] IDENTITY(1,1) NOT NULL,
	[ErrorUserID] [nvarchar](50) NULL,
	[ErrorUserName] [nvarchar](50) NULL,
	[ErrorWebsite] [varchar](200) NULL,
	[ErrorUrl] [varchar](2000) NULL,
	[ErrorMachineName] [varchar](50) NULL,
	[ErrorApplicationName] [varchar](200) NULL,
	[ErrorApplicationVersion] [varchar](50) NULL,
	[ErrorType] [varchar](200) NULL,
	[ErrorTypeHash] [int] NULL,
	[ErrorMessage] [varchar](3000) NULL,
	[ErrorMessageHash] [int] NULL,
	[ErrorStacktrace] [varchar](5000) NULL,
	[ErrorInnerType] [varchar](200) NULL,
	[ErrorInnerTypeHash] [int] NULL,
	[ErrorInnerMessage] [varchar](3000) NULL,
	[ErrorInnerMessageHash] [int] NULL,
	[ErrorInnerStacktrace] [varchar](5000) NULL,
	[ErrorDateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_tblAudit_Error] PRIMARY KEY CLUSTERED 
(
	[ErrorID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE NONCLUSTERED INDEX [tblAuditError_HashedLongStrings] ON [dbo].[tblAudit_Error] 
(
	[ErrorTypeHash] ASC,
	[ErrorMessageHash] ASC,
	[ErrorInnerTypeHash] ASC,
	[ErrorInnerMessageHash] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblAudit_Type]    Script Date: 07/09/2010 03:22:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblAudit_Type](
	[TypeId] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [varchar](50) NULL,
 CONSTRAINT [PK_tblAudit_Type] PRIMARY KEY CLUSTERED 
(
	[TypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[tblAudit_Type] ON
INSERT [dbo].[tblAudit_Type] ([TypeId], [TypeName]) VALUES (1, N'Delete')
INSERT [dbo].[tblAudit_Type] ([TypeId], [TypeName]) VALUES (2, N'Insert')
INSERT [dbo].[tblAudit_Type] ([TypeId], [TypeName]) VALUES (3, N'Update')
SET IDENTITY_INSERT [dbo].[tblAudit_Type] OFF
/****** Object:  View [dbo].[vwAudit_Error]    Script Date: 07/09/2010 03:22:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vwAudit_Error]
AS
SELECT		COUNT(ErrorID) AS ErrorID, 
			MAX(ErrorDateCreated) AS ErrorDateCreated, 
			MAX(ErrorUserID) AS ErrorUserID, 
			MAX(ErrorUserName) AS ErrorUserName, 
			MAX(ErrorWebsite) AS ErrorWebsite, 
			MAX(ErrorUrl) AS ErrorUrl, 
			MAX(ErrorMachineName) AS ErrorMachineName, 
			MAX(ErrorApplicationName) AS ErrorApplicationName, 
			MAX(ErrorApplicationVersion) AS ErrorApplicationVersion, 
			MAX(ErrorStacktrace) As ErrorStacktrace, 
			MAX(ErrorInnerStacktrace) As ErrorInnerStacktrace, 
			MAX(ErrorType) AS ErrorType, 
            MAX(ErrorMessage) AS ErrorMessage,
			MAX(ErrorInnerType) AS ErrorInnerType, 
			MAX(ErrorInnerMessage) AS ErrorInnerMessage,
			ErrorTypeHash, 
			ErrorMessageHash, 
			ErrorInnerTypeHash, 
			ErrorInnerMessageHash
			
FROM   dbo.tblAudit_Error

GROUP BY 
			ErrorTypeHash, 
			ErrorMessageHash, 
			ErrorInnerTypeHash, 
			ErrorInnerMessageHash
GO
/****** Object:  Table [dbo].[tblAudit_Trail]    Script Date: 07/09/2010 03:22:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblAudit_Trail](
	[AuditId] [int] IDENTITY(1,1) NOT NULL,
	[AuditTypeId] [int] NOT NULL,
	[AuditDate] [datetime] NOT NULL,
	[AuditUserLoginName] [varchar](100) NOT NULL,
	[AuditUrl] [varchar](500) NOT NULL,
	[AuditUrlNoQuerystring] [varchar](500) NOT NULL,
	[AuditDataTableName] [varchar](50) NOT NULL,
	[AuditDataPrimaryKey] [varchar](36) NULL,
 CONSTRAINT [PK_tblAudit_Trail] PRIMARY KEY CLUSTERED 
(
	[AuditId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE NONCLUSTERED INDEX [AuditDataTableName] ON [dbo].[tblAudit_Trail] 
(
	[AuditDataTableName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [AuditDataTableNameAndAuditTypeID] ON [dbo].[tblAudit_Trail] 
(
	[AuditTypeId] ASC,
	[AuditDataTableName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [AuditDataTableNameAndPrimaryKey] ON [dbo].[tblAudit_Trail] 
(
	[AuditDataTableName] ASC,
	[AuditDataPrimaryKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [AuditTypeID] ON [dbo].[tblAudit_Trail] 
(
	[AuditTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [AuditUrl] ON [dbo].[tblAudit_Trail] 
(
	[AuditUrl] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [AuditUserLoginName] ON [dbo].[tblAudit_Trail] 
(
	[AuditUserLoginName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblAudit_Data]    Script Date: 07/09/2010 03:22:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblAudit_Data](
	[DataId] [int] IDENTITY(1,1) NOT NULL,
	[DataTrailId] [int] NOT NULL,
	[DataIsBefore] [bit] NOT NULL,
	[DataName] [varchar](200) NOT NULL,
	[DataValue] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_tblAudit_Data] PRIMARY KEY CLUSTERED 
(
	[DataId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE NONCLUSTERED INDEX [IX_tblAudit_Data] ON [dbo].[tblAudit_Data] 
(
	[DataId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_tblAudit_DataName] ON [dbo].[tblAudit_Data] 
(
	[DataName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_tblAudit_DataTrailId] ON [dbo].[tblAudit_Data] 
(
	[DataTrailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Default [DF_tblAudit_Error_DateCreated]    Script Date: 07/09/2010 03:22:13 ******/
ALTER TABLE [dbo].[tblAudit_Error] ADD  CONSTRAINT [DF_tblAudit_Error_DateCreated]  DEFAULT (getdate()) FOR [ErrorDateCreated]
GO
/****** Object:  ForeignKey [FK_tblAudit_Data_tblAudit_Trail]    Script Date: 07/09/2010 03:22:13 ******/
ALTER TABLE [dbo].[tblAudit_Data]  WITH CHECK ADD  CONSTRAINT [FK_tblAudit_Data_tblAudit_Trail] FOREIGN KEY([DataTrailId])
REFERENCES [dbo].[tblAudit_Trail] ([AuditId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblAudit_Data] CHECK CONSTRAINT [FK_tblAudit_Data_tblAudit_Trail]
GO
/****** Object:  ForeignKey [FK_tblAudit_Trail_tblAudit_Type]    Script Date: 07/09/2010 03:22:13 ******/
ALTER TABLE [dbo].[tblAudit_Trail]  WITH NOCHECK ADD  CONSTRAINT [FK_tblAudit_Trail_tblAudit_Type] FOREIGN KEY([AuditTypeId])
REFERENCES [dbo].[tblAudit_Type] ([TypeId])
GO
ALTER TABLE [dbo].[tblAudit_Trail] CHECK CONSTRAINT [FK_tblAudit_Trail_tblAudit_Type]
GO



/****** Object:  Table [dbo].[tblAudit_Log]    Script Date: 10/02/2015 23:39:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblAudit_Log](
	[LogId] [int] IDENTITY(1,1) NOT NULL,
	[LogTypeId] [int] NOT NULL,
	[LogMessage] [nvarchar](max) NOT NULL,
	[LogCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_tblAudit_Log] PRIMARY KEY CLUSTERED 
(
	[LogId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO



/****** Object:  Index [IX_LogTypeId]    Script Date: 10/02/2015 23:39:30 ******/
CREATE NONCLUSTERED INDEX [IX_LogTypeId] ON [dbo].[tblAudit_Log] 
(
	[LogTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblAudit_Sql](
	[SqlId] [int] IDENTITY(1,1) NOT NULL,
	[SqlText] [nvarchar](max) NOT NULL,
	[SqlCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_tblAudit_Sql] PRIMARY KEY CLUSTERED 
(
	[SqlId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO