using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{
    public class Teacher:BaseReference
    {
         private tb_Teacher  teacher;
         public Teacher(tb_Teacher obj) { teacher = obj; }
         public Teacher(long id) { teacher = _Entities.tb_Teacher.FirstOrDefault(z => z.TeacherId == id); }
         public long TeacherId { get { return teacher.TeacherId; } }
         public string TeacherSpecialId { get { return teacher.TeacherSpecialId; } }
         public string TeacherName { get { return teacher.TeacherName; } }
         public long SchoolId { get { return teacher.SchoolId; } }
         public string ContactNumber { get { return teacher.ContactNumber; } }
         public string Email { get { return teacher.Email; } }
         public System.DateTime TimeStamp { get { return teacher.TimeStamp; } }
         public System.Guid TeacherGuid { get { return teacher.TeacherGuid; } }
         public bool IsActive { get { return teacher.IsActive; } }
         public string FilePath { get { return teacher.FilePath; } }
         public Nullable<bool> IsPermanent { get { return teacher.IsPermanent; } }
        //public List<TeacherClass> TeacherClass { get { return teacher.tb_TeacherClass.ToList().Select(z => new TeacherClass(z)).ToList(); } }
        public School SchoolData { get { return new School(teacher.tb_School); } }
         public List<TeacherClass> TeacherClass { get { return GetTeacherClassData(teacher); } }
         public List<TeacherClass> GetTeacherClassData(tb_Teacher teacher)
         {
             var teacherData = _Entities.tb_TeacherClass.Where(x => x.TeacherId == teacher.TeacherId).ToList().Select(x => new TeacherClass(x)).ToList();
             return teacherData;
         }
        public List<tb_TeacherFiles> TeacherFiles()
        {
            var data = teacher.tb_TeacherFiles.Where(x => x.IsActive).ToList();
            return data;
        }
        public Nullable<decimal> SalaryAmount  { get { return teacher.SalaryAmount; } }
        public Nullable<decimal> PFPercentage  { get { return teacher.PFPercentage; } }
        public Nullable<decimal> ESIPercentage { get { return teacher.ESIPercentage; } }
        public Nullable<long> UserType { get { return teacher.UserType ?? 0; } }


        public Nullable<System.DateTime> DOJ { get { return teacher.DOJ; } }

        //Basheer on 30-09-2019 for role module
        public Teacher(long id, int status) { teacher = _Entities.tb_Teacher.Where(z => z.UserId == id).FirstOrDefault(); }
        public List<Teacher> GetTeacherDetails(Int64 schoolid)
        {
            return _Entities.tb_Teacher.Where(z => z.IsActive && z.SchoolId==schoolid).ToList().Select(q => new Teacher(q)).OrderBy(x => x.TeacherName).ToList();
        }

        public List<UserModule> GetTearhersModules()
        {
            List<UserModule> list = new List<UserModule>();
            if (teacher.UserType != null)
            {
                var data = _Entities.tb_UserModuleDetails.Where(c => c.UserModuleId == teacher.UserType && c.IsActive).ToList();
                foreach (var item in data)
                {
                    var isactive = _Entities.tb_SchoolModuleDetails.Where(x => x.SchoolSubModuleId == item.SubModuleId && x.IsActive && x.SchoolId == teacher.SchoolId).Count();
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

        //Created by Gayathri A(23/01/2024)
        public List<TeacherAttendance> GetAttendance_Teacher(DateTime maxDate, DateTime minDate, int shift)
        {
            return _Entities.tb_AttendanceTeacher.Where(z => z.AttendanceDate >= minDate && z.AttendanceDate <= maxDate && z.ShiftStatus == shift).ToList().Select(z => new TeacherAttendance(z)).ToList();
        }

    }
}
