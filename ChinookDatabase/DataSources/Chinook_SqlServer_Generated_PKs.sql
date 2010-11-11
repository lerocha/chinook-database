/*******************************************************************************
   Chinook Database - Version 1.2
   Script: Chinook_SqlServer_Generated_PKs.sql
   Description: Creates and populates the Chinook database.
   DB Server: SqlServer
   Author: Luis Rocha
   License: http://www.codeplex.com/ChinookDatabase/license
********************************************************************************/

/*******************************************************************************
   Drop database if it exists
********************************************************************************/
IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'Chinook')
BEGIN
	ALTER DATABASE [Chinook] SET OFFLINE WITH ROLLBACK IMMEDIATE;
	ALTER DATABASE [Chinook] SET ONLINE;
	DROP DATABASE [Chinook];
END


/*******************************************************************************
   Create database
********************************************************************************/
CREATE DATABASE [Chinook];
USE [Chinook];

/*******************************************************************************
   Create Tables
********************************************************************************/
CREATE TABLE [dbo].[Album]
(
    [AlbumId] INT NOT NULL IDENTITY,
    [Title] NVARCHAR(160) NOT NULL,
    [ArtistId] INT NOT NULL,
    CONSTRAINT [PK_Album] PRIMARY KEY CLUSTERED ([AlbumId])
);
GO
CREATE TABLE [dbo].[Artist]
(
    [ArtistId] INT NOT NULL IDENTITY,
    [Name] NVARCHAR(120),
    CONSTRAINT [PK_Artist] PRIMARY KEY CLUSTERED ([ArtistId])
);
GO
CREATE TABLE [dbo].[Customer]
(
    [CustomerId] INT NOT NULL IDENTITY,
    [FirstName] NVARCHAR(40) NOT NULL,
    [LastName] NVARCHAR(20) NOT NULL,
    [Company] NVARCHAR(80),
    [Address] NVARCHAR(70),
    [City] NVARCHAR(40),
    [State] NVARCHAR(40),
    [Country] NVARCHAR(40),
    [PostalCode] NVARCHAR(10),
    [Phone] NVARCHAR(24),
    [Fax] NVARCHAR(24),
    [Email] NVARCHAR(60) NOT NULL,
    [SupportRepId] INT,
    CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED ([CustomerId])
);
GO
CREATE TABLE [dbo].[Employee]
(
    [EmployeeId] INT NOT NULL IDENTITY,
    [LastName] NVARCHAR(20) NOT NULL,
    [FirstName] NVARCHAR(20) NOT NULL,
    [Title] NVARCHAR(30),
    [ReportsTo] INT,
    [BirthDate] DATETIME,
    [HireDate] DATETIME,
    [Address] NVARCHAR(70),
    [City] NVARCHAR(40),
    [State] NVARCHAR(40),
    [Country] NVARCHAR(40),
    [PostalCode] NVARCHAR(10),
    [Phone] NVARCHAR(24),
    [Fax] NVARCHAR(24),
    [Email] NVARCHAR(60),
    CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED ([EmployeeId])
);
GO
CREATE TABLE [dbo].[Genre]
(
    [GenreId] INT NOT NULL IDENTITY,
    [Name] NVARCHAR(120),
    CONSTRAINT [PK_Genre] PRIMARY KEY CLUSTERED ([GenreId])
);
GO
CREATE TABLE [dbo].[Invoice]
(
    [InvoiceId] INT NOT NULL IDENTITY,
    [CustomerId] INT NOT NULL,
    [InvoiceDate] DATETIME NOT NULL,
    [BillingAddress] NVARCHAR(70),
    [BillingCity] NVARCHAR(40),
    [BillingState] NVARCHAR(40),
    [BillingCountry] NVARCHAR(40),
    [BillingPostalCode] NVARCHAR(10),
    [Total] NUMERIC(10,2) NOT NULL,
    CONSTRAINT [PK_Invoice] PRIMARY KEY CLUSTERED ([InvoiceId])
);
GO
CREATE TABLE [dbo].[InvoiceLine]
(
    [InvoiceLineId] INT NOT NULL IDENTITY,
    [InvoiceId] INT NOT NULL,
    [TrackId] INT NOT NULL,
    [UnitPrice] NUMERIC(10,2) NOT NULL,
    [Quantity] INT NOT NULL,
    CONSTRAINT [PK_InvoiceLine] PRIMARY KEY CLUSTERED ([InvoiceLineId])
);
GO
CREATE TABLE [dbo].[MediaType]
(
    [MediaTypeId] INT NOT NULL IDENTITY,
    [Name] NVARCHAR(120),
    CONSTRAINT [PK_MediaType] PRIMARY KEY CLUSTERED ([MediaTypeId])
);
GO
CREATE TABLE [dbo].[Playlist]
(
    [PlaylistId] INT NOT NULL IDENTITY,
    [Name] NVARCHAR(120),
    CONSTRAINT [PK_Playlist] PRIMARY KEY CLUSTERED ([PlaylistId])
);
GO
CREATE TABLE [dbo].[PlaylistTrack]
(
    [PlaylistId] INT NOT NULL,
    [TrackId] INT NOT NULL,
    CONSTRAINT [PK_PlaylistTrack] PRIMARY KEY NONCLUSTERED ([PlaylistId], [TrackId])
);
GO
CREATE TABLE [dbo].[Track]
(
    [TrackId] INT NOT NULL IDENTITY,
    [Name] NVARCHAR(200) NOT NULL,
    [AlbumId] INT,
    [MediaTypeId] INT NOT NULL,
    [GenreId] INT,
    [Composer] NVARCHAR(220),
    [Milliseconds] INT NOT NULL,
    [Bytes] INT,
    [UnitPrice] NUMERIC(10,2) NOT NULL,
    CONSTRAINT [PK_Track] PRIMARY KEY CLUSTERED ([TrackId])
);
GO

/*******************************************************************************
   Create Primary Key Unique Indexes
********************************************************************************/
CREATE UNIQUE INDEX [IPK_Album] ON [dbo].[Album]([AlbumId]);
GO
CREATE UNIQUE INDEX [IPK_Artist] ON [dbo].[Artist]([ArtistId]);
GO
CREATE UNIQUE INDEX [IPK_Customer] ON [dbo].[Customer]([CustomerId]);
GO
CREATE UNIQUE INDEX [IPK_Employee] ON [dbo].[Employee]([EmployeeId]);
GO
CREATE UNIQUE INDEX [IPK_Genre] ON [dbo].[Genre]([GenreId]);
GO
CREATE UNIQUE INDEX [IPK_Invoice] ON [dbo].[Invoice]([InvoiceId]);
GO
CREATE UNIQUE INDEX [IPK_InvoiceLine] ON [dbo].[InvoiceLine]([InvoiceLineId]);
GO
CREATE UNIQUE INDEX [IPK_MediaType] ON [dbo].[MediaType]([MediaTypeId]);
GO
CREATE UNIQUE INDEX [IPK_Playlist] ON [dbo].[Playlist]([PlaylistId]);
GO
CREATE UNIQUE INDEX [IPK_PlaylistTrack] ON [dbo].[PlaylistTrack]([PlaylistId], [TrackId]);
GO
CREATE UNIQUE INDEX [IPK_Track] ON [dbo].[Track]([TrackId]);
GO

/*******************************************************************************
   Create Foreign Keys
********************************************************************************/
ALTER TABLE [dbo].[Album] ADD CONSTRAINT [FK_AlbumArtistId_ArtistArtistId]
    FOREIGN KEY ([ArtistId]) REFERENCES [dbo].[Artist] ([ArtistId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX [IFK_AlbumArtistId_ArtistArtistId] ON [dbo].[Album] ([ArtistId]);
GO

ALTER TABLE [dbo].[Customer] ADD CONSTRAINT [FK_CustomerSupportRepId_EmployeeEmployeeId]
    FOREIGN KEY ([SupportRepId]) REFERENCES [dbo].[Employee] ([EmployeeId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX [IFK_CustomerSupportRepId_EmployeeEmployeeId] ON [dbo].[Customer] ([SupportRepId]);
GO

ALTER TABLE [dbo].[Employee] ADD CONSTRAINT [FK_EmployeeReportsTo_EmployeeEmployeeId]
    FOREIGN KEY ([ReportsTo]) REFERENCES [dbo].[Employee] ([EmployeeId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX [IFK_EmployeeReportsTo_EmployeeEmployeeId] ON [dbo].[Employee] ([ReportsTo]);
GO

ALTER TABLE [dbo].[Invoice] ADD CONSTRAINT [FK_InvoiceCustomerId_CustomerCustomerId]
    FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customer] ([CustomerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX [IFK_InvoiceCustomerId_CustomerCustomerId] ON [dbo].[Invoice] ([CustomerId]);
GO

ALTER TABLE [dbo].[InvoiceLine] ADD CONSTRAINT [FK_InvoiceLineInvoiceId_InvoiceInvoiceId]
    FOREIGN KEY ([InvoiceId]) REFERENCES [dbo].[Invoice] ([InvoiceId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX [IFK_InvoiceLineInvoiceId_InvoiceInvoiceId] ON [dbo].[InvoiceLine] ([InvoiceId]);
GO

ALTER TABLE [dbo].[InvoiceLine] ADD CONSTRAINT [FK_InvoiceLineTrackId_TrackTrackId]
    FOREIGN KEY ([TrackId]) REFERENCES [dbo].[Track] ([TrackId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX [IFK_InvoiceLineTrackId_TrackTrackId] ON [dbo].[InvoiceLine] ([TrackId]);
GO

ALTER TABLE [dbo].[PlaylistTrack] ADD CONSTRAINT [FK_PlaylistTrackPlaylistId_PlaylistPlaylistId]
    FOREIGN KEY ([PlaylistId]) REFERENCES [dbo].[Playlist] ([PlaylistId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[PlaylistTrack] ADD CONSTRAINT [FK_PlaylistTrackTrackId_TrackTrackId]
    FOREIGN KEY ([TrackId]) REFERENCES [dbo].[Track] ([TrackId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX [IFK_PlaylistTrackTrackId_TrackTrackId] ON [dbo].[PlaylistTrack] ([TrackId]);
GO

ALTER TABLE [dbo].[Track] ADD CONSTRAINT [FK_TrackAlbumId_AlbumAlbumId]
    FOREIGN KEY ([AlbumId]) REFERENCES [dbo].[Album] ([AlbumId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX [IFK_TrackAlbumId_AlbumAlbumId] ON [dbo].[Track] ([AlbumId]);
GO

ALTER TABLE [dbo].[Track] ADD CONSTRAINT [FK_TrackGenreId_GenreGenreId]
    FOREIGN KEY ([GenreId]) REFERENCES [dbo].[Genre] ([GenreId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX [IFK_TrackGenreId_GenreGenreId] ON [dbo].[Track] ([GenreId]);
GO

ALTER TABLE [dbo].[Track] ADD CONSTRAINT [FK_TrackMediaTypeId_MediaTypeMediaTypeId]
    FOREIGN KEY ([MediaTypeId]) REFERENCES [dbo].[MediaType] ([MediaTypeId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX [IFK_TrackMediaTypeId_MediaTypeMediaTypeId] ON [dbo].[Track] ([MediaTypeId]);
GO



/*******************************************************************************
   Populate Tables
********************************************************************************/

-- TODO

