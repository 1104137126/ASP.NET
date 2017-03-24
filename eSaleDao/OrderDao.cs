using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSaleDao
{
    public class OrderDao
    {
        /// <summary>
        ///查詢訂單編號 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public eSaleModel.Order GetOrderID(int id) {

            return new eSaleModel.Order() { CustomerID=id};
        }
    }
}
