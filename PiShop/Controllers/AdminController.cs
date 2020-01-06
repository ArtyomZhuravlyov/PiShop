using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PiShop.Models;
using Domain;

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
    }
}