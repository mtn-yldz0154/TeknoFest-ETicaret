using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuissnessLayer.Abstract
{
    public interface ICartService
    {
        void InitilazerCart(string userId);

        Cart GetCartByUserId(string userId);

        void AddToCart(string userId,int productId,int quantity);
        void DeleteFromCart(string userId, int productId);
    }
}
