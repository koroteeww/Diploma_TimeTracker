
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 03/21/2013 18:47:01
-- Generated from EDMX file: C:\diplom\project3\WebExplorer\Entity\WeContext.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Diploma];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_SiteUserNews]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NewsSet] DROP CONSTRAINT [FK_SiteUserNews];
GO
IF OBJECT_ID(N'[dbo].[FK_NewsNewsComments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NewsCommentsSet] DROP CONSTRAINT [FK_NewsNewsComments];
GO
IF OBJECT_ID(N'[dbo].[FK_SiteUserNewsComments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NewsCommentsSet] DROP CONSTRAINT [FK_SiteUserNewsComments];
GO
IF OBJECT_ID(N'[dbo].[FK_DepartmentDepartment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DepartmentSet] DROP CONSTRAINT [FK_DepartmentDepartment];
GO
IF OBJECT_ID(N'[dbo].[FK_ClientDepartment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ClientSet] DROP CONSTRAINT [FK_ClientDepartment];
GO
IF OBJECT_ID(N'[dbo].[FK_SiteUserClient]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SiteUserSet] DROP CONSTRAINT [FK_SiteUserClient];
GO
IF OBJECT_ID(N'[dbo].[FK_SiteUserSiteUserRole]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SiteUserSet] DROP CONSTRAINT [FK_SiteUserSiteUserRole];
GO
IF OBJECT_ID(N'[dbo].[FK_TaskTypesTask]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskSet] DROP CONSTRAINT [FK_TaskTypesTask];
GO
IF OBJECT_ID(N'[dbo].[FK_TaskTastStates]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskSet] DROP CONSTRAINT [FK_TaskTastStates];
GO
IF OBJECT_ID(N'[dbo].[FK_TaskTimeCostTask]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskTimeCostSet] DROP CONSTRAINT [FK_TaskTimeCostTask];
GO
IF OBJECT_ID(N'[dbo].[FK_TaskSiteUserAutor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskSet] DROP CONSTRAINT [FK_TaskSiteUserAutor];
GO
IF OBJECT_ID(N'[dbo].[FK_TaskPerformers_Task]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskPerformers] DROP CONSTRAINT [FK_TaskPerformers_Task];
GO
IF OBJECT_ID(N'[dbo].[FK_TaskPerformers_SiteUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskPerformers] DROP CONSTRAINT [FK_TaskPerformers_SiteUser];
GO
IF OBJECT_ID(N'[dbo].[FK_TaskClient_Task]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskClient] DROP CONSTRAINT [FK_TaskClient_Task];
GO
IF OBJECT_ID(N'[dbo].[FK_TaskClient_Client]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskClient] DROP CONSTRAINT [FK_TaskClient_Client];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[SiteUserSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SiteUserSet];
GO
IF OBJECT_ID(N'[dbo].[ClientSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClientSet];
GO
IF OBJECT_ID(N'[dbo].[TaskSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaskSet];
GO
IF OBJECT_ID(N'[dbo].[TaskTypesSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaskTypesSet];
GO
IF OBJECT_ID(N'[dbo].[TastStatesSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TastStatesSet];
GO
IF OBJECT_ID(N'[dbo].[SiteUserRoleSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SiteUserRoleSet];
GO
IF OBJECT_ID(N'[dbo].[DepartmentSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DepartmentSet];
GO
IF OBJECT_ID(N'[dbo].[PasswordRecoverSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PasswordRecoverSet];
GO
IF OBJECT_ID(N'[dbo].[TaskTimeCostSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaskTimeCostSet];
GO
IF OBJECT_ID(N'[dbo].[NewsCommentsSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NewsCommentsSet];
GO
IF OBJECT_ID(N'[dbo].[NewsSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NewsSet];
GO
IF OBJECT_ID(N'[dbo].[TaskPerformers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaskPerformers];
GO
IF OBJECT_ID(N'[dbo].[TaskClient]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaskClient];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'SiteUserSet'
CREATE TABLE [dbo].[SiteUserSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Email] nvarchar(255)  NOT NULL,
    [Password] nvarchar(255)  NOT NULL,
    [Hash] nvarchar(255)  NOT NULL,
    [Login] nvarchar(255)  NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [LastLoginDate] datetime  NULL,
    [IsApproved] bit  NOT NULL,
    [IsLocked] bit  NOT NULL,
    [IsAdmin] bit  NOT NULL,
    [Comment] nvarchar(500)  NULL,
    [CompanyWorker_Id] int  NOT NULL,
    [SiteUserRole_Id] int  NOT NULL
);
GO

-- Creating table 'ClientSet'
CREATE TABLE [dbo].[ClientSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(512)  NOT NULL,
    [LastName] nvarchar(512)  NOT NULL,
    [MiddleName] nvarchar(512)  NULL,
    [Location] nvarchar(1024)  NOT NULL,
    [Position] nvarchar(1024)  NULL,
    [Company] nvarchar(1024)  NOT NULL,
    [PhoneNumber] nvarchar(255)  NULL,
    [IsManager] bit  NOT NULL,
    [Comment] nvarchar(1024)  NULL,
    [Department_Id] int  NOT NULL
);
GO

-- Creating table 'TaskSet'
CREATE TABLE [dbo].[TaskSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(512)  NOT NULL,
    [Description] nvarchar(1024)  NOT NULL,
    [DateBegin] datetime  NOT NULL,
    [DateEnd] datetime  NOT NULL,
    [Comments] nvarchar(max)  NOT NULL,
    [PlannedDateEnd] datetime  NULL,
    [PlannedTimeCost] decimal(18,0)  NULL,
    [NeedToCheck] bit  NOT NULL,
    [TaskType_Id] int  NOT NULL,
    [TastState_Id] int  NOT NULL,
    [TaskAuthor_Id] int  NOT NULL
);
GO

-- Creating table 'TaskTypesSet'
CREATE TABLE [dbo].[TaskTypesSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TypeName] nvarchar(255)  NOT NULL,
    [TypeDescription] nvarchar(1024)  NULL
);
GO

-- Creating table 'TastStatesSet'
CREATE TABLE [dbo].[TastStatesSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StateName] nvarchar(255)  NOT NULL,
    [StateDescription] nvarchar(1024)  NULL
);
GO

-- Creating table 'SiteUserRoleSet'
CREATE TABLE [dbo].[SiteUserRoleSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RoleType] int  NOT NULL,
    [RoleDescription] nvarchar(512)  NOT NULL
);
GO

-- Creating table 'DepartmentSet'
CREATE TABLE [dbo].[DepartmentSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(1024)  NOT NULL,
    [ParentDepartment_Id] int  NULL
);
GO

-- Creating table 'PasswordRecoverSet'
CREATE TABLE [dbo].[PasswordRecoverSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [Hash] nvarchar(255)  NOT NULL,
    [Email] nvarchar(255)  NOT NULL
);
GO

-- Creating table 'TaskTimeCostSet'
CREATE TABLE [dbo].[TaskTimeCostSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DbDateTime] datetime  NOT NULL,
    [SpentHours] decimal(18,0)  NOT NULL,
    [NeedToCheck] bit  NOT NULL,
    [Task_Id] int  NOT NULL,
    [TimeSpender_Id] int  NOT NULL
);
GO

-- Creating table 'NewsCommentsSet'
CREATE TABLE [dbo].[NewsCommentsSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [Body] nvarchar(max)  NOT NULL,
    [News_Id] int  NOT NULL,
    [Author_Id] int  NOT NULL
);
GO

-- Creating table 'NewsSet'
CREATE TABLE [dbo].[NewsSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(1000)  NOT NULL,
    [Body] nvarchar(max)  NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [Author_Id] int  NOT NULL
);
GO

-- Creating table 'TaskPerformers'
CREATE TABLE [dbo].[TaskPerformers] (
    [TasksForUser_Id] int  NOT NULL,
    [TaskPerformers_Id] int  NOT NULL
);
GO

-- Creating table 'TaskClient'
CREATE TABLE [dbo].[TaskClient] (
    [TasksForClient_Id] int  NOT NULL,
    [TaskClients_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'SiteUserSet'
ALTER TABLE [dbo].[SiteUserSet]
ADD CONSTRAINT [PK_SiteUserSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClientSet'
ALTER TABLE [dbo].[ClientSet]
ADD CONSTRAINT [PK_ClientSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TaskSet'
ALTER TABLE [dbo].[TaskSet]
ADD CONSTRAINT [PK_TaskSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TaskTypesSet'
ALTER TABLE [dbo].[TaskTypesSet]
ADD CONSTRAINT [PK_TaskTypesSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TastStatesSet'
ALTER TABLE [dbo].[TastStatesSet]
ADD CONSTRAINT [PK_TastStatesSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SiteUserRoleSet'
ALTER TABLE [dbo].[SiteUserRoleSet]
ADD CONSTRAINT [PK_SiteUserRoleSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DepartmentSet'
ALTER TABLE [dbo].[DepartmentSet]
ADD CONSTRAINT [PK_DepartmentSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PasswordRecoverSet'
ALTER TABLE [dbo].[PasswordRecoverSet]
ADD CONSTRAINT [PK_PasswordRecoverSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TaskTimeCostSet'
ALTER TABLE [dbo].[TaskTimeCostSet]
ADD CONSTRAINT [PK_TaskTimeCostSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'NewsCommentsSet'
ALTER TABLE [dbo].[NewsCommentsSet]
ADD CONSTRAINT [PK_NewsCommentsSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'NewsSet'
ALTER TABLE [dbo].[NewsSet]
ADD CONSTRAINT [PK_NewsSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [TasksForUser_Id], [TaskPerformers_Id] in table 'TaskPerformers'
ALTER TABLE [dbo].[TaskPerformers]
ADD CONSTRAINT [PK_TaskPerformers]
    PRIMARY KEY NONCLUSTERED ([TasksForUser_Id], [TaskPerformers_Id] ASC);
GO

-- Creating primary key on [TasksForClient_Id], [TaskClients_Id] in table 'TaskClient'
ALTER TABLE [dbo].[TaskClient]
ADD CONSTRAINT [PK_TaskClient]
    PRIMARY KEY NONCLUSTERED ([TasksForClient_Id], [TaskClients_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Author_Id] in table 'NewsSet'
ALTER TABLE [dbo].[NewsSet]
ADD CONSTRAINT [FK_SiteUserNews]
    FOREIGN KEY ([Author_Id])
    REFERENCES [dbo].[SiteUserSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SiteUserNews'
CREATE INDEX [IX_FK_SiteUserNews]
ON [dbo].[NewsSet]
    ([Author_Id]);
GO

-- Creating foreign key on [News_Id] in table 'NewsCommentsSet'
ALTER TABLE [dbo].[NewsCommentsSet]
ADD CONSTRAINT [FK_NewsNewsComments]
    FOREIGN KEY ([News_Id])
    REFERENCES [dbo].[NewsSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_NewsNewsComments'
CREATE INDEX [IX_FK_NewsNewsComments]
ON [dbo].[NewsCommentsSet]
    ([News_Id]);
GO

-- Creating foreign key on [Author_Id] in table 'NewsCommentsSet'
ALTER TABLE [dbo].[NewsCommentsSet]
ADD CONSTRAINT [FK_SiteUserNewsComments]
    FOREIGN KEY ([Author_Id])
    REFERENCES [dbo].[SiteUserSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SiteUserNewsComments'
CREATE INDEX [IX_FK_SiteUserNewsComments]
ON [dbo].[NewsCommentsSet]
    ([Author_Id]);
GO

-- Creating foreign key on [ParentDepartment_Id] in table 'DepartmentSet'
ALTER TABLE [dbo].[DepartmentSet]
ADD CONSTRAINT [FK_DepartmentDepartment]
    FOREIGN KEY ([ParentDepartment_Id])
    REFERENCES [dbo].[DepartmentSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DepartmentDepartment'
CREATE INDEX [IX_FK_DepartmentDepartment]
ON [dbo].[DepartmentSet]
    ([ParentDepartment_Id]);
GO

-- Creating foreign key on [Department_Id] in table 'ClientSet'
ALTER TABLE [dbo].[ClientSet]
ADD CONSTRAINT [FK_ClientDepartment]
    FOREIGN KEY ([Department_Id])
    REFERENCES [dbo].[DepartmentSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientDepartment'
CREATE INDEX [IX_FK_ClientDepartment]
ON [dbo].[ClientSet]
    ([Department_Id]);
GO

-- Creating foreign key on [CompanyWorker_Id] in table 'SiteUserSet'
ALTER TABLE [dbo].[SiteUserSet]
ADD CONSTRAINT [FK_SiteUserClient]
    FOREIGN KEY ([CompanyWorker_Id])
    REFERENCES [dbo].[ClientSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SiteUserClient'
CREATE INDEX [IX_FK_SiteUserClient]
ON [dbo].[SiteUserSet]
    ([CompanyWorker_Id]);
GO

-- Creating foreign key on [SiteUserRole_Id] in table 'SiteUserSet'
ALTER TABLE [dbo].[SiteUserSet]
ADD CONSTRAINT [FK_SiteUserSiteUserRole]
    FOREIGN KEY ([SiteUserRole_Id])
    REFERENCES [dbo].[SiteUserRoleSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SiteUserSiteUserRole'
CREATE INDEX [IX_FK_SiteUserSiteUserRole]
ON [dbo].[SiteUserSet]
    ([SiteUserRole_Id]);
GO

-- Creating foreign key on [TaskType_Id] in table 'TaskSet'
ALTER TABLE [dbo].[TaskSet]
ADD CONSTRAINT [FK_TaskTypesTask]
    FOREIGN KEY ([TaskType_Id])
    REFERENCES [dbo].[TaskTypesSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TaskTypesTask'
CREATE INDEX [IX_FK_TaskTypesTask]
ON [dbo].[TaskSet]
    ([TaskType_Id]);
GO

-- Creating foreign key on [TastState_Id] in table 'TaskSet'
ALTER TABLE [dbo].[TaskSet]
ADD CONSTRAINT [FK_TaskTastStates]
    FOREIGN KEY ([TastState_Id])
    REFERENCES [dbo].[TastStatesSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TaskTastStates'
CREATE INDEX [IX_FK_TaskTastStates]
ON [dbo].[TaskSet]
    ([TastState_Id]);
GO

-- Creating foreign key on [Task_Id] in table 'TaskTimeCostSet'
ALTER TABLE [dbo].[TaskTimeCostSet]
ADD CONSTRAINT [FK_TaskTimeCostTask]
    FOREIGN KEY ([Task_Id])
    REFERENCES [dbo].[TaskSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TaskTimeCostTask'
CREATE INDEX [IX_FK_TaskTimeCostTask]
ON [dbo].[TaskTimeCostSet]
    ([Task_Id]);
GO

-- Creating foreign key on [TaskAuthor_Id] in table 'TaskSet'
ALTER TABLE [dbo].[TaskSet]
ADD CONSTRAINT [FK_TaskSiteUserAutor]
    FOREIGN KEY ([TaskAuthor_Id])
    REFERENCES [dbo].[SiteUserSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TaskSiteUserAutor'
CREATE INDEX [IX_FK_TaskSiteUserAutor]
ON [dbo].[TaskSet]
    ([TaskAuthor_Id]);
GO

-- Creating foreign key on [TasksForUser_Id] in table 'TaskPerformers'
ALTER TABLE [dbo].[TaskPerformers]
ADD CONSTRAINT [FK_TaskPerformers_Task]
    FOREIGN KEY ([TasksForUser_Id])
    REFERENCES [dbo].[TaskSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [TaskPerformers_Id] in table 'TaskPerformers'
ALTER TABLE [dbo].[TaskPerformers]
ADD CONSTRAINT [FK_TaskPerformers_SiteUser]
    FOREIGN KEY ([TaskPerformers_Id])
    REFERENCES [dbo].[SiteUserSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TaskPerformers_SiteUser'
CREATE INDEX [IX_FK_TaskPerformers_SiteUser]
ON [dbo].[TaskPerformers]
    ([TaskPerformers_Id]);
GO

-- Creating foreign key on [TasksForClient_Id] in table 'TaskClient'
ALTER TABLE [dbo].[TaskClient]
ADD CONSTRAINT [FK_TaskClient_Task]
    FOREIGN KEY ([TasksForClient_Id])
    REFERENCES [dbo].[TaskSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [TaskClients_Id] in table 'TaskClient'
ALTER TABLE [dbo].[TaskClient]
ADD CONSTRAINT [FK_TaskClient_Client]
    FOREIGN KEY ([TaskClients_Id])
    REFERENCES [dbo].[ClientSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TaskClient_Client'
CREATE INDEX [IX_FK_TaskClient_Client]
ON [dbo].[TaskClient]
    ([TaskClients_Id]);
GO

-- Creating foreign key on [TimeSpender_Id] in table 'TaskTimeCostSet'
ALTER TABLE [dbo].[TaskTimeCostSet]
ADD CONSTRAINT [FK_TaskTimeCostSiteUser]
    FOREIGN KEY ([TimeSpender_Id])
    REFERENCES [dbo].[SiteUserSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TaskTimeCostSiteUser'
CREATE INDEX [IX_FK_TaskTimeCostSiteUser]
ON [dbo].[TaskTimeCostSet]
    ([TimeSpender_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------