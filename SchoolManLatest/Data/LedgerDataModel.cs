using System;
using System.Collections.Generic;

namespace TrackTap.Data
{
    public class LedgerDataModel
    {
        public long HeadId { get; set; }
        public string HeadName { get; set; }
        public List<SubLedgerDetails> list { get; set; }
        public decimal DebitTotal { get; set; }
        public decimal CreditTotal { get; set; }
    }
    public class SubLedgerDetails
    {
        public long DayBookId { get; set; }
        public DateTime EntryDate { get; set; }
        public string VoucherNumber { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public string Narration { get; set; }
        public string Symbol { get; set;}
    }
}