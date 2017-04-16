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
        // 首頁
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 查詢訂單
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Result(int orderid)
        {
            var result=orderservice.GetOrderID(orderid);
            @TempData["Order"] = result;
            return View(result);
        }
        /// <summary>
        /// 新增訂單頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult InsertOrder() {
            ViewBag.CustomerID = orderservice.GetCustomerID();
            ViewBag.EmployeeID = orderservice.GetEmployeeID();
            ViewBag.ShipperID = orderservice.GetShipperID();
            return View();
        }
        /// <summary>
        /// 新增結果
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InsertOrderResult(eSaleModel.Order order) {
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
        /// <summary>
        /// 刪除訂單
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
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
        [HttpPost]
        public ActionResult ModifyOrder(int orderid) {
            var result = orderservice.GetOrderID(orderid);
            ViewBag.CustomerID = orderservice.GetCustomerID();
            ViewBag.EmployeeID = orderservice.GetEmployeeID();
            ViewBag.ShipperID = orderservice.GetShipperID();
            return View(result);
        }
        public ActionResult ModifyOrderResult(eSaleModel.Order order) {
            if (orderservice.ModifyOrder(order))
            {
                Response.Write("<script>alert('修改成功');</script>");
            }
            else
            {
                Response.Write("<script>alert('修改失敗');</script>");
            }
            return View("Index");
        }
    }
}