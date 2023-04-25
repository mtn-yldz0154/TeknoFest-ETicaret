using BuissnessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuissnessLayer.Concrete
{
    public class OrderManager : IOrderService
    {
         private  IOrderRepository _orderRepository;
        public OrderManager(IOrderRepository orderRepository) 
        {
            _orderRepository= orderRepository;
        }

        public void ClearCart(string userId)
        {
            

            _orderRepository.ClearCart(userId);
        }

        public void Create(Order entity)
        {
          _orderRepository.Create(entity);
        }

        public List<Order> GetOrders(string userId)
        {
            return  _orderRepository.GetOrder(userId);
        }
    }
}
