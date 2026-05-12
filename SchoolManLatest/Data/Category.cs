using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.Data
{
   public class Category:BaseReference
    {
        private tb_Category category;
        public Category(tb_Category obj) { category = obj; }
        public Category(long id) { category = _Entities.tb_Category.FirstOrDefault(z => z.Id == id); }
        public long Id { get { return category.Id; } }
        public string CategoryName { get { return category.CategoryName; } }
        public bool IsActive { get { return category.IsActive; } }
        public System.DateTime TimeStamp { get { return category.TimeStamp; } }
    }
}
