-- Creating database Pharmacy
CREATE DATABASE Pharmacy;
GO
-- Use Pharmacy database
USE Pharmacy;

-- Creating table Medicines
CREATE TABLE [Medicines](
    [MedicineID] [INT] IDENTITY(1,1)NOT NULL PRIMARY KEY,
    [Name] [NVARCHAR](40) UNIQUE NOT NULL,
    [Manufacturer] [NVARCHAR](30),
    [Price] [DECIMAL](30),
    [Amount] [INT],
    [WithPrescription] [BIT] NOT NULL
	);

-- Creating table Prescriptions
CREATE TABLE [Prescriptions](
	[PrescriptionID] [INT] IDENTITY(1,1)NOT NULL PRIMARY KEY,
	[CustomerName] [NVARCHAR](50) NOT NULL,
	[PESEL] [NVARCHAR](11) NOT NULL,
	[PrescriptionNumber] [INT] NOT NULL
	);
-- Creating table Orders
CREATE TABLE [Orders](
	[ID] [INT] IDENTITY(1,1)NOT NULL PRIMARY KEY,
	[PrescriptionID] [INT],
	[MedicineID] [INT] NOT NULL,
	[Date] [DATETIME] NOT NULL,
	[Amount] [INT] NOT NULL
	);

-- Connecting tables
	ALTER TABLE Orders
	ADD CONSTRAINT FK_Orders_Medicine
	FOREIGN KEY (MedicineID)
	REFERENCES Medicines (MedicineID)
	;

	ALTER TABLE Orders
	ADD CONSTRAINT FK_Orders_Prescriptions
	FOREIGN KEY (PrescriptionID)
	REFERENCES Prescriptions (PrescriptionID)
	ON DELETE CASCADE;



