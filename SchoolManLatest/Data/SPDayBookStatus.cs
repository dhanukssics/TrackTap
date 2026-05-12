using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.Data
{
    public class SPDayBookStatus : BaseReference
    {
        private SP_DayBookStatus_Result status;
        public SPDayBookStatus(SP_DayBookStatus_Result obj) { status = obj; }
        public decimal? OpeningBalance { get { return status.OpeningBalance; } }
        public decimal? ClosingBalance { get { return status.ClosingBalance; } }
        public decimal? TotalDebit { get { return status.TotalDebit; } }
        public decimal? TotalCredit { get { return status.TotalCredit; } }
    }
}
