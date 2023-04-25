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
    public class EfCoreCartRepository : EfCoreGenericRepository<Cart, Context>, ICartRepository
    {

        public override void Update(Cart entity)
        {
            using (var context = new Context())
            {
                context.Carts.Update(entity);
                context.SaveChanges();
            }
        }

        public Cart GetCartByUserId(string userId)
        {
            using (var context =new Context())
            {
                return context.Carts.Include(i=>i.CartItems).ThenInclude(i=>i.Product).FirstOrDefault(i=>i.UserId== userId);
            }
        }

        public void DeleteFromCart(int cartId, int productId)
        {
            using (var context=new Context())
            {
           var cartItems=context.CartItems.Where(i => i.CartId == cartId && i.ProductId == productId).FirstOrDefault();

                context.CartItems.Remove(cartItems);
                context.SaveChanges();
            }
        }
    }
}
