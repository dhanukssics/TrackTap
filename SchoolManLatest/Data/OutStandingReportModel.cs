
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.Data
{
    public class OutStandingReportModel
    {
        public long StudentId { get; set; }
        public long FeeId { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<System.Guid> FeeGuid { get; set; }
        public string Feename { get; set; }
        public System.DateTime DueDate { get; set; }
        public int DiscountAllowed { get; set; }
        public int StudentspecialFee { get; set; }
    }
}
