using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{
    public class Circular : BaseReference
    {
        private tb_Circular circular;
        public Circular(tb_Circular obj) { circular = obj; }
        public Circular(long id) { circular = _Entities.tb_Circular.FirstOrDefault(z => z.CircularId == id); }
        public long CircularId { get { return circular.CircularId; } }
        public long SchoolId { get { return circular.SchoolId; } }
        public int LoginType { get { return circular.LoginType; } }
        public long USerId { get { return circular.USerId; } }
        public DateTime CircularDate { get { return circular.CircularDate; } }
        public string Description { get { return circular.Description; } }
        public string FilePath { get { return circular.FilePath; } }
        public bool IsActive { get { return circular.IsActive; } }
        public DateTime TimeStamp { get { return circular.TimeStamp; } }
        
    }
}
