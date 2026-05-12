using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.Data
{
    public class SPStockDayBookReport : BaseReference
    {
        private sp_StockDayBookReport_Result dayBookReportSP;
        public SPStockDayBookReport(sp_StockDayBookReport_Result obj) { dayBookReportSP = obj; }
        public long Id { get { return dayBookReportSP.Id; } }
        public string VoucherNumber { get { return dayBookReportSP.VoucherNumber; } }
        public string VoucherType { get { return dayBookReportSP.VoucherType; } }
        public string BillNo { get { return dayBookReportSP.BillNo; } }
        public string Bank_Cash { get { return dayBookReportSP.Bank_Cash; } }
        public long HeadId { get { return dayBookReportSP.HeadId; } }
        public string AccHeadName { get { return dayBookReportSP.AccHeadName; } }
        public System.DateTime EnterDate { get { return dayBookReportSP.EnterDate; } }
     //   public long SubId { get { return dayBookReportSP.SubId; } }
        public long UserId { get { return dayBookReportSP.UserId; } }
        public Nullable<decimal> Income { get { return dayBookReportSP.Income; } }
        public Nullable<decimal> Expense { get { return dayBookReportSP.Expense; } }
        public string Narration { get { return dayBookReportSP.Narration; } }
        public string EditStatus { get { return dayBookReportSP.EditStatus; } }

        public List<SPDayBookReports> _list { get; set; }
    }

    public class SPDayBookReports
    {
        public string VoucherNo { get; set; }
        public string AccountHeadName { get; set; }
        public string SubLedger { get; set; }
        public decimal IncomeAmount { get; set; }
        public decimal ExpenseAmount { get; set; }
        public string TransactionType { get; set; }
        public string Narration { get; set; }
        public string FromStatus { get; set; }

        public string VoucherType { get; set; }
        public DateTime EnterDate { get; set; }

    }


}
