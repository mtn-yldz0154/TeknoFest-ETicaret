﻿using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface ICartRepository:IRepository<Cart>
    {
        void DeleteFromCart(int cartId, int productId);
        Cart GetCartByUserId(string userId);

    }
}
