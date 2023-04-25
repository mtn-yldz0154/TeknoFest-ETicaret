using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IOrderRepository : IRepository<Order>
    {
        void ClearCart(string userId);
        List<Order> GetOrder(string userId);
       
    }
}
