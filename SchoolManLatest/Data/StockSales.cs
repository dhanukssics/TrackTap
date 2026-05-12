using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.Data
{
    public class StockSales : BaseReference
    {
        private tb_SalesStock stock;
        public StockSales(tb_SalesStock obj) { stock = obj; }
        public StockSales(long Id) { stock = _Entities.tb_SalesStock.FirstOrDefault(z => z.CategoryId == Id); }
        public long StockId { get { return stock.StockId; } }
        public long CategoryId { get { return stock.CategoryId; } }
        public string Item { get { return stock.Item; } }
        public decimal Price { get { return stock.Price; } }
      
        public long SchoolId { get { return stock.SchoolId; } }
        public long UserId { get { return stock.UserId; } }

        public long StockQuantity { get { return Convert.ToInt32(stock.StockQuantity); } }// 9/21/2020

        public bool IsActive { get { return stock.IsActive; } }
        public System.DateTime TimeStamp { get { return stock.TimeStamp; } }
       
      
        
    }
}
