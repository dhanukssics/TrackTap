using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{
    public class SPReceiptPayment : BaseReference
    {
        private sp_ReceiptPayment_Result rp;
        public SPReceiptPayment(sp_ReceiptPayment_Result obj) { rp = obj; }
        public string AccHeadName { get { return rp.AccHeadName; } }
        public int BillNo { get { return rp.BillNo??0; } }
        public int FromData { get { return rp.FromData; } }
        public Nullable<long> Id { get { return rp.Id; } }
        public Nullable<decimal> Receipt_ { get { return rp.Receipt; } }
        public Nullable<decimal> Payment_ { get { return rp.Payment; } }
    }

}
