using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{

    public class SPEditFeeBilling : BaseReference
    {
         private SP_EditFullFee_Result student;
         public SPEditFeeBilling(SP_EditFullFee_Result obj) { student = obj; }
         public long StudentFeeId { get { return student.StudentFeeId; } }
         public long FeeId { get { return student.FeeId; } }
         public string FeeName { get { return student.Feename; } }
         public decimal Amount { get { return student.Amount; } }
         public System.DateTime DueDate { get { return student.DueDate; } }

         public System.Guid FeeGuid { get { return student.FeeGuid; } }

    }
}
