/****** Object:  Table [dbo].[Users]    Script Date: 16/01/2015 22:49:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Password] [varchar](12) NOT NULL,
	[About] [text] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[UserPosts]    Script Date: 16/01/2015 22:49:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[UserPosts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PostContent] [varchar](max) NOT NULL,
	[PostDate] [datetime] NOT NULL,
	[User_Id] [int] NOT NULL,
 CONSTRAINT [PK_UserPosts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[UserPosts]  WITH CHECK ADD  CONSTRAINT [FK_UserPosts_User] FOREIGN KEY([User_Id])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[UserPosts] CHECK CONSTRAINT [FK_UserPosts_User]
GO

/****** Object:  Table [dbo].[UserPostLikes]    Script Date: 16/01/2015 22:49:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserPostLikes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[User_Id] [int] NOT NULL,
	[UserPost_Id] [int] NOT NULL,
 CONSTRAINT [PK_UserPostLikes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[UserPostLikes]  WITH CHECK ADD  CONSTRAINT [FK_UserPostLikes_User] FOREIGN KEY([User_Id])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[UserPostLikes] CHECK CONSTRAINT [FK_UserPostLikes_User]
GO

ALTER TABLE [dbo].[UserPostLikes]  WITH CHECK ADD  CONSTRAINT [FK_UserPostLikes_UserPosts] FOREIGN KEY([UserPost_Id])
REFERENCES [dbo].[UserPosts] ([Id])
GO

ALTER TABLE [dbo].[UserPostLikes] CHECK CONSTRAINT [FK_UserPostLikes_UserPosts]
GO

/****** Object:  Table [dbo].[UserPostFiles]    Script Date: 16/01/2015 22:50:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[UserPostFiles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserPostId] [int] NOT NULL,
	[FileName] [varchar](1000) NOT NULL,
	[Type] [varchar](20) NOT NULL,
 CONSTRAINT [PK_UserPostFiles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[UserPostFiles]  WITH CHECK ADD  CONSTRAINT [FK_UserPostFiles_UserPosts] FOREIGN KEY([UserPostId])
REFERENCES [dbo].[UserPosts] ([Id])
GO

ALTER TABLE [dbo].[UserPostFiles] CHECK CONSTRAINT [FK_UserPostFiles_UserPosts]
GO

/****** Object:  Table [dbo].[UserPostComments]    Script Date: 16/01/2015 22:50:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[UserPostComments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Comment] [varchar](max) NOT NULL,
	[CommentDate] [datetime] NULL,
	[User_Id] [int] NOT NULL,
	[UserPost_Id] [int] NOT NULL,
 CONSTRAINT [PK_UserPostComments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[UserPostComments]  WITH CHECK ADD  CONSTRAINT [FK_UserPostComments_User] FOREIGN KEY([User_Id])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[UserPostComments] CHECK CONSTRAINT [FK_UserPostComments_User]
GO

ALTER TABLE [dbo].[UserPostComments]  WITH CHECK ADD  CONSTRAINT [FK_UserPostComments_UserPosts] FOREIGN KEY([UserPost_Id])
REFERENCES [dbo].[UserPosts] ([Id])
GO

ALTER TABLE [dbo].[UserPostComments] CHECK CONSTRAINT [FK_UserPostComments_UserPosts]
GO

/****** Object:  Table [dbo].[UserPostCommentLikes]    Script Date: 16/01/2015 22:50:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserPostCommentLikes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[User_Id] [int] NOT NULL,
	[UserPostComment_Id] [int] NOT NULL,
 CONSTRAINT [PK_UserPostCommentLikes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[UserPostCommentLikes]  WITH CHECK ADD  CONSTRAINT [FK_UserPostCommentLikes_User] FOREIGN KEY([User_Id])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[UserPostCommentLikes] CHECK CONSTRAINT [FK_UserPostCommentLikes_User]
GO

ALTER TABLE [dbo].[UserPostCommentLikes]  WITH CHECK ADD  CONSTRAINT [FK_UserPostCommentLikes_UserPostComments] FOREIGN KEY([UserPostComment_Id])
REFERENCES [dbo].[UserPostComments] ([Id])
GO

ALTER TABLE [dbo].[UserPostCommentLikes] CHECK CONSTRAINT [FK_UserPostCommentLikes_UserPostComments]
GO

