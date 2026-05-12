using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{
    public class SchoolSubModule : BaseReference
    {
        private tb_SchoolSubModule sub;
        public SchoolSubModule(tb_SchoolSubModule obj) { sub = obj; }
        public SchoolSubModule(long id) { sub = _Entities.tb_SchoolSubModule.FirstOrDefault(z => z.Id == id); }
        public long Id { get { return sub.Id; } }
        public long MainId { get { return sub.MainId; } }
        public string SclSubModule { get { return sub.SchoolSubModule; } }
        public bool IsActive { get { return sub.IsActive; } }
        public System.DateTime TimeStamp { get { return sub.TimeStamp; } }
        public string ModuleName { get { return sub.tb_SchoolModuleHome.MainModule; } }
    }
}
