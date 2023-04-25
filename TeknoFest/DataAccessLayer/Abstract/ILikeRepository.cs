using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface ILikeRepository : IRepository<Like>
    {
        void DeleteLike(int LikeId, int productId);
        Like GetLikeByUser(string userId);
    }
}
