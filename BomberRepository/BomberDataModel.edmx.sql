
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 05/02/2013 11:12:45
-- Generated from EDMX file: C:\Users\Mehdi\Documents\Visual Studio 2010\Projects\BombermanOnlineV2\BomberRepository\BomberDataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [BombermanOnlineDatabase];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_AccountPlayer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Accounts] DROP CONSTRAINT [FK_AccountPlayer];
GO
IF OBJECT_ID(N'[dbo].[FK_ConversationPlayer_Conversation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ConversationPlayerParticipation] DROP CONSTRAINT [FK_ConversationPlayer_Conversation];
GO
IF OBJECT_ID(N'[dbo].[FK_ConversationPlayer_Player]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ConversationPlayerParticipation] DROP CONSTRAINT [FK_ConversationPlayer_Player];
GO
IF OBJECT_ID(N'[dbo].[FK_PlayerConversation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Conversations] DROP CONSTRAINT [FK_PlayerConversation];
GO
IF OBJECT_ID(N'[dbo].[FK_PlayerGame]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Games] DROP CONSTRAINT [FK_PlayerGame];
GO
IF OBJECT_ID(N'[dbo].[FK_ConversationGame]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Games] DROP CONSTRAINT [FK_ConversationGame];
GO
IF OBJECT_ID(N'[dbo].[FK_PlayerPlayer_Player]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Friendships] DROP CONSTRAINT [FK_PlayerPlayer_Player];
GO
IF OBJECT_ID(N'[dbo].[FK_PlayerPlayer_Player1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Friendships] DROP CONSTRAINT [FK_PlayerPlayer_Player1];
GO
IF OBJECT_ID(N'[dbo].[FK_FriendshipRequests_Player]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FriendshipRequests] DROP CONSTRAINT [FK_FriendshipRequests_Player];
GO
IF OBJECT_ID(N'[dbo].[FK_FriendshipRequests_Player1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FriendshipRequests] DROP CONSTRAINT [FK_FriendshipRequests_Player1];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Accounts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Accounts];
GO
IF OBJECT_ID(N'[dbo].[Players]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Players];
GO
IF OBJECT_ID(N'[dbo].[Games]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Games];
GO
IF OBJECT_ID(N'[dbo].[Conversations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Conversations];
GO
IF OBJECT_ID(N'[dbo].[ConversationPlayerParticipation]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ConversationPlayerParticipation];
GO
IF OBJECT_ID(N'[dbo].[Friendships]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Friendships];
GO
IF OBJECT_ID(N'[dbo].[FriendshipRequests]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FriendshipRequests];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Accounts'
CREATE TABLE [dbo].[Accounts] (
    [AccountId] int IDENTITY(1,1) NOT NULL,
    [Login] nvarchar(255)  NOT NULL UNIQUE,
    [Password] nvarchar(max)  NOT NULL,
    [AccountType] nvarchar(max)  NOT NULL,
    [Player_PlayerId] int  NOT NULL
);
GO

-- Creating table 'Players'
CREATE TABLE [dbo].[Players] (
    [PlayerId] int IDENTITY(1,1) NOT NULL,
    [Pseudonym] nvarchar(max)  NOT NULL,
    [PlayerStatus] nvarchar(max)  NOT NULL,
    [PlayerDescription] nvarchar(max)  NULL
);
GO

-- Creating table 'Games'
CREATE TABLE [dbo].[Games] (
    [GameId] int IDENTITY(1,1) NOT NULL,
    [WinnerPlayerId] int  NULL,
    [Conversation_ConversationId] int  NULL
);
GO

-- Creating table 'Conversations'
CREATE TABLE [dbo].[Conversations] (
    [ConversationId] int IDENTITY(1,1) NOT NULL,
    [ConversationLog] nvarchar(max)  NOT NULL,
    [HostPlayerId] int  NOT NULL
);
GO

-- Creating table 'ConversationPlayerParticipation'
CREATE TABLE [dbo].[ConversationPlayerParticipation] (
    [Conversations_ConversationId] int  NOT NULL,
    [Players_PlayerId] int  NOT NULL
);
GO

-- Creating table 'Friendships'
CREATE TABLE [dbo].[Friendships] (
    [PlayerPlayer_Player1_PlayerId] int  NOT NULL,
    [Friends_PlayerId] int  NOT NULL
);
GO

-- Creating table 'FriendshipRequests'
CREATE TABLE [dbo].[FriendshipRequests] (
    [PlayersRequestingFriendship_PlayerId] int  NOT NULL,
    [FriendshipRequestedPlayers_PlayerId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [AccountId] in table 'Accounts'
ALTER TABLE [dbo].[Accounts]
ADD CONSTRAINT [PK_Accounts]
    PRIMARY KEY CLUSTERED ([AccountId] ASC);
GO

-- Creating primary key on [PlayerId] in table 'Players'
ALTER TABLE [dbo].[Players]
ADD CONSTRAINT [PK_Players]
    PRIMARY KEY CLUSTERED ([PlayerId] ASC);
GO

-- Creating primary key on [GameId] in table 'Games'
ALTER TABLE [dbo].[Games]
ADD CONSTRAINT [PK_Games]
    PRIMARY KEY CLUSTERED ([GameId] ASC);
GO

-- Creating primary key on [ConversationId] in table 'Conversations'
ALTER TABLE [dbo].[Conversations]
ADD CONSTRAINT [PK_Conversations]
    PRIMARY KEY CLUSTERED ([ConversationId] ASC);
GO

-- Creating primary key on [Conversations_ConversationId], [Players_PlayerId] in table 'ConversationPlayerParticipation'
ALTER TABLE [dbo].[ConversationPlayerParticipation]
ADD CONSTRAINT [PK_ConversationPlayerParticipation]
    PRIMARY KEY NONCLUSTERED ([Conversations_ConversationId], [Players_PlayerId] ASC);
GO

-- Creating primary key on [PlayerPlayer_Player1_PlayerId], [Friends_PlayerId] in table 'Friendships'
ALTER TABLE [dbo].[Friendships]
ADD CONSTRAINT [PK_Friendships]
    PRIMARY KEY NONCLUSTERED ([PlayerPlayer_Player1_PlayerId], [Friends_PlayerId] ASC);
GO

-- Creating primary key on [PlayersRequestingFriendship_PlayerId], [FriendshipRequestedPlayers_PlayerId] in table 'FriendshipRequests'
ALTER TABLE [dbo].[FriendshipRequests]
ADD CONSTRAINT [PK_FriendshipRequests]
    PRIMARY KEY NONCLUSTERED ([PlayersRequestingFriendship_PlayerId], [FriendshipRequestedPlayers_PlayerId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Player_PlayerId] in table 'Accounts'
ALTER TABLE [dbo].[Accounts]
ADD CONSTRAINT [FK_AccountPlayer]
    FOREIGN KEY ([Player_PlayerId])
    REFERENCES [dbo].[Players]
        ([PlayerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AccountPlayer'
CREATE INDEX [IX_FK_AccountPlayer]
ON [dbo].[Accounts]
    ([Player_PlayerId]);
GO

-- Creating foreign key on [Conversations_ConversationId] in table 'ConversationPlayerParticipation'
ALTER TABLE [dbo].[ConversationPlayerParticipation]
ADD CONSTRAINT [FK_ConversationPlayer_Conversation]
    FOREIGN KEY ([Conversations_ConversationId])
    REFERENCES [dbo].[Conversations]
        ([ConversationId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Players_PlayerId] in table 'ConversationPlayerParticipation'
ALTER TABLE [dbo].[ConversationPlayerParticipation]
ADD CONSTRAINT [FK_ConversationPlayer_Player]
    FOREIGN KEY ([Players_PlayerId])
    REFERENCES [dbo].[Players]
        ([PlayerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ConversationPlayer_Player'
CREATE INDEX [IX_FK_ConversationPlayer_Player]
ON [dbo].[ConversationPlayerParticipation]
    ([Players_PlayerId]);
GO

-- Creating foreign key on [HostPlayerId] in table 'Conversations'
ALTER TABLE [dbo].[Conversations]
ADD CONSTRAINT [FK_PlayerConversation]
    FOREIGN KEY ([HostPlayerId])
    REFERENCES [dbo].[Players]
        ([PlayerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PlayerConversation'
CREATE INDEX [IX_FK_PlayerConversation]
ON [dbo].[Conversations]
    ([HostPlayerId]);
GO

-- Creating foreign key on [WinnerPlayerId] in table 'Games'
ALTER TABLE [dbo].[Games]
ADD CONSTRAINT [FK_PlayerGame]
    FOREIGN KEY ([WinnerPlayerId])
    REFERENCES [dbo].[Players]
        ([PlayerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PlayerGame'
CREATE INDEX [IX_FK_PlayerGame]
ON [dbo].[Games]
    ([WinnerPlayerId]);
GO

-- Creating foreign key on [Conversation_ConversationId] in table 'Games'
ALTER TABLE [dbo].[Games]
ADD CONSTRAINT [FK_ConversationGame]
    FOREIGN KEY ([Conversation_ConversationId])
    REFERENCES [dbo].[Conversations]
        ([ConversationId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ConversationGame'
CREATE INDEX [IX_FK_ConversationGame]
ON [dbo].[Games]
    ([Conversation_ConversationId]);
GO

-- Creating foreign key on [PlayerPlayer_Player1_PlayerId] in table 'Friendships'
ALTER TABLE [dbo].[Friendships]
ADD CONSTRAINT [FK_PlayerPlayer_Player]
    FOREIGN KEY ([PlayerPlayer_Player1_PlayerId])
    REFERENCES [dbo].[Players]
        ([PlayerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Friends_PlayerId] in table 'Friendships'
ALTER TABLE [dbo].[Friendships]
ADD CONSTRAINT [FK_PlayerPlayer_Player1]
    FOREIGN KEY ([Friends_PlayerId])
    REFERENCES [dbo].[Players]
        ([PlayerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PlayerPlayer_Player1'
CREATE INDEX [IX_FK_PlayerPlayer_Player1]
ON [dbo].[Friendships]
    ([Friends_PlayerId]);
GO

-- Creating foreign key on [PlayersRequestingFriendship_PlayerId] in table 'FriendshipRequests'
ALTER TABLE [dbo].[FriendshipRequests]
ADD CONSTRAINT [FK_FriendshipRequests_Player]
    FOREIGN KEY ([PlayersRequestingFriendship_PlayerId])
    REFERENCES [dbo].[Players]
        ([PlayerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [FriendshipRequestedPlayers_PlayerId] in table 'FriendshipRequests'
ALTER TABLE [dbo].[FriendshipRequests]
ADD CONSTRAINT [FK_FriendshipRequests_Player1]
    FOREIGN KEY ([FriendshipRequestedPlayers_PlayerId])
    REFERENCES [dbo].[Players]
        ([PlayerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_FriendshipRequests_Player1'
CREATE INDEX [IX_FK_FriendshipRequests_Player1]
ON [dbo].[FriendshipRequests]
    ([FriendshipRequestedPlayers_PlayerId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------