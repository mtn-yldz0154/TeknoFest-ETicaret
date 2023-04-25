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
    public class EfCoreLikeRepository : EfCoreGenericRepository<Like, Context>, ILikeRepository
    {
        public void DeleteLike(int LikeId, int productId)
        {
            using (var context=new Context())
            {
                var like=context.LikeItems.Where(i=>i.LikeId==LikeId && i.ProductId==productId).FirstOrDefault();
                context.LikeItems.Remove(like);
                context.SaveChanges();
            }
        }

        public Like GetLikeByUser(string userId)
        {
         
            using (var context=new Context())
            {
                return context.Likes.Include(i => i.LikeItems).ThenInclude(i => i.Product).FirstOrDefault(i=>i.UserId==userId);
            }
        }

        public override void Update(Like entity)
        {
            using (var context = new Context())
            {
                context.Likes.Update(entity);
                context.SaveChanges();
            }
        }
    }
}
