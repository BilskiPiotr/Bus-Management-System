/* Employee_InWorkNow - InWork (1) / None (0) */
/* Employee_Priv - Admin (0) / Alocator (1) / Driver (2) */

CREATE TABLE [dbo].[Employees_Basic] 
(
    [Id]                      INT PRIMARY KEY IDENTITY(1,1)	NOT NULL,
    [Employee_CompanyId]      VARCHAR (5)    				NOT NULL,
    [Employee_PESEL]          VARCHAR (11)   				NOT NULL,
    [Employee_Imie]           NVARCHAR (32)  				NOT NULL,
    [Employee_Nazwisko]       NVARCHAR (64)  				NOT NULL,
    [Employee_InWorkNow]      INT   DEFAULT (0)				NOT NULL,
	[Employee_Priv]			  INT				 			NOT NULL,
    [timestamp]               DATETIME DEFAULT (getdate()) 	NOT NULL
);

CREATE TABLE [dbo].[Employees_Status]
(
	[Id]						INT	PRIMARY KEY IDENTITY(1,1)	NOT NULL,
	[Employee_Id]				INT								NOT NULL,
	[Employee_Login]			DATETIME					    NULL,
	[Employee_Logout]			DATETIME				 	    NULL
);

CREATE TABLE [dbo].[AirPorts]
(
	[Id]						INT	PRIMARY KEY IDENTITY(1,1)	NOT NULL,
	[IATA_Name]					VARCHAR(3)						NOT NULL,
	[Full_Name]					NVARCHAR(50)					NOT NULL,
	[Country_Id]				INT								NOT NULL
);

/* Shengen - Shengen (0) / NonShengen (1) */
CREATE TABLE [dbo].[Countries]
(
	[Id]						INT PRIMARY KEY IDENTITY(1,1)	NOT NULL,
	[Country_Name]				NVARCHAR(50)					NOT NULL,
	[Shengen]					INT								NOT NULL	
);

CREATE TABLE [dbo].[Stations]
(
	[Id]						INT PRIMARY KEY IDENTITY(1,1)	NOT NULL,
	[StationNb]					VARCHAR(4)						NOT NULL,
	[GPS_Latitude]				NVARCHAR(50)					NOT NULL,
    [GPS_Longitude]				NVARCHAR(50)					NOT NULL
);

CREATE TABLE [dbo].[Gates]
(
	[Id]						INT PRIMARY KEY IDENTITY(1,1)	NOT NULL,
	[GateNb]					VARCHAR(2)						NOT NULL,
	[GPS_Latitude]				NVARCHAR(50)					NOT NULL,
    [GPS_Longitude]				NVARCHAR(50)					NOT NULL

);

/* Operations - Odlot (0) / Przylot (1) */
CREATE TABLE [dbo].[Operations]
(
	[Id]						INT PRIMARY KEY IDENTITY(1,1)	NOT NULL,
	[Employee_Id]				INT								NOT NULL,
	[Operations]				INT								NOT NULL,
	[Station_Id]				INT								NOT NULL,
	[Gate_Id]					INT								NOT NULL,
	[AirPort_Id]				INT								NOT NULL,
	[Start_Boardiong]			DATETIME						NULL,
	[Start_DeBoarding]			DATETIME						NULL,
	[Begin_Operation]			DATETIME						NOT NULL,
	[End_Operation]				DATETIME						NULL
);

INSERT INTO [dbo].[Employees_Basic] ([Employee_CompanyId], [Employee_PESEL], [Employee_Imie], [Employee_Nazwisko], [Employee_Priv]) VALUES ('12345', '11111111111', 'Imie1', 'Nazwisko1', 0);
INSERT INTO [dbo].[Employees_Basic] ([Employee_CompanyId], [Employee_PESEL], [Employee_Imie], [Employee_Nazwisko], [Employee_Priv]) VALUES ('23232', '22222222222', 'Imie2', 'Nazwisko2', 1);
INSERT INTO [dbo].[Employees_Basic] ([Employee_CompanyId], [Employee_PESEL], [Employee_Imie], [Employee_Nazwisko], [Employee_Priv]) VALUES ('35353', '33333333333', 'Imie3', 'Nazwisko3', 2);
INSERT INTO [dbo].[Employees_Basic] ([Employee_CompanyId], [Employee_PESEL], [Employee_Imie], [Employee_Nazwisko], [Employee_Priv]) VALUES ('41232', '44444444444', 'Imie4', 'Nazwisko4', 2);
INSERT INTO [dbo].[Employees_Basic] ([Employee_CompanyId], [Employee_PESEL], [Employee_Imie], [Employee_Nazwisko], [Employee_Priv]) VALUES ('56353', '55555555555', 'Imie5', 'Nazwisko5', 2);

INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('1','','');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('2','','');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('3','','');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('4','','');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('5','','');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('6','','');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('7','','');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('8','','');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('9','','');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('10','','');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('11','','');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('12','','');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('13','','');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('14','','');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('15','','');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('1','','');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('1','','');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('1','','');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('1','','');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('1','','');

ALTER TABLE Employees_Basic ADD CONSTRAINT UC_EmployeeBasic UNIQUE (Id, Employee_CompanyId);
ALTER TABLE Employees_Status ADD CONSTRAINT UC_EmployeeStatus UNIQUE (Id);
ALTER TABLE AirPorts ADD CONSTRAINT UC_AirPorts UNIQUE (Id);
ALTER TABLE Countries ADD CONSTRAINT UC_Countries UNIQUE (Id);
ALTER TABLE Stations ADD CONSTRAINT UC_Stations UNIQUE (Id);
ALTER TABLE Gates ADD CONSTRAINT UC_Gates UNIQUE (Id);
ALTER TABLE Operations ADD CONSTRAINT UC_Operations UNIQUE (Id);

ALTER TABLE Employees_Status ADD CONSTRAINT FK_Employees_Status_01 FOREIGN KEY (Employee_Id) REFERENCES Employees_Basic(Id);
ALTER TABLE AirPorts ADD CONSTRAINT FK_AirPorts_01 FOREIGN KEY (Country_Id) REFERENCES Countries(Id);
ALTER TABLE Operations ADD CONSTRAINT FK_Operations_01 FOREIGN KEY (Employee_Id) REFERENCES Employees_Basic(Id);
ALTER TABLE Operations ADD CONSTRAINT FK_Operations_02 FOREIGN KEY (Station_Id) REFERENCES Stations(Id);
ALTER TABLE Operations ADD CONSTRAINT FK_Operations_03 FOREIGN KEY (Gate_Id) REFERENCES Gates(Id);
ALTER TABLE Operations ADD CONSTRAINT FK_Operations_04 FOREIGN KEY (AirPort_Id) REFERENCES AirPorts(Id);