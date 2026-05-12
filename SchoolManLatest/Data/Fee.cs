using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{
    public class Fee : BaseReference
    {
        private tb_Fee fee;
        public Fee(tb_Fee obj) { fee = obj; }
        public Fee(long id) { fee = _Entities.tb_Fee.FirstOrDefault(z => z.FeeId == id); }
        public long FeeId { get { return fee.FeeId; } }
        public int? FeeType { get { return fee.FeeType; } }
        public string FeeName { get { return fee.FeesName; } }
        public long schoolId { get { return fee.SchoolId; } }
        public System.DateTime Timestamp { get { return fee.TimeStamp; } }
        public bool IsActive { get { return fee.IsActive; } }
        public DateTime? DueDate { get { return fee.DueDate; } }
        public decimal? FineAmount { get { return fee.FineAmount; } }
        public int? NoOfDays { get { return fee.NoOfDays; } }
    }
}
