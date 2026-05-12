using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.Data
{
   public class Staff:BaseReference
    {
        private tb_Staff staff;
        public Staff(tb_Staff obj) { staff = obj; }
        public Staff(long id) { staff = _Entities.tb_Staff.FirstOrDefault(z => z.StaffId == id); }
        public long StaffId { get { return staff.StaffId; } }
        public long UserId { get { return staff.UserId; } }
        public string StaffName { get { return staff.StaffName; } }
        public string Contact { get { return staff.Contact; } }
        public string Address { get { return staff.Address; } }
        public System.DateTime DOB { get { return staff.DOB; } }
        public bool IsActive { get { return staff.IsActive; } }
        public System.DateTime TimeStamp { get { return staff.TimeStamp; } }

        public List<tb_StaffFileCollection> StaffFiles()
        {
            var data = staff.tb_StaffFileCollection.Where(x => x.IsActive).ToList();
            return data;
        }

        //Basheer on 30-09-2019 for role module

        public Staff(long id, int status) { staff = _Entities.tb_Staff.Where(z => z.UserId == id).FirstOrDefault(); }
        public List<UserModule> GetStaffModules(long schoolid)
        {
            List<UserModule> list = new List<UserModule>();
            if (staff.UserType != null)
            {
                var data = _Entities.tb_UserModuleDetails.Where(c => c.UserModuleId == staff.UserType && c.IsActive).ToList();
                foreach (var item in data)
                {
                    var isactive = _Entities.tb_SchoolModuleDetails.Where(x => x.SchoolSubModuleId == item.SubModuleId && x.IsActive && x.SchoolId == schoolid).Count();
                    if (isactive > 0)
                    {
                        UserModule one = new UserModule();
                        one.MainId = item.MainId;
                        one.SubId = item.SubModuleId;
                        list.Add(one);
                    }
                }
            }
            return list;
        }
    }
}
