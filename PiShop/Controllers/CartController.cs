using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
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

        public void/*ActionResult*/ AddToCart(int id , string size )
        {
            // Product product = (Product)db.Products.FirstOrDefault(x => x.Id == id);
            //if (id > 0)
            //{
            Product product = db.Products
             .FirstOrDefault(g => g.Id == id);
            product.Size = size;
            Cart cart = GetCart();
            cart.AddItem(product, 1);
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
            //cart = GetCart();


            //return RedirectToAction("Summary", "Cart");

        }

        //[HttpPost]
        //public void/*ActionResult*/ AddToCart(int id = 1, string size = "sl")
        //{
        //    // Product product = (Product)db.Products.FirstOrDefault(x => x.Id == id);
        //    //if (id > 0)
        //    //{
        //    Product product = db.Products
        //     .FirstOrDefault(g => g.Id == id);
        //    product.Size = size;
        //    Cart cart = GetCart();
        //    cart.AddItem(product, 1);
        //    HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
        //    //cart = GetCart();


        //}


        /// <summary>
        /// Удаляет только одно кол-во выбранного товара (1 штуку)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        public void RemoveOneProductToCart(int id, string size, int l = 50)
        {
            // Product product = (Product)db.Products.FirstOrDefault(x => x.Id == id);
            //if (id > 0)
            //{
            Product product = db.Products
             .FirstOrDefault(g => g.Id == id);
            product.Size = size;
            Cart cart = GetCart();
            cart.RemoveItem(product, 1);
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
            cart = GetCart();
        }

        /// <summary>
        /// Удаляет полностью товар из корзины (неважно какое количество этого товара было)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public void/*RedirectToActionResult*/ RemoveLine(int id,  string size)
        {

            Product product = db.Products
                .FirstOrDefault(g => g.Id == id);
            product.Size = size;
            Cart cart = GetCart();
            cart.RemoveLine(product);
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
            cart = GetCart();

            //  return Redirect($"/Cart/Summary/{returnUrl}/");
           // return RedirectToAction("Summary", "Cart");
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
            //if (!IsValidEmail(shippingDetails.Mail))
            //    ModelState.AddModelError("Mail", "почта указана некорректно ");

            //if (CheckPhone(shippingDetails.Phone, out string Phone))
            //    shippingDetails.Phone = Phone;

            if (ModelState.IsValid && shippingDetails.UserAccess /*&& CheckPhone(shippingDetails.Phone)*/)
            {

                string value = HttpContext.Session.GetString("Cart");
                Cart cart = JsonConvert.DeserializeObject<Cart>(value);
                    var order = CreateAndFillOrder(shippingDetails, cart); //добавляем в базу заказ
                    //Sberbank sberbank = new Sberbank((order.Id).ToString() + "test", cart, shippingDetails);
                    //string url = sberbank.GetResponseSoap();
                    //if (!string.IsNullOrEmpty(url))
                    //    return Redirect(url);
                    //else
                    //{
                    //    ModelState.AddModelError("Mail", "Проверьте правильность введённых данных");
                    //    return View(shippingDetails);
                    //}
                     return RedirectToAction("Index", "Home", new { successOrder = true });

            }
            else
            {
                if (!shippingDetails.UserAccess)
                    ModelState.AddModelError("UserAccess", "Примите пользовательское соглашение");
                HttpContext.Session.SetString("shippingDetails", JsonConvert.SerializeObject(shippingDetails));
                return View(shippingDetails);
            }

        }

        private bool CheckPhone(string Phone, out string phone)
        {
            phone = Phone;
            //if(Phone.Length!=11 || !long.TryParse(Phone, out long empty))
            if (!Regex.IsMatch(phone, @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$"))
            {
                ModelState.AddModelError("Phone", "Неверный формат номера телефона");
                return false;
            }
            phone = phone.Replace(" ", "").Replace("(", "").Replace(")", "");
            return true;

        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public PartialViewResult/*PartialViewResult*/ Cart()
        {

            string value = HttpContext.Session.GetString("Cart");
            if (!string.IsNullOrEmpty(value))
            {
                Cart cart = JsonConvert.DeserializeObject<Cart>(value);
                if(cart?.Lines?.Count()>0)
                    return PartialView(new CartIndexViewModel { Cart = cart });
                else return PartialView(null);
            }
            else return PartialView(null);
        }

        public PartialViewResult Summary()
        {

            string value = HttpContext.Session.GetString("Cart");
            if (!string.IsNullOrEmpty(value))
            {
                Cart cart = JsonConvert.DeserializeObject<Cart>(value);
                return PartialView(new CartIndexViewModel { Cart = cart });

            }
            else return PartialView(null);
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
                Amount = cart.ComputeTotalValueWithDelivery(),
                TypePay = "При получении"
                //to do
            };
            db.Orders.Add(order);
            db.SaveChanges();
            return order;
        }
    }
}