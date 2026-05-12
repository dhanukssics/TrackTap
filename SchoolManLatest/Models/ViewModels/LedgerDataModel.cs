using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrackTap.Models
{
    public class LedgerDataModel
    {
       public long HeadId { get; set; }
        public string HeadName { get; set; }
        public List<SubLedgerDetails> list { get; set; }
    }
    public class SubLedgerDetails
    {
        public long DayBookId { get; set; }
        public DateTime EntryDate { get; set; }
        public string VoucherNumber { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
    }
}