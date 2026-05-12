using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{
    public class SPCashBookReportData : BaseReference
    {
        private sp_CashBookReportData_Result cashBook;
        public SPCashBookReportData(sp_CashBookReportData_Result obj) { cashBook = obj; }
        public long DayBookId { get { return cashBook.DayBookId; } }
        public System.DateTime? EntryDate { get { return cashBook.EntryDate; } }
        public string AccHeadName { get { return cashBook.AccHeadName; } }
        public decimal? Expense { get { return cashBook.Expense; } }
        public decimal? Income { get { return cashBook.Income; } }
        public string VoucherNo { get { return cashBook.VoucherNo; } }
        public string Narration { get { return cashBook.Narration; } }
    }
}
