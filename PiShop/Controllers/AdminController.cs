using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PiShop.Models;
using Domain;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace PiShop.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        ProductContext db;

        public AdminController(ProductContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View(db.Orders.ToList());
        }

        public IActionResult IndexProduct()
        {
            return View(db.Products);
        }

        public ActionResult OrderView(int Id)
        {
            Order Order = db.Orders.Where(x => x.Id == Id).FirstOrDefault();
            OrderEditViewModal orderEditViewModal = new OrderEditViewModal()
            {
                order = Order,
                XmlCartLineList = Cart.GetLineCollecionFromXML(Order.OrdersAndQuantity)
            };
            return View(orderEditViewModal);
        }

        public ViewResult Create()
        {
            return View("Edit", new Product());
        }

        [HttpPost]
        public ActionResult Delete(int productId)
        {
            Product deletedProduct = db.DeleteProduct(productId);
            if (deletedProduct != null)
            {
                TempData["message"] = string.Format("Продукт \"{0}\" был удалён",
                    deletedProduct.Name);
            }
            return RedirectToAction("IndexProduct");
        }

        public ViewResult Edit(int id)
        {
            Product product = db.Products
                .FirstOrDefault(g => g.Id == id);
            return View(product);
        }


        /// <summary>
        /// Перегруженная версия Edit() для сохранения изменений
        /// </summary>
        /// <param name="product">Продукт который отредактировали</param>
        /// <param name="Image"></param>
        /// <param name="action">Определяет нужно ли перейти к редактированию следующего продукта</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Product product, IFormFile Image, string action)
        {
            if (ModelState.IsValid)
            {
                if (Image != null)
                {
                    using (var binaryReader = new BinaryReader(Image.OpenReadStream()))
                    {
                        product.ImageData = binaryReader.ReadBytes((int)Image.Length);
                        product.ImageMimeType = Image.ContentType;
                    }
                }
                db.SaveProduct(product);
                TempData["message"] = string.Format("Изменения \"{0}\" были сохранены", product.Name);
                if (action == "SaveAndNextProduct")
                    return RedirectToAction("Edit", new { id = db.FindNextId(product.Id) });
                return RedirectToAction("IndexProduct");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(product);
            }
        }

        public IActionResult PriceAdmin()
        {
            //return View(db.Products);
            return View(db.Products.ToList());
        }

        [HttpPost]
        public/* PartialViewResult*/IActionResult PriceAdmin(List<Product> products) /*Так и не удалось передать product2*/
        {
            foreach (var product in products)
            {
                Product productNew = db.Products.Where(x => x.Id == product.Id).FirstOrDefault();
                productNew.Price = product.Price;
                // db.SaveProduct(productNew);
            }
            db.SaveChanges();
            //Product product = db.Products.Where(x => x.Id == id).FirstOrDefault();
            //product.Price = Price;
            //product.Weight = Weight;
            //db.SaveProduct(product);
            TempData["message"] = string.Format("Изменения  были сохранены");
            return View(db.Products.ToList());
            // return PartialView("Message", $" {product.Name} Изменён");

        }


        [HttpPost]
        public IActionResult ChangeCategoryPrice(string category,int generalPrice)
        {
            var products = db.Products.Where(x => x.Category == category);

            foreach (var product in products)
            {
                product.Price = generalPrice;
            }
            db.SaveChanges();
            TempData["message"] = string.Format("Изменения  были сохранены");
            return Redirect("PriceAdmin");
        }

        [HttpPost]
        public IActionResult ChangeGeneralPrice(int generalPrice)
        {
            foreach (var product in db.Products)
            {
                product.Price = generalPrice;
            }
            db.SaveChanges();
            TempData["message"] = string.Format("Изменения  были сохранены");
            return Redirect("PriceAdmin"); 
        }
    }
}