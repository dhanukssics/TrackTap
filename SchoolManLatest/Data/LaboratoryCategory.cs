using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{
    public class LaboratoryCategory : BaseReference
    {
        private tb_LaboratoryCategory category;
        public LaboratoryCategory(tb_LaboratoryCategory obj) { category = obj; }
        public LaboratoryCategory(long id) { category = _Entities.tb_LaboratoryCategory.FirstOrDefault(z => z.CategoryId == id); }
        public long categoryId { get { return category.CategoryId; } }
        public string laboratoryName { get { return category.LaboratoryName; } }
        public long schoolId { get { return category.SchoolId; } }
        public bool IsActive { get { return category.IsActive; } }
    }
}
