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
            return View(db.Orders);
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
                return RedirectToAction("Index");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(product);
            }
        }
    }
}