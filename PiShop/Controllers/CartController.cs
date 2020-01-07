using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PiShop.Models;
using Sber;

namespace PiShop.Controllers
{
    public class CartController : Controller
    {
        ProductContext db;

        public CartController(ProductContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        public int GetQuantity()
        {
            return GetCart().Lines.Sum(x => x.Quantity);
        }

        public RedirectToActionResult/*ActionResult*/ AddToCart(int id, int l = 50)
        {
            // Product product = (Product)db.Products.FirstOrDefault(x => x.Id == id);
            //if (id > 0)
            //{
            Product product = db.Products
             .FirstOrDefault(g => g.Id == id);

            Cart cart = GetCart();
            cart.AddItem(product, 1);
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
            //cart = GetCart();


            return RedirectToAction("Summary", "Cart");

        }


        /// <summary>
        /// Удаляет только одно кол-во выбранного товара (1 штуку)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        public RedirectToActionResult RemoveOneProductToCart(int id, int l = 50)
        {
            // Product product = (Product)db.Products.FirstOrDefault(x => x.Id == id);
            //if (id > 0)
            //{
            Product product = db.Products
             .FirstOrDefault(g => g.Id == id);

            Cart cart = GetCart();
            cart.RemoveItem(product, 1);
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
            cart = GetCart();

            return RedirectToAction("Summary", "Cart");
            //}
            //else //если отрицательное значение (-1), то нужно просто получить Корзину
            //{
            //    Cart cart = GetCart();
            //    return Redirect("/Home/AddToCart/-1");
            //}
        }

        /// <summary>
        /// Удаляет полностью товар из корзины (неважно какое количество этого товара было)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public RedirectToActionResult RemoveLine(int id, string returnUrl = "")
        {

            Product product = db.Products
                .FirstOrDefault(g => g.Id == id);

            Cart cart = GetCart();
            cart.RemoveLine(product);
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
            cart = GetCart();

            //  return Redirect($"/Cart/Summary/{returnUrl}/");
            return RedirectToAction("Summary", "Cart", new { returnUrl });
        }

        public RedirectToActionResult RemoveFromCart(int productId, string returnUrl)
        {
            Product product = db.Products
                .FirstOrDefault(g => g.Id == productId);

            if (product != null)
            {
                Cart cart = GetCart();
                cart.RemoveLine(product);
                HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public Cart GetCart()
        {
            Cart cart;
            if (HttpContext.Session.Keys.Contains("Cart"))
            {
                string value = HttpContext.Session.GetString("Cart");
                cart = JsonConvert.DeserializeObject<Cart>(value);
            }
            else
            {
                cart = new Cart();
                HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
                // Session["Cart"] = cart;
            }
            return cart;
        }


        public ActionResult ModalText(string text)
        {
            //modaltext?text=привет
            return PartialView(text);
        }


        public ViewResult Checkout()
        {
            ShippingDetails shippingDetails = new ShippingDetails();
            if (HttpContext.Session.Keys.Contains("shippingDetails"))
            {
                string value = HttpContext.Session.GetString("shippingDetails");
                shippingDetails = JsonConvert.DeserializeObject<ShippingDetails>(value);
            }
            return View(shippingDetails/*new ShippingDetails()*/);
        }

        /// <summary>
        /// Проверка введённых контактных данных
        /// </summary>
        /// <param name="shippingDetails"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Checkout(ShippingDetails shippingDetails)
        {
            if (ModelState.IsValid && shippingDetails.UserAccess)
            {

                string value = HttpContext.Session.GetString("Cart");
                Cart cart = JsonConvert.DeserializeObject<Cart>(value);
                try
                {
                    var order = CreateAndFillOrder(shippingDetails, cart); //добавляем в базу заказ
                    Sberbank sberbank = new Sberbank((order.Id).ToString() + "test", cart, shippingDetails);
                    string url = sberbank.GetResponseSoap();
                    return Redirect(url);
                }
                catch (Exception ex)
                {
                    //to do
                    return Redirect("");
                }
            }
            else
            {
                if (!shippingDetails.UserAccess)
                    ModelState.AddModelError("UserAccess", "Примите пользовательское соглашение");
                HttpContext.Session.SetString("shippingDetails", JsonConvert.SerializeObject(shippingDetails));
                return View(shippingDetails);
            }

        }


        public ViewResult/*PartialViewResult*/ Summary(string returnUrl = "")
        {

            string value = HttpContext.Session.GetString("Cart");
            if (!string.IsNullOrEmpty(value))
            {
                Cart cart = JsonConvert.DeserializeObject<Cart>(value);
                returnUrl = HttpContext.Session.GetString("ReturnUrl");
                 return View(new CartIndexViewModel { Cart = cart, ReturnUrl = returnUrl, });
                
            }
            else return View(null);
        }

        public Order CreateAndFillOrder(ShippingDetails shippingDetails, Cart cart)
        {
            Order order = new Order()
            {
                Received = false,
                Delivered = false,
                Shipped = false,
                Adress = shippingDetails.Adress,
                Name = shippingDetails.Name,
                Mail = shippingDetails.Mail,
                City = shippingDetails.City,
                MiddleName = shippingDetails.MiddleName,
                Surname = shippingDetails.Surname,
                TimeOrder = DateTime.Now.ToUniversalTime(),
                Phone = shippingDetails.Phone,
                Comment = shippingDetails.Comment,
                OrdersAndQuantity = cart.GetXmlLineCollection(),
                Amount = cart.ComputeTotalValueWithDelivery()
                //to do
            };
            db.Orders.Add(order);
            db.SaveChanges();
            return order;
        }
    }
}