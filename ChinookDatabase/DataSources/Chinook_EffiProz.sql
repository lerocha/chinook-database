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
CREATE SCHEMA "Chinook" AUTHORIZATION DBA;


/*******************************************************************************
   Create Tables
********************************************************************************/
CREATE TABLE "Chinook"."Album"
(
    "AlbumId" INT  NOT NULL,
    "Title" NVARCHAR(160)  NOT NULL,
    "ArtistId" INT  NOT NULL
);
\
CREATE TABLE "Chinook"."Artist"
(
    "ArtistId" INT  NOT NULL,
    "Name" NVARCHAR(120)
);
\
CREATE TABLE "Chinook"."Customer"
(
    "CustomerId" INT  NOT NULL,
    "FirstName" NVARCHAR(40)  NOT NULL,
    "LastName" NVARCHAR(20)  NOT NULL,
    "Company" NVARCHAR(80),
    "Address" NVARCHAR(70),
    "City" NVARCHAR(40),
    "State" NVARCHAR(40),
    "Country" NVARCHAR(40),
    "PostalCode" NVARCHAR(10),
    "Phone" NVARCHAR(24),
    "Fax" NVARCHAR(24),
    "Email" NVARCHAR(60)  NOT NULL,
    "SupportRepId" INT
);
\
CREATE TABLE "Chinook"."Employee"
(
    "EmployeeId" INT  NOT NULL,
    "LastName" NVARCHAR(20)  NOT NULL,
    "FirstName" NVARCHAR(20)  NOT NULL,
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
\
CREATE TABLE "Chinook"."Genre"
(
    "GenreId" INT  NOT NULL,
    "Name" NVARCHAR(120)
);
\
CREATE TABLE "Chinook"."Invoice"
(
    "InvoiceId" INT  NOT NULL,
    "CustomerId" INT  NOT NULL,
    "InvoiceDate" DATETIME  NOT NULL,
    "BillingAddress" NVARCHAR(70),
    "BillingCity" NVARCHAR(40),
    "BillingState" NVARCHAR(40),
    "BillingCountry" NVARCHAR(40),
    "BillingPostalCode" NVARCHAR(10),
    "Total" NUMERIC(10,2)  NOT NULL
);
\
CREATE TABLE "Chinook"."InvoiceLine"
(
    "InvoiceLineId" INT  NOT NULL,
    "InvoiceId" INT  NOT NULL,
    "TrackId" INT  NOT NULL,
    "UnitPrice" NUMERIC(10,2)  NOT NULL,
    "Quantity" INT  NOT NULL
);
\
CREATE TABLE "Chinook"."MediaType"
(
    "MediaTypeId" INT  NOT NULL,
    "Name" NVARCHAR(120)
);
\
CREATE TABLE "Chinook"."Playlist"
(
    "PlaylistId" INT  NOT NULL,
    "Name" NVARCHAR(120)
);
\
CREATE TABLE "Chinook"."PlaylistTrack"
(
    "PlaylistId" INT  NOT NULL,
    "TrackId" INT  NOT NULL
);
\
CREATE TABLE "Chinook"."Track"
(
    "TrackId" INT  NOT NULL,
    "Name" NVARCHAR(200)  NOT NULL,
    "AlbumId" INT,
    "MediaTypeId" INT  NOT NULL,
    "GenreId" INT,
    "Composer" NVARCHAR(220),
    "Milliseconds" INT  NOT NULL,
    "Bytes" INT,
    "UnitPrice" NUMERIC(10,2)  NOT NULL
);
\

/*******************************************************************************
   Create Primary Key Unique Indexes
********************************************************************************/
ALTER TABLE "Chinook"."Album" ADD CONSTRAINT "PK_Album" PRIMARY KEY ("AlbumId");
CREATE UNIQUE INDEX "IPK_Album" ON "Chinook"."Album"("AlbumId");

ALTER TABLE "Chinook"."Artist" ADD CONSTRAINT "PK_Artist" PRIMARY KEY ("ArtistId");
CREATE UNIQUE INDEX "IPK_Artist" ON "Chinook"."Artist"("ArtistId");

ALTER TABLE "Chinook"."Customer" ADD CONSTRAINT "PK_Customer" PRIMARY KEY ("CustomerId");
CREATE UNIQUE INDEX "IPK_Customer" ON "Chinook"."Customer"("CustomerId");

ALTER TABLE "Chinook"."Employee" ADD CONSTRAINT "PK_Employee" PRIMARY KEY ("EmployeeId");
CREATE UNIQUE INDEX "IPK_Employee" ON "Chinook"."Employee"("EmployeeId");

ALTER TABLE "Chinook"."Genre" ADD CONSTRAINT "PK_Genre" PRIMARY KEY ("GenreId");
CREATE UNIQUE INDEX "IPK_Genre" ON "Chinook"."Genre"("GenreId");

ALTER TABLE "Chinook"."Invoice" ADD CONSTRAINT "PK_Invoice" PRIMARY KEY ("InvoiceId");
CREATE UNIQUE INDEX "IPK_Invoice" ON "Chinook"."Invoice"("InvoiceId");

ALTER TABLE "Chinook"."InvoiceLine" ADD CONSTRAINT "PK_InvoiceLine" PRIMARY KEY ("InvoiceLineId");
CREATE UNIQUE INDEX "IPK_InvoiceLine" ON "Chinook"."InvoiceLine"("InvoiceLineId");

ALTER TABLE "Chinook"."MediaType" ADD CONSTRAINT "PK_MediaType" PRIMARY KEY ("MediaTypeId");
CREATE UNIQUE INDEX "IPK_MediaType" ON "Chinook"."MediaType"("MediaTypeId");

ALTER TABLE "Chinook"."Playlist" ADD CONSTRAINT "PK_Playlist" PRIMARY KEY ("PlaylistId");
CREATE UNIQUE INDEX "IPK_Playlist" ON "Chinook"."Playlist"("PlaylistId");

ALTER TABLE "Chinook"."PlaylistTrack" ADD CONSTRAINT "PK_PlaylistTrack" PRIMARY KEY ("PlaylistId", "TrackId");
CREATE UNIQUE INDEX "IPK_PlaylistTrack" ON "Chinook"."PlaylistTrack"("PlaylistId", "TrackId");

ALTER TABLE "Chinook"."Track" ADD CONSTRAINT "PK_Track" PRIMARY KEY ("TrackId");
CREATE UNIQUE INDEX "IPK_Track" ON "Chinook"."Track"("TrackId");


/*******************************************************************************
   Create Foreign Keys
********************************************************************************/
ALTER TABLE "Chinook"."Album" ADD CONSTRAINT "FK_AlbumArtistId_ArtistArtistId"
    FOREIGN KEY ("ArtistId") REFERENCES "Chinook"."Artist" ("ArtistId")
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX "IFK_AlbumArtistId_ArtistArtistId" ON "Chinook"."Album" ("ArtistId");


ALTER TABLE "Chinook"."Customer" ADD CONSTRAINT "FK_CustomerSupportRepId_EmployeeEmployeeId"
    FOREIGN KEY ("SupportRepId") REFERENCES "Chinook"."Employee" ("EmployeeId")
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX "IFK_CustomerSupportRepId_EmployeeEmployeeId" ON "Chinook"."Customer" ("SupportRepId");


ALTER TABLE "Chinook"."Employee" ADD CONSTRAINT "FK_EmployeeReportsTo_EmployeeEmployeeId"
    FOREIGN KEY ("ReportsTo") REFERENCES "Chinook"."Employee" ("EmployeeId")
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX "IFK_EmployeeReportsTo_EmployeeEmployeeId" ON "Chinook"."Employee" ("ReportsTo");


ALTER TABLE "Chinook"."Invoice" ADD CONSTRAINT "FK_InvoiceCustomerId_CustomerCustomerId"
    FOREIGN KEY ("CustomerId") REFERENCES "Chinook"."Customer" ("CustomerId")
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX "IFK_InvoiceCustomerId_CustomerCustomerId" ON "Chinook"."Invoice" ("CustomerId");


ALTER TABLE "Chinook"."InvoiceLine" ADD CONSTRAINT "FK_InvoiceLineInvoiceId_InvoiceInvoiceId"
    FOREIGN KEY ("InvoiceId") REFERENCES "Chinook"."Invoice" ("InvoiceId")
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX "IFK_InvoiceLineInvoiceId_InvoiceInvoiceId" ON "Chinook"."InvoiceLine" ("InvoiceId");


ALTER TABLE "Chinook"."InvoiceLine" ADD CONSTRAINT "FK_InvoiceLineTrackId_TrackTrackId"
    FOREIGN KEY ("TrackId") REFERENCES "Chinook"."Track" ("TrackId")
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX "IFK_InvoiceLineTrackId_TrackTrackId" ON "Chinook"."InvoiceLine" ("TrackId");


ALTER TABLE "Chinook"."PlaylistTrack" ADD CONSTRAINT "FK_PlaylistTrackPlaylistId_PlaylistPlaylistId"
    FOREIGN KEY ("PlaylistId") REFERENCES "Chinook"."Playlist" ("PlaylistId")
    ON DELETE NO ACTION ON UPDATE NO ACTION;


ALTER TABLE "Chinook"."PlaylistTrack" ADD CONSTRAINT "FK_PlaylistTrackTrackId_TrackTrackId"
    FOREIGN KEY ("TrackId") REFERENCES "Chinook"."Track" ("TrackId")
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX "IFK_PlaylistTrackTrackId_TrackTrackId" ON "Chinook"."PlaylistTrack" ("TrackId");


ALTER TABLE "Chinook"."Track" ADD CONSTRAINT "FK_TrackAlbumId_AlbumAlbumId"
    FOREIGN KEY ("AlbumId") REFERENCES "Chinook"."Album" ("AlbumId")
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX "IFK_TrackAlbumId_AlbumAlbumId" ON "Chinook"."Track" ("AlbumId");


ALTER TABLE "Chinook"."Track" ADD CONSTRAINT "FK_TrackGenreId_GenreGenreId"
    FOREIGN KEY ("GenreId") REFERENCES "Chinook"."Genre" ("GenreId")
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX "IFK_TrackGenreId_GenreGenreId" ON "Chinook"."Track" ("GenreId");


ALTER TABLE "Chinook"."Track" ADD CONSTRAINT "FK_TrackMediaTypeId_MediaTypeMediaTypeId"
    FOREIGN KEY ("MediaTypeId") REFERENCES "Chinook"."MediaType" ("MediaTypeId")
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX "IFK_TrackMediaTypeId_MediaTypeMediaTypeId" ON "Chinook"."Track" ("MediaTypeId");




/*******************************************************************************
   Populate Tables
********************************************************************************/

-- TODO

