using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{
    public class SPBankEntryReport:BaseReference
    {
        private sp_BankEntryReport_Result bank;
        public SPBankEntryReport(sp_BankEntryReport_Result obj) { bank = obj; }
        public System.DateTime EnterDate { get { return bank.EnterDate; } }
        public string VoucherNumber { get { return bank.VoucherNumber; } }
        public string TransactionType { get { return bank.TransactionType; } }
        public string BankName { get { return bank.BankName; } }
        public string AccHeadName { get { return bank.AccHeadName; } }
        public Nullable<decimal> Deposit { get { return bank.Deposit; } }
        public Nullable<decimal> Withdraw { get { return bank.Withdraw; } }
        public string Narration { get { return bank.Narration; } }
    }
}
