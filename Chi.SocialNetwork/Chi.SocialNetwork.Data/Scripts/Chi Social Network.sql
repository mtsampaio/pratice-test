-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UserPostComments_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserPostComments] DROP CONSTRAINT [FK_UserPostComments_User];
GO
IF OBJECT_ID(N'[dbo].[FK_UserPostComments_UserPosts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserPostComments] DROP CONSTRAINT [FK_UserPostComments_UserPosts];
GO
IF OBJECT_ID(N'[dbo].[FK_UserPostLikes_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserPostLikes] DROP CONSTRAINT [FK_UserPostLikes_User];
GO
IF OBJECT_ID(N'[dbo].[FK_UserPostLikes_UserPosts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserPostLikes] DROP CONSTRAINT [FK_UserPostLikes_UserPosts];
GO
IF OBJECT_ID(N'[dbo].[FK_UserPosts_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserPosts] DROP CONSTRAINT [FK_UserPosts_User];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[UserPostComments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserPostComments];
GO
IF OBJECT_ID(N'[dbo].[UserPostLikes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserPostLikes];
GO
IF OBJECT_ID(N'[dbo].[UserPosts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserPosts];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UserPostComments'
CREATE TABLE [dbo].[UserPostComments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Comment] varchar(max)  NOT NULL,
    [CommentDate] datetime  NULL,
    [User_Id] int  NOT NULL,
    [UserPost_Id] int  NOT NULL
);
GO

-- Creating table 'UserPostLikes'
CREATE TABLE [dbo].[UserPostLikes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [User_Id] int  NOT NULL,
    [UserPost_Id] int  NOT NULL
);
GO

-- Creating table 'UserPosts'
CREATE TABLE [dbo].[UserPosts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PostContent] varchar(max)  NOT NULL,
    [ContentType] int  NOT NULL,
    [PostDate] datetime  NULL,
    [User_Id] int  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NOT NULL,
    [LastName] varchar(50)  NOT NULL,
    [Email] varchar(100)  NOT NULL,
    [Password] varchar(12)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'UserPostComments'
ALTER TABLE [dbo].[UserPostComments]
ADD CONSTRAINT [PK_UserPostComments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserPostLikes'
ALTER TABLE [dbo].[UserPostLikes]
ADD CONSTRAINT [PK_UserPostLikes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserPosts'
ALTER TABLE [dbo].[UserPosts]
ADD CONSTRAINT [PK_UserPosts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [User_Id] in table 'UserPostComments'
ALTER TABLE [dbo].[UserPostComments]
ADD CONSTRAINT [FK_UserPostComments_User]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserPostComments_User'
CREATE INDEX [IX_FK_UserPostComments_User]
ON [dbo].[UserPostComments]
    ([User_Id]);
GO

-- Creating foreign key on [UserPost_Id] in table 'UserPostComments'
ALTER TABLE [dbo].[UserPostComments]
ADD CONSTRAINT [FK_UserPostComments_UserPosts]
    FOREIGN KEY ([UserPost_Id])
    REFERENCES [dbo].[UserPosts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserPostComments_UserPosts'
CREATE INDEX [IX_FK_UserPostComments_UserPosts]
ON [dbo].[UserPostComments]
    ([UserPost_Id]);
GO

-- Creating foreign key on [User_Id] in table 'UserPostLikes'
ALTER TABLE [dbo].[UserPostLikes]
ADD CONSTRAINT [FK_UserPostLikes_User]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserPostLikes_User'
CREATE INDEX [IX_FK_UserPostLikes_User]
ON [dbo].[UserPostLikes]
    ([User_Id]);
GO

-- Creating foreign key on [UserPost_Id] in table 'UserPostLikes'
ALTER TABLE [dbo].[UserPostLikes]
ADD CONSTRAINT [FK_UserPostLikes_UserPosts]
    FOREIGN KEY ([UserPost_Id])
    REFERENCES [dbo].[UserPosts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserPostLikes_UserPosts'
CREATE INDEX [IX_FK_UserPostLikes_UserPosts]
ON [dbo].[UserPostLikes]
    ([UserPost_Id]);
GO

-- Creating foreign key on [User_Id] in table 'UserPosts'
ALTER TABLE [dbo].[UserPosts]
ADD CONSTRAINT [FK_UserPosts_User]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserPosts_User'
CREATE INDEX [IX_FK_UserPosts_User]
ON [dbo].[UserPosts]
    ([User_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------