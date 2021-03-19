using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shopapp.Bussiness.Abstract;
using Shopapp.Entities;
using Shopapp.WebUI.Identity;
using Shopapp.WebUI.Models;

namespace Shopapp.WebUI.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private ICartService _cartservice;
        private UserManager<ApplicationUser> _usermanager;
        private IOrderService _orderService;

        public CartController(IOrderService orderService,UserManager<ApplicationUser> usermanager, ICartService cartservice)
        {
            _cartservice = cartservice;
            _usermanager = usermanager;
            _orderService = orderService;
        }
        public IActionResult Index()
        {
            var cart = _cartservice.GetCartByUserId(_usermanager.GetUserId(User));
            if (cart!=null)
            {
                return View(new CartModel()
                {
                    CartID = cart.ID,
                    CartItems = cart.CartItems.Select(i => new CartItemModel()
                    {
                        CartItemID = i.ID,
                        Name = i.Product.Name,
                        Price = (decimal)i.Product.Price,
                        ImageUrl = i.Product.ImageURL,
                        ProductID = i.Product.ID,
                        Quantity = i.Quantity
                    }).ToList()
                });
            }
            else
            {
                return View();
            }
            
        }
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            _cartservice.AddToCart(_usermanager.GetUserId(User), productId, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteFromCart(int productId)
        {
            _cartservice.DeleteFromCart(_usermanager.GetUserId(User), productId);
            return RedirectToAction("Index");
        }

        public IActionResult Checkout()
        {
            var cart = _cartservice.GetCartByUserId(_usermanager.GetUserId(User));
            OrderModel orderModel = new OrderModel
            {
                CartModel = new CartModel()
                {
                    CartID = cart.ID,
                    CartItems = cart.CartItems.Select(i => new CartItemModel()
                    {
                        CartItemID = i.ID,
                        Name = i.Product.Name,
                        Price = (decimal)i.Product.Price,
                        ImageUrl = i.Product.ImageURL,
                        ProductID = i.Product.ID,
                        Quantity = i.Quantity
                    }).ToList()
                }
            };
            return View();
        }
        [HttpPost]
        public IActionResult Checkout(OrderModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _usermanager.GetUserId(User);
                var cart = _cartservice.GetCartByUserId(userId);

                model.CartModel = new CartModel()
                {
                    CartID = cart.ID,
                    CartItems = cart.CartItems.Select(i => new CartItemModel()
                    {
                        CartItemID = i.ID,
                        Name = i.Product.Name,
                        Price = (decimal)i.Product.Price,
                        ImageUrl = i.Product.ImageURL,
                        ProductID = i.Product.ID,
                        Quantity = i.Quantity

                    }).ToList()
                };

                SaveOrder(model, userId);
                ClearCart(cart.ID.ToString());

                return View();
            }


            else
            {
                return View(model);
            }
        }

        private void SaveOrder(OrderModel model, string userId)
        {
            var order = new Order();
            order.OrderState = EnumOrderState.Completed;
            order.PaymentTypes = EnumPaymentTypes.CreditCard;
            order.PaymentId = new Random().Next(0, 999999).ToString();
            order.ConversationId = new Random().Next(0, 999999).ToString();
            order.OrderDate = new DateTime();
            order.FirstName = model.FirstName;
            order.LastName = model.LastName;
            order.Email = model.Email;
            order.Phone = model.Phone;
            order.Address = model.Address;
            order.UserId = userId;

            foreach (var item in model.CartModel.CartItems)
            {
                var orderitem = new OrderItems()
                {
                    Price = item.Price,
                    Quantity = item.Quantity,
                    ProductId = item.ProductID
                };
                order.OrderItems.Add(orderitem);
            }
            _orderService.Create(order);
        }

         private void ClearCart(string cartID)
         {
            _cartservice.ClearCart(cartID);
         }
        public IActionResult GetOrders()
        {
            var orders =_orderService.GetOrders(_usermanager.GetUserId(User));
            var orderListModel = new List<OrderListModel>();
            OrderListModel ordermodel;

            foreach (var order in orders)
            {
                ordermodel = new OrderListModel();
                ordermodel.Id = order.Id;
                ordermodel.OrderDate = order.OrderDate;
                ordermodel.OrderNote = order.OrderNote;
                ordermodel.Phone = order.Phone;
                ordermodel.FirstName = order.FirstName;
                ordermodel.LastName = order.LastName;
                ordermodel.Email = order.Email;
                ordermodel.Address = order.Address;
                ordermodel.City = order.City;
                ordermodel.OrderItems = order.OrderItems.Select(i => new OrderItemModel()
                {
                    OrderItemId = i.Id,
                    Name = i.Product.Name,
                    Price = i.Price,
                    Quantity = i.Quantity,
                    ImageUrl = i.Product.ImageURL
                }).ToList();

                orderListModel.Add(ordermodel);

            }
            return View(orderListModel);
        }
    }
}
       
        
        
   