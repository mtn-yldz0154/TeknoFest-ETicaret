using DataAccessLayer.Abstract;
using EntityLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.EfCore
{
    public class EfCoreOrderRepository : EfCoreGenericRepository<Order, Context>, IOrderRepository
    {
        public void ClearCart(string userId)
        {
            using (var context=new Context())
            {
                var cart = context.Carts.Where(i => i.UserId == userId).FirstOrDefault();

                var cartItem=context.CartItems.Where(i=>i.CartId==cart.Id).ToList();

                foreach (var item in cartItem)
                {
                    context.CartItems.Remove(item);
                    context.SaveChanges();
                }
            }
        }

        public List<Order> GetOrder(string userId)
        {
            using (var context=new Context())
            {
                var orders = context.Orders.
                    Include(i => i.OrderItems).
                    ThenInclude(i => i.Product).
                    Where(i => i.UserId == userId).OrderByDescending(i=>i.OrderDate).ToList();

                return orders;
            }


        }

       
    }
}
