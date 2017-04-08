using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP.NET.Controllers
{
    public class OrderController : Controller
    {
        eSaleService.OrderService orderservice = new eSaleService.OrderService();
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Result(int orderid)
        {
            var result=orderservice.GetOrderID(orderid);
            @TempData["Order"] = result;
            return View(result);
        }
        public ActionResult InsertOrder() {
            ViewBag.CustomerID = orderservice.GetCustomerID();
            ViewBag.EmployeeID = orderservice.GetEmployeeID();
            ViewBag.ShipperID = orderservice.GetShipperID();
            return View();
        }
        [HttpPost]
        public ActionResult InsertOrder(eSaleModel.Order order) {
            ViewBag.CustomerID = orderservice.GetCustomerID();
            ViewBag.EmployeeID = orderservice.GetEmployeeID();
            ViewBag.ShipperID = orderservice.GetShipperID();
            if (orderservice.InsertOrder(order))
            {
                Response.Write("<script>alert('新增成功');</script>");
            }
            else {
                Response.Write("<script>alert('新增失敗');</script>");
            }
            return View("Index");
        }
        [HttpPost]
        public ActionResult DeleteOrder(string orderid) {
            if (orderservice.DeleteOrder(Convert.ToInt32(orderid)))
            {
                Response.Write("<script>alert('刪除成功');</script>");
            }
            else
            {
                Response.Write("<script>alert('刪除失敗');</script>");
            }
            return View("Index");
        }
    }
}