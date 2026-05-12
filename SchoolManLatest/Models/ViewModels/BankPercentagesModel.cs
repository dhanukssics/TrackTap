using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrackTap.Models
{
    public class BankPercentagesModel
    {
        public long BankPercentageId { get; set; }
        public string SchoolName { get; set; }
        public long SchoolId { get; set; }
        public int Amount { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime UpdateTimeStamp { get; set; }
        public System.DateTime TimeStamp { get; set; }
    }
}