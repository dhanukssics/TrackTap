using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{
    public class SPDetailedCollectionReport:BaseReference
    {
        private sp_DetailedCollectionReport_Result data;
        public SPDetailedCollectionReport(sp_DetailedCollectionReport_Result obj) { data = obj; }
        public long PaymentId { get { return data.PaymentId; } }
        public System.Guid FeeGuid { get { return data.FeeGuid; } }
        public decimal Amount { get { return data.Amount; } }
        public bool IsPaid { get { return data.IsPaid; } }
        public long FeeId { get { return data.FeeId; } }
        public long StudentId { get { return data.StudentId; } }
        public long ClassId { get { return data.ClassId; } }
        public long SchoolId { get { return data.SchoolId; } }
        public System.DateTime TimeStamp { get { return data.TimeStamp; } }
        public bool IsActive { get { return data.IsActive; } }
        public Nullable<decimal> MaxAmount { get { return data.MaxAmount; } }
        public Nullable<decimal> Discount { get { return data.Discount; } }
        public Nullable<System.Guid> PaymentGuid { get { return data.PaymentGuid; } }
        public Nullable<long> BillNo { get { return data.BillNo; } }
        public long PaymentType { get { return data.PaymentType; } }
        public Nullable<int> BillType { get { return data.BillType; } }
    }
}
