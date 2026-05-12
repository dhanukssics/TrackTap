using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.Data
{
   public class Subjects:BaseReference
    {
        private tb_Subjects sub;
        public Subjects(tb_Subjects obj) { sub = obj; }
        public Subjects(long id) { sub = _Entities.tb_Subjects.FirstOrDefault(z => z.SubId == id); }
        public long SubId { get { return sub.SubId; } }
        public long SchoolI { get { return sub.SchoolI; } }
        public string SubjectName { get { return sub.SubjectName; } }
        public bool IsActive { get { return sub.IsActive; } }
        public System.DateTime TmeStamp { get { return sub.TmeStamp; } }
    }
}
