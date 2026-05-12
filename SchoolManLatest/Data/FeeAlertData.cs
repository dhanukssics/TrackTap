using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.Data
{
    public class FeeAlertData:BaseReference
    {
        private tb_FeeAlertData feeAlertData;
        public FeeAlertData(tb_FeeAlertData obj) { feeAlertData = obj; }
        public FeeAlertData(long id) { feeAlertData = _Entities.tb_FeeAlertData.FirstOrDefault(z => z.Id == id); }
        public long Id { get { return feeAlertData.Id; } }
        public long SchoolId { get { return feeAlertData.SchoolId; } }
        public DateTime AlertDate { get { return feeAlertData.AlertDate; } }
        public bool IsActive { get { return feeAlertData.IsActive; } }
        public DateTime? TimeStamp { get { return feeAlertData.TimeStamp; } }
        

       
    }
}
