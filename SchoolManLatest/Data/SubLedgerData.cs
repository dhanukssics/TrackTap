using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{
  public  class SubLedgerData: BaseReference
    {
        private tb_SubLedgerData subLedgerData;
        public SubLedgerData(tb_SubLedgerData obj) { subLedgerData = obj; }
        public SubLedgerData(long id) { subLedgerData = _Entities.tb_SubLedgerData.FirstOrDefault(z => z.LedgerId == id); }
        public long LedgerId { get { return subLedgerData.LedgerId; } }
        public string SubLedgerName { get { return subLedgerData.SubLedgerName; } }
        public long AccHeadId { get { return subLedgerData.AccHeadId; } }
        public bool IsActive { get { return subLedgerData.IsActive; } }
        public System.DateTime TimeStamp { get { return subLedgerData.TimeStamp; } }
        public string AccountHeadName { get { return subLedgerData.tb_AccountHead.AccHeadName; } }

       
    }
}
