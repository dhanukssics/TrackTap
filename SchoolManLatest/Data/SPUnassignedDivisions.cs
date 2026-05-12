using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.Data
{
    public class SPUnassignedDivisions:BaseReference
    {
         private SP_UnassignedDivisions_Result freeDivisionList;
         public SPUnassignedDivisions(SP_UnassignedDivisions_Result obj) { freeDivisionList = obj; }
         public string Division { get { return freeDivisionList.Division; } }
         public long DivisionId { get { return freeDivisionList.DivisionId; } }
    }
}
