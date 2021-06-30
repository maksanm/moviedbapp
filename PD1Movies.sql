USE master
GO

IF exists(SELECT * FROM sysdatabases WHERE NAME='PD1Movies')
	DROP DATABASE PD1Movies

CREATE DATABASE PD1Movies
GO

USE PD1Movies
GO

CREATE TABLE Movies(
	ID int NOT NULL PRIMARY KEY IDENTITY(1,1),
	Title varchar(50) NOT NULL,
	ReleaseDate date NOT NULL,
	Budget money NOT NULL,
	AvgRating numeric(18,2) NOT NULL,
	Imax3D bit NOT NULL,
	TicketsSold int NOT NULL
	)

INSERT INTO Movies (Title, ReleaseDate, Budget, AvgRating, Imax3D, TicketsSold)
VALUES ('Shrek 1', '30.07.2001', 6000000, 9.2, 0, 500000)
INSERT INTO Movies (Title, ReleaseDate, Budget, AvgRating, Imax3D, TicketsSold)
VALUES ('1984', '24.01.1956', 84000, 7.3, 0, 30000)
INSERT INTO Movies (Title, ReleaseDate, Budget, AvgRating, Imax3D, TicketsSold)
VALUES ('Joker', '31.08.2019', 7000000, 8.3, 1, 1000000)
INSERT INTO Movies (Title, ReleaseDate, Budget, AvgRating, Imax3D, TicketsSold)
VALUES ('Best FIlm Ever', '25.05.2001', 40000, 2, 1, 300)