using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using PiShop.Models;

namespace PiShop.Controllers
{
    public class ViewController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        ProductContext db;
        public ViewController(ProductContext context)
        {
            db = context;
        }

        public ActionResult ViewProduct(int Id)
        {
            Product product = db.Products.FirstOrDefault(x => x.Id == Id);
            return View(product);
        }


    }
}