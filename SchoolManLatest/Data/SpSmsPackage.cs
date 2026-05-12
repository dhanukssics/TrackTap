using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.Data
{
    public class SpSmsPackage : BaseReference
    {
        private SP_GetSmsPackage_Result msg;
        public SpSmsPackage(SP_GetSmsPackage_Result obj) { msg = obj; }
        public long PackageId { get { return msg.PackageId; } }
        public long SchoolId { get { return msg.SchoolId; } }
        public System.DateTime FromDate { get { return msg.FromDate; } }
        public System.DateTime ToDate { get { return msg.ToDate; } }
        public long AllowedSms { get { return msg.AllowedSms; } }
        public decimal SmsRate
        {
            get
            {
                string rat = String.Format("{0:0.00}", msg.SmsRate);
                return Convert.ToDecimal(rat);
            }
        }
        public bool IsActive { get { return msg.IsActive; } }
        public bool IsDisabled { get { return msg.IsDisabled; } }
        public long ExtraSmsCount
        {
            get
            {
                long smsCount = 0;
                long extraSms = 0;
                var count = _Entities.Sp_SmsTotalCount(msg.FromDate, msg.ToDate).ToList().Where(z => z.ScholId == msg.SchoolId).FirstOrDefault();
                if (count != null)
                {
                    smsCount = count.Count ?? 0;
                }
                extraSms = smsCount - msg.AllowedSms;
                if (extraSms > 0)
                {
                    return extraSms;
                }
                else
                {
                    return 0;
                };
            }
        }



        public decimal ExtraAmount
        {
            get
            {
                return SmsRate * ExtraSmsCount;
            }
        }


    }
}
