using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace eSaleService
{
    public class OrderService
    {
        public eSaleModel.Order GetOrderID(int id) {
            DataTable dt = new DataTable();
            string SQL = @"Select A.Orderid,A.Customerid,B.Companyname,A.Employeeid,C.Lastname + C.Firstname As Empname,
                           A.Orderdate,A.Requireddate,A.Shippeddate,A.Shipperid,D.Companyname AS Shippername,A.Freight,
                         A.Shipname,A.Shipaddress,A.Shipcity,A.Shipregion,A.Shippostalcode,A.Shipcountry From Sales.Orders AS A
                         Inner Join Sales.Customers As B On A.Customerid = B.Customerid Inner Join Hr.Employees As C On A.Employeeid = C.Employeeid
                         Inner Join Sales.Shippers As D On A.Shipperid = D.Shipperid Where A.Orderid = @Orderid";
            using (SqlConnection conn = new SqlConnection()) {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.Add(new SqlParameter("@Orderid",id));
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(dt);
                conn.Close();
            }
            return this.MapOrderDataToList(dt).FirstOrDefault();
        }
        private List<eSaleModel.Order> MapOrderDataToList(DataTable dt) {
            List<eSaleModel.Order> result = new List<eSaleModel.Order>();
            foreach (DataRow row in dt.Rows) {
                result.Add(new eSaleModel.Order() {
                    CustomerID = (int)row["CustomerID"],
                    // CustomerName = row["CustomerName"].ToString(),
                    EmployeeID = (int)row["EmployeeID"],
                    //EmployeeName = row["EmployeeName"].ToString(),
                    Freight = (decimal)row["Freight"],


                }
           }
            return 0;
        }

    }
}
