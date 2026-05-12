using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.MapModel
{
    public class StaffSMSMapModel
    {
        public string TypeName { get; set; }
        public int MemberType { get; set; }
        public List<MemberList> MemberList { get; set; }
    }
    public class MemberList
    {
        public string MemmberName { get; set; }
        public long MemberId { get; set; }
        public string ContactNumber { get; set; }
    }
}
