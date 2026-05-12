using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{
    public class Division : BaseReference
    {
        private tb_Division division;
        public Division(tb_Division obj) { division = obj; }
        public Division(long id) { division = _Entities.tb_Division.FirstOrDefault(z => z.DivisionId == id); }
        public long DivisionId { get { return division.DivisionId; } }
        public long ClassId { get { return division.ClassId; } }
        public string ClassName { get { return division.tb_Class.Class; } }
        public string DivisionName { get { return division.Division; } }
        public System.DateTime TimeStamp { get { return division.TimeStamp; } }
        public System.Guid DivisionGuid { get { return division.DivisionGuid; } }
        public bool IsActive { get { return division.IsActive; } }
        public Class Class { get { return new Class(division.tb_Class); } }
        public int ClassOrder { get { return division.tb_Class.ClassOrder; } }
        public List<Student> GetStudentDetails()
        {
            return division.tb_Student.Where(z => z.IsActive && z.tb_Class.IsActive==true && z.tb_Class.PublishStatus==true).ToList().Select(q => new Student(q)).OrderBy(x=>x.StundentName).ToList();
        }

        public int GetStudentCount()
        {
            return division.tb_Student.Where(z => z.IsActive).Count();
        }
        public List<FeeDiscount> GetStudentDiscountList()
        {
            return division.tb_Student.SelectMany(z => z.tb_FeeDiscount).Where(z => z.IsActive).ToList().Select(q => new FeeDiscount(q)).ToList();
        }
        public List<FeeStudent> GetSpecialFeeStudentList(long feeId)
        {
            return division.tb_Student.SelectMany(z => z.tb_FeeStudent).Where(z => z.IsActive && z.FeeId == feeId).ToList().Select(q => new FeeStudent(q)).ToList();
        }
        public string getTeacherClass()
        {
            var data = division.tb_TeacherClass.ToList().Select(z => new Teacher(z.tb_Teacher)).FirstOrDefault();
            if (data != null)
                return data.TeacherName;
            else
                return string.Empty;
        }

        public List<Attendance> GetAttendance(DateTime maxDate, DateTime minDate, int shift)
        {
            return division.tb_Attendance.Where(z =>z.AttendanceDate >= minDate && z.AttendanceDate <= maxDate && z.ShiftStatus == shift).ToList().Select(z => new Attendance(z)).ToList();
        }



        public List<Attendance> GetAttendanceCount(int shift)
        {
            ///Testing 
            ///
            //string ssss = DateTime.Now.ToString();
            //string Maxdate = CurrentTime.Date.ToString("dd-MM-yyyy") + ' ' + "11:59:00 PM";
            //Temp comment this system onley...
            string Maxdate = CurrentTime.Date.ToString("MM-dd-yyyy") + ' ' + "11:59:00 PM";
            DateTime maxDate = Convert.ToDateTime(Maxdate);
            DateTime minDate= Convert.ToDateTime(CurrentTime.Date);
            return division.tb_Attendance.Where(z => z.AttendanceDate >= minDate && z.AttendanceDate <= maxDate && z.ShiftStatus == shift).ToList().Select(z => new Attendance(z)).ToList();
        }
    }
}
