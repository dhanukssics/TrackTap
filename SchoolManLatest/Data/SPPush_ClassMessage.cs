using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{
    public class SPPush_ClassMessage
    {
        private SP_Push_ClassMessage_Result classMessage;
        public SPPush_ClassMessage(SP_Push_ClassMessage_Result obj) { classMessage = obj; }
        public long TokenId { get { return classMessage.TokenId; } }
        public int RoleId { get { return classMessage.RoleId; } }
        public long UserId { get { return classMessage.UserId; } }
        public string Token { get { return classMessage.Token; } }
        public System.DateTime TimeStamp { get { return classMessage.TimeStamp; } }
        public bool IsActive { get { return classMessage.IsActive; } }
        public int LoginStatus { get { return classMessage.LoginStatus; } }
    }
}
