using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.Data
{
    public class SmsHistory : BaseReference
    {
        private tb_SmsHistory sms;
        public SmsHistory(tb_SmsHistory obj) { sms = obj; }
        public SmsHistory(long id) { sms = _Entities.tb_SmsHistory.FirstOrDefault(z => z.Id == id); }
        public long Id { get { return sms.Id; } }
        public long StuentId { get { return sms.StuentId; } }
        public string MessageContent { get { return sms.MessageContent; } }
        public string DelivaryStatus { get { return sms.DelivaryStatus; } }
        public DateTime? MessageDate { get { return sms.MessageDate; } }
        public Nullable<bool> IsActive { get { return sms.IsActive; } }
        public string SendStatus { get { return sms.SendStatus; } }
        public string StuedentName { get { return sms.tb_Student.StundentName; } }
        public string StudentDivision { get { return sms.tb_Student.tb_Division.Division; } }
        public string StudentClass { get { return sms.tb_Student.tb_Class.Class; } }
        public int? SmsPerStudent { get { return sms.SmsSentPerStudent; } }
        public string MessageReturnId { get { return sms.MessageReturnId; } }
    }

}
