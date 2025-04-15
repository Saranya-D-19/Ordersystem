using Order_Management_System.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Order_Management_System.DAO;

namespace Order_Management_System.DAO
{
    public interface IOrderManagementRepository
    {
        void CreateUser(User user);
        void CreateProduct(User user, Product product);
        public void CreateOrder(User user, List<Product> products, int orderId);
        void CancelOrder(int userId, int orderId);
        List<Product> GetAllProducts();
        List<Product> GetOrderByUser(User user);
    }
}
