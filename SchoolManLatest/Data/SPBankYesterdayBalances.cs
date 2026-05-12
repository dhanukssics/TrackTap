using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{
    public class SPBankYesterdayBalances:BaseReference
    {
        private sp_BankYesterdayBalance_Result balance;
        public SPBankYesterdayBalances(sp_BankYesterdayBalance_Result obj) { balance = obj; }
        public Nullable<decimal> Balance { get { return balance.Balance; } }
        public Nullable<decimal> incomeBank { get { return balance.incomeBank; } }
        public Nullable<decimal> ExpenseBank { get { return balance.ExpenseBank; } }
    }
}
