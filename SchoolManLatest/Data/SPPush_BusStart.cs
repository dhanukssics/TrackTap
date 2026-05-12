using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.Data
{
    public class SPPush_BusStart:BaseReference
    {
        private SP_Push_BusStart_Result pushData;
        public SPPush_BusStart(SP_Push_BusStart_Result obj) { pushData = obj; }
        public long TokenId { get { return pushData.TokenId; } }
        public int RoleId { get { return pushData.RoleId; } }
        public long UserId { get { return pushData.UserId; } }
        public string Token { get { return pushData.Token; } }
        public System.DateTime TimeStamp { get { return pushData.TimeStamp; } }
        public bool IsActive { get { return pushData.IsActive; } }
        public int LoginStatus { get { return pushData.LoginStatus; } }
    }
}
