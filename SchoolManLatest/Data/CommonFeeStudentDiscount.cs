using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{
    public class CommonFeeStudentDiscount:BaseReference
    {
        private tb_CommonFeeStudentDiscount cfsd;
        public CommonFeeStudentDiscount(tb_CommonFeeStudentDiscount obj) { cfsd = obj; }
        public CommonFeeStudentDiscount(long Id) { cfsd = _Entities.tb_CommonFeeStudentDiscount.FirstOrDefault(z => z.Id == Id); }
        public long Id { get { return cfsd.Id; } }
        public long SchoolId { get { return cfsd.SchoolId; } }
        public long StudentId { get { return cfsd.StudentId; } }
        public long FeeId { get { return cfsd.FeeId; } }
        public System.DateTime DiscountAllowFeeDate { get { return cfsd.DiscountAllowFeeDate; } }
        public long UserId { get { return cfsd.UserId; } }
        public bool IsActive { get { return cfsd.IsActive; } }
        public Nullable<System.DateTime> TimeStamp { get { return cfsd.TimeStamp; } }
    }
}
