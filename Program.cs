using System;
using System.Collections.Generic;
using Order_Management_System.DAO;
using Order_Management_System.Entity;
using Order_Management_System.Exceptions;


namespace Order_Management_System.main
{
    class MainModule
    {
        static void Main(string[] args)
        {
            IOrderManagementRepository repo = new OrderProcessor();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n===== ORDER MANAGEMENT MENU =====");
                Console.WriteLine("1. Create User");
                Console.WriteLine("2. Create Product (Admin Only)");
                Console.WriteLine("3. Create Order");
                Console.WriteLine("4. Cancel Order");
                Console.WriteLine("5. Get All Products");
                Console.WriteLine("6. Get Orders by User");
                Console.WriteLine("7. Exit");
                Console.Write("Enter your choice: ");

                string input = Console.ReadLine();
                int choice;
                if (!int.TryParse(input, out choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                try
                {
                    switch (choice)
                    {
                        case 1:
                            Console.Write("Enter User ID: ");
                            int userId = int.Parse(Console.ReadLine());
                            Console.Write("Enter Username: ");
                            string username = Console.ReadLine();
                            Console.Write("Enter Password: ");
                            string password = Console.ReadLine();
                            Console.Write("Enter Role (Admin/User): ");
                            string role = Console.ReadLine();

                            User user = new User { UserId = userId, Username = username, Password = password, Role = role };
                            repo.CreateUser(user);
                            Console.WriteLine("User created successfully.");
                            break;

                        case 2:
                            Console.Write("Enter Admin Username: ");
                            string adminName = Console.ReadLine();
                            Console.Write("Enter Role: ");
                            string adminRole = Console.ReadLine();
                            User admin = new User { Username = adminName, Role = adminRole };

                            Console.Write("Enter Product ID: ");
                            int productId = int.Parse(Console.ReadLine());
                            Console.Write("Product Name: ");
                            string pname = Console.ReadLine();
                            Console.Write("Description: ");
                            string desc = Console.ReadLine();
                            Console.Write("Price: ");
                            double price = double.Parse(Console.ReadLine());
                            Console.Write("Stock: ");
                            int stock = int.Parse(Console.ReadLine());
                            Console.Write("Type (Electronics/Clothing): ");
                            string type = Console.ReadLine();

                            Product product;
                            if (type.ToLower() == "electronics")
                            {
                                Console.Write("Brand: ");
                                string brand = Console.ReadLine();
                                Console.Write("Warranty Period: ");
                                int warranty = int.Parse(Console.ReadLine());

                                product = new Electronics
                                {
                                    ProductId = productId,
                                    ProductName = pname,
                                    Description = desc,
                                    Price = price,
                                    QuantityInStock = stock,
                                    Type = type,
                                    Brand = brand,
                                    WarrantyPeriod = warranty
                                };
                            }
                            else
                            {
                                Console.Write("Size: ");
                                string size = Console.ReadLine();
                                Console.Write("Color: ");
                                string color = Console.ReadLine();

                                product = new Clothing
                                {
                                    ProductId = productId,
                                    ProductName = pname,
                                    Description = desc,
                                    Price = price,
                                    QuantityInStock = stock,
                                    Type = type,
                                    Size = size,
                                    Color = color
                                };
                            }

                            repo.CreateProduct(admin, product);
                            Console.WriteLine("Product created successfully.");
                            break;

                        case 3:
                            Console.Write("Enter Username: ");
                            string orderUser = Console.ReadLine();
                            Console.Write("Enter Role: ");
                            string orderRole = Console.ReadLine();
                            User orderCustomer = new User { Username = orderUser, Role = orderRole };

                            Console.Write("Enter Order ID: ");
                            int orderId = int.Parse(Console.ReadLine());

                            Console.Write("Number of products in the order: ");
                            int count = int.Parse(Console.ReadLine());

                            List<Product> productList = new List<Product>();
                            for (int i = 0; i < count; i++)
                            {
                                Console.Write($"Enter Product ID for item {i + 1}: ");
                                int pid = int.Parse(Console.ReadLine());
                                Console.Write("Quantity: ");
                                int qty = int.Parse(Console.ReadLine());
                                productList.Add(new Product { ProductId = pid, QuantityInStock = qty });
                            }

                            repo.CreateOrder(orderCustomer, productList, orderId); 
                            Console.WriteLine("Order created successfully.");
                            break;


                        case 4:
                            Console.Write("Enter User ID: ");
                            int cancelUserId = int.Parse(Console.ReadLine());
                            Console.Write("Enter Order ID to cancel: ");
                            int cancelOrderId = int.Parse(Console.ReadLine());
                            repo.CancelOrder(cancelUserId, cancelOrderId);
                            Console.WriteLine("Order cancelled successfully.");
                            break;

                        case 5:
                            var products = repo.GetAllProducts();
                            Console.WriteLine("\n--- All Products ---");
                            foreach (var p in products)
                            {
                                Console.WriteLine($"{p.ProductId}: {p.ProductName} | {p.Type} | ₹{p.Price} | Qty: {p.QuantityInStock}");
                            }
                            break;

                        case 6:
                            Console.Write("Enter Username: ");
                            string userU = Console.ReadLine();
                            Console.Write("Enter Role: ");
                            string roleU = Console.ReadLine();
                            User getUser = new User { Username = userU, Role = roleU };

                            var userOrders = repo.GetOrderByUser(getUser);
                            Console.WriteLine($"\n--- Products ordered by {userU} ---");
                            foreach (var p in userOrders)
                            {
                                Console.WriteLine($"{p.ProductId}: {p.ProductName} | ₹{p.Price} | Qty: {p.QuantityInStock}");
                            }
                            break;

                        case 7:
                            exit = true;
                            Console.WriteLine("Exiting the system. Goodbye!");
                            break;

                        default:
                            Console.WriteLine("Invalid choice. Please select a valid option.");
                            break;
                    }
                }
                catch (UserNotFoundException ex)
                {
                    Console.WriteLine("User Error: " + ex.Message);
                }
                catch (OrderNotFoundException ex)
                {
                    Console.WriteLine("Order Error: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
        }
    }
}

