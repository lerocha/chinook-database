/*******************************************************************************
   Chinook Database - Version 1.2
   Script: Chinook_EffiProz.sql
   Description: Creates and populates the Chinook database.
   DB Server: EffiProz
   Author: Luis Rocha
   License: http://www.codeplex.com/ChinookDatabase/license
********************************************************************************/

/*******************************************************************************
   Drop database if it exists
********************************************************************************/


/*******************************************************************************
   Create database
********************************************************************************/
CREATE SCHEMA "dbo" AUTHORIZATION DBA;


/*******************************************************************************
   Create Tables
********************************************************************************/
CREATE TABLE "dbo"."Album"
(
    "AlbumId" INT NOT NULL,
    "Title" NVARCHAR(160) NOT NULL,
    "ArtistId" INT NOT NULL
);

CREATE TABLE "dbo"."Artist"
(
    "ArtistId" INT NOT NULL,
    "Name" NVARCHAR(120)
);

CREATE TABLE "dbo"."Customer"
(
    "CustomerId" INT NOT NULL,
    "FirstName" NVARCHAR(40) NOT NULL,
    "LastName" NVARCHAR(20) NOT NULL,
    "Company" NVARCHAR(80),
    "Address" NVARCHAR(70),
    "City" NVARCHAR(40),
    "State" NVARCHAR(40),
    "Country" NVARCHAR(40),
    "PostalCode" NVARCHAR(10),
    "Phone" NVARCHAR(24),
    "Fax" NVARCHAR(24),
    "Email" NVARCHAR(60) NOT NULL,
    "SupportRepId" INT
);

CREATE TABLE "dbo"."Employee"
(
    "EmployeeId" INT NOT NULL,
    "LastName" NVARCHAR(20) NOT NULL,
    "FirstName" NVARCHAR(20) NOT NULL,
    "Title" NVARCHAR(30),
    "ReportsTo" INT,
    "BirthDate" DATETIME,
    "HireDate" DATETIME,
    "Address" NVARCHAR(70),
    "City" NVARCHAR(40),
    "State" NVARCHAR(40),
    "Country" NVARCHAR(40),
    "PostalCode" NVARCHAR(10),
    "Phone" NVARCHAR(24),
    "Fax" NVARCHAR(24),
    "Email" NVARCHAR(60)
);

CREATE TABLE "dbo"."Genre"
(
    "GenreId" INT NOT NULL,
    "Name" NVARCHAR(120)
);

CREATE TABLE "dbo"."Invoice"
(
    "InvoiceId" INT NOT NULL,
    "CustomerId" INT NOT NULL,
    "InvoiceDate" DATETIME NOT NULL,
    "BillingAddress" NVARCHAR(70),
    "BillingCity" NVARCHAR(40),
    "BillingState" NVARCHAR(40),
    "BillingCountry" NVARCHAR(40),
    "BillingPostalCode" NVARCHAR(10),
    "Total" NUMERIC(10,2) NOT NULL
);

CREATE TABLE "dbo"."InvoiceLine"
(
    "InvoiceLineId" INT NOT NULL,
    "InvoiceId" INT NOT NULL,
    "TrackId" INT NOT NULL,
    "UnitPrice" NUMERIC(10,2) NOT NULL,
    "Quantity" INT NOT NULL
);

CREATE TABLE "dbo"."MediaType"
(
    "MediaTypeId" INT NOT NULL,
    "Name" NVARCHAR(120)
);

CREATE TABLE "dbo"."Playlist"
(
    "PlaylistId" INT NOT NULL,
    "Name" NVARCHAR(120)
);

CREATE TABLE "dbo"."PlaylistTrack"
(
    "PlaylistId" INT NOT NULL,
    "TrackId" INT NOT NULL
);

CREATE TABLE "dbo"."Track"
(
    "TrackId" INT NOT NULL,
    "Name" NVARCHAR(200) NOT NULL,
    "AlbumId" INT,
    "MediaTypeId" INT NOT NULL,
    "GenreId" INT,
    "Composer" NVARCHAR(220),
    "Milliseconds" INT NOT NULL,
    "Bytes" INT,
    "UnitPrice" NUMERIC(10,2) NOT NULL
);


/*******************************************************************************
   Create Primary Key Unique Indexes
********************************************************************************/
ALTER TABLE "dbo"."Album" ADD CONSTRAINT "PK_Album" PRIMARY KEY ("AlbumId");
CREATE UNIQUE INDEX "IPK_Album" ON "dbo"."Album"("AlbumId");

ALTER TABLE "dbo"."Artist" ADD CONSTRAINT "PK_Artist" PRIMARY KEY ("ArtistId");
CREATE UNIQUE INDEX "IPK_Artist" ON "dbo"."Artist"("ArtistId");

ALTER TABLE "dbo"."Customer" ADD CONSTRAINT "PK_Customer" PRIMARY KEY ("CustomerId");
CREATE UNIQUE INDEX "IPK_Customer" ON "dbo"."Customer"("CustomerId");

ALTER TABLE "dbo"."Employee" ADD CONSTRAINT "PK_Employee" PRIMARY KEY ("EmployeeId");
CREATE UNIQUE INDEX "IPK_Employee" ON "dbo"."Employee"("EmployeeId");

ALTER TABLE "dbo"."Genre" ADD CONSTRAINT "PK_Genre" PRIMARY KEY ("GenreId");
CREATE UNIQUE INDEX "IPK_Genre" ON "dbo"."Genre"("GenreId");

ALTER TABLE "dbo"."Invoice" ADD CONSTRAINT "PK_Invoice" PRIMARY KEY ("InvoiceId");
CREATE UNIQUE INDEX "IPK_Invoice" ON "dbo"."Invoice"("InvoiceId");

ALTER TABLE "dbo"."InvoiceLine" ADD CONSTRAINT "PK_InvoiceLine" PRIMARY KEY ("InvoiceLineId");
CREATE UNIQUE INDEX "IPK_InvoiceLine" ON "dbo"."InvoiceLine"("InvoiceLineId");

ALTER TABLE "dbo"."MediaType" ADD CONSTRAINT "PK_MediaType" PRIMARY KEY ("MediaTypeId");
CREATE UNIQUE INDEX "IPK_MediaType" ON "dbo"."MediaType"("MediaTypeId");

ALTER TABLE "dbo"."Playlist" ADD CONSTRAINT "PK_Playlist" PRIMARY KEY ("PlaylistId");
CREATE UNIQUE INDEX "IPK_Playlist" ON "dbo"."Playlist"("PlaylistId");

ALTER TABLE "dbo"."PlaylistTrack" ADD CONSTRAINT "PK_PlaylistTrack" PRIMARY KEY ("PlaylistId", "TrackId");
CREATE UNIQUE INDEX "IPK_PlaylistTrack" ON "dbo"."PlaylistTrack"("PlaylistId", "TrackId");

ALTER TABLE "dbo"."Track" ADD CONSTRAINT "PK_Track" PRIMARY KEY ("TrackId");
CREATE UNIQUE INDEX "IPK_Track" ON "dbo"."Track"("TrackId");


/*******************************************************************************
   Create Foreign Keys
********************************************************************************/
ALTER TABLE "dbo"."Album" ADD CONSTRAINT "FK_AlbumArtistId_ArtistArtistId"
    FOREIGN KEY ("ArtistId") REFERENCES "dbo"."Artist" ("ArtistId")
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX "IFK_AlbumArtistId_ArtistArtistId" ON "dbo"."Album" ("ArtistId");


ALTER TABLE "dbo"."Customer" ADD CONSTRAINT "FK_CustomerSupportRepId_EmployeeEmployeeId"
    FOREIGN KEY ("SupportRepId") REFERENCES "dbo"."Employee" ("EmployeeId")
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX "IFK_CustomerSupportRepId_EmployeeEmployeeId" ON "dbo"."Customer" ("SupportRepId");


ALTER TABLE "dbo"."Employee" ADD CONSTRAINT "FK_EmployeeReportsTo_EmployeeEmployeeId"
    FOREIGN KEY ("ReportsTo") REFERENCES "dbo"."Employee" ("EmployeeId")
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX "IFK_EmployeeReportsTo_EmployeeEmployeeId" ON "dbo"."Employee" ("ReportsTo");


ALTER TABLE "dbo"."Invoice" ADD CONSTRAINT "FK_InvoiceCustomerId_CustomerCustomerId"
    FOREIGN KEY ("CustomerId") REFERENCES "dbo"."Customer" ("CustomerId")
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX "IFK_InvoiceCustomerId_CustomerCustomerId" ON "dbo"."Invoice" ("CustomerId");


ALTER TABLE "dbo"."InvoiceLine" ADD CONSTRAINT "FK_InvoiceLineInvoiceId_InvoiceInvoiceId"
    FOREIGN KEY ("InvoiceId") REFERENCES "dbo"."Invoice" ("InvoiceId")
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX "IFK_InvoiceLineInvoiceId_InvoiceInvoiceId" ON "dbo"."InvoiceLine" ("InvoiceId");


ALTER TABLE "dbo"."InvoiceLine" ADD CONSTRAINT "FK_InvoiceLineTrackId_TrackTrackId"
    FOREIGN KEY ("TrackId") REFERENCES "dbo"."Track" ("TrackId")
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX "IFK_InvoiceLineTrackId_TrackTrackId" ON "dbo"."InvoiceLine" ("TrackId");


ALTER TABLE "dbo"."PlaylistTrack" ADD CONSTRAINT "FK_PlaylistTrackPlaylistId_PlaylistPlaylistId"
    FOREIGN KEY ("PlaylistId") REFERENCES "dbo"."Playlist" ("PlaylistId")
    ON DELETE NO ACTION ON UPDATE NO ACTION;


ALTER TABLE "dbo"."PlaylistTrack" ADD CONSTRAINT "FK_PlaylistTrackTrackId_TrackTrackId"
    FOREIGN KEY ("TrackId") REFERENCES "dbo"."Track" ("TrackId")
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX "IFK_PlaylistTrackTrackId_TrackTrackId" ON "dbo"."PlaylistTrack" ("TrackId");


ALTER TABLE "dbo"."Track" ADD CONSTRAINT "FK_TrackAlbumId_AlbumAlbumId"
    FOREIGN KEY ("AlbumId") REFERENCES "dbo"."Album" ("AlbumId")
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX "IFK_TrackAlbumId_AlbumAlbumId" ON "dbo"."Track" ("AlbumId");


ALTER TABLE "dbo"."Track" ADD CONSTRAINT "FK_TrackGenreId_GenreGenreId"
    FOREIGN KEY ("GenreId") REFERENCES "dbo"."Genre" ("GenreId")
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX "IFK_TrackGenreId_GenreGenreId" ON "dbo"."Track" ("GenreId");


ALTER TABLE "dbo"."Track" ADD CONSTRAINT "FK_TrackMediaTypeId_MediaTypeMediaTypeId"
    FOREIGN KEY ("MediaTypeId") REFERENCES "dbo"."MediaType" ("MediaTypeId")
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX "IFK_TrackMediaTypeId_MediaTypeMediaTypeId" ON "dbo"."Track" ("MediaTypeId");




/*******************************************************************************
   Populate Tables
********************************************************************************/

-- TODO

