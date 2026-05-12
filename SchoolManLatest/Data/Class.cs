using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{
    public class Class : BaseReference
    {
        private tb_Class schoolClass;
        public Class(tb_Class obj) { schoolClass = obj; }
        public Class(long id) { schoolClass = _Entities.tb_Class.FirstOrDefault(z => z.ClassId == id); }
        public long ClassId { get { return schoolClass.ClassId; } }
        public long SchoolId { get { return schoolClass.SchoolId; } }
        public string ClassName { get { return schoolClass.Class; } }
        public System.DateTime Timestamp { get { return schoolClass.Timestamp; } }
        public System.Guid ClassGuild { get { return schoolClass.ClassGuild; } }
        public bool IsActive { get { return schoolClass.IsActive; } }
        public bool PublishStatus { get { return schoolClass.PublishStatus; } }
        public long AcademicYearId { get { return schoolClass.AcademicYearId; } }
        public string AccademicYearName { get { return schoolClass.tb_AcademicYear.AcademicYear; } }

        public int ClassOrder { get { return schoolClass.ClassOrder; } }
        public List<Division> Division { get { return schoolClass.tb_Division.Where(z => z.IsActive).ToList().Select(z => new Division(z)).ToList(); } }


        public List<Student> GetStudentDetails()
        {
            return schoolClass.tb_Student.Where(z => z.IsActive).ToList().Select(q => new Student(q)).ToList();
        }

    }
}
