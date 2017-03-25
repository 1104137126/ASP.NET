using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP.NET.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Result(int orderid)
        {
            eSaleService.OrderService orderservice = new eSaleService.OrderService();
            var result=orderservice.GetOrderID(orderid);
            return View(result);
        }
    }
}