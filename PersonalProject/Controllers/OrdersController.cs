//using ECommerce.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using Stripe;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace ECommerce.Controllers
//{
//    public class OrdersController : Controller
//    {
//        private readonly IConfiguration _configuration;

//        public OrdersController(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }

//        [HttpGet]
//        public IActionResult OrderSummary()
//        {
//            // Replace with logic to retrieve the user's cart and shipping details
//            var cart = GetCartForCurrentUser();
//            var orderSummaryModel = new OrderSummaryViewModel
//            {
//                Cart = cart
//            };

//            return View(orderSummaryModel);
//        }

//        [HttpPost]
//        public IActionResult PlaceOrder(OrderSummaryViewModel orderSummaryModel)
//        {
//            // Handle placing the order and payment processing using Stripe
//            // You can use the provided orderSummaryModel to get the user's shipping details and cart

//            // Set up Stripe
//            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];

//            var options = new PaymentIntentCreateOptions
//            {
//                Amount = (long)(orderSummaryModel.Cart.TotalPrice * 100), // amount in cents
//                Currency = "usd",
//                PaymentMethod = "pm_card_visa",
//                ConfirmationMethod = "manual",
//                Confirm = true,
//            };

//            var service = new PaymentIntentService();
//            var paymentIntent = service.Create(options);

//            ViewData["ClientSecret"] = paymentIntent.ClientSecret;

//            return View("PaymentConfirmation");
//        }

//        private Cart GetCartForCurrentUser()
//        {
//            // Replace with logic to retrieve the user's cart based on their authentication
//            // For simplicity, let's assume a hardcoded cart
//            var productsCart = new List<ProductsCart>
//            {
//                new ProductsCart { product = new Models.Product { ProductName = "Product A", Price = 10.99M }, Quantity = 2 },
//                new ProductsCart { product = new Models.Product { ProductName = "Product B", Price = 19.99M }, Quantity = 1 }
//            };

//            return new Cart
//            {
//                TotalPrice = productsCart.Sum(item => item.Quantity * item.product.Price),
//                productsCarts = productsCart
//            };
//        }
//    }
//}
