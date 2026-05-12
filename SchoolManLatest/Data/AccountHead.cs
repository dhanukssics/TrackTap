using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.Data
{
    public class AccountHead:BaseReference
    {
        private tb_AccountHead accountHead;
        public AccountHead(tb_AccountHead obj) { accountHead = obj; }
        public AccountHead(long id) { accountHead = _Entities.tb_AccountHead.FirstOrDefault(z => z.AccountId == id); }
        public long AccountId { get { return accountHead.AccountId; } }
        public string AccHeadName { get { return accountHead.AccHeadName; } }
        public long SchoolId { get { return accountHead.SchoolId; } }
        public bool IsActive { get { return accountHead.IsActive; } }
        public System.DateTime TimeStamp { get { return accountHead.TimeStamp; } }

        
    }
}
