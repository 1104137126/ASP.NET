using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            @TempData["Result"]= "";
            return View();
        }
        public ActionResult Result(eSaleModel.Order order) {
            ViewBag.EmployeeName = orderservice.GetEmployeeName();
            ViewBag.CompanyName = orderservice.GetShipCompanyName();
            IEnumerable<eSaleModel.Order> result = orderservice.GetOrder(order);
            var grid = new WebGrid(source:result,
                                   rowsPerPage: 20,
                                   ajaxUpdateContainerId: "result",
                                   canPage:true,
                                   canSort:true
                                   );
            @TempData["Result"] = grid.GetHtml(
                               columns: new List<WebGridColumn> {
                                    new WebGridColumn() { ColumnName = "OrderID", Header = "訂單編號" ,CanSort=true},
                                    new WebGridColumn() { ColumnName = "CompanyName", Header = "客戶名稱",CanSort=true },
                                    new WebGridColumn() { ColumnName = "OrderDate", Header = "訂購日期" ,CanSort=true,
                                        Format = item =>string.Format("{0:yyyy/MM/dd}",item.OrderDate)
                                    },
                                    new WebGridColumn() { ColumnName = "RequiredDate", Header = "發貨日期",CanSort=true,
                                        Format = item =>string.Format("{0:yyyy/MM/dd}",item.RequiredDate)
                                    },
                                    new WebGridColumn() {
                                    ColumnName = "Edit",
                                    Header = "編輯",
                                    Format = item => "編輯"
                                    },
                                    new WebGridColumn() {
                                    ColumnName = "Delete",
                                    Header = "刪除",
                                    Format = item=> "刪除"
                                    }
                                }, htmlAttributes: new { border="1" ,cellspacing="0",cellpadding="4"}
                           );
            return View("Index");
        }
        /// <summary>
        /// 新增訂單頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult InsertOrder() {
            ViewBag.CustomerID = orderservice.GetCustomerID();
            ViewBag.EmployeeName = orderservice.GetEmployeeName();
            ViewBag.ShipperID = orderservice.GetShipCompanyName();
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
            ViewBag.EmployeeID = orderservice.GetEmployeeName();
            ViewBag.ShipperID = orderservice.GetShipCompanyName();
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
        public ActionResult ModifyOrder(eSaleModel.Order order) {
            var result = orderservice.GetOrder(order);
            ViewBag.CustomerID = orderservice.GetCustomerID();
            ViewBag.EmployeeID = orderservice.GetEmployeeName();
            ViewBag.ShipperID = orderservice.GetShipCompanyName();
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