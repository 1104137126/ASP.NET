using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSaleService
{
    public class OrderService
    {
        public eSaleModel.Order GetOrderID(int id) {
            eSaleDao.OrderDao orderdao = new eSaleDao.OrderDao();
            return orderdao.GetOrderID(id);
        }
    }
}
