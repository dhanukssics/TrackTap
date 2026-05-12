using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{
  public  class Religion:BaseReference
    {
        private tb_Religion religion;
        public Religion(tb_Religion obj) { religion = obj; }
        public Religion(long id) { religion = _Entities.tb_Religion.FirstOrDefault(z => z.Id == id); }
        public long Id { get { return religion.Id; } }
        public string ReligionName { get { return religion.ReligionName; } }
        public bool IsActive { get { return religion.IsActive; } }
        public System.DateTime TimeStamp { get { return religion.TimeStamp; } }
    }
}
