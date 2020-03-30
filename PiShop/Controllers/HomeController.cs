using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PiShop.Models;

namespace PiShop.Controllers
{
    public class HomeController : Controller
    {

        ProductContext db;
        public HomeController(ProductContext context)
        {

            db = context;
        }

        public IActionResult Index(bool successOrder=false)
        {
            List<Product> Hits = db.GetFavoutiteProducts();
            IndexViewModal indexViewModal = new IndexViewModal()
            {
                SimpleTop = db.Products.Where(x=>x.Category=="Обычный топ").Take(6),
                HitsProducts = Hits,
                ShortTop = db.Products.Where(x => x.Category == "Короткий топ").Take(6),
                Phlis = db.Products.Where(x => x.Category == "Флис").Take(3),
                Pants = db.Products.Where(x => x.Category == "Брюки").Take(3),
                Complex = db.Products.Where(x => x.Category == "Комплекты").Take(6),
                SuccessOrder = successOrder
            };
            return View(indexViewModal);
        }

        public ActionResult CatalogMore(string Category)
        {
            IQueryable Products;
            if (Category == "брюки" || Category=="флис")
                Products = db.Products.Where(x => x.Category == Category).Skip(3);
            else
                Products = db.Products.Where(x => x.Category == Category).Skip(6);
            return View(Products);
        }

        public ActionResult Details(int id, string returnUrl = null)
        {
            Product product = db.Products.FirstOrDefault(x => x.Id == id);

            return PartialView(product);
        }


        /// <summary>
        /// Дублирование страницы и вызов Успешной оплаты
        /// </summary>
        /// <returns></returns>
        public ActionResult SuccessOrder()
        {
            return View();
        }

        public ActionResult SuccessDetail()
        {
            return PartialView();
        }

        public PartialViewResult popupFade(int Id)
        {
            Product product = db.Products.FirstOrDefault(x => x.Id == Id);
            return PartialView(product);
        }

        public PartialViewResult ModalText()
        {
            return PartialView();
        }

        public PartialViewResult GetPicturesSlick(int Id)
        {
            Product product = db.Products.FirstOrDefault(x => x.Id == Id);
            return PartialView(product);
        }
        

        public PartialViewResult popupBuy(int Id)
        {
            Product product = db.Products.FirstOrDefault(x => x.Id == Id);
            return PartialView(product);
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
