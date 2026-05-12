using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.Data
{
    public class Login:BaseReference
    {
         private tb_Login login;
         public Login(tb_Login obj) { login = obj; }
         public Login(long id) { login = _Entities.tb_Login.FirstOrDefault(z => z.UserId == id); }
         public long UserId { get { return login.UserId; } }
         public long SchoolId { get { return login.SchoolId; } }
         public int RoleId { get { return login.RoleId; } }
         public string Name { get { return login.Name; } }
         public string Username { get { return login.Username; } }
         public string Password { get { return login.Password; } }
         public bool IsActive { get { return login.IsActive; } }
         public System.DateTime TimeStamp { get { return login.TimeStamp; } }
         public bool DisableStatus { get { return login.DisableStatus; } }
         public System.Guid LoginGuid { get { return login.LoginGuid; } }
         public School School { get { return new School(login.tb_School); } }


        public List<tb_UserAllotedMenu> GetUserMenuList()
        {
            var data = login.tb_UserAllotedMenu.ToList().OrderBy(z=>z.tb_MenuList.OrderValue).ToList();
            return data;
        }
    }
}
