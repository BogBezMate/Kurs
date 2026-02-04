IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'CarServiceDB')
BEGIN
    CREATE DATABASE CarServiceDB;
END
GO

USE CarServiceDB;
GO

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'WorkOrderEmployees')
    DROP TABLE WorkOrderEmployees;

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'WorkOrderServices')
    DROP TABLE WorkOrderServices;

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'WorkOrderParts')
    DROP TABLE WorkOrderParts;

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'WorkOrders')
    DROP TABLE WorkOrders;

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Vehicles')
    DROP TABLE Vehicles;

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Customers')
    DROP TABLE Customers;

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
    DROP TABLE Users;

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Parts')
    DROP TABLE Parts;

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Employees')
    DROP TABLE Employees;
GO


/* Работники */
CREATE TABLE Employees (
    ID_Employees INT PRIMARY KEY IDENTITY(1,1),
    Last_Name NVARCHAR(50) NOT NULL,
    First_Name NVARCHAR(50) NOT NULL,
    Middle_Name NVARCHAR(50) NULL,
    Position NVARCHAR(50) NOT NULL,
    Phone NVARCHAR(20) NULL,
    Email NVARCHAR(100) NULL
);
GO

/* Пользователи */
CREATE TABLE Users (
    ID_Users INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL,
    Password NVARCHAR(100) NOT NULL
);
GO

/* Запчасти */
CREATE TABLE Parts (
    ID_Parts INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Quantity INT NOT NULL
);
GO

/* Клиенты */
CREATE TABLE Customers (
    ID_Customers INT PRIMARY KEY IDENTITY(1,1),
    Last_Name NVARCHAR(50) NOT NULL,
    First_Name NVARCHAR(50) NOT NULL,
    Middle_Name NVARCHAR(50) NULL,
    Phone NVARCHAR(20) NULL
);
GO

/* Автомобили */
CREATE TABLE Vehicles (
    ID_Vehicles INT PRIMARY KEY IDENTITY(1,1),
    ID_Customers INT NOT NULL,
    VIN NVARCHAR(50) NOT NULL,
    Plate_Number NVARCHAR(20) NOT NULL,
    Brand NVARCHAR(50) NULL,
    Model NVARCHAR(50) NULL,
    Year INT NULL,
    CONSTRAINT FK_Vehicles_Customers FOREIGN KEY (ID_Customers)
        REFERENCES Customers(ID_Customers)
);
GO

/* Заказ-наряды */
CREATE TABLE WorkOrders (
    ID_WorkOrders INT PRIMARY KEY IDENTITY(1,1),
    Number NVARCHAR(20) NOT NULL,
    ID_Customers INT NOT NULL,
    ID_Vehicles INT NOT NULL,
    Date_Open DATE NOT NULL,
    Status NVARCHAR(30) NULL,
    Total_Parts DECIMAL(10,2) NULL,
    Total_Services DECIMAL(10,2) NULL,
    Total_Amount DECIMAL(10,2) NULL,
    CONSTRAINT FK_WorkOrders_Customers FOREIGN KEY (ID_Customers)
        REFERENCES Customers(ID_Customers),
    CONSTRAINT FK_WorkOrders_Vehicles FOREIGN KEY (ID_Vehicles)
        REFERENCES Vehicles(ID_Vehicles)
);
GO

/* Запчасти в наряде */
CREATE TABLE WorkOrderParts (
    ID_WorkOrderParts INT PRIMARY KEY IDENTITY(1,1),
    ID_WorkOrders INT NOT NULL,
    ID_Parts INT NOT NULL,
    Quantity INT NOT NULL,
    Unit_Price DECIMAL(10,2) NOT NULL,
    Line_Sum DECIMAL(10,2) NOT NULL,
    CONSTRAINT FK_WorkOrderParts_WorkOrders FOREIGN KEY (ID_WorkOrders)
        REFERENCES WorkOrders(ID_WorkOrders),
    CONSTRAINT FK_WorkOrderParts_Parts FOREIGN KEY (ID_Parts)
        REFERENCES Parts(ID_Parts)
);
GO

/* Выполненные работы */
CREATE TABLE WorkOrderServices (
    ID_WorkOrderServices INT PRIMARY KEY IDENTITY(1,1),
    ID_WorkOrders INT NOT NULL,
    Description NVARCHAR(200) NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    CONSTRAINT FK_WorkOrderServices_WorkOrders FOREIGN KEY (ID_WorkOrders)
        REFERENCES WorkOrders(ID_WorkOrders)
);
GO

/* Назначение сотрудников на наряд  */
CREATE TABLE WorkOrderEmployees (
    ID_WorkOrderEmployees INT PRIMARY KEY IDENTITY(1,1),
    ID_WorkOrders INT NOT NULL,
    ID_Employees INT NOT NULL,
    CONSTRAINT FK_WorkOrderEmployees_WorkOrders FOREIGN KEY (ID_WorkOrders)
        REFERENCES WorkOrders(ID_WorkOrders),
    CONSTRAINT FK_WorkOrderEmployees_Employees FOREIGN KEY (ID_Employees)
        REFERENCES Employees(ID_Employees)
);
GO
