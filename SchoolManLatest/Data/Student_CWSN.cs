using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.Data
{
    public class Student_CWSN:BaseReference
    {
        private tb_Student_CWSN cwsn;
        public Student_CWSN(tb_Student_CWSN obj) { cwsn = obj; }
        public Student_CWSN(long id) { cwsn = _Entities.tb_Student_CWSN.FirstOrDefault(z => z.Id == id); }
        public long Id { get { return cwsn.Id; } }
        public string CWSN_Data { get { return cwsn.CWSN_Data; } }
        public bool IsActive { get { return cwsn.IsActive; } }
        public System.DateTime TimeStamp { get { return cwsn.TimeStamp; } }
    }
}
