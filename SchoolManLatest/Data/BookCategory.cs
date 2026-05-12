using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{
    public class BookCategory : BaseReference
    {
        private tb_BookCategory category;
        public BookCategory(tb_BookCategory obj) { category = obj; }
        public BookCategory(long id) { category = _Entities.tb_BookCategory.FirstOrDefault(z => z.CategoryId == id); }
        public long categoryId { get { return category.CategoryId; } }
        public string categoryName { get { return category.Category; } }
        public long schoolId { get { return category.SchoolId; } }
        public bool IsActive { get { return category.IsActive; } }
    }
}
