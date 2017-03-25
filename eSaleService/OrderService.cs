using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace eSaleService
{
    public class OrderService
    {
        /// <summary>
        /// 取得DataBase
        /// </summary>
        /// <returns></returns>
        protected string GetDBconnectionstring()
        {
            return ConfigurationManager.ConnectionStrings["DBconn"].ConnectionString;
        }
        /// <summary>
        /// 取得訂單編號
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public eSaleModel.Order GetOrderID(int id) {
            DataTable dt = new DataTable();
            string SQL = @"Select A.Orderid,A.Customerid,B.Companyname,A.Employeeid,C.Lastname + C.Firstname As Employeename,
                           A.Orderdate,A.Requireddate,A.Shippeddate,A.Shipperid,D.Companyname AS Shippername,A.Freight,
                         A.Shipname,A.Shipaddress,A.Shipcity,A.Shipregion,A.Shippostalcode,A.Shipcountry From Sales.Orders AS A
                         Inner Join Sales.Customers As B On A.Customerid = B.Customerid Inner Join Hr.Employees As C On A.Employeeid = C.Employeeid
                         Inner Join Sales.Shippers As D On A.Shipperid = D.Shipperid Where A.Orderid = @Orderid";
            using (SqlConnection conn = new SqlConnection(this.GetDBconnectionstring())) {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("Success");
                }
                else {
                    Console.WriteLine("Fasle");
                }
                SqlCommand cmd = new SqlCommand(SQL,conn);
                cmd.Parameters.Add(new SqlParameter("@Orderid",id));
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(dt);
                conn.Close();
            }
            return this.MapOrderDataToList(dt).FirstOrDefault();
        }
        /// <summary>
        /// 找到條件訂單，放入DataTable
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<eSaleModel.Order> MapOrderDataToList(DataTable dt) {
            List<eSaleModel.Order> result = new List<eSaleModel.Order>();
            foreach (DataRow row in dt.Rows) {
                result.Add(new eSaleModel.Order()
                {
                    OrderID = (int)row["OrderID"],
                    CustomerID = (int)row["CustomerID"],
                    CustomerName = row["CompanyName"].ToString(),
                    EmployeeID = (int)row["EmployeeID"],
                    EmployeeName = row["EmployeeName"].ToString(),
                    Freight = (decimal)row["Freight"],
                    OrderDate = row["OrderDate"] == DBNull.Value ? (DateTime?)null : (DateTime)row["OrderDate"],
                    RequiredDate = row["RequiredDate"] == DBNull.Value ? (DateTime?)null : (DateTime)row["RequiredDate"],
                    ShippedDate = row["Shippeddate"] == DBNull.Value ? (DateTime?)null : (DateTime)row["Shippeddate"],
                    ShipperID = (int)row["ShipperID"],
                    ShipName = row["ShipName"].ToString(),
                    ShipPostalCode = row["ShipPostalCode"].ToString(),
                    ShipCity = row["ShipCity"].ToString(),
                    ShipRegion = row["ShipRegion"].ToString(),
                    ShipCountry = row["ShipCountry"].ToString(),
                    ShipAddress = row["ShipAddress"].ToString(),
                    ShipperName = row["ShipperName"].ToString()
                });
            }
            return result;
        }
    }
}
