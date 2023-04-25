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
    public class LikeManager : ILikeService
    {
        private ILikeRepository _likeRepository;
        public LikeManager(ILikeRepository likeRepository)
        {
            _likeRepository= likeRepository;
        }

        public void AddToLike(string userId, int productId)
        {
          var like=GetLikeByUser(userId);
            if (like != null)
            {
                like.LikeItems.Add(new LikeItem
                {
                    LikeId=like.Id,
                    ProductId=productId
                });

                _likeRepository.Update(like);
            }
        }

        public void DeleteLike(string userId, int productId)
        {
            var like= GetLikeByUser(userId);
            if(like!= null)
            {
               _likeRepository.DeleteLike(like.Id,productId);
            }
        }

        public Like GetLikeByUser(string userId)
        {
            return _likeRepository.GetLikeByUser(userId);
        }

        public void InitilazerLike(string userId)
        {
            _likeRepository.Create(new Like()
            {
                UserId= userId
            });
        }
    }
}
