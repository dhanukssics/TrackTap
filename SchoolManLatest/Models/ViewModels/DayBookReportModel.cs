using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrackTap.Models
{
    public class DayBookReportModel
    {
        public long SchoolId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public long BankId { get; set; }
        public string SchoolName { get; set; }
        public string Heading { get; set; }

        public int IsContra { get; set; }  //Basheer to check contra in receipt and payment
      
    }
}