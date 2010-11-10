/*******************************************************************************
   Chinook Database - Version 1.2
   Script: Chinook-MySql.sql
   Description: Creates and populates the Chinook database.
   DB Server: MySql
   Author: Luis Rocha
   License: http://www.codeplex.com/ChinookDatabase/license
********************************************************************************/

/*******************************************************************************
   Drop database if it exists
********************************************************************************/
DROP DATABASE IF EXISTS `Chinook`;

/*******************************************************************************
   Create database
********************************************************************************/
CREATE DATABASE `Chinook`;
USE `Chinook`;

/*******************************************************************************
   Create Tables
********************************************************************************/
CREATE TABLE `Album`
(
    `AlbumId` INT NOT NULL AUTO_INCREMENT,
    `Title` NVARCHAR(160) NOT NULL,
    `ArtistId` INT NOT NULL,
    CONSTRAINT `PK_Album` PRIMARY KEY  (`AlbumId`)
);

CREATE TABLE `Artist`
(
    `ArtistId` INT NOT NULL AUTO_INCREMENT,
    `Name` NVARCHAR(120),
    CONSTRAINT `PK_Artist` PRIMARY KEY  (`ArtistId`)
);

CREATE TABLE `Customer`
(
    `CustomerId` INT NOT NULL AUTO_INCREMENT,
    `FirstName` NVARCHAR(40) NOT NULL,
    `LastName` NVARCHAR(20) NOT NULL,
    `Company` NVARCHAR(80),
    `Address` NVARCHAR(70),
    `City` NVARCHAR(40),
    `State` NVARCHAR(40),
    `Country` NVARCHAR(40),
    `PostalCode` NVARCHAR(10),
    `Phone` NVARCHAR(24),
    `Fax` NVARCHAR(24),
    `Email` NVARCHAR(60) NOT NULL,
    `SupportRepId` INT,
    CONSTRAINT `PK_Customer` PRIMARY KEY  (`CustomerId`)
);

CREATE TABLE `Employee`
(
    `EmployeeId` INT NOT NULL AUTO_INCREMENT,
    `LastName` NVARCHAR(20) NOT NULL,
    `FirstName` NVARCHAR(20) NOT NULL,
    `Title` NVARCHAR(30),
    `ReportsTo` INT,
    `BirthDate` DATETIME,
    `HireDate` DATETIME,
    `Address` NVARCHAR(70),
    `City` NVARCHAR(40),
    `State` NVARCHAR(40),
    `Country` NVARCHAR(40),
    `PostalCode` NVARCHAR(10),
    `Phone` NVARCHAR(24),
    `Fax` NVARCHAR(24),
    `Email` NVARCHAR(60),
    CONSTRAINT `PK_Employee` PRIMARY KEY  (`EmployeeId`)
);

CREATE TABLE `Genre`
(
    `GenreId` INT NOT NULL AUTO_INCREMENT,
    `Name` NVARCHAR(120),
    CONSTRAINT `PK_Genre` PRIMARY KEY  (`GenreId`)
);

CREATE TABLE `Invoice`
(
    `InvoiceId` INT NOT NULL AUTO_INCREMENT,
    `CustomerId` INT NOT NULL,
    `InvoiceDate` DATETIME NOT NULL,
    `BillingAddress` NVARCHAR(70),
    `BillingCity` NVARCHAR(40),
    `BillingState` NVARCHAR(40),
    `BillingCountry` NVARCHAR(40),
    `BillingPostalCode` NVARCHAR(10),
    `Total` NUMERIC(10,2) NOT NULL,
    CONSTRAINT `PK_Invoice` PRIMARY KEY  (`InvoiceId`)
);

CREATE TABLE `InvoiceLine`
(
    `InvoiceLineId` INT NOT NULL AUTO_INCREMENT,
    `InvoiceId` INT NOT NULL,
    `TrackId` INT NOT NULL,
    `UnitPrice` NUMERIC(10,2) NOT NULL,
    `Quantity` INT NOT NULL,
    CONSTRAINT `PK_InvoiceLine` PRIMARY KEY  (`InvoiceLineId`)
);

CREATE TABLE `MediaType`
(
    `MediaTypeId` INT NOT NULL AUTO_INCREMENT,
    `Name` NVARCHAR(120),
    CONSTRAINT `PK_MediaType` PRIMARY KEY  (`MediaTypeId`)
);

CREATE TABLE `Playlist`
(
    `PlaylistId` INT NOT NULL AUTO_INCREMENT,
    `Name` NVARCHAR(120),
    CONSTRAINT `PK_Playlist` PRIMARY KEY  (`PlaylistId`)
);

CREATE TABLE `PlaylistTrack`
(
    `PlaylistId` INT NOT NULL,
    `TrackId` INT NOT NULL,
    CONSTRAINT `PK_PlaylistTrack` PRIMARY KEY  (`PlaylistId`, `TrackId`)
);

CREATE TABLE `Track`
(
    `TrackId` INT NOT NULL AUTO_INCREMENT,
    `Name` NVARCHAR(200) NOT NULL,
    `AlbumId` INT,
    `MediaTypeId` INT NOT NULL,
    `GenreId` INT,
    `Composer` NVARCHAR(220),
    `Milliseconds` INT NOT NULL,
    `Bytes` INT,
    `UnitPrice` NUMERIC(10,2) NOT NULL,
    CONSTRAINT `PK_Track` PRIMARY KEY  (`TrackId`)
);


/*******************************************************************************
   Create Primary Key Unique Indexes
********************************************************************************/
CREATE UNIQUE INDEX `IPK_Album` ON `Album`(`AlbumId`);

CREATE UNIQUE INDEX `IPK_Artist` ON `Artist`(`ArtistId`);

CREATE UNIQUE INDEX `IPK_Customer` ON `Customer`(`CustomerId`);

CREATE UNIQUE INDEX `IPK_Employee` ON `Employee`(`EmployeeId`);

CREATE UNIQUE INDEX `IPK_Genre` ON `Genre`(`GenreId`);

CREATE UNIQUE INDEX `IPK_Invoice` ON `Invoice`(`InvoiceId`);

CREATE UNIQUE INDEX `IPK_InvoiceLine` ON `InvoiceLine`(`InvoiceLineId`);

CREATE UNIQUE INDEX `IPK_MediaType` ON `MediaType`(`MediaTypeId`);

CREATE UNIQUE INDEX `IPK_Playlist` ON `Playlist`(`PlaylistId`);

CREATE UNIQUE INDEX `IPK_PlaylistTrack` ON `PlaylistTrack`(`PlaylistId`, `TrackId`);

CREATE UNIQUE INDEX `IPK_Track` ON `Track`(`TrackId`);


/*******************************************************************************
   Create Foreign Keys
********************************************************************************/
ALTER TABLE `Album` ADD CONSTRAINT `FK_AlbumArtistId_ArtistArtistId`
    FOREIGN KEY (`ArtistId`) REFERENCES `Artist` (`ArtistId`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX `IFK_AlbumArtistId_ArtistArtistId` ON `Album` (`ArtistId`);


ALTER TABLE `Customer` ADD CONSTRAINT `FK_CustomerSupportRepId_EmployeeEmployeeId`
    FOREIGN KEY (`SupportRepId`) REFERENCES `Employee` (`EmployeeId`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX `IFK_CustomerSupportRepId_EmployeeEmployeeId` ON `Customer` (`SupportRepId`);


ALTER TABLE `Employee` ADD CONSTRAINT `FK_EmployeeReportsTo_EmployeeEmployeeId`
    FOREIGN KEY (`ReportsTo`) REFERENCES `Employee` (`EmployeeId`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX `IFK_EmployeeReportsTo_EmployeeEmployeeId` ON `Employee` (`ReportsTo`);


ALTER TABLE `Invoice` ADD CONSTRAINT `FK_InvoiceCustomerId_CustomerCustomerId`
    FOREIGN KEY (`CustomerId`) REFERENCES `Customer` (`CustomerId`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX `IFK_InvoiceCustomerId_CustomerCustomerId` ON `Invoice` (`CustomerId`);


ALTER TABLE `InvoiceLine` ADD CONSTRAINT `FK_InvoiceLineInvoiceId_InvoiceInvoiceId`
    FOREIGN KEY (`InvoiceId`) REFERENCES `Invoice` (`InvoiceId`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX `IFK_InvoiceLineInvoiceId_InvoiceInvoiceId` ON `InvoiceLine` (`InvoiceId`);


ALTER TABLE `InvoiceLine` ADD CONSTRAINT `FK_InvoiceLineTrackId_TrackTrackId`
    FOREIGN KEY (`TrackId`) REFERENCES `Track` (`TrackId`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX `IFK_InvoiceLineTrackId_TrackTrackId` ON `InvoiceLine` (`TrackId`);


ALTER TABLE `PlaylistTrack` ADD CONSTRAINT `FK_PlaylistTrackPlaylistId_PlaylistPlaylistId`
    FOREIGN KEY (`PlaylistId`) REFERENCES `Playlist` (`PlaylistId`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;


ALTER TABLE `PlaylistTrack` ADD CONSTRAINT `FK_PlaylistTrackTrackId_TrackTrackId`
    FOREIGN KEY (`TrackId`) REFERENCES `Track` (`TrackId`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX `IFK_PlaylistTrackTrackId_TrackTrackId` ON `PlaylistTrack` (`TrackId`);


ALTER TABLE `Track` ADD CONSTRAINT `FK_TrackAlbumId_AlbumAlbumId`
    FOREIGN KEY (`AlbumId`) REFERENCES `Album` (`AlbumId`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX `IFK_TrackAlbumId_AlbumAlbumId` ON `Track` (`AlbumId`);


ALTER TABLE `Track` ADD CONSTRAINT `FK_TrackGenreId_GenreGenreId`
    FOREIGN KEY (`GenreId`) REFERENCES `Genre` (`GenreId`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX `IFK_TrackGenreId_GenreGenreId` ON `Track` (`GenreId`);


ALTER TABLE `Track` ADD CONSTRAINT `FK_TrackMediaTypeId_MediaTypeMediaTypeId`
    FOREIGN KEY (`MediaTypeId`) REFERENCES `MediaType` (`MediaTypeId`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

CREATE INDEX `IFK_TrackMediaTypeId_MediaTypeMediaTypeId` ON `Track` (`MediaTypeId`);




/*******************************************************************************
   Populate Tables
********************************************************************************/

-- TODO

