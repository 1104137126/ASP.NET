using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Helpers;

namespace ASP.NET.Controllers
{
    public class OrderController : Controller
    {
        eSaleService.OrderService orderservice = new eSaleService.OrderService();
        // 首頁
        public ActionResult Index()
        {
            ViewBag.EmployeeName = orderservice.GetEmployeeName();
            ViewBag.CompanyName = orderservice.GetShipCompanyName();
            @TempData["Result"]= new List<eSaleModel.Order>();
            return View();
        }
        public ActionResult Result(eSaleModel.Order order) {
            ViewBag.EmployeeName = orderservice.GetEmployeeName();
            ViewBag.CompanyName = orderservice.GetShipCompanyName();
            @TempData["Result"] = orderservice.GetOrder(order);
            return View("Index");
        }

        /// <summary>
        /// 新增訂單頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult InsertOrder() {
            ViewBag.CustomerID = orderservice.GetCustomerID();
            ViewBag.EmployeeID = orderservice.GetEmployeeID();
            ViewBag.ShipperID = orderservice.GetShipperID();
            ViewBag.CompanyName = orderservice.GetShipCompanyName();
            ViewBag.ProductName = orderservice.GetProductName();
            return View();
        }
        /// <summary>
        /// 新增結果
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InsertOrderResult(eSaleModel.Order order) {
            ViewBag.EmployeeName = orderservice.GetEmployeeName();
            ViewBag.CompanyName = orderservice.GetShipCompanyName();
            @TempData["Result"] = new List<eSaleModel.Order>();
            int id=(orderservice.InsertOrder(order));
            orderservice.InsertOrderDetails(id, order);
            return Redirect("Index");
        }
        /// <summary>
        /// 刪除訂單
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public ActionResult DeleteOrder(eSaleModel.Order order) {
            ViewBag.EmployeeName = orderservice.GetEmployeeName();
            ViewBag.CompanyName = orderservice.GetShipCompanyName();
            @TempData["Result"] = new List<eSaleModel.Order>();
            if (orderservice.DeleteOrderDetails(Convert.ToInt32(order.OrderID)) &orderservice.DeleteOrder(Convert.ToInt32(order.OrderID)))
            {
                Response.Write("<script>alert('刪除成功');</script>");
            }
            else
            {
                Response.Write("<script>alert('刪除失敗');</script>");
            }
            return Redirect("Index");
        }
        public ActionResult ModifyOrder(eSaleModel.Order order) {
            var result = orderservice.GetOrderResult(order);
            ViewBag.CustomerID = orderservice.GetCustomerID();
            ViewBag.EmployeeID = orderservice.GetEmployeeID();
            ViewBag.ShipperID = orderservice.GetShipperID();
            ViewBag.ProductName = orderservice.GetProductName();
            ViewBag.ProductList = orderservice.ModifyOrderProduct(order.OrderID);
            return View(result);
        }
        public ActionResult ModifyOrderResult(eSaleModel.Order order) {
            ViewBag.EmployeeName = orderservice.GetEmployeeName();
            ViewBag.CompanyName = orderservice.GetShipCompanyName();
            @TempData["Result"] = new List<eSaleModel.Order>();
            bool check= orderservice.DeleteOrderDetails(Convert.ToInt32(order.OrderID));
            orderservice.InsertOrderDetails(order.OrderID, order);
            if (orderservice.ModifyOrder(order) && check)
            {
                Response.Write("<script>alert('修改成功');</script>");
            }
            else
            {
                Response.Write("<script>alert('修改失敗');</script>");
            }
            return Redirect("Index");
        }
    }
}