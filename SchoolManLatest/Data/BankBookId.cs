using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{
    public class BankBookId:BaseReference
    {
        private tb_BankBookId bankBookId;
        public BankBookId(tb_BankBookId obj) { bankBookId = obj; }
        public BankBookId(long Id) { bankBookId = _Entities.tb_BankBookId.FirstOrDefault(z => z.Id == Id); }
        public long SchoolId { get { return bankBookId.SchoolId; } }
        public long DepositId { get { return bankBookId.DepositId; } }
        public long WithdrawId { get { return bankBookId.WithdrawId; } }
    }
}
