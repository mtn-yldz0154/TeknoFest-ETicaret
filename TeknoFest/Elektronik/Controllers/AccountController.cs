using BuissnessLayer.Abstract;
using DataAccessLayer.Concrete.EfCore;
using ElektronikWebUI.EmailServices;
using ElektronikWebUI.Identity;
using ElektronikWebUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ElektronikWebUI.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private UserManager<Identity.Customer> _userManager;
        private SignInManager<Identity.Customer> _signInManager;
        private IEmailSender _emailSender;
        private ICartService _cartService;
        private ILikeService _likeService;


        public AccountController(ILikeService likeService,ICartService cartService,UserManager<Identity.Customer> userManager, SignInManager<Identity.Customer> signInManager,IEmailSender emailSender)
        {
            _likeService = likeService;
            _cartService = cartService;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı Bulunamadı");
                return View(model);
            }

          

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);

            if (result.Succeeded)
            {
                return Redirect("/user/index");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new Identity.Customer
            {
               
                FirstName = model.Name,
                Email = model.Email,
                UserName = model.UserName,
                Password=model.Password
               

            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                //Cart Objesi
                //Like Objesi
                _cartService.InitilazerCart(user.Id);
                _likeService.InitilazerLike(user.Id);
                
                //generite token
               //var code=await _userManager.GenerateEmailConfirmationTokenAsync(user);
               // var url = Url.Action("ConfirmEmail", "Account", new
               // {
               //     userId=user.Id,
               //     token=code
               // });
               // //email
               // await _emailSender.SendEmailAsync(model.Email, "Hesabınızı onaylayınız.", $"Lütfen email hesabınızı onaylamak için linke <a href='https://localhost:44303{url}'>tıklayınız.</a>");
                return RedirectToAction("login", "account");
            }
            ModelState.AddModelError("", "Bilinmeyen Bir Hata Oluştu Tekrar Deneyiniz");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("index", "user");
        }

        public IActionResult Accessdenied()
        {
            return View();
        }

        public async Task<IActionResult> ConfirmEmail(string userId,string token)
        {
            if(userId != null && token!=null)
            {
                var user=await _userManager.FindByIdAsync(userId);
                if (user!=null)
                {
                    var result=await _userManager.ConfirmEmailAsync(user, token);
                    if(result.Succeeded)
                    {
                        return View();
                    }
                }
                return RedirectToAction("index", "user");
            }
            return RedirectToAction("index", "user");
            
        }

    }
}
