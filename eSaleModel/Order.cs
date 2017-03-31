using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSaleModel
{
    public class Order
    {
        /// <summary>
        /// 訂單編號
        /// </summary>
        [DisplayName("訂單編號")]
        public int OrderID { get; set; }
        /// <summary>
        /// 客戶編號
        /// </summary>
        [DisplayName("客戶編號")]
        public int CustomerID { get; set; }
        /// <summary>
        /// 客戶姓名
        /// </summary>
        [DisplayName("客戶姓名")]
        public string CustomerName { get; set; }
        /// <summary>
        /// 員工編號
        /// </summary>
        [DisplayName("員工編號")]
        public int EmployeeID { get; set; }
        /// <summary>
        /// 員工姓名
        /// </summary>
        [DisplayName("員工姓名")]
        public string EmployeeName { get; set; }
        /// <summary>
        /// 訂單日期
        /// </summary>
        [DisplayName("訂單日期")]
        public DateTime? OrderDate { get; set; }
        /// <summary>
        /// 需求日期
        /// </summary>
        [DisplayName("需求日期")]
        public DateTime? RequiredDate { get; set; }
        /// <summary>
        /// 出貨日期
        /// </summary>
        [DisplayName("出貨日期")]
        public DateTime? ShippedDate { get; set; }
        /// <summary>
        /// 船隻編號
        /// </summary>
        [DisplayName("船隻編號")]
        public int ShipperID { get; set; }
        /// <summary>
        /// 海運運費
        /// </summary>
        [DisplayName("海運運費")]
        public decimal Freight { get; set; }
        /// <summary>
        /// 船隻名稱
        /// </summary>
        [DisplayName("船隻名稱")]
        public string ShipName { get; set; }
        /// <summary>
        /// 運送地址
        /// </summary>
        [DisplayName("運送地址")]
        public string ShipAddress { get; set; }
        /// <summary>
        /// 運送城市
        /// </summary>
        [DisplayName("運送城市")]
        public string ShipCity { get; set; }
        /// <summary>
        /// 運送地區
        /// </summary>
        [DisplayName("運送地區")]
        public string ShipRegion { get; set; }
        /// <summary>
        /// 郵遞區號
        /// </summary>
        [DisplayName("郵遞區號")]
        public string ShipPostalCode { get; set; }
        /// <summary>
        /// 運送國家
        /// </summary>
        [DisplayName("國家")]
        public string ShipCountry { get; set; }
        /// <summary>
        /// 發貨人
        /// </summary>
        [DisplayName("發貨人")]
        public string ShipperName { get; set; }

    }
}
