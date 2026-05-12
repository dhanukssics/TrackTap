using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{

    public class SPFullFee : BaseReference
    {
         private SP_FullFee_Result student;
         public SPFullFee(SP_FullFee_Result obj) { student = obj; }
         public long FeeId { get { return student.FeeId; } }
         public string FeeName { get { return student.Feename; } }
         public decimal? Amount { get { return student.Amount; } }
         public System.DateTime DueDate { get { return student.DueDate; } }
         public System.Guid? FeeGuid { get { return student.FeeGuid; } }
        public int? DiscountAllowed { get { return student.DiscountAllowed; } }
        public int IsStudentspecialFee { get { return student.StudentspecialFee; } }
        public int From { get { return student.From; } }
        public Nullable<long> BillNo { get { return student.BillNo; } }

    }
}
