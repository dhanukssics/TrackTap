using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.Data
{
   public  class Expence:BaseReference
    {
        private tb_Expense expn;
        public Expence(tb_Expense obj) { expn = obj; }
        public Expence(long id) { expn = _Entities.tb_Expense.FirstOrDefault(z => z.Id == id); }
        public long Id { get { return expn.Id; } }
        public string AccountHead { get { return expn.AccountHead; } }
        public string Particular { get { return expn.Particular; } }
        public Nullable<double> Amount { get { return expn.Amount; } }
        public bool IsActive { get { return expn.IsActive; } }
        public long SchoolId { get { return expn.SchoolId; } }
    
    }

}
