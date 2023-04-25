using BuissnessLayer.Abstract;
using DataAccessLayer.Concrete.EfCore;
using ElektronikWebUI.Identity;
using ElektronikWebUI.Models;
using EntityLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElektronikWebUI.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private ICategoryService _categoryService;
        private IProductService _productService;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<Customer> _userManager;
        public AdminController(IProductService productService,ICategoryService categoryService,RoleManager<IdentityRole> roleManager,UserManager<Customer> userManager)
        {
            _categoryService= categoryService;
            _productService= productService;
            _roleManager= roleManager;
            _userManager= userManager;
        }


        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> UserEdit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var selectedRoles = await _userManager.GetRolesAsync(user);
                var roles = _roleManager.Roles.Select(i => i.Name);

                ViewBag.Roles = roles;
                return View(new UserDetailsModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    SelectedRoles = selectedRoles
                });
            }
            return Redirect("~/admin/users/");
        }


        [HttpPost]
        public async Task<IActionResult> UserEdit(UserDetailsModel model, string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.EmailConfirmed = model.EmailConfirmed;

                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        var userRoles = await _userManager.GetRolesAsync(user);
                        selectedRoles = selectedRoles ?? new string[] { };
                        await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles).ToArray<string>());
                        await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles).ToArray<string>());

                        return Redirect("/admin/users");
                    }
                }
                return Redirect("/admin/users");
            }

            return View(model);

        }

        public IActionResult Users()
        {
            return View(_userManager.Users);
        }
        public async Task<IActionResult> RoleEdit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            var members = new List<Customer>();
            var nonmembers = new List<Customer>();

            foreach (var user in _userManager.Users.ToList())
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    members.Add(user);
                }
                else
                {
                    nonmembers.Add(user);
                }

            }
            var model = new RoleDetails()
            {
                Role = role,
                Members = members,
                NonMembers = nonmembers
            };
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> RoleEdit(RoleEditModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var userId in model.IdsToAdd ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.AddToRoleAsync(user, model.RoleName);

                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }

                foreach (var userId in model.IdsToDelete ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);

                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }
            }
            return Redirect("/Admin/roleEdit/" + model.RoleId);


        }
        public IActionResult RoleList()
        {
            return View(_roleManager.Roles);
        }

        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
		public async Task<IActionResult> CreateRole(RoleModel model)
		{
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(model.RoleName));

            if(result.Succeeded)
            {
                return RedirectToAction("rolelist", "admin");
            }

			return View(model);
		}

        

		public IActionResult GetCategory()
        {
          var entity=_categoryService.GetAll();
            return View(entity);
        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddCategory(Category category)
        {
            _categoryService.Create(category);
            return RedirectToAction("GetCategory");
        }

        public IActionResult DeleteCategory(int id)
        {
            _categoryService.Delete(id);
            return RedirectToAction("GetCategory");
        }

        [HttpGet]
        public IActionResult UpdateCategory(int id)
        {
            var entity=_categoryService.GetById(id);

            return View(entity);
        }

        [HttpPost]
        public IActionResult UpdateCategory(Category category)
        {
            _categoryService.Update(category);
            return RedirectToAction("GetCategory");
        }

        public IActionResult CategoryDetails(int id)
        {
            var product=_productService.GetAll().Where(i=>i.CategoryId==id).ToList();

            
            var entity=_categoryService.GetAll().Where(i=>i.CategoryId== id).FirstOrDefault();
            ViewBag.entity = entity.CategoryName;

            return View(product);
        }

        public IActionResult GetProduct()
        {
            var entity=_productService.GetAll();    
            return View(entity);
        }
        [HttpGet]
        public IActionResult AddProduct()
        {
            List<SelectListItem> values = (from i in _categoryService.GetAll().ToList()
                                           select new SelectListItem
                                           {
                                               Text = i.CategoryName,
                                               Value = i.CategoryId.ToString()
                                           }).ToList();
            ViewBag.Values = values;
            return View();
        }
        [HttpPost]
        public IActionResult AddProduct(Product product,IFormFile file,IFormFile files, IFormFile file2, IFormFile file3)
        {
            if (ModelState.IsValid)
            {
                product.ProductImageUrl = file.FileName;
                product.ProductSmallImageUrl = files.FileName;
                product.ProductImageUrl2= file2.FileName;
                product.ProductImageUrl3= file3.FileName;

                _productService.Create(product);
                return RedirectToAction("GetProduct");

            }
            return View(product);
       
        }

        public IActionResult DeleteProduct(int id)
        {
            _productService.Delete(id);
            return RedirectToAction("GetProduct");
        }

        [HttpGet]
        public IActionResult UpdateProduct(int id)
        {
            List<SelectListItem> values = (from i in _categoryService.GetAll().ToList()
                                           select new SelectListItem
                                           {
                                               Text = i.CategoryName,
                                               Value = i.CategoryId.ToString()
                                           }).ToList();
            ViewBag.Values = values;

            var entity=_productService.GetById(id);

            return View(entity);
        }

        [HttpPost]
        public IActionResult UpdateProduct(Product product,IFormFile file,IFormFile files)
        {
            var entity = _productService.GetById(product.ProductId);
            if(ModelState.IsValid)
            {
                if(file!=null)
                {
                    entity.ProductImageUrl = file.FileName;
                }
                if (files != null)
                {
                    entity.ProductSmallImageUrl = files.FileName;
                }

              


                _productService.Edit(entity);
                return RedirectToAction("GetProduct");
            }
            return View(product);


        }

        public IActionResult ProductDetails(int id)
        {
            var entity= _productService.GetById(id);

            var ctg=_categoryService.GetAll().Where(i=>i.CategoryId==entity.CategoryId).FirstOrDefault();
            ViewBag.ctg=ctg.CategoryName;

            return View(entity);
        }

        public IActionResult AdminOrders()
        {
            using (var context=new Context())
            {
              var orders= context.Orders.Include(i => i.OrderItems).ThenInclude(i => i.Product).Where(i=>i.StateEnumOrder!=OrderStateEnum.waitig).ToList().OrderByDescending(a=>a.OrderDate);
                var orderListModel = new List<OrderListModel>();
                OrderListModel orderlist;
                foreach (var order in orders)
                {
                    orderlist = new OrderListModel();
                    orderlist.OrderNumber = order.OrderNumber;
                    orderlist.OrderDate = order.OrderDate;
                    orderlist.StateEnumOrder = order.StateEnumOrder;
                    orderlist.OrderId = order.Id;
                    orderlist.FirstName = order.FirstName;
                    orderlist.LastName = order.Lastname;

                    orderlist.OrderItems = order.OrderItems.Select(i => new OrderItemModel()
                    {
                        ImageUrl = i.Product.ProductImageUrl,
                        Name = i.Product.ProductName,
                        OrderItemId = i.OrderItemId,
                        Price = i.ProductPrice,
                        Quantity = i.Quantity,
                        Ozellik = i.Product.ProductOzellik

                    }).ToList();

                    orderListModel.Add(orderlist);
                }
                return View(orderListModel);
            }        
        }

        public IActionResult WaitingOrder()
        {
            using (var context = new Context())
            {
                var orders = context.Orders.Include(i => i.OrderItems).ThenInclude(i => i.Product).Where(i => i.StateEnumOrder == OrderStateEnum.waitig).ToList().OrderByDescending(a => a.OrderDate);
                var orderListModel = new List<OrderListModel>();
                OrderListModel orderlist;
                foreach (var order in orders)
                {
                    orderlist = new OrderListModel();
                    orderlist.OrderNumber = order.OrderNumber;
                    orderlist.OrderDate = order.OrderDate;
                    orderlist.StateEnumOrder = order.StateEnumOrder;
                    orderlist.OrderId = order.Id;
                    orderlist.FirstName = order.FirstName;
                    orderlist.LastName = order.Lastname;

                    orderlist.OrderItems = order.OrderItems.Select(i => new OrderItemModel()
                    {
                        ImageUrl = i.Product.ProductImageUrl,
                        Name = i.Product.ProductName,
                        OrderItemId = i.OrderItemId,
                        Price = i.ProductPrice,
                        Quantity = i.Quantity,
                        Ozellik = i.Product.ProductOzellik

                    }).ToList();

                    orderListModel.Add(orderlist);
                }
                return View(orderListModel);
            }
        }
        public IActionResult OrderStatus(int id,OrderStateEnum status)
        {
            using (var context=new Context())
            {
                var order = context.Orders.Where(i => i.Id == id).FirstOrDefault();
                if(order != null)
                {
                    order.StateEnumOrder = status;
                    context.Orders.Update(order);
                    context.SaveChanges();
                    return RedirectToAction("AdminOrders");
                }
                return RedirectToAction("Index");
            }

            
        }
    }
}
