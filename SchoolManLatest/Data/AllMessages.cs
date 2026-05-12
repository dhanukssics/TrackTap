using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{
    public class AllMessages : BaseReference
    {
        private tb_AllMessage msg;
        public AllMessages(tb_AllMessage obj) { msg = obj; }
        public AllMessages(long Id) { msg = _Entities.tb_AllMessage.FirstOrDefault(z => z.MessageId == Id); }
        public long MessageId { get { return msg.MessageId; } }
        public long TeacherId { get { return msg.TeacherId; } }
        public long ToMsgSentId { get { return msg.ToMsgSentId; } }
        public string Subject { get { return msg.Subject; } }
        public string Description { get { return msg.Description; } }
        public int MessageType { get { return msg.MessageType; } }
        public string MessageTypeString { get { return ((ClassLibrary.MessageType)msg.MessageType).ToString(); } }
        public string Filepath { get { return msg.Filepath; } }
        public bool IsActive { get { return msg.IsActive; } }
        public System.DateTime Timestamp { get { return msg.Timestamp; } }
        public string teacherName { get { return msg.tb_Teacher.TeacherName; } }
        public string teacherContact { get { return msg.tb_Teacher.ContactNumber; } }
    }
}
