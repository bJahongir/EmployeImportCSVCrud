CREATE DATABASE EmployeeDB;
GO
USE EmployeeDB;
GO
CREATE TABLE Employees (
    Id INT IDENTITY PRIMARY KEY,
    PayrollNumber NVARCHAR(50),
    Forenames NVARCHAR(100),
    Surname NVARCHAR(100),
    DateOfBirth DATE,
    Telephone NVARCHAR(50),
    Mobile NVARCHAR(50),
    Address NVARCHAR(200),
    Address2 NVARCHAR(200),
    Postcode NVARCHAR(20),
    EmailHome NVARCHAR(150),
    StartDate DATE
);
