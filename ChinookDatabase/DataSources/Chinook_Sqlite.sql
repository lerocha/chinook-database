/*******************************************************************************
   Chinook Database - Version 1.2
   Script: Chinook_Sqlite.sql
   Description: Creates and populates the Chinook database.
   DB Server: Sqlite
   Author: Luis Rocha
   License: http://www.codeplex.com/ChinookDatabase/license
********************************************************************************/

/*******************************************************************************
   Drop Foreign Keys Constraints
********************************************************************************/























/*******************************************************************************
   Drop Tables
********************************************************************************/
DROP TABLE IF EXISTS [Album];

DROP TABLE IF EXISTS [Artist];

DROP TABLE IF EXISTS [Customer];

DROP TABLE IF EXISTS [Employee];

DROP TABLE IF EXISTS [Genre];

DROP TABLE IF EXISTS [Invoice];

DROP TABLE IF EXISTS [InvoiceLine];

DROP TABLE IF EXISTS [MediaType];

DROP TABLE IF EXISTS [Playlist];

DROP TABLE IF EXISTS [PlaylistTrack];

DROP TABLE IF EXISTS [Track];


/*******************************************************************************
   Create Tables
********************************************************************************/
CREATE TABLE [Album]
(
    [AlbumId] INT NOT NULL,
    [Title] NVARCHAR(160) NOT NULL,
    [ArtistId] INT NOT NULL,
    CONSTRAINT [PK_Album] PRIMARY KEY  ([AlbumId])
    -- FOREIGN KEY ...
);

CREATE TABLE [Artist]
(
    [ArtistId] INT NOT NULL,
    [Name] NVARCHAR(120),
    CONSTRAINT [PK_Artist] PRIMARY KEY  ([ArtistId])
    -- FOREIGN KEY ...
);

CREATE TABLE [Customer]
(
    [CustomerId] INT NOT NULL,
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
    CONSTRAINT [PK_Customer] PRIMARY KEY  ([CustomerId])
    -- FOREIGN KEY ...
);

CREATE TABLE [Employee]
(
    [EmployeeId] INT NOT NULL,
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
    CONSTRAINT [PK_Employee] PRIMARY KEY  ([EmployeeId])
    -- FOREIGN KEY ...
);

CREATE TABLE [Genre]
(
    [GenreId] INT NOT NULL,
    [Name] NVARCHAR(120),
    CONSTRAINT [PK_Genre] PRIMARY KEY  ([GenreId])
    -- FOREIGN KEY ...
);

CREATE TABLE [Invoice]
(
    [InvoiceId] INT NOT NULL,
    [CustomerId] INT NOT NULL,
    [InvoiceDate] DATETIME NOT NULL,
    [BillingAddress] NVARCHAR(70),
    [BillingCity] NVARCHAR(40),
    [BillingState] NVARCHAR(40),
    [BillingCountry] NVARCHAR(40),
    [BillingPostalCode] NVARCHAR(10),
    [Total] NUMERIC(10,2) NOT NULL,
    CONSTRAINT [PK_Invoice] PRIMARY KEY  ([InvoiceId])
    -- FOREIGN KEY ...
);

CREATE TABLE [InvoiceLine]
(
    [InvoiceLineId] INT NOT NULL,
    [InvoiceId] INT NOT NULL,
    [TrackId] INT NOT NULL,
    [UnitPrice] NUMERIC(10,2) NOT NULL,
    [Quantity] INT NOT NULL,
    CONSTRAINT [PK_InvoiceLine] PRIMARY KEY  ([InvoiceLineId])
    -- FOREIGN KEY ...
);

CREATE TABLE [MediaType]
(
    [MediaTypeId] INT NOT NULL,
    [Name] NVARCHAR(120),
    CONSTRAINT [PK_MediaType] PRIMARY KEY  ([MediaTypeId])
    -- FOREIGN KEY ...
);

CREATE TABLE [Playlist]
(
    [PlaylistId] INT NOT NULL,
    [Name] NVARCHAR(120),
    CONSTRAINT [PK_Playlist] PRIMARY KEY  ([PlaylistId])
    -- FOREIGN KEY ...
);

CREATE TABLE [PlaylistTrack]
(
    [PlaylistId] INT NOT NULL,
    [TrackId] INT NOT NULL,
    CONSTRAINT [PK_PlaylistTrack] PRIMARY KEY  ([PlaylistId], [TrackId])
    -- FOREIGN KEY ...
);

CREATE TABLE [Track]
(
    [TrackId] INT NOT NULL,
    [Name] NVARCHAR(200) NOT NULL,
    [AlbumId] INT,
    [MediaTypeId] INT NOT NULL,
    [GenreId] INT,
    [Composer] NVARCHAR(220),
    [Milliseconds] INT NOT NULL,
    [Bytes] INT,
    [UnitPrice] NUMERIC(10,2) NOT NULL,
    CONSTRAINT [PK_Track] PRIMARY KEY  ([TrackId])
    -- FOREIGN KEY ...
);


/*******************************************************************************
   Create Primary Key Unique Indexes
********************************************************************************/
CREATE UNIQUE INDEX [IPK_Album] ON [Album]([AlbumId]);

CREATE UNIQUE INDEX [IPK_Artist] ON [Artist]([ArtistId]);

CREATE UNIQUE INDEX [IPK_Customer] ON [Customer]([CustomerId]);

CREATE UNIQUE INDEX [IPK_Employee] ON [Employee]([EmployeeId]);

CREATE UNIQUE INDEX [IPK_Genre] ON [Genre]([GenreId]);

CREATE UNIQUE INDEX [IPK_Invoice] ON [Invoice]([InvoiceId]);

CREATE UNIQUE INDEX [IPK_InvoiceLine] ON [InvoiceLine]([InvoiceLineId]);

CREATE UNIQUE INDEX [IPK_MediaType] ON [MediaType]([MediaTypeId]);

CREATE UNIQUE INDEX [IPK_Playlist] ON [Playlist]([PlaylistId]);

CREATE UNIQUE INDEX [IPK_PlaylistTrack] ON [PlaylistTrack]([PlaylistId], [TrackId]);

CREATE UNIQUE INDEX [IPK_Track] ON [Track]([TrackId]);


/*******************************************************************************
   Create Foreign Keys
********************************************************************************/

CREATE INDEX [IFK_AlbumArtistId_ArtistArtistId] ON [Album] ([ArtistId]);



CREATE INDEX [IFK_CustomerSupportRepId_EmployeeEmployeeId] ON [Customer] ([SupportRepId]);



CREATE INDEX [IFK_EmployeeReportsTo_EmployeeEmployeeId] ON [Employee] ([ReportsTo]);



CREATE INDEX [IFK_InvoiceCustomerId_CustomerCustomerId] ON [Invoice] ([CustomerId]);



CREATE INDEX [IFK_InvoiceLineInvoiceId_InvoiceInvoiceId] ON [InvoiceLine] ([InvoiceId]);



CREATE INDEX [IFK_InvoiceLineTrackId_TrackTrackId] ON [InvoiceLine] ([TrackId]);





CREATE INDEX [IFK_PlaylistTrackTrackId_TrackTrackId] ON [PlaylistTrack] ([TrackId]);



CREATE INDEX [IFK_TrackAlbumId_AlbumAlbumId] ON [Track] ([AlbumId]);



CREATE INDEX [IFK_TrackGenreId_GenreGenreId] ON [Track] ([GenreId]);



CREATE INDEX [IFK_TrackMediaTypeId_MediaTypeMediaTypeId] ON [Track] ([MediaTypeId]);




/*******************************************************************************
   Populate Tables
********************************************************************************/

-- TODO

