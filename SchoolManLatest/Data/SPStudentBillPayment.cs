using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{
    public class SPStudentBillPayment:BaseReference 
    {
        private SP_StudentBillPayment_Result paidData;
         public SPStudentBillPayment(SP_StudentBillPayment_Result obj) { paidData = obj; }
         public string FeesName { get { return paidData.FeesName; } }
         public decimal Amount { get { return paidData.Amount; } }
         public long FeeId { get { return paidData.FeeId; } }
         public decimal Discount { get { return paidData.Discount; } }
         public System.DateTime TimeStamp { get { return paidData.TimeStamp; } }
    }
}
