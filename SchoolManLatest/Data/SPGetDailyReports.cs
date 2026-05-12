using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.Data
{
    public class SPGetDailyReports:BaseReference
    {
        private SP_GetDaily_Report_Result bill;
         public SPGetDailyReports(SP_GetDaily_Report_Result obj) { bill = obj; }
         public long StudentId { get { return bill.StudentId; } }
        public long? BillNo { get { return bill.BillNo; } }
        public long ClassId { get { return bill.ClassId; } }
         public decimal? Amount { get { return bill.Amount; } }
         public System.DateTime? Date { get { return bill.Date; } }
         public Student Student { get { return new Student(bill.StudentId); } }

    }
}
