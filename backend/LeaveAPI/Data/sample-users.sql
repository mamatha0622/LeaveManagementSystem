-- Sample SQL script to create Roles and Users tables and insert example users.
-- Adjust passwords to hashed values if you implement password hashing.

CREATE TABLE Roles (
    RoleId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL
);

CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    RoleId INT NOT NULL,
    ManagerId INT NULL,
    CONSTRAINT FK_Users_Roles FOREIGN KEY (RoleId) REFERENCES Roles(RoleId),
    CONSTRAINT FK_Users_Manager FOREIGN KEY (ManagerId) REFERENCES Users(UserId)
);

INSERT INTO Roles (Name)
VALUES
    ('Employee'),
    ('Manager'),
    ('Admin');

INSERT INTO Users (Name, Email, PasswordHash, RoleId, ManagerId)
VALUES
    ('Alice Employee', 'alice.employee@company.com', 'PasswordHash123', 1, 2),
    ('Bob Manager', 'bob.manager@company.com', 'PasswordHash123', 2, NULL),
    ('Carol Admin', 'carol.admin@company.com', 'PasswordHash123', 3, NULL),
    ('David Employee', 'david.employee@company.com', 'PasswordHash123', 1, 2);
