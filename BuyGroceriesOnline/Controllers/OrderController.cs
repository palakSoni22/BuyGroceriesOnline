﻿using BuyGroceriesOnline.Models;
using Microsoft.AspNetCore.Mvc;

namespace BuyGroceriesOnline.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ShoppingCart _shoppingCart;
        private readonly IFeedbackRepository _FeedbackRepository;

        public OrderController(IOrderRepository orderRepository, ShoppingCart shoppingCart, IFeedbackRepository FeedbackRepository)
        {
            _orderRepository = orderRepository;
            _shoppingCart = shoppingCart;
            this._FeedbackRepository = FeedbackRepository;
        }

        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            if (_shoppingCart.ShoppingCartItems.Count == 0)
            {
                ModelState.AddModelError("", "Your cart is empty, add some products first");
            }

            if (ModelState.IsValid)
            {
                _orderRepository.CreateOrder(order);
                _shoppingCart.ClearCart();
                return RedirectToAction("CheckoutComplete");
            }
            return View(order);
        }

        public IActionResult CheckoutComplete()
        {
            ViewBag.CheckoutCompleteMessage = "Thank You for your order. Hope your experience was Good!";
            return View();
        }
        
        

        public ViewResult FeedBack()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateFeedBack(Feedback feed)
        {           
            _FeedbackRepository.InsertFeedback(feed);
            return RedirectToAction("Index", "Home", new { area = "" });            
        }

        public ViewResult AllFeedback()
        {
            var allFeedback =  _FeedbackRepository.GetAllFeedback();
            return View(allFeedback);
        }
    }
}
