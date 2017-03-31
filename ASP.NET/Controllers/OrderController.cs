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
        public ActionResult InsertOrder() {
            /*eSaleService.OrderService orderservice = new eSaleService.OrderService();
            @ViewBag.CustomerID= new SelectList(orderservice.GetCustomerID());*/
            return View();
        }
        [HttpPost]
        public ActionResult InsertOrder(eSaleModel.Order order) {
            eSaleService.OrderService orderservice = new eSaleService.OrderService();
            if (orderservice.InsertOrder(order))
            {
                Response.Write("<script>alert('新增成功');</script>");
            }
            else {
                Response.Write("<script>alert('新增失敗');</script>");
            }
            return View();
        }
    }
}