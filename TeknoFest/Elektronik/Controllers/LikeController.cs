using BuissnessLayer.Abstract;
using ElektronikWebUI.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ElektronikWebUI.Controllers
{
    [Authorize]
    public class LikeController : Controller
    {
        private ILikeService _likeService;
        UserManager<Customer> _userManager;
        public LikeController(ILikeService likeService, UserManager<Customer> userManager) 
        {
            _likeService = likeService;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
           var userId = _userManager.GetUserId(User);
            return View(_likeService.GetLikeByUser(userId));
        }

        public IActionResult AddToLike(int productId)
        {
            _likeService.AddToLike(_userManager.GetUserId(User), productId);

            return RedirectToAction("Index");
        }
        
        public IActionResult DeleteLike(int productId)
        {
            _likeService.DeleteLike(_userManager.GetUserId(User), productId);

            return RedirectToAction("Index");
        }
    }
}
