using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.Data
{
    public class Payment : BaseReference
    {
        private tb_Payment payment;
        public Payment(tb_Payment obj) { payment = obj; }
        public Payment(long id) { payment = _Entities.tb_Payment.FirstOrDefault(z => z.PaymentId == id); }
        public System.Guid FeeGuid { get { return payment.FeeGuid; } }
        public long PaymentId { get { return payment.PaymentId; } }
        public decimal Amount { get { return payment.Amount; } }
        public decimal? Discount { get { return payment.Discount; } }
        public decimal? MaxAmount { get { return payment.MaxAmount; } }
        public System.Guid? PaymentGuid { get { return payment.PaymentGuid; } }
        public long? BillNo { get { return payment.BillNo; } }
        public bool IsPaid { get { return payment.IsPaid; } }
        public long FeeId { get { return payment.FeeId; } }
        public long StudentId { get { return payment.StudentId; } }
        public long ClassId { get { return payment.ClassId; } }
        public long SchoolId { get { return payment.ClassId; } }
        public System.DateTime TimeStamp { get { return payment.TimeStamp; } }
        public bool IsActive { get { return payment.IsActive; } }
        public Fee Fee { get { return new Fee(payment.tb_Fee); } }
        public Class Class { get { return new Class(payment.tb_Class); } }
        public FeeClass FeeClass { get { return new FeeClass(payment.FeeGuid); } }
        public int? BillType { get { return payment.BillType; } }
        public string StudentName { get { return payment.tb_Student.StundentName; } }
        public string DivisionName { get { return payment.tb_Student.tb_Division.Division; } }
        public string ClassName { get { return payment.tb_Class.Class; } }
    }
}
