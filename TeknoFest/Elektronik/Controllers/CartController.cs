using BuissnessLayer.Abstract;
using ElektronikWebUI.Identity;
using ElektronikWebUI.Migrations;
using ElektronikWebUI.Models;
using EntityLayer;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElektronikWebUI.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private ICartService _cartService;
        private UserManager<Customer> _userManager;
        private IOrderService _orderService;
        public CartController(ICartService cartService, UserManager<Customer> userManager, IOrderService orderService)
        {
            _cartService= cartService;
            _userManager= userManager;  
            _orderService= orderService;
        }


        public IActionResult Index()
        {
            var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));     
            return View(new CartModel()
            {
                CartId= cart.Id,              
                CartItems=cart.CartItems.Select(i=>new CartItemModel()
                {
                    CartItemId=i.Id,
                    ImageUrl=i.Product.ProductImageUrl,
                    Name=i.Product.ProductName,
                    Quantity=i.Quantity,
                    Price=(double)i.Product.ProductPrice,
                    ProductId=i.ProductId,
                    Color=i.Product.ProductOzellik

                }).ToList()
            });
        }

        [HttpPost]
        public IActionResult AddToCart(int productId,int quantity)
        {
            _cartService.AddToCart(_userManager.GetUserId(User), productId, quantity);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteCart(int productId)
        {
            var userId = _userManager.GetUserId(User);
            _cartService.DeleteFromCart(userId, productId);

            return RedirectToAction("Index");
        }
        public IActionResult Checkout()
        {
            var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));

            var orderModel = new OrderModel();

            orderModel.CartModel = new CartModel()
            {
                CartId = cart.Id,
                CartItems = cart.CartItems.Select(i => new CartItemModel()
                {
                    CartItemId = i.Id,
                    
                    ProductId = i.ProductId,
                    Name = i.Product.ProductName,
                    Price = (double)i.Product.ProductPrice,
                    ImageUrl = i.Product.ProductImageUrl,
                    Quantity = i.Quantity

                }).ToList()
            };
            orderModel.Shipping = 20;
            return View(orderModel);
        }

        [HttpPost]
        public IActionResult Checkout(OrderModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var cart = _cartService.GetCartByUserId(userId);

                model.CartModel = new CartModel()
                {
                    CartId = cart.Id,
                    CartItems = cart.CartItems.Select(i => new CartItemModel()
                    {
                        CartItemId = i.Id,
                        ProductId = i.ProductId,
                        Name = i.Product.ProductName,
                        Price = (double)i.Product.ProductPrice,
                        ImageUrl = i.Product.ProductImageUrl,
                        Quantity = i.Quantity
                    }).ToList()
                };

                var payment = PaymentProcess(model);

                if (payment.Status == "success")
                {
                    SaveOrder(model, payment, userId);
                    ClearCart(userId);
                    return View("Success");
                }
                else
                {
                    return RedirectToAction("index");
                }
            }
            return View(model);
        }


        private void ClearCart(string userId)
        {
            _orderService.ClearCart(userId);
        }

        private void SaveOrder(OrderModel model, Payment payment, string userId)
        {
            var order = new Order();

            order.OrderNumber = new Random().Next(111111, 999999).ToString();
            order.StateEnumOrder = OrderStateEnum.waitig;          
            order.PaymentId = payment.PaymentId;
            order.ConverstionId = payment.ConversationId;
            order.OrderDate = DateTime.Now;           
            order.UserId = userId;
            order.FirstName = model.FirstName;
            order.Lastname= model.LastName;

            order.OrderItems = new List<EntityLayer.OrderItem>();

            foreach (var item in model.CartModel.CartItems)
            {
                var orderItem = new EntityLayer.OrderItem()
                {
                    ProductPrice = item.Price,
                    Quantity = item.Quantity,
                    ProductId = item.ProductId
                };
                order.OrderItems.Add(orderItem);
            }
            _orderService.Create(order);
        }

        private Payment PaymentProcess(OrderModel model)
        {
            Options options = new Options();
            options.ApiKey = "sandbox-JCaQB0rkV1U6vJz0maqIFf8Wystgk9Px";
            options.SecretKey = "R7cHBtPDjQn9WClT87IgfZdp1CqId75k";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";

            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = new Random().Next(111111111, 999999999).ToString();
            request.Price = model.CartModel.TotalPrice().ToString();
            request.PaidPrice = model.CartModel.TotalPrice().ToString();
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;
            request.BasketId = "B67832";
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = model.CardName;
            paymentCard.CardNumber = model.CardNumber;
            paymentCard.ExpireMonth = model.ExpairationMonth;
            paymentCard.ExpireYear = model.ExpairationYear;
            paymentCard.Cvc = model.Cvc;
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            //paymentCard.CardNumber = "5528790000000008";
            //paymentCard.ExpireMonth = "12";
            //paymentCard.ExpireYear = "2030";
            //paymentCard.Cvc = "123";

            Buyer buyer = new Buyer();
            buyer.Id = "BY789";
            buyer.Name = model.FirstName;
            buyer.Surname = model.LastName;
            buyer.GsmNumber = "+905350000000";
            buyer.Email = "email@email.com";
            buyer.IdentityNumber = "74300864791";
            buyer.LastLoginDate = "2015-10-05 12:43:35";
            buyer.RegistrationDate = "2013-04-21 15:12:09";
            buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            buyer.Ip = "85.34.78.112";
            buyer.City = "Istanbul";
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = "Jane Doe";
            shippingAddress.City = "Istanbul";
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = "Jane Doe";
            billingAddress.City = "Istanbul";
            billingAddress.Country = "Turkey";
            billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();
            BasketItem basketItem;

            foreach (var item in model.CartModel.CartItems)
            {
                basketItem = new BasketItem();
                basketItem.Id = item.ProductId.ToString();
                basketItem.Name = item.Name;
                basketItem.Category1 = "Telefon";
                basketItem.Price = item.Price.ToString();             
                basketItem.ItemType = BasketItemType.PHYSICAL.ToString();
                basketItems.Add(basketItem);              
            }
            request.BasketItems = basketItems;
            return Payment.Create(request, options);


        }

        public IActionResult GetOrders()
        {
          var userId= _userManager.GetUserId(User);

            var orders=_orderService.GetOrders(userId);

            var orderListModel = new List<OrderListModel>();
            OrderListModel orderlist;
            foreach (var order in orders)
            {
                orderlist= new OrderListModel();
                orderlist.OrderNumber = order.OrderNumber;
                orderlist.OrderDate = order.OrderDate;
                orderlist.StateEnumOrder = order.StateEnumOrder;
                orderlist.OrderId = order.Id;
                orderlist.FirstName = order.FirstName;
                orderlist.LastName = order.Lastname;

                orderlist.OrderItems = order.OrderItems.Select(i => new OrderItemModel()
                {
                    ImageUrl=i.Product.ProductImageUrl,
                    Name=i.Product.ProductName,
                    OrderItemId=i.OrderItemId,
                    Price=i.ProductPrice,
                    Quantity=i.Quantity,
                    Ozellik=i.Product.ProductOzellik

                }).ToList();

                orderListModel.Add(orderlist);
            }
            return View(orderListModel);
        }
    }
}
