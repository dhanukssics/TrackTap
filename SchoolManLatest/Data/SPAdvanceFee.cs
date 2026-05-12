using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.Data
{

    public class SPAdvanceFee : BaseReference
    {
        private SP_AdvanceFee_Result student;
        public SPAdvanceFee(SP_AdvanceFee_Result obj) { student = obj; }
        public long FeeId { get { return student.FeeId; } }
        public string FeeName { get { return student.Feename; } }
        public decimal Amount { get { return student.Amount; } }
        public System.DateTime DueDate { get { return student.DueDate; } }
        public System.Guid FeeGuid { get { return student.FeeGuid; } }

    }
}
