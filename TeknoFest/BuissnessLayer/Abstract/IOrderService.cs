using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuissnessLayer.Abstract
{
    public interface IOrderService
    {
        void Create(Order entity);

        void ClearCart(string userId);

       List< Order> GetOrders(string userId);
    }
}
