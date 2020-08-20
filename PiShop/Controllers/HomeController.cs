using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
            //List<Product> Hits = db.GetFavoutiteProducts();
            IndexViewModal indexViewModal = new IndexViewModal()
            {
                SimpleTop = db.Products.Where(x=>x.Category=="Обычный топ").Take(12),
                //HitsProducts = Hits,
                //ShortTop = db.Products.Where(x => x.Category == "Короткий топ").Take(6),
                Phlis = db.Products.Where(x => x.Category == "Флис").Take(3),
                Pants = db.Products.Where(x => x.Category == "Брюки").Take(3),
                Complex = db.Products.Where(x => x.Category == "Комплекты").Take(6),
                Alladins = db.Products.Where(x => x.Category == "алладины").Take(6),
                Gift = db.Products.Where(x => x.Category == "gift").Take(3),
                SuccessOrder = successOrder
            };
            return View(indexViewModal);
        }

        public ActionResult CatalogMore(string Category)
        {
            IQueryable Products;
            switch(Category)
            {
                case "брюки":
                case "флис" : Products = db.Products.Where(x => x.Category == Category).Skip(3); break;
                case "обычный топ": Products = db.Products.Where(x => x.Category == Category).Skip(12); break;
                case "gift": Products = db.Products.Where(x => x.Category == Category).Skip(3); break;
                default: Products = db.Products.Where(x => x.Category == Category).Skip(6); break;

            }

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

        public PartialViewResult ModalText(string text = null, string id = null)
        {
                return PartialView("ModalText", new ModalTextModel() { Text = text, Id = id });
        }

        [HttpPost]
        public void sendPhone(string Name, string Phone1)
        {
            
            var parameters = new NameValueCollection {
             { "token", "avf36ak95s7h5mc31u4chx9a7nzhk3" },
             { "user", "u9ki332gg4ei1jza8tms2vxpcnsuu6" },
             //{ "device", "redminote6pro" },
             { "message",      Phone1 +  " "+ Name },
             { "title", "Заявка на обратный звонок" },
             {"sound", "climb" },
             { "priority", "2" },
             { "retry", "30" },
             { "expire", "300" },


             };
            try
            {
                using (var client = new WebClient())
                {
                    client.UploadValues("https://api.pushover.net/1/messages.json", parameters);
                }

            }
            catch (Exception exp)
            {
            }

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
        

        public ActionResult ViewProduct(int Id)
        {
            Product product = db.Products.FirstOrDefault(x => x.Id == Id);
            return View(product);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
