using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Order_Management_System.DAO;
using Order_Management_System.Entity;
using Order_Management_System.Exceptions;
using Order_Management_System.Util;


namespace Order_Management_System.DAO
{
    public class OrderProcessor : IOrderManagementRepository
    {
        public void CreateUser(User user)
        {
            using (SqlConnection conn = DBConnUtil.GetDBConnection())
            {
                string query = "INSERT INTO Users (UserId, Username, Password, Role) VALUES (@Id, @Username, @Password, @Role)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", user.UserId);
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Role", user.Role);
                cmd.ExecuteNonQuery();
            }
        }

        public void CreateProduct(User user, Product product)
        {
            if (user.Role.ToLower() != "admin") return;

            using (SqlConnection conn = DBConnUtil.GetDBConnection())
            {
                string query = "INSERT INTO Products (ProductId, ProductName, Description, Price, QuantityInStock, Type) " +
                               "VALUES (@Id, @Name, @Desc, @Price, @Qty, @Type)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", product.ProductId);
                cmd.Parameters.AddWithValue("@Name", product.ProductName);
                cmd.Parameters.AddWithValue("@Desc", product.Description);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Qty", product.QuantityInStock);
                cmd.Parameters.AddWithValue("@Type", product.Type);
                cmd.ExecuteNonQuery();

                if (product.Type.ToLower() == "electronics" && product is Electronics e)
                {
                    string q = "INSERT INTO Electronics (ProductId, Brand, WarrantyPeriod) VALUES (@Id, @Brand, @Warranty)";
                    SqlCommand c2 = new SqlCommand(q, conn);
                    c2.Parameters.AddWithValue("@Id", product.ProductId);
                    c2.Parameters.AddWithValue("@Brand", e.Brand);
                    c2.Parameters.AddWithValue("@Warranty", e.WarrantyPeriod);
                    c2.ExecuteNonQuery();
                }
                else if (product.Type.ToLower() == "clothing" && product is Clothing c)
                {
                    string q = "INSERT INTO Clothing (ProductId, Size, Color) VALUES (@Id, @Size, @Color)";
                    SqlCommand c2 = new SqlCommand(q, conn);
                    c2.Parameters.AddWithValue("@Id", product.ProductId);
                    c2.Parameters.AddWithValue("@Size", c.Size);
                    c2.Parameters.AddWithValue("@Color", c.Color);
                    c2.ExecuteNonQuery();
                }
            }
        }

        public void CreateOrder(User user, List<Product> products, int orderId)
        {
            int userId = GetUserId(user.Username);
            using (SqlConnection conn = DBConnUtil.GetDBConnection())
            {
                string query = "INSERT INTO Orders (OrderId, UserId, OrderDate) VALUES (@OrderId, @UserId, GETDATE())";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@OrderId", orderId);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.ExecuteNonQuery();

                foreach (var p in products)
                {
                    Console.Write($"Enter OrderDetail ID for Product ID {p.ProductId}: ");
                    int detailId = int.Parse(Console.ReadLine());

                    string q = "INSERT INTO OrderDetails (OrderDetailId, OrderId, ProductId, Quantity) " +
                               "VALUES (@DetailId, @OrderId, @ProductId, @Qty)";
                    SqlCommand c2 = new SqlCommand(q, conn);
                    c2.Parameters.AddWithValue("@DetailId", detailId);
                    c2.Parameters.AddWithValue("@OrderId", orderId);
                    c2.Parameters.AddWithValue("@ProductId", p.ProductId);
                    c2.Parameters.AddWithValue("@Qty", p.QuantityInStock);
                    c2.ExecuteNonQuery();
                }
            }
        }

        public void CancelOrder(int userId, int orderId)
        {
            using (SqlConnection conn = DBConnUtil.GetDBConnection())
            {
                string check = "SELECT COUNT(*) FROM Orders WHERE OrderId = @OrderId AND UserId = @UserId";
                SqlCommand cmd = new SqlCommand(check, conn);
                cmd.Parameters.AddWithValue("@OrderId", orderId);
                cmd.Parameters.AddWithValue("@UserId", userId);

                int count = (int)cmd.ExecuteScalar();
                if (count == 0)
                    throw new OrderNotFoundException("Order not found.");

                // Delete from OrderDetails
                SqlCommand deleteDetails = new SqlCommand("DELETE FROM OrderDetails WHERE OrderId = @OrderId", conn);
                deleteDetails.Parameters.AddWithValue("@OrderId", orderId);
                deleteDetails.ExecuteNonQuery();

                // Delete from Orders
                SqlCommand deleteOrder = new SqlCommand("DELETE FROM Orders WHERE OrderId = @OrderId", conn);
                deleteOrder.Parameters.AddWithValue("@OrderId", orderId);
                deleteOrder.ExecuteNonQuery();

                Console.WriteLine("Order cancelled successfully.");
            }
        }


        public List<Product> GetAllProducts()
        {
            List<Product> list = new List<Product>();
            using (SqlConnection conn = DBConnUtil.GetDBConnection())
            {
                string query = "SELECT * FROM Products";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Product
                    {
                        ProductId = (int)reader["ProductId"],
                        ProductName = reader["ProductName"].ToString(),
                        Description = reader["Description"].ToString(),
                        Price = Convert.ToDouble(reader["Price"]),
                        QuantityInStock = (int)reader["QuantityInStock"],
                        Type = reader["Type"].ToString()
                    });
                }
            }
            return list;
        }

        public List<Product> GetOrderByUser(User user)
        {
            List<Product> products = new List<Product>();
            int userId = GetUserId(user.Username);
            using (SqlConnection conn = DBConnUtil.GetDBConnection())
            {
                string query = @"SELECT p.* FROM Products p
                                 JOIN OrderDetails od ON p.ProductId = od.ProductId
                                 JOIN Orders o ON o.OrderId = od.OrderId
                                 WHERE o.UserId = @UserId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ProductId = (int)reader["ProductId"],
                        ProductName = reader["ProductName"].ToString(),
                        Description = reader["Description"].ToString(),
                        Price = Convert.ToDouble(reader["Price"]),
                        QuantityInStock = (int)reader["QuantityInStock"],
                        Type = reader["Type"].ToString()
                    });
                }
            }
            return products;
        }

        private int GetUserId(string username)
        {
            using (SqlConnection conn = DBConnUtil.GetDBConnection())
            {
                string query = "SELECT UserId FROM Users WHERE Username = @Username";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                return (int)cmd.ExecuteScalar();
            }
        }
    }
}

