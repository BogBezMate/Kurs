IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'CarServiceDB')
BEGIN
    CREATE DATABASE CarServiceDB;
END
GO

USE CarServiceDB;
GO


IF EXISTS (SELECT * FROM sys.tables WHERE name = 'WorkOrderEmployees') DROP TABLE WorkOrderEmployees;
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'WorkOrderServices')  DROP TABLE WorkOrderServices;
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'WorkOrderParts')     DROP TABLE WorkOrderParts;
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'WorkOrders')         DROP TABLE WorkOrders;
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Vehicles')           DROP TABLE Vehicles;
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Customers')          DROP TABLE Customers;
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')              DROP TABLE Users;
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Parts')              DROP TABLE Parts;
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Suppliers')          DROP TABLE Suppliers;
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Employees')          DROP TABLE Employees;
GO


/* Сотрудники */
CREATE TABLE Employees (
    ID_Employees INT PRIMARY KEY IDENTITY(1,1),
    Last_Name    NVARCHAR(50)  NOT NULL,
    First_Name   NVARCHAR(50)  NOT NULL,
    Middle_Name  NVARCHAR(50)  NULL,
    Position     NVARCHAR(50)  NOT NULL,
    Phone        NVARCHAR(20)  NULL,
    Email        NVARCHAR(100) NULL
);
GO

/* Пользователи */
CREATE TABLE Users (
    ID_Users     INT PRIMARY KEY IDENTITY(1,1),
    ID_Employees INT           NOT NULL,
    Username     NVARCHAR(50)  NOT NULL UNIQUE,
    Password     NVARCHAR(100) NOT NULL,
    CONSTRAINT FK_Users_Employees FOREIGN KEY (ID_Employees)
        REFERENCES Employees(ID_Employees)
);
GO

/* Поставщики */
CREATE TABLE Suppliers (
    ID_Suppliers INT PRIMARY KEY IDENTITY(1,1),
    Name         NVARCHAR(100) NOT NULL,
    Phone        NVARCHAR(20)  NULL,
    Email        NVARCHAR(100) NULL,
    Contact_Name NVARCHAR(100) NULL
);
GO

/* Запчасти */
CREATE TABLE Parts (
    ID_Parts     INT PRIMARY KEY IDENTITY(1,1),
    ID_Suppliers INT           NULL,
    Name         NVARCHAR(100) NOT NULL,
    Quantity     INT           NOT NULL DEFAULT 0,
    Price        DECIMAL(10,2) NOT NULL DEFAULT 0,
    CONSTRAINT FK_Parts_Suppliers FOREIGN KEY (ID_Suppliers)
        REFERENCES Suppliers(ID_Suppliers) ON DELETE SET NULL,
    CONSTRAINT CHK_Parts_Quantity CHECK (Quantity >= 0)
);
GO

/* Клиенты */
CREATE TABLE Customers (
    ID_Customers INT PRIMARY KEY IDENTITY(1,1),
    Last_Name    NVARCHAR(50) NOT NULL,
    First_Name   NVARCHAR(50) NOT NULL,
    Middle_Name  NVARCHAR(50) NULL,
    Phone        NVARCHAR(20) NULL
);
GO

/* Автомобили */
CREATE TABLE Vehicles (
    ID_Vehicles  INT PRIMARY KEY IDENTITY(1,1),
    ID_Customers INT          NOT NULL,
    VIN          NVARCHAR(17) NOT NULL,
    Plate_Number NVARCHAR(20) NOT NULL,
    Brand        NVARCHAR(50) NULL,
    Model        NVARCHAR(50) NULL,
    Year         INT          NULL,
    CONSTRAINT FK_Vehicles_Customers FOREIGN KEY (ID_Customers)
        REFERENCES Customers(ID_Customers),
    CONSTRAINT UQ_Vehicles_VIN CHECK (LEN(VIN) = 17),
    CONSTRAINT CHK_Vehicles_Year CHECK (Year IS NULL OR (Year >= 1900 AND Year <= 2100))
);
GO

/* Заказ-наряды */
CREATE TABLE WorkOrders (
    ID_WorkOrders INT PRIMARY KEY IDENTITY(1,1),
    Number        NVARCHAR(20)  NOT NULL UNIQUE,
    ID_Customers  INT           NOT NULL,
    ID_Vehicles   INT           NOT NULL,
    Date_Open     DATE          NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    Date_Close    DATE          NULL,
    Status        NVARCHAR(30)  NOT NULL DEFAULT 'Открыт',
    Notes         NVARCHAR(500) NULL,
    CONSTRAINT FK_WorkOrders_Customers FOREIGN KEY (ID_Customers)
        REFERENCES Customers(ID_Customers),
    CONSTRAINT FK_WorkOrders_Vehicles FOREIGN KEY (ID_Vehicles)
        REFERENCES Vehicles(ID_Vehicles),
    CONSTRAINT CHK_WorkOrders_Status CHECK (Status IN (N'Открыт', N'В работе', N'Закрыт', N'Отменён')),
    CONSTRAINT CHK_WorkOrders_Dates  CHECK (Date_Close IS NULL OR Date_Close >= Date_Open)
);
GO

/* Запчасти в наряде */
CREATE TABLE WorkOrderParts (
    ID_WorkOrderParts INT PRIMARY KEY IDENTITY(1,1),
    ID_WorkOrders     INT           NOT NULL,
    ID_Parts          INT           NOT NULL,
    Quantity          INT           NOT NULL,
    Unit_Price        DECIMAL(10,2) NOT NULL,
    CONSTRAINT FK_WorkOrderParts_WorkOrders FOREIGN KEY (ID_WorkOrders)
        REFERENCES WorkOrders(ID_WorkOrders) ON DELETE CASCADE,
    CONSTRAINT FK_WorkOrderParts_Parts FOREIGN KEY (ID_Parts)
        REFERENCES Parts(ID_Parts),
    CONSTRAINT CHK_WorkOrderParts_Qty   CHECK (Quantity > 0),
    CONSTRAINT CHK_WorkOrderParts_Price CHECK (Unit_Price >= 0)
);
GO

/* Работы в наряде */
CREATE TABLE WorkOrderServices (
    ID_WorkOrderServices INT PRIMARY KEY IDENTITY(1,1),
    ID_WorkOrders        INT           NOT NULL,
    Description          NVARCHAR(200) NOT NULL,
    Price                DECIMAL(10,2) NOT NULL,
    CONSTRAINT FK_WorkOrderServices_WorkOrders FOREIGN KEY (ID_WorkOrders)
        REFERENCES WorkOrders(ID_WorkOrders) ON DELETE CASCADE,
    CONSTRAINT CHK_WorkOrderServices_Price CHECK (Price >= 0)
);
GO

/* Сотрудники на наряде  */
CREATE TABLE WorkOrderEmployees (
    ID_WorkOrderEmployees INT PRIMARY KEY IDENTITY(1,1),
    ID_WorkOrders         INT NOT NULL,
    ID_Employees          INT NOT NULL,
    CONSTRAINT FK_WorkOrderEmployees_WorkOrders FOREIGN KEY (ID_WorkOrders)
        REFERENCES WorkOrders(ID_WorkOrders) ON DELETE CASCADE,
    CONSTRAINT FK_WorkOrderEmployees_Employees FOREIGN KEY (ID_Employees)
        REFERENCES Employees(ID_Employees),
    CONSTRAINT UQ_WorkOrderEmployees UNIQUE (ID_WorkOrders, ID_Employees)
);
GO


/* Сотрудники */
INSERT INTO Employees (Last_Name, First_Name, Middle_Name, Position, Phone)
VALUES
    (N'Иванов',   N'Иван',    N'Иванович',   N'Механик',        N'+7 (900) 111-11-11'),
    (N'Петров',   N'Пётр',    N'Петрович',   N'Старший механик', N'+7 (900) 222-22-22'),
    (N'Сидоров',  N'Сидор',   N'Сидорович',  N'Кладовщик',      N'+7 (900) 333-33-33'),
    (N'Козлова',  N'Мария',   N'Александровна', N'Администратор', N'+7 (900) 444-44-44');
GO

/* Пользователь admin — логин: admin, пароль: admin */
INSERT INTO Users (ID_Employees, Username, Password)
VALUES (4, N'admin', N'admin');
GO

/* Поставщики */
INSERT INTO Suppliers (Name, Phone, Email, Contact_Name)
VALUES
    (N'АвтоДеталь Плюс',  N'+7 (812) 100-10-10', N'info@autod.ru',   N'Алексеев Дмитрий'),
    (N'МоторКомплект',    N'+7 (495) 200-20-20', N'sales@motork.ru', N'Борисов Сергей');
GO

/* Запчасти */
INSERT INTO Parts (ID_Suppliers, Name, Quantity, Price)
VALUES
    (1, N'Масляный фильтр Mann W712',     50, 450.00),
    (1, N'Воздушный фильтр Bosch S0001',  30, 680.00),
    (1, N'Тормозные колодки Brembo',      20, 1850.00),
    (2, N'Моторное масло 5W-40 (1л)',     100, 520.00),
    (2, N'Свечи зажигания NGK (к-т 4шт)', 25, 960.00),
    (2, N'Ремень ГРМ Gates',              15, 2200.00);
GO

/* Клиенты */
INSERT INTO Customers (Last_Name, First_Name, Middle_Name, Phone)
VALUES
    (N'Смирнов',  N'Алексей',  N'Владимирович', N'+7 (911) 555-55-55'),
    (N'Попова',   N'Елена',    N'Сергеевна',    N'+7 (911) 666-66-66'),
    (N'Новиков',  N'Виктор',   N'Павлович',     N'+7 (911) 777-77-77');
GO

/* Автомобили */
INSERT INTO Vehicles (ID_Customers, VIN, Plate_Number, Brand, Model, Year)
VALUES
    (1, N'WAUZZZ4G5DN123456', N'А123БВ178', N'Audi',  N'A4',     2019),
    (2, N'XTA210930Y2345678', N'В456ГД 78', N'Lada',  N'Vesta',  2021),
    (3, N'Z94CB41BBFR901234', N'Е789ЖЗ178', N'Kia',   N'Rio',    2020);
GO
