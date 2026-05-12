using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{
    public class SchoolSenderId : BaseReference
    {
        private tb_SchoolSenderId senderId;
        public SchoolSenderId(tb_SchoolSenderId obj) { senderId = obj; }
        public SchoolSenderId(long id) { senderId = _Entities.tb_SchoolSenderId.FirstOrDefault(z => z.Id == id); }
        public long Id { get { return senderId.Id; } }
        public long SchoolId { get { return senderId.SchoolId; } }
        public string SenderId { get { return senderId.SenderId; } }
        public Nullable<bool> IsActive { get { return senderId.IsActive; } }
    }
}
