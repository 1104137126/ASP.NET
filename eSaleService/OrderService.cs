using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
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
                    ShipperName = row["ShipperName"].ToString(),
                    CompanyName = row["CompanyName"].ToString()
                });
            }
            return result;
        }
        /// <summary>
        /// 取得訂單編號
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<eSaleModel.Order> GetOrder(eSaleModel.Order order) {
            DataTable dt = new DataTable();
            string SQL = @"Select A.Orderid,A.Customerid,B.Companyname,A.Employeeid,C.Lastname +' '+ C.Firstname As Employeename,
                           A.Orderdate,A.Requireddate,ISNULL(A.Shippeddate,null) Shippeddate,A.Shipperid,D.Companyname AS Shippername,A.Freight,
                           A.Shipname,A.Shipaddress,A.Shipcity,A.Shipregion,A.Shippostalcode,A.Shipcountry 
                           From Sales.Orders AS A Inner Join Sales.Customers As B On A.Customerid = B.Customerid 
                           Inner Join Hr.Employees As C On A.Employeeid = C.Employeeid
                           Inner Join Sales.Shippers As D On A.Shipperid = D.Shipperid 
                           Where A.Orderid like @Orderid and B.CompanyName like @CustomerName and (C.LastName+' '+C.FirstName) like @EmployeeName
                                 and D.CompanyName like @CompanyName and convert(varchar(10),A.OrderDate,111) like @OrderDate and convert(varchar(10),A.ShippedDate,111) like @ShippedDate
                                 and convert(varchar(10),A.RequiredDate,111) like @RequiredDate";
            using (SqlConnection conn = new SqlConnection(this.GetDBconnectionstring())) {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.Parameters.Add(new SqlParameter("@OrderID",order.OrderID==0?"%%":"%"+Convert.ToString(order.OrderID)+"%"));
                    cmd.Parameters.Add(new SqlParameter("@CustomerName", order.CustomerName==null?"%%":"%"+Convert.ToString(order.CustomerName)+"%"));
                    cmd.Parameters.Add(new SqlParameter("@EmployeeName", order.EmployeeName == null ? "%%" :Convert.ToString(order.EmployeeName)));
                    cmd.Parameters.Add(new SqlParameter("@CompanyName", order.CompanyName == null ? "%%" : Convert.ToString(order.CompanyName)));
                    cmd.Parameters.Add(new SqlParameter("@OrderDate", order.OrderDate == null ? "%%" : "%"+string.Format("{0:yyyy/MM/dd}", order.OrderDate)+"%"));
                    cmd.Parameters.Add(new SqlParameter("@ShippedDate", order.ShippedDate == null ? "%%" : "%" + string.Format("{0:yyyy/MM/dd}", order.ShippedDate) + "%"));
                    cmd.Parameters.Add(new SqlParameter("@RequiredDate", order.RequiredDate == null ? "%%" : "%" + string.Format("{0:yyyy/MM/dd}", order.RequiredDate) + "%"));
                    SqlDataAdapter ad = new SqlDataAdapter(cmd);
                    ad.Fill(dt);
                    conn.Close();
                }
                catch (Exception e) {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            }
            return this.MapOrderDataToList(dt);
        }
        /// <summary>
        /// 取得結果
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public eSaleModel.Order GetOrderResult(eSaleModel.Order order)
        {
            DataTable dt = new DataTable();
            string SQL = @"Select A.Orderid,A.Customerid,B.Companyname,A.Employeeid,C.Lastname +' '+ C.Firstname As Employeename,
                           A.Orderdate,A.Requireddate,A.Shippeddate,A.Shipperid,D.Companyname AS Shippername,A.Freight,
                           A.Shipname,A.Shipaddress,A.Shipcity,A.Shipregion,A.Shippostalcode,A.Shipcountry 
                           From Sales.Orders AS A Inner Join Sales.Customers As B On A.Customerid = B.Customerid 
                           Inner Join Hr.Employees As C On A.Employeeid = C.Employeeid
                           Inner Join Sales.Shippers As D On A.Shipperid = D.Shipperid 
                           Where A.Orderid like @Orderid and B.CompanyName like @CustomerName and (C.FirstName+' '+C.FirstName) like @EmployeeName
                                 and D.CompanyName like @CompanyName and A.OrderDate like @OrderDate and A.ShippedDate like @ShippedDate
                                 and A.RequiredDate like @RequiredDate";
            using (SqlConnection conn = new SqlConnection(this.GetDBconnectionstring()))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.Parameters.Add(new SqlParameter("@OrderID", order.OrderID == 0 ? "%%" : "%" + Convert.ToString(order.OrderID) + "%"));
                    cmd.Parameters.Add(new SqlParameter("@CustomerName", order.CustomerName == null ? "%%" : "%" + Convert.ToString(order.CustomerName) + "%"));
                    cmd.Parameters.Add(new SqlParameter("@EmployeeName", order.EmployeeName == null ? "%%" : Convert.ToString(order.EmployeeName)));
                    cmd.Parameters.Add(new SqlParameter("@CompanyName", order.CompanyName == null ? "%%" : Convert.ToString(order.CompanyName)));
                    cmd.Parameters.Add(new SqlParameter("@OrderDate", order.OrderDate == null ? "%%" : Convert.ToString(order.OrderDate)));
                    cmd.Parameters.Add(new SqlParameter("@ShippedDate", order.ShippedDate == null ? "%%" : Convert.ToString(order.ShippedDate)));
                    cmd.Parameters.Add(new SqlParameter("@RequiredDate", order.RequiredDate == null ? "%%" : Convert.ToString(order.RequiredDate)));
                    SqlDataAdapter ad = new SqlDataAdapter(cmd);
                    ad.Fill(dt);
                    conn.Close();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            }
            return this.MapOrderDataToList(dt).First();
        }
        /// <summary>
        /// 取得CustomerID
        /// </summary>
        /// <returns></returns>清單
        public List<SelectListItem> GetCustomerID()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            string SQL = "Select CustomerID from Sales.Customers order by CustomerID";
            using (SqlConnection conn = new SqlConnection(this.GetDBconnectionstring()))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        list.Add(new SelectListItem { Text = rd[0].ToString(), Value = rd[0].ToString() });
                    }

                    conn.Close();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            }
            return list;
        }
        /// <summary>
        /// 取得CustomerName
        /// </summary>
        /// <returns></returns>清單
        public List<SelectListItem> GetCustomerName() {
            List <SelectListItem> list= new List<SelectListItem>();
            string SQL = "Select CompanyName from Sales.Customers order by CompanyName";
            using (SqlConnection conn = new SqlConnection(this.GetDBconnectionstring()))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        list.Add(new SelectListItem { Text = rd[0].ToString(), Value = rd[0].ToString() });
                    }

                    conn.Close();
                }
                catch (Exception e) {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            }
            return list;
        }
        /// <summary>
        /// 取得EmployeeID清單
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetEmployeeID()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            string SQL = "Select EmployeeID from HR.Employees order by EmployeeID";
            using (SqlConnection conn = new SqlConnection(this.GetDBconnectionstring()))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        list.Add(new SelectListItem { Text = rd[0].ToString(), Value = rd[0].ToString() });
                    }

                    conn.Close();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            }
            return list;
        }
        /// <summary>
        /// 取得EmployeeName清單
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetEmployeeName()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            string SQL = "Select LastName+' '+FirstName from HR.Employees order by LastName,FirstName";
            using (SqlConnection conn = new SqlConnection(this.GetDBconnectionstring()))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        list.Add(new SelectListItem { Text = rd[0].ToString(), Value = rd[0].ToString() });
                    }

                    conn.Close();
                }
                catch (Exception e) {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            }
            return list;
        }
        /// <summary>
        /// 取得ShipperID清單
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetShipperID()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            string SQL = "Select ShipperID from Sales.Shippers order by ShipperID";
            using (SqlConnection conn = new SqlConnection(this.GetDBconnectionstring()))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        list.Add(new SelectListItem { Text = rd[0].ToString(), Value = rd[0].ToString() });
                    }

                    conn.Close();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            }
            return list;
        }
        /// <summary>
        /// 取得ShipCompanyName清單
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetShipCompanyName()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            string SQL = "Select CompanyName from Sales.Shippers order by CompanyName";
            using (SqlConnection conn = new SqlConnection(this.GetDBconnectionstring()))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        list.Add(new SelectListItem { Text = rd[0].ToString(), Value = rd[0].ToString() });
                    }

                    conn.Close();
                }
                catch (Exception e) {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            }
            return list;
        }

        /// <summary>
        /// 取得ProductName清單
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetProductName()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            string SQL = "Select ProductID,ProductName from Production.Products order by ProductName";
            using (SqlConnection conn = new SqlConnection(this.GetDBconnectionstring()))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        list.Add(new SelectListItem { Text = rd[1].ToString(), Value = rd[0].ToString() });
                    }

                    conn.Close();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            }
            return list;
        }

        /// <summary>
        /// 新增訂單
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public int InsertOrder(eSaleModel.Order order) {
            string SQL = @"Insert into Sales.Orders values(@CustomerID,
                         @EmployeeID,@OrderDate,@RequiredDate,@ShippedDate,@ShipperID,
                         @Freight,@ShipName,@ShipAddress,@ShipCity,@ShipRegion,@ShipPostalCode,
                         @ShipCountry)
                         Select Cast(SCOPE_IDENTITY() as int)";
            int id=0;
            using (SqlConnection conn = new SqlConnection(this.GetDBconnectionstring()))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.Parameters.Add(new SqlParameter("@CustomerID", order.CustomerID));
                    cmd.Parameters.Add(new SqlParameter("@EmployeeID", order.EmployeeID));
                    cmd.Parameters.Add(new SqlParameter("@OrderDate", string.Format("{0:yyyy/MM/dd}",order.OrderDate)));
                    cmd.Parameters.Add(new SqlParameter("@RequiredDate", string.Format("{0:yyyy/MM/dd}", order.RequiredDate)));
                    cmd.Parameters.Add(new SqlParameter("@ShippedDate", string.Format("{0:yyyy/MM/dd}",order.ShippedDate)));
                    cmd.Parameters.Add(new SqlParameter("@ShipperID", order.ShipperID));
                    cmd.Parameters.Add(new SqlParameter("@Freight", order.Freight));
                    cmd.Parameters.Add(new SqlParameter("@ShipName", order.ShipName==null?"":order.ShipName));
                    cmd.Parameters.Add(new SqlParameter("@ShipAddress", order.ShipAddress));
                    cmd.Parameters.Add(new SqlParameter("@ShipCity", order.ShipCity));
                    cmd.Parameters.Add(new SqlParameter("@ShipRegion", order.ShipRegion));
                    cmd.Parameters.Add(new SqlParameter("@ShipPostalCode", order.ShipPostalCode));
                    cmd.Parameters.Add(new SqlParameter("@ShipCountry", order.ShipCountry));
                    id = (int)cmd.ExecuteScalar();
                    conn.Close();
                }
                catch(Exception e){
                    System.Diagnostics.Debug.WriteLine(e);
                }
            }
            return id ;
        }

        /// <summary>
        /// 新增訂單明細
        /// </summary>
        /// <param name="id"></param>
        /// <param name="order"></param>
        public void InsertOrderDetails(int id, eSaleModel.Order order)
        {
            if (order.ProductName !=null)
            {
                for (int i = 0; i < order.ProductName.Length; i++)
                {
                    string SQL = @"Insert into Sales.OrderDetails values(@ID,@ProductID,@UnitPrice,@Qty,@Discount)";

                    using (SqlConnection conn = new SqlConnection(this.GetDBconnectionstring()))
                    {
                        try
                        {
                            conn.Open();
                            SqlCommand cmd = new SqlCommand(SQL, conn);
                            cmd.Parameters.Add(new SqlParameter("@ID", id));
                            cmd.Parameters.Add(new SqlParameter("@ProductID", order.ProductName[i]));
                            cmd.Parameters.Add(new SqlParameter("@UnitPrice", order.UnitPrice[i]));
                            cmd.Parameters.Add(new SqlParameter("@Qty", order.Qty[i]));
                            cmd.Parameters.Add(new SqlParameter("@Discount", order.Discount[i]));
                            if (cmd.ExecuteNonQuery() == 1)
                            {
                                System.Diagnostics.Debug.WriteLine("SSSSSSSSSSS");
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("FFFFFFFFFFF");
                            }
                            conn.Close();
                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.WriteLine(e);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 刪除訂單
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public Boolean DeleteOrder(int orderid) {
            string SQL = @"Delete from Sales.Orders where Orderid=@Orderid";
            Boolean check = false;
            using (SqlConnection conn = new SqlConnection(this.GetDBconnectionstring()))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.Parameters.Add(new SqlParameter("@OrderID", orderid));
                    if (cmd.ExecuteNonQuery().ToString().Equals("1"))
                    {
                        check = true;
                    }
                    conn.Close();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            }
            return check;
        }

        /// <summary>
        /// 刪除訂單明細
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public Boolean DeleteOrderDetails(int orderid)
        {
            string SQL = @"Delete from Sales.OrderDetails where Orderid=@Orderid";
            Boolean check = false;
            using (SqlConnection conn = new SqlConnection(this.GetDBconnectionstring()))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.Parameters.Add(new SqlParameter("@OrderID", orderid));
                    if (cmd.ExecuteNonQuery().ToString().Equals("1"))
                    {
                        check = true;
                    }
                    conn.Close();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            }
            return check;
        }


        /// <summary>
        /// 修改訂單
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public Boolean ModifyOrder(eSaleModel.Order order) {
            string SQL = @"Update Sales.orders set CustomerID=@CustomerID,EmployeeID=@EmployeeID,OrderDate=@OrderDate,RequiredDate=@RequiredDate,
                           ShippedDate=@ShippedDate,ShipperID=@ShipperID,Freight=@Freight,ShipName=@ShipName,ShipAddress=@ShipAddress,ShipCity=@ShipCity,
                           ShipRegion=@ShipRegion,ShipPostalCode=@ShipPostalCode,ShipCountry=@ShipCountry where orderid=@OrderID";
            Boolean check = false;
            using (SqlConnection conn = new SqlConnection(this.GetDBconnectionstring()))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.Parameters.Add(new SqlParameter("@OrderID", order.OrderID));
                    cmd.Parameters.Add(new SqlParameter("@CustomerID", order.CustomerID));
                    cmd.Parameters.Add(new SqlParameter("@EmployeeID", order.EmployeeID));
                    cmd.Parameters.Add(new SqlParameter("@OrderDate", string.Format("{0:yyyy/MM/dd}", order.OrderDate)));
                    cmd.Parameters.Add(new SqlParameter("@RequiredDate", string.Format("{0:yyyy/MM/dd}", order.RequiredDate)));
                    cmd.Parameters.Add(new SqlParameter("@ShippedDate", string.Format("{0:yyyy/MM/dd}", order.ShippedDate)));
                    cmd.Parameters.Add(new SqlParameter("@ShipperID", order.ShipperID));
                    cmd.Parameters.Add(new SqlParameter("@Freight", order.Freight));
                    cmd.Parameters.Add(new SqlParameter("@ShipName", order.ShipName==null?"":order.ShipName));
                    cmd.Parameters.Add(new SqlParameter("@ShipAddress", order.ShipAddress));
                    cmd.Parameters.Add(new SqlParameter("@ShipCity", order.ShipCity));
                    cmd.Parameters.Add(new SqlParameter("@ShipRegion", order.ShipRegion));
                    cmd.Parameters.Add(new SqlParameter("@ShipPostalCode", order.ShipPostalCode));
                    cmd.Parameters.Add(new SqlParameter("@ShipCountry", order.ShipCountry));
                    
                    if (cmd.ExecuteNonQuery().ToString().Equals("1"))
                    {
                        check = true;
                    }
                    conn.Close();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            }
            return check;
        }

        public List<eSaleModel.Order> ModifyOrderProduct(int orderid) {
            DataTable dt = new DataTable();
            string SQL = @"Select * from Sales.OrderDetails where orderID=@OrderID";
            using (SqlConnection conn = new SqlConnection(this.GetDBconnectionstring()))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.Parameters.Add(new SqlParameter("@OrderID", orderid));
                    
                    SqlDataAdapter ad = new SqlDataAdapter(cmd);
                    ad.Fill(dt);
                    conn.Close();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            }
            return this.MapModifyOrderDataToList(dt);
        }

        private List<eSaleModel.Order> MapModifyOrderDataToList(DataTable dt)
        {
            List<eSaleModel.Order> result = new List<eSaleModel.Order>();
            int[] tmp = new int[1];
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new eSaleModel.Order() {
                    ProductName = new int[] { Convert.ToInt32(row["ProductID"]) },
                    UnitPrice = new int[] { Convert.ToInt32(row["UnitPrice"]) },
                    Qty = new int[] { Convert.ToInt32(row["Qty"]) },
                    Discount = new float[] { Convert.ToSingle(row["Discount"]) }

                });
            }
            return result;
        }
    }
}
