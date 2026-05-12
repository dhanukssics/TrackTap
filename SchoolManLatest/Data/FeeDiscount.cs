using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.Data
{
    public class FeeDiscount : BaseReference
    {
        private tb_FeeDiscount feeDiscount;
        public FeeDiscount(tb_FeeDiscount obj) { feeDiscount = obj; }
        public FeeDiscount(long id) { feeDiscount = _Entities.tb_FeeDiscount.FirstOrDefault(z => z.DiscountId == id); }
        public long discountId { get { return feeDiscount.DiscountId; } }
        public long studentId { get { return feeDiscount.StudentId; } }
        public long feeId { get { return feeDiscount.FeeId; } }
        public string feename { get { return feeDiscount.tb_Fee.FeesName ?? string.Empty; } }
        public decimal discountAmount { get { return feeDiscount.DiscountAmount; } }
        public System.DateTime Timestamp { get { return feeDiscount.TimeStamp; } }
        public bool IsActive { get { return feeDiscount.IsActive; } }
        public Student student { get { return new Student(feeDiscount.StudentId); } }

    }
}
