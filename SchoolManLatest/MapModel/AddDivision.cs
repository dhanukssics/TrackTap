using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.ClassLibrary.MapModel
{
 public   class AddDivision
    {
     public long DivisionId { get; set; }
     public long ClassId { get; set; }
     public string DivisionName { get; set; }
     public System.DateTime TimeStamp { get; set; }
     public System.Guid DivisionGuid { get; set; }
     public bool IsActive { get; set; }
     public string ClassName { get; set; }

    }
}
