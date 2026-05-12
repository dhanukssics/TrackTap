using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{
    public class FeeClass : BaseReference
    {
        private tb_FeeClass feeClass;
        public FeeClass(tb_FeeClass obj) { feeClass = obj; }
        public FeeClass(long id) { feeClass = _Entities.tb_FeeClass.FirstOrDefault(z => z.FeeClassId == id); }
        public FeeClass(Guid Gid) { feeClass = _Entities.tb_FeeClass.FirstOrDefault(z => z.FeeClassGuid == Gid); }

        public long FeeClassId { get { return feeClass.FeeClassId; } }
        public long FeeId { get { return feeClass.FeeId; } }
        public decimal Amount { get { return feeClass.Amount; } }
        public long ClassId { get { return feeClass.ClassId; } }
        public System.DateTime Timestamp { get { return feeClass.TimeStamp; } }
        public System.DateTime DueDate { get { return feeClass.DueDate; } }

        public bool PublishStatus { get { return feeClass.PublishStatus; } }
        public bool IsActive { get { return feeClass.IsActive; } }
        public Fee FeeDetail { get { return new Fee(feeClass.tb_Fee); } }
        public Class ClassDetail { get { return new Class(feeClass.ClassId); } }


    }
}
