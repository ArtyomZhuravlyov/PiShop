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

        public IActionResult Index()
        {
            List<Product> Hits = db.GetFavoutiteProducts();
            IndexViewModal indexViewModal = new IndexViewModal()
            {
                Products = db.Products,
                HitsProducts = Hits
            };
            return View(indexViewModal);
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
