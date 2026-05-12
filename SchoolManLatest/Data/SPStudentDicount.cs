using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{
    public class SPStudentDicount:BaseReference
    {
        private sp_StudentDicount_Result dis;
        public SPStudentDicount(sp_StudentDicount_Result obj) { dis = obj; }
        public long DiscountId { get { return dis.DiscountId; } }
        public long StudentId { get { return dis.StudentId; } }
        public long Feeid { get { return dis.Feeid; } }
        public Nullable<decimal> DiscountAmount { get { return dis.DiscountAmount; } }
        public System.DateTime DiscountDate { get { return dis.DiscountDate; } }
        public int FromDiscount { get { return dis.FromDiscount; } }
    }
}
