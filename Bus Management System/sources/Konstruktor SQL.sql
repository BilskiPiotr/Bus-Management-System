/* Employee_InWorkNow - InWork (1) / None (0) */
/* Employee_Priv - Admin (0) / Alocator (1) / Driver (2) */

USE piotrbilski_sysbusmanagement;

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

/* Vehicles Bus_Status: 0 - Not Available, 1 - Empty, 2 - Have Driver */
/* Vehicles Work_Status: 0 - Free, 1 - In Work */
CREATE TABLE [dbo].[Vehicles]
(
	[Id]				INT PRIMARY KEY IDENTITY(1,1)	NOT NULL,
	[VehicleNb]			VARCHAR(4)						NOT NULL,
	[Bus_Status]		INT	DEFAULT (1)					NOT NULL,
	[Work_Status]		INT DEFAULT (0)					NOT NULL,
	[Latitude]			VARCHAR(18)	DEFAULT(0)			NULL,
	[Longitude]			VARCHAR(18) DEFAULT(0)			NULL,
	[EmergencyDistance]	VARCHAR(10)	DEFAULT(0)			NULL,
	[TimeStamp]			DATETIME						NULL		
);

CREATE TABLE [dbo].[Vehicles_Status]
(
	[Id]			INT PRIMARY KEY IDENTITY(1,1)	NOT NULL,
	[Employees_Id]	INT								NOT NULL,
	[Vehicles_Id]	INT								NOT NULL,
	[Taked]			DATETIME						NULL,
	[Returned]		DATETIME						NULL
);


/* Operations - Odlot (0) / Przylot (1) */
CREATE TABLE [dbo].[Operations]
(
	[Id]						INT PRIMARY KEY IDENTITY(1,1)	NOT NULL,
	[Employee_Id]				INT								NOT NULL,
	[Operation]					INT								NOT NULL,
	[GodzinaRozkladowa]			DATETIME						NOT NULL,
	[FlightNb]					VARCHAR(7)						NOT NULL,
	[Pax]						INT								NOT NULL,
	[AirPort]					INT								NOT NULL,
	[PPS]						INT								NOT NULL,
	[Gate]						INT								NOT NULL,
	[Bus]						INT								NOT NULL,
	[RadioGate]					VARCHAR(3)						NULL,
	[RadioNeon]					VARCHAR(3)						NULL,
	[Created]					DATETIME						NOT NULL,
	[Accepted]					DATETIME						NULL,
	[StartLoad]					DATETIME						NULL,
	[StartDrive]				DATETIME						NULL,
	[StartUnload]				DATETIME						NULL,
	[EndOp]						DATETIME						NULL
);


/* 0 - Odlot, 1 - Przylot */
CREATE TABLE [dbo].[OperationType]
(
	[Id]			INT PRIMARY KEY IDENTITY(1,1)	NOT NULL,
	[OpValue]		INT								NOT NULL,
	[Operation]		VARCHAR(7)						NOT NULL
);


ALTER TABLE Employees_Status ADD CONSTRAINT FK_Employees_Status_01 FOREIGN KEY (Employee_Id) REFERENCES Employees_Basic(Id);
ALTER TABLE AirPorts ADD CONSTRAINT FK_AirPorts_01 FOREIGN KEY (Country_Id) REFERENCES Countries(Id);
ALTER TABLE Operations ADD CONSTRAINT FK_Operations_01 FOREIGN KEY (Employee_Id) REFERENCES Employees_Basic(Id);
ALTER TABLE Operations ADD CONSTRAINT FK_Operations_02 FOREIGN KEY (PPS) REFERENCES Stations(Id);
ALTER TABLE Operations ADD CONSTRAINT FK_Operations_03 FOREIGN KEY (Gate) REFERENCES Gates(Id);
ALTER TABLE Operations ADD CONSTRAINT FK_Operations_04 FOREIGN KEY (AirPort) REFERENCES AirPorts(Id);
ALTER TABLE Operations ADD CONSTRAINT FK_Operations_05 FOREIGN KEY (Bus) REFERENCES Vehicles(Id);
ALTER TABLE Operations ADD CONSTRAINT FK_Operations_06 FOREIGN KEY (Operation) REFERENCES OperationType(Id);
ALTER TABLE Vehicles_Status ADD CONSTRAINT FK_Vehicles_Status_01 FOREIGN KEY (Employees_Id) REFERENCES Employees_Basic(Id);
ALTER TABLE Vehicles_Status ADD CONSTRAINT FK_Vehicles_Status_02 FOREIGN KEY (Vehicles_Id) REFERENCES Vehicles(Id);



ALTER TABLE Employees_Basic ADD CONSTRAINT UC_EmployeeBasic UNIQUE (Id, Employee_CompanyId);
ALTER TABLE Employees_Status ADD CONSTRAINT UC_EmployeeStatus UNIQUE (Id);
ALTER TABLE AirPorts ADD CONSTRAINT UC_AirPorts UNIQUE (Id);
ALTER TABLE Countries ADD CONSTRAINT UC_Countries UNIQUE (Id);
ALTER TABLE Stations ADD CONSTRAINT UC_Stations UNIQUE (Id);
ALTER TABLE Gates ADD CONSTRAINT UC_Gates UNIQUE (Id);
ALTER TABLE Operations ADD CONSTRAINT UC_Operations UNIQUE (Id);
ALTER TABLE OperationType ADD CONSTRAINT UC_OperationType UNIQUE (Id); 
ALTER TABLE Vehicles ADD CONSTRAINT UC_Vehicles UNIQUE (Id);
ALTER TABLE Vehicles_Status ADD CONSTRAINT UC_Vehicle_Status UNIQUE (Id);




INSERT INTO [dbo].[OperationType] ([OpValue], [Operation]) VALUES (0, 'Przylot');
INSERT INTO [dbo].[OperationType] ([OpValue], [Operation]) VALUES (1, 'Odlot');


INSERT INTO [dbo].[Employees_Basic] ([Employee_CompanyId], [Employee_PESEL], [Employee_Imie], [Employee_Nazwisko], [Employee_Priv]) VALUES ('12345', '11111111111', 'Imie1', 'Nazwisko1', 0);
INSERT INTO [dbo].[Employees_Basic] ([Employee_CompanyId], [Employee_PESEL], [Employee_Imie], [Employee_Nazwisko], [Employee_Priv]) VALUES ('23232', '22222222222', 'Imie2', 'Nazwisko2', 1);
INSERT INTO [dbo].[Employees_Basic] ([Employee_CompanyId], [Employee_PESEL], [Employee_Imie], [Employee_Nazwisko], [Employee_Priv]) VALUES ('35353', '33333333333', 'Imie3', 'Nazwisko3', 2);
INSERT INTO [dbo].[Employees_Basic] ([Employee_CompanyId], [Employee_PESEL], [Employee_Imie], [Employee_Nazwisko], [Employee_Priv]) VALUES ('41232', '44444444444', 'Imie4', 'Nazwisko4', 2);
INSERT INTO [dbo].[Employees_Basic] ([Employee_CompanyId], [Employee_PESEL], [Employee_Imie], [Employee_Nazwisko], [Employee_Priv]) VALUES ('56353', '55555555555', 'Imie5', 'Nazwisko5', 2);


/* unavailable - 0,  no driver - 1, free - 2, in work - 3  (default - empty) */
INSERT INTO [dbo].[Vehicles] ([VehicleNb]) VALUES ('032');
INSERT INTO [dbo].[Vehicles] ([VehicleNb]) VALUES ('033');
INSERT INTO [dbo].[Vehicles] ([VehicleNb]) VALUES ('1165');
INSERT INTO [dbo].[Vehicles] ([VehicleNb]) VALUES ('1167');
INSERT INTO [dbo].[Vehicles] ([VehicleNb]) VALUES ('2067');
INSERT INTO [dbo].[Vehicles] ([VehicleNb]) VALUES ('2068');
INSERT INTO [dbo].[Vehicles] ([VehicleNb]) VALUES ('2069');
INSERT INTO [dbo].[Vehicles] ([VehicleNb]) VALUES ('2072');
INSERT INTO [dbo].[Vehicles] ([VehicleNb]) VALUES ('2073');
INSERT INTO [dbo].[Vehicles] ([VehicleNb]) VALUES ('2074');
INSERT INTO [dbo].[Vehicles] ([VehicleNb]) VALUES ('2075');
INSERT INTO [dbo].[Vehicles] ([VehicleNb]) VALUES ('2076');
INSERT INTO [dbo].[Vehicles] ([VehicleNb]) VALUES ('2077');
INSERT INTO [dbo].[Vehicles] ([VehicleNb]) VALUES ('2078');
INSERT INTO [dbo].[Vehicles] ([VehicleNb]) VALUES ('2079');
INSERT INTO [dbo].[Vehicles] ([VehicleNb]) VALUES ('2082');
INSERT INTO [dbo].[Vehicles] ([VehicleNb]) VALUES ('2083');
INSERT INTO [dbo].[Vehicles] ([VehicleNb]) VALUES ('2084');

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
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('GDN', 'Gdańsk', 47);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('GIG', 'Rio', 8);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('GNB', 'Grenoble', 19);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('GOA', 'Goa', 25);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('GOT', 'Goeteborg', 58);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('GPA', 'Patras', 21);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('GRO', 'Girona', 23);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('GRZ', 'Graz', 5);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('GVA', 'Geneva', 57);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('HAJ', 'Hanower', 44);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('HAM', 'Hamburg', 44);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('HAU', 'Haugesund', 45);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('HEL', 'Helsinki', 18);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('HER', 'Heraklion', 21);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('HHN', 'Hahn', 44);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('HKG', 'Hong Kong', 10);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('HRG', 'Hurgada', 16);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('HRK', 'Harków', 61);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('IBZ', 'Ibiza', 23);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('ICN', 'Seul', 34);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('IEG', 'Zielona Góra', 47);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('IEV', 'Kijów', 61);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('IST', 'Istambuł', 60);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('JFK', 'Nowy Jork', 62);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('KBP', 'Kijów', 61);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('KEF', 'Keflawik', 27);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('KGD', 'Kaliningrad', 50);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('KGS', 'Kos', 21);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('KIV', 'Kiszyniów', 43);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('KRK', 'Kraków', 47);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('KSC', 'Koszyce', 53);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('KTW', 'Katowice', 47);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('KUN', 'Kowno', 37);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('KVA', 'Kavala', 23);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('LAX', 'Los Angeles', 62);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('LCA', 'Larnaka', 12);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('LCY', 'Londyn', 2);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('LED', 'St. Petersburg', 50);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('LEI', 'Almeria', 23);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('LEJ', 'Leizping', 44);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('LGG', 'Liege', 6);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('LGW', 'Londyn', 2);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('LHR', 'Londyn', 2);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('LIL', 'Lille', 19);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('LIS', 'Lisbona', 48);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('LJU', 'Lublana', 54);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('LPA', 'Las Palmas', 23);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('LPL', 'Liverpool', 2);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('LRM', 'La Romania', 15);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('LTN', 'Londyn', 2);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('LUX', 'Luxemburg', 38);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('LWO', 'Lwów', 61);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('LYS', 'Lyon', 19);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('MAD', 'Madryt', 23);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('MAN', 'Manchester', 2);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('MBA', 'Mombasa', 33);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('MIR', 'Monastyr', 59);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('MLA', 'Malta', 40);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('MME', 'Durham', 2);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('MMX', 'Malmo', 58);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('MSQ', 'Mińsk', 7);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('MUC', 'Monachium', 44);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('MXP', 'Mediolan', 64);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('NCE', 'Nicea', 19);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('NRT', 'Tokio', 29);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('NUE', 'Norymberga', 44);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('NYO', 'Sztokcholm', 58);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('ODS', 'Odessa', 61);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('OLB', 'Olbia', 64);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('OPO', 'Porto', 48);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('ORD', 'Chicago', 62);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('ORY', 'Paryż', 19);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('OTP', 'Bukareszt', 51);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('PEK', 'Pekin', 10);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('PHO', 'Paphos', 12);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('PIK', 'Glasgow', 56);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('PLQ', 'Palanga', 37);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('PMI', 'Palma De Majorka', 23);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('PMO', 'Palermo', 64);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('PRG', 'Praga', 13);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('PSA', 'Pisa', 64);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('REG', 'Regio Di Calabria', 64);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('REK', 'Rejkiawik', 27);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('RIX', 'Ryga', 39);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('RKT', 'Ras Al. Khaimah', 66);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('RMF', 'MArsa Alam', 16);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('RMI', 'Rimini', 64);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('RTM', 'Roterdam', 24);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('SHA', 'Sharjak', 66);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('SID', 'SAL', 65);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('SKG', 'Saloniki', 21);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('SLL', 'Salalah', 45);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('SMA', 'Santa Maria', 48);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('SNN', 'Shannon', 26);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('SOF', 'Sofia', 9);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('SPU', 'Split', 11);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('SSH', 'Sharm El Sheikh', 16);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('STN', 'Londyn', 2);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('STR', 'Sztudgart', 44);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('SVG', 'Stavanger', 45);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('SVO', 'Moskwa', 50);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('SXF', 'Berlin', 44);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('SYD', 'Sydney', 4);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('SZG', 'Salzburg', 5);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('SZZ', 'Szczecin', 47);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('TAT', 'Poprad', 53);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('TBS', 'Tibilisi', 22);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('TCP', 'Taba', 16);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('TFS', 'Teneryfa', 23);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('THL', 'Berlin', 44);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('TIA', 'Tirana', 1);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('TLL', 'Tallin', 17);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('TLS', 'Tuluza', 19);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('TLV', 'Tel Aviv', 28);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('TPS', 'Trapani', 64);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('TRF', 'Oslo', 45);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('TSE', 'Astana', 32);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('TXL', 'Berlin', 44);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('VAR', 'Warna', 9);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('VCE', 'Wenecja', 64);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('VIE', 'Wiedeń', 5);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('VLC', 'Walencia', 23);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('VNO', 'Wilno', 37);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('VRA', 'Varadero', 35);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('VRN', 'Werona', 64);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('XRY', 'Frontiera', 23);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('YYZ', 'Toronto', 31);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('ZAD', 'Zadar', 11);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('ZAG', 'Zagrzeb', 11);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('ZRH', 'Zurich', 57);
INSERT INTO [dbo].[AirPorts] ([IATA_Name], [Full_Name], [Country_Id]) VALUES ('ZTH', 'Zakintos', 21);


/* Gates - Gates (1 - 45) / Shengen 1-4 (S1 - S4)  / Non Shengen (N1) */
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('1', '52.17308333333334', '20.97014333333333');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('2', '52.17308333333334', '20.97014333333333');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('3', '52.173235', '20.97002166666667');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('4', '52.173235', '20.97002166666667');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('5', '52.17343500000005', '20.96979666666667');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('6', '52.17343500000005', '20.96979666666667');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('7', '52.17384666666667', '20.96930500000002');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('8', '52.17384666666667', '20.96930500000002');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('9', '52.174175', '20.96917999999998');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('10', '52.174175', '20.96917999999998');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('11', '52.17441166666667', '20.96901666666667');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('12', '52.17441166666667', '20.96901666666667');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('13', '52.174455', '20.96865000000004');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('14', '52.174455', '20.96865000000004');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('15', '52.174355', '20.96837333333336');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('16', '52.174355', '20.96837333333336');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('17', '52.17421833333334', '20.96833999999998');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('18', '52.17421833333334', '20.96833999999998');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('19', '52.17376166666667', '20.968845');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('20', '52.17376166666667', '20.968845');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('21', '52.17315833333334', '20.96926000000000');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('22', '52.17315833333334', '20.96926000000000');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('23', '52.17254666666666', '20.96973333333338');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('24', '52.17254666666666', '20.96973333333338');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('25', '52.17167333333334', '20.97035833333333');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('26', '52.17167333333334', '20.97035833333333');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('27', '52.17134333333333', '20.970725');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('28', '52.17134333333333', '20.970725');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('29', '52.17094333333333', '20.97113000000002');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('30', '52.17094333333333', '20.97113000000002');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('31', '52.17018166666664', '20.97182833333333');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('32', '52.17018166666664', '20.97182833333333');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('33', '52.17005333333335', '20.97214333333333');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('34', '52.17005000000001', '20.97214333333333');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('35', '52.17011666666667', '20.97199333333333');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('36', '52.17009500000001', '20.97189166666667');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('37', '52.17009500000001', '20.97189166666667');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('38', '52.16946166666666', '20.972348333333335');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('39', '52.16946166666666', '20.972348333333335');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('40', '52.16919166666666', '20.972688333333334');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('41', '52.16919166666666', '20.972688333333334');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('42', '52.16887666666666', '20.972845');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('43', '52.16887666666666', '20.972845');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('44', '52.16811333333333', '20.97388');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('45', '52.16811333333333', '20.97388');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('S1', '52.17021166666667', '20.971659999999996');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('S2', '52.17021166666667', '20.971659999999996');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('S3', '52.17039166666667', '20.971601666666667');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('S4', '52.17039166666667', '20.971601666666667');
INSERT INTO [dbo].[Gates] ([GateNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('N1', '52.17296666666667', '20.969999999999995');

INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('1','52.17301666666667','20.97113833333336');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('2','52.17346666666667','20.970875');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('3','52.17375166666675','20.97058833333332');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('4','52.17409999999996','20.97039166666668');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('5','52.17449833333333','20.96992000000002');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('6','52.174805','20.9696316666666');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('7','52.17513166666666','20.96932333333333');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('8','52.17535666666666','20.96904499999998');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('9','52.175043333333333','20.96830333333333');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('10','52.174491666666667','20.96743166666666');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('10L','52.174815','20.96766833333336');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('10R','52.174614999999996','20.96759333333337');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('11','52.173911666666666','20.96774000000003');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('12','52.173783333333326','20.96786000000005');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('13','52.173311666666666','20.96840833333333');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('13L','52.17345','20.96813166666668');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('13R','52.17318','20.96853666666667');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('14','52.172659999999999','20.96888666666667');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('14L','52.17279','20.96862166666664');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('14R','52.172546666666666','20.96899500000003');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('15','52.172083333333333','20.96953333333334');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('15L','52.172226666666667','20.96921833333334');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('15R','52.171988333333334','20.96966166666667');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('16','52.171450000000001','20.970035');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('17','52.170989999999996','20.97057166666668');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('18','52.1704','20.97112833333333');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('19','52.170048333333334','20.97128');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('20','52.169531666666667','20.97184333333333');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('21','52.169186666666667','20.97217');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('22','52.168768333333334','20.97246833333333');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('23','52.168526666666667','20.972716666666663');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('24','52.168166666666667','20.973066666666668');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('31','52.173788333333334','20.965306666666667');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('31B','52.174031666666664','20.965166666666665');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('32','52.173388333333335','20.965510000000002');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('33','52.173101666666666','20.965756666666667');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('34','52.172836666666667','20.9660316666666662');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('35','52.172606666666666','20.9663366666666667');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('36','52.172029999999999','20.9668233333333333');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('36L','52.17218','20.9666199999999995');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('36R','52.171941666666666','20.966925');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('37','52.170809999999996','20.9678933333333336');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('37L','52.170944999999996','20.9678000000000004');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('37R','52.170668333333333','20.968');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('39','52.169999999999995','20.9685316666666664');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('40','52.169798333333333','20.9688116666666667');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('41','52.169380000000004','20.9691116666666663');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('42','52.169095','20.969373333333333');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('43','52.168826666666667','20.969636666666663');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('43B','52.168544999999995','20.969853333333337');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('44','52.168124999999996','20.970485');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('45','52.167314999999995','20.971368333333333');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('46','52.166484999999994','20.973630000000004');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('46L','52.1666','20.973386666666667');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('46R','52.165445','20.973991666666667');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('47','52.166259999999994','20.974708333333336');
INSERT INTO [dbo].[Stations] ([StationNb], [GPS_Latitude], [GPS_Longitude]) VALUES ('48','52.166145000000001','20.975101666666667');

INSERT INTO [dbo].[Operations] ([Employee_Id], [Operation], [FlightNb], [Pax], [AirPort], [PPS], [Gate], [Bus], [RadioGate], [RadioNeon], [Created]) VALUES (2, 1, 'EN671', '121', 65, 28, 13, 7, '612', '377', '2018-09-17 09:11:12');
INSERT INTO [dbo].[Operations] ([Employee_Id], [Operation], [FlightNb], [Pax], [AirPort], [PPS], [Gate], [Bus], [RadioGate], [RadioNeon], [Created]) VALUES (3, 2, 'LO8991', '97', 26, 11, 17, 13, '891', '443', '2018-09-17 09:17:26');
INSERT INTO [dbo].[Operations] ([Employee_Id], [Operation], [FlightNb], [Pax], [AirPort], [PPS], [Gate], [Bus], [RadioGate], [RadioNeon], [Created]) VALUES (4, 2, 'EN772', '189', 44, 14, 14, 11, '278', '719', '2018-09-17 09:23:41');

UPDATE [dbo].[Vehicles] SET [Status] = 2 WHERE Id = 7;
UPDATE [dbo].[Vehicles] SET [Status] = 2 WHERE Id = 11;
UPDATE [dbo].[Vehicles] SET [Status] = 2 WHERE Id = 13;

UPDATE [dbo].[Vehicles] SET [Work_Status] = 1 WHERE [Status] = 2;

INSERT INTO [dbo].[Employees_Status] ([Employee_Id], [Employee_Login]) VALUES (2, '2018-09-17 08:01:12');
INSERT INTO [dbo].[Employees_Status] ([Employee_Id], [Employee_Login]) VALUES (2, '2018-09-17 08:02:27');
INSERT INTO [dbo].[Employees_Status] ([Employee_Id], [Employee_Login]) VALUES (2, '2018-09-17 08:02:39');


/* zmiana nazwy kolumny w tabeli */
EXEC sp_RENAME 'Vehicles.Bus_Status', 'Status', 'COLUMN'