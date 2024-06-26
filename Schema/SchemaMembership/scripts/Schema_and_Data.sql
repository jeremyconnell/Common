/****** Object:  Table [dbo].[tblMembership_User]    Script Date: 05/15/2010 19:30:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblMembership_User](
	[UserLoginName] [nvarchar](50) NOT NULL,
	[UserPasswordHashedSha1] [varchar](50) NULL,
	[UserPasswordSalt] [varchar](50) NULL,
	[UserFirstName] [nvarchar](50) NULL,
	[UserLastName] [nvarchar](50) NULL,
	[UserEmail] [varchar](50) NULL,
	[UserIsDisabled] [bit] NULL,
	[UserCreatedDate] [datetime] NULL,
	[UserLastLoginDate] [datetime] NULL,
	[UserLastPasswordChangedDate] [datetime] NULL,
	[UserPasswordQuestion] [nvarchar](256) NULL,
	[UserPasswordAnswer] [nvarchar](256) NULL,
	[UserFailedPasswordAttemptCount] [int] NULL,
	[UserFailedPasswordAttemptStartDate] [datetime] NULL,
	[UserIsLockedOut] [bit] NULL,
	[UserLastLockoutDate] [datetime] NULL,
	[UserComments] [text] NULL,
 CONSTRAINT [PK_tblMembership_User] PRIMARY KEY CLUSTERED 
(
	[UserLoginName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_UserEmailUnique] UNIQUE NONCLUSTERED 
(
	[UserEmail] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[tblMembership_User] ([UserLoginName], [UserPasswordHashedSha1], [UserPasswordSalt], [UserFirstName], [UserLastName], [UserEmail], [UserIsDisabled], [UserCreatedDate], [UserLastLoginDate], [UserLastPasswordChangedDate], [UserPasswordQuestion], [UserPasswordAnswer], [UserFailedPasswordAttemptCount], [UserFailedPasswordAttemptStartDate], [UserIsLockedOut], [UserLastLockoutDate], [UserComments]) VALUES (N'asdf', N'', N'jd5YbsPQ', N'asdf', N'asdf', N'asdf', 0, CAST(0x00009D610010F142 AS DateTime), NULL, NULL, N'asdf', N'asdf', NULL, NULL, 0, NULL, N'')
INSERT [dbo].[tblMembership_User] ([UserLoginName], [UserPasswordHashedSha1], [UserPasswordSalt], [UserFirstName], [UserLastName], [UserEmail], [UserIsDisabled], [UserCreatedDate], [UserLastLoginDate], [UserLastPasswordChangedDate], [UserPasswordQuestion], [UserPasswordAnswer], [UserFailedPasswordAttemptCount], [UserFailedPasswordAttemptStartDate], [UserIsLockedOut], [UserLastLockoutDate], [UserComments]) VALUES (N'asdfasdf', N'asdfasdf', N'3AfmEBEf', N'asdfasdfasdfasd', N'asdfasdfasdf', N'asdfasdfasdf', 0, CAST(0x00009D6100114AA9 AS DateTime), NULL, NULL, N'asdf', N'asdf', NULL, NULL, 0, NULL, N'')
/****** Object:  Table [dbo].[tblMembership_Role]    Script Date: 05/15/2010 19:30:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblMembership_Role](
	[RoleName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_tblMembership_Role] PRIMARY KEY CLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[tblMembership_Role] ([RoleName]) VALUES (N'Administrators')
INSERT [dbo].[tblMembership_Role] ([RoleName]) VALUES (N'Customers')
INSERT [dbo].[tblMembership_Role] ([RoleName]) VALUES (N'Developers')
/****** Object:  Table [dbo].[tblMembership_UserRole]    Script Date: 05/15/2010 19:30:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblMembership_UserRole](
	[URUserLogin] [nvarchar](50) NOT NULL,
	[URRoleName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_tblMembership_UserRole] PRIMARY KEY CLUSTERED 
(
	[URUserLogin] ASC,
	[URRoleName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[tblMembership_UserRole] ([URUserLogin], [URRoleName]) VALUES (N'asdf', N'Customers')
/****** Object:  Default [DF_tblUsers_UserCreatedDate]    Script Date: 05/15/2010 19:30:05 ******/
ALTER TABLE [dbo].[tblMembership_User] ADD  CONSTRAINT [DF_tblUsers_UserCreatedDate]  DEFAULT (getdate()) FOR [UserCreatedDate]
GO
/****** Object:  ForeignKey [FK_tblMembership_UserRole_tblMembership_Role]    Script Date: 05/15/2010 19:30:05 ******/
ALTER TABLE [dbo].[tblMembership_UserRole]  WITH CHECK ADD  CONSTRAINT [FK_tblMembership_UserRole_tblMembership_Role] FOREIGN KEY([URRoleName])
REFERENCES [dbo].[tblMembership_Role] ([RoleName])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblMembership_UserRole] CHECK CONSTRAINT [FK_tblMembership_UserRole_tblMembership_Role]
GO
/****** Object:  ForeignKey [FK_tblMembership_UserRole_tblMembership_User]    Script Date: 05/15/2010 19:30:05 ******/
ALTER TABLE [dbo].[tblMembership_UserRole]  WITH CHECK ADD  CONSTRAINT [FK_tblMembership_UserRole_tblMembership_User] FOREIGN KEY([URUserLogin])
REFERENCES [dbo].[tblMembership_User] ([UserLoginName])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblMembership_UserRole] CHECK CONSTRAINT [FK_tblMembership_UserRole_tblMembership_User]
GO

/****** Object:  Table [dbo].[tblMembership_Session]    Script Date: 07/11/2010 02:10:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblMembership_Session](
	[SessionId] [int] IDENTITY(1,1) NOT NULL,
	[SessionUserLoginName] [nvarchar](50) NULL,
 CONSTRAINT [PK_tblAudit_Session] PRIMARY KEY CLUSTERED 
(
	[SessionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_tblMembership_SessionUserLoginName] ON [dbo].[tblMembership_Session] 
(
	[SessionUserLoginName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblMembership_Click]    Script Date: 07/11/2010 02:10:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblMembership_Click](
	[ClickId] [int] IDENTITY(1,1) NOT NULL,
	[ClickSessionId] [int] NOT NULL,
	[ClickHost] [varchar](200) NOT NULL,
	[ClickUrl] [varchar](900) NOT NULL,
	[ClickQuerystring] [varchar](6000) NOT NULL,
	[ClickDate] [datetime] NOT NULL,
 CONSTRAINT [PK_tblAudit_Click] PRIMARY KEY CLUSTERED 
(
	[ClickId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE NONCLUSTERED INDEX [IX_tblAudit_ClickSessionId] ON [dbo].[tblMembership_Click] 
(
	[ClickSessionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  ForeignKey [FK_tblAudit_Click_tblAudit_Session]    Script Date: 07/11/2010 02:10:51 ******/
ALTER TABLE [dbo].[tblMembership_Click]  WITH CHECK ADD  CONSTRAINT [FK_tblAudit_Click_tblAudit_Session] FOREIGN KEY([ClickSessionId])
REFERENCES [dbo].[tblMembership_Session] ([SessionId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblMembership_Click] CHECK CONSTRAINT [FK_tblAudit_Click_tblAudit_Session]
GO
CREATE VIEW [dbo].[vwMembership_Session]
AS
SELECT     s.SessionUserLoginName, s.SessionId, COUNT(c.ClickId) AS ClickCount, MAX(c.ClickDate) AS MaxDate, MIN(c.ClickDate) AS MinDate
FROM         dbo.tblMembership_Click AS c INNER JOIN
                      dbo.tblMembership_Session AS s ON c.ClickSessionId = s.SessionId
GROUP BY s.SessionUserLoginName, s.SessionId

GO
/****** Object:  View [dbo].[vwMembership_Click]    Script Date: 07/11/2010 19:18:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vwMembership_Click]
AS
SELECT     c.*, s.SessionUserLoginName
FROM         dbo.tblMembership_Click AS c INNER JOIN
                      dbo.tblMembership_Session AS s ON c.ClickSessionId = s.SessionId

GO

CREATE NONCLUSTERED INDEX [IX_ClickUrl] ON [dbo].[tblMembership_Click] (	[ClickUrl] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
