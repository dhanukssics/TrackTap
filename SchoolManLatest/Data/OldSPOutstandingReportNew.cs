using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.Data
{
    public class OldSPOutstandingReportNew: BaseReference
    {
        private SP_OldClassOutstandingReport_Result outstand;
        public OldSPOutstandingReportNew(SP_OldClassOutstandingReport_Result obj) { outstand = obj; }
        public long StudentId { get { return outstand.StudentId; } }
        public long FeeId { get { return outstand.FeeId; } }
        public Nullable<decimal> Amount { get { return outstand.Amount; } }
        public Nullable<System.Guid> FeeGuid { get { return outstand.FeeGuid; } }
        public string Feename { get { return outstand.Feename; } }
        public System.DateTime DueDate { get { return outstand.DueDate; } }
        public int DiscountAllowed { get { return outstand.DiscountAllowed; } }
        public int StudentspecialFee { get { return outstand.StudentspecialFee; } }
    }
}
