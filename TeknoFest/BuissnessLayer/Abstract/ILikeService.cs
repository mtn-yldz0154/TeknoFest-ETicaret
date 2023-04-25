using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuissnessLayer.Abstract
{
    public interface ILikeService
    {
        void InitilazerLike(string userId);

        Like GetLikeByUser(string userId);
        void  AddToLike(string userId,int productId);
        void DeleteLike(string userId,int productId);
    }
}
