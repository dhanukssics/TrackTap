using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{
    public class SPEditCommonFeeBilling:BaseReference
    {
        private sp_CommonFeeList_Result student;
        public SPEditCommonFeeBilling(sp_CommonFeeList_Result obj) { student = obj; }
        public long FeeId { get { return student.FeeId; } }
        public decimal? Amount { get { return student.Amount; } }
        public System.Guid FeeGuid { get { return student.FeeGuid; } }
        public string Feename { get { return student.Feename; } }
        public System.DateTime DueDate { get { return student.DueDate; } }
        public int DiscountAllowed { get { return student.DiscountAllowed; } }
        public int StudentspecialFee { get { return student.StudentspecialFee; } }
        public int From { get { return student.From; } }
        public int BillNo { get { return student.BillNo; } }
    }
}
