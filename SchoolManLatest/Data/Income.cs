using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.Data
{
   public  class Income:BaseReference
    {
        private tb_Income income;
        public Income(tb_Income obj) { income = obj; }
        public Income(long id) { income = _Entities.tb_Income.FirstOrDefault(z => z.Id == id); }
        public long Id { get { return income.Id; } }

        public long SchoolId { get { return income.SchoolId; } }
        public string AccountHead { get { return income.AccountHead; } }
        public string Particular { get { return income.Particular; } }
        public Nullable<double> Amount { get { return income.Amount; } }
        public bool IsActive { get { return income.IsActive; } }
    }
}
