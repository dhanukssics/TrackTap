using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{
    public class ResetPassword:BaseReference
    {
        private tb_ResetPassword resetPassword;
        public ResetPassword(tb_ResetPassword obj) { resetPassword = obj; }
        public ResetPassword(long id) { resetPassword = _Entities.tb_ResetPassword.FirstOrDefault(z => z.ResetPasswordId == id); }
        public long ResetPasswordId { get { return resetPassword.ResetPasswordId; } }
        public bool LinkExpireStatus { get { return resetPassword.LinkExpireStatus; } }
        public long UserId { get { return resetPassword.UserId; } }
        public System.Guid UserGuid { get { return resetPassword.UserGuid; } }
        public bool IsActive { get { return resetPassword.IsActive; } }
        public System.DateTime TimeStamp { get { return resetPassword.TimeStamp; } }
    }
}
