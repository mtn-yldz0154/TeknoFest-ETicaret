using BuissnessLayer.Abstract;
using DataAccessLayer.Concrete.EfCore;
using ElektronikWebUI.Identity;
using ElektronikWebUI.Models;
using EntityLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElektronikWebUI.Controllers
{
    public class UserController : Controller
    {
        private IProductService _productService;
        
        private ICategoryService _categoryService;
        private UserManager<Customer> _userManager;
       ApplicationContext _context=new ApplicationContext();
        public UserController(IProductService productService, ICategoryService categoryService, UserManager<Customer> userManager)
        {
            _categoryService= categoryService;
            _productService= productService;
            _userManager= userManager;
        }

        public IActionResult Page()
        {
            return View();
        }

        public IActionResult Index()
        {
            var entity = _productService.GetAll();

            var model = new List<ProductModel>();

            foreach (var item in entity)
            {
                ProductModel urun=new ProductModel();
                urun.Category = item.Category;
                urun.StarNumber = item.StarNumber;
                urun.Yorums = item.Yorums;
                urun.CategoryId = item.CategoryId;
                urun.ProductStock = item.ProductStock;
                urun.ProductOzellik = item.ProductOzellik;
                urun.ProductName = item.ProductName;
                urun.ProductLongName = item.ProductLongName;
                urun.ProductSmallImageUrl = item.ProductSmallImageUrl;
                urun.SaleProductPrice = item.SaleProductPrice;
                urun.ProductId = item.ProductId;
                urun.ProductImageUrl = item.ProductImageUrl;
                urun.ProductPrice = item.ProductPrice;

                model.Add(urun);
            }

          

            return View(model);
        }

        public IActionResult ProductDetails(int id)
        {
            var entity=_productService.GetById(id);

            var product = new ProductModel()
            {
                CategoryId= entity.CategoryId,
                ProductDespcription=entity.ProductDespcription,
                ProductImageUrl=entity.ProductImageUrl,
                ProductName=entity.ProductName,
                ProductImageUrl2=entity.ProductImageUrl2,
                ProductImageUrl3=entity.ProductImageUrl3,
                ProductSmallImageUrl=entity.ProductSmallImageUrl,
                ProductPrice=entity.ProductPrice,
                ProductStock=entity.ProductStock,
                ProductLongName=entity.ProductLongName,
                SaleProductPrice=entity.SaleProductPrice,
                ProductOzellik = entity.ProductOzellik ,
                StarNumber=entity.StarNumber,
                ProductId=entity.ProductId,
                Category = entity.Category,               
            };

            using (var db=new Context())
            {
                product.BenzerUrunler=db.Products.Where(i=>i.CategoryId==entity.CategoryId && i.ProductId!=entity.ProductId).ToList();
                product.Yorums=db.Yorums.Where(i=>i.ProductId==entity.ProductId).ToList();  
            }
          


            return View(product);
        }

        public IActionResult CategoryDetails(int id)
        {
            var ctg=_categoryService.GetById(id);
            ViewBag.ctg=ctg.CategoryName;


            var entity=_productService.GetAll().Where(i=>i.CategoryId==id).ToList();
            return View(entity);
        }

        [HttpPost]
        public IActionResult Search(string q)
        {
            var entity = _productService.GetAll();

            if (!string.IsNullOrEmpty(q))
            {
                entity=_productService.GetAll().Where(i=>i.ProductName.ToLower().Contains(q.ToLower())).ToList();   

            }

            ViewBag.q=q;

            return View(entity);
        }

        [HttpGet]
        public async Task<IActionResult> AccountSetting(string id)
        {
            var user=await _userManager.FindByNameAsync(id);
            if(user!=null)
            {
                var account = new UserAccountModel()
                {
                    UserId= user.Id,
                    FirstName=user.FirstName,
                    LastName=user.LastName,
                    Email=user.Email,
                    UserName=user.UserName,
                    Phone=user.PhoneNumber,
                    ProfilePhoto=user.ProfilePhoto,
                    Password=user.Password
                };
                return View(account);
            }
            else
            {
                return RedirectToAction("page");
            }
           
           
        }
        [HttpPost]
        public async Task<IActionResult> AccountSetting(UserAccountModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user!=null)
                {
                    user.FirstName= model.FirstName;
                    user.LastName= model.LastName;
                    user.Email= model.Email;
                    user.UserName= model.UserName;
                    user.PhoneNumber = model.Phone;
                    user.Password= model.Password;
                   ;

                   var result= await _userManager.UpdateAsync(user);

                    if(result.Succeeded)
                    {
                        return Redirect("~/user/AccountSetting/"+model.UserName);
                    }          
                }

            }
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddComment(int productId,string comment)
        {
            if(ModelState.IsValid)
            {
                var yorum = new Yorum()
                {
                    Comment = comment,
                    ProductId = productId,
                    UserName = User.Identity.Name,
                    UserPP = User.Identity.Name+ " .jpg",
                    UserDate=DateTime.Parse(DateTime.Now.ToLongDateString())
                };

                using (var context = new Context())
                {
                    context.Yorums.Add(yorum);
                    context.SaveChanges();
                }

                return Redirect("/user/productdetails/" + productId);
            }

            return Redirect("/user/productdetails/" + productId);
        }

        [HttpPost]
        public IActionResult DeleteComment(int commentId,int productId)
        {
            if(ModelState.IsValid)
            {
                using (var context=new Context())
                {
                    var yorum =context.Yorums.Where(i=>i.Id==commentId&& i.ProductId==productId).FirstOrDefault();
                    context.Yorums.Remove(yorum);
                    context.SaveChanges();

                }
                return Redirect("/user/productdetails/" + productId);
            }
            return Redirect("/user/productdetails/" + productId);
        }


    }
}
