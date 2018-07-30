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



INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Albania', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Anglia', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Armenia', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Australia', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Austria', 0);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Belgia', 0);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Białoruś', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Brazylia', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Bułgaria', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Chiny', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Chorwacja', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Cypr', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Czechy', 0);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Dania', 0);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Dominikana', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Egipt', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Estonia', 0);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Finlandia', 0);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Francja', 0);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Gambia', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Grecja', 0);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Gruzja', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Hiszpania', 0);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Holandia', 0);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Indie', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Irlandia', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Islandia', 0);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Izrael', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Japonia', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Jordania', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Kanada', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Kazachstan', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Kenia', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Korea Płd', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Kuba', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Liban', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Litwa', 0);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Luksemburg', 0);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Łotwa', 0);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Malta', 0);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Maroko', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Meksyk', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Mołdawia', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Niemcy', 0);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Norwegia', 0);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Oman', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Polska', 0);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Portugalia', 0);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Qatar', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Rosja', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Rumunia', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Serbia', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Słowacja', 0);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Słowenia', 0);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Sri Lanka', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Szkocja', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Szwajcaria', 0);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Szwecja', 0);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Tunezja', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Turcja', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Ukraina', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('USA', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Węgry', 0);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Włochy', 0);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Wyspy Zielonego Przylądka', 1);
INSERT INTO [dbo].[Countries] ([Country_Name], [Shengen]) VALUES ('Zjednoczone Emiraty Arabskie', 1);



INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('ACE', 'Lanzarote', 23);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('ADB', 'Izmir', 60);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('AGA', 'Agadir', 41);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('AGP', 'Malaga', 23);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('AJA', 'Ajaccio', 19);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('ALC', 'Alicante', 23);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('AMM', 'Amman', 30);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('AMS', 'Amsterdam', 24);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('AQJ', 'Akaka', 30);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('ARN', 'Sztokcholm', 58);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('ATH', 'Ateny', 21);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('AYT', 'Antalia', 60);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('BCN', 'Barcelona', 23);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('BEG', 'Belgrad', 52);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('BEY', 'Bejrut', 36);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('BFS', 'Belfast', 26);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('BGO', 'Bergen', 45);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('BGY', 'Bergamo', 64);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('BHS', 'Birmingham', 2);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('BHY', 'Bergamo', 64);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('BIO', 'Bilbao', 23);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('BJL', 'Bandżul', 20);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('BJV', 'Bodrum', 60);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('BLL', 'Blund', 14);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('BLQ', 'Bolonia', 64);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('BOJ', 'Burgas', 60);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('BRI', 'Bari', 64);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('BRS', 'Bristol', 2);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('BRU', 'Bruksela', 6);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('BLS', 'Bazylea', 57);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('BTS', 'Btatysława', 53);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('BUD', 'Budapeszt', 63);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('BVA', 'Beauvais', 19);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('BZG', 'Bydgoszcz', 47);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('CAI', 'Cair', 16);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('CDG', 'Paryż', 19);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('CFU', 'Corfu', 21);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('CGN', 'Kolonia', 44);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('CHQ', 'Chania', 21);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('CIA', 'Ciampino', 64);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('CMB', 'Kolombo', 55);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('CPH', 'Kopenchaga', 14);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('CRL', 'Bruksela', 6);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('CTA', 'Catania', 64);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('CUN', 'Cancun', 42);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('CWL', 'Cardiff', 56);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('DBV', 'Dubrownik', 11);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('DEL', 'Dehli', 25);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('DJE', 'Djerba', 59);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('DLM', 'Dalaman', 60);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('DOH', 'Doha', 49);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('DOK', 'Donieck', 61);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('DSA', 'Dancaster', 2);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('DTM', 'Dortmund', 44);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('DUB', 'Dublin', 26);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('DUS', 'Dusseldorf', 44);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('EVN', 'Tibilisi', 3);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('EWR', 'Newark', 62);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('FAO', 'Faro', 48);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('FCO', 'Rzym', 64);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('FNC', 'Madeira', 48);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('FRA', 'Frankfurt', 44);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('FUE', 'Fuertaventura', 23);



INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('1', '', '');

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