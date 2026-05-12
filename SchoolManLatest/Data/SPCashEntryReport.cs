using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{
    public class SPCashEntryReport:BaseReference
    {
        private sp_CashEntryReport_Result status;
        public SPCashEntryReport(sp_CashEntryReport_Result obj) { status = obj; }
        public System.DateTime EnterDate { get { return status.EnterDate; } }
        public string VoucherNumber { get { return status.VoucherNumber; } }
        public string VoucherType { get { return status.VoucherType; } }
        public string AccHeadName { get { return status.AccHeadName; } }
        public string Narration { get { return status.Narration; } }
        public Nullable<decimal> Income { get { return status.Income; } }
        public Nullable<decimal> Expense { get { return status.Expense; } }
        public string Status { get { return status.Status; } }

    }
}
