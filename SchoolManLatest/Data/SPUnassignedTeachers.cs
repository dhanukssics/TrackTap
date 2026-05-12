using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.Data
{
    public class SPUnassignedTeachers:BaseReference
    {
        private SP_UnassignedTeachers_Result freeClassList;
        public SPUnassignedTeachers(SP_UnassignedTeachers_Result obj) { freeClassList = obj; }
        public string ClassName { get { return freeClassList.ClassName; } }
        public long ClassId { get { return freeClassList.ClassId; } }
    }
}
