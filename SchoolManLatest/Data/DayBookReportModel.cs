using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{
    public class DayBookReportModel
    {
        public DateTime EnterDate { get; set; }
        public decimal Opening { get; set; }
        public decimal Closing { get; set; }
        public List<DayBookReportDetails> _list { get; set; }
        

        public string voucheridgroup { get; set; }
    }
    public class DayBookReportDetails
    {
        public long Commen_Id { get; set; }
        public string VoucherNo { get; set; }
      
        public string AccountHeadName { get; set; }
        public string SubLedger { get; set; }
        public decimal IncomeAmount { get; set; }
        public decimal ExpenseAmount { get; set; }
        public string TransactionType { get; set; }
        public string Narration { get; set; }
        public string FromStatus { get; set; }
        //public string VoucherType { get; set; } //Basheer on 29-11-2019 to Check the voucher type
    }
   





    public class DayBookReportModelAccountHide
    {
        public DateTime EnterDate { get; set; }
        public decimal Opening { get; set; }
        public decimal Closing { get; set; }
        public List<DayBookReportDetailsAccountHide> _list { get; set; }
    }
    public class DayBookReportDetailsAccountHide
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
