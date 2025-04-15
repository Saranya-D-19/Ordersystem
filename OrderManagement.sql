CREATE DATABASE OrderManagementDB;
GO
USE OrderManagementDB;

CREATE TABLE Users (
    UserId INT PRIMARY KEY ,
    Username NVARCHAR(100),
    Password NVARCHAR(100),
    Role NVARCHAR(50)
);

CREATE TABLE Products (
    ProductId INT PRIMARY KEY ,
    ProductName NVARCHAR(100),
    Description NVARCHAR(255),
    Price FLOAT,
    QuantityInStock INT,
    Type NVARCHAR(50)
);

CREATE TABLE Electronics (
    ProductId INT PRIMARY KEY,
    Brand NVARCHAR(100),
    WarrantyPeriod INT,
    FOREIGN KEY(ProductId) REFERENCES Products(ProductId)
);

CREATE TABLE Clothing (
    ProductId INT PRIMARY KEY,
    Size NVARCHAR(10),
    Color NVARCHAR(50),
    FOREIGN KEY(ProductId) REFERENCES Products(ProductId)
);

CREATE TABLE Orders (
    OrderId INT PRIMARY KEY ,
    UserId INT,
    OrderDate DATETIME,
    FOREIGN KEY(UserId) REFERENCES Users(UserId)
);

CREATE TABLE OrderDetails (
    OrderDetailId INT PRIMARY KEY ,
    OrderId INT,
    ProductId INT,
    Quantity INT,
    FOREIGN KEY(OrderId) REFERENCES Orders(OrderId),
    FOREIGN KEY(ProductId) REFERENCES Products(ProductId)
);

INSERT INTO Users (UserId, Username, Password, Role) VALUES
(1, 'admin1', 'admin@123', 'Admin'),
(2, 'john_doe', 'john123', 'User'),
(3, 'sarah_lee', 'sarahpass', 'User'),
(4, 'mike_smith', 'mike456', 'User'),
(5, 'admin2', 'admin@456', 'Admin'),
(6, 'emily', 'emily789', 'User'),
(7, 'david', 'davidpwd', 'User'),
(8, 'alice', 'alicepass', 'User'),
(9, 'robert', 'rob123', 'User'),
(10, 'nancy', 'nancy456', 'User');

INSERT INTO Products (ProductId, ProductName, Description, Price, QuantityInStock, Type) VALUES
(101, 'iPhone 14', 'Apple smartphone', 79999, 10, 'Electronics'),
(102, 'Samsung TV', '55 inch LED Smart TV', 49999, 5, 'Electronics'),
(103, 'Bluetooth Speaker', 'Portable speaker', 1999, 15, 'Electronics'),
(104, 'Laptop', 'Gaming laptop', 85999, 7, 'Electronics'),
(105, 'Formal Shirt', 'Men white shirt', 999, 20, 'Clothing'),
(106, 'Jeans', 'Denim jeans', 1499, 30, 'Clothing'),
(107, 'T-Shirt', 'Cotton casual wear', 799, 25, 'Clothing'),
(108, 'Smartwatch', 'Fitness tracker watch', 2999, 12, 'Electronics'),
(109, 'Hoodie', 'Winter hoodie', 1299, 18, 'Clothing'),
(110, 'Microwave Oven', 'Convection type', 6999, 6, 'Electronics');

INSERT INTO Electronics (ProductId, Brand, WarrantyPeriod) VALUES
(101, 'Apple', 12),
(102, 'Samsung', 24),
(103, 'Boat', 6),
(104, 'ASUS', 24),
(108, 'Realme', 12),
(110, 'IFB', 18);

INSERT INTO Clothing (ProductId, Size, Color) VALUES
(105, 'L', 'White'),
(106, 'M', 'Blue'),
(107, 'XL', 'Black'),
(109, 'L', 'Grey');

INSERT INTO Orders (OrderId, UserId, OrderDate) VALUES
(1, 2, '2024-04-10'),
(2, 3, '2024-04-11'),
(3, 4, '2024-04-12'),
(4, 6, '2024-04-13'),
(5, 7, '2024-04-13'),
(6, 8, '2024-04-14'),
(7, 9, '2024-04-14'),
(8, 10, '2024-04-15'),
(9, 3, '2024-04-15'),
(10, 2, '2024-04-16');


INSERT INTO OrderDetails (OrderDetailId, OrderId, ProductId, Quantity) VALUES
(1, 1, 101, 1),
(2, 2, 105, 2),
(3, 3, 106, 1),
(4, 4, 108, 1),
(5, 5, 103, 2),
(6, 6, 109, 1),
(7, 7, 107, 1),
(8, 8, 110, 1),
(9, 9, 102, 1),
(10, 10, 104, 1);



SELECT * FROM Users;
SELECT * FROM Products;
SELECT * FROM Electronics;
SELECT * FROM Clothing;
SELECT * FROM Orders;
SELECT * FROM OrderDetails;



