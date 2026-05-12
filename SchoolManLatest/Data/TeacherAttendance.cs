using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{
  public  class TeacherAttendance : BaseReference
    {
        public tb_AttendanceTeacher teachattend;
        public TeacherAttendance(tb_AttendanceTeacher obj) { teachattend = obj; }
        public TeacherAttendance(long id) { teachattend = _Entities.tb_AttendanceTeacher.FirstOrDefault(z => z.Attendance_Id == id); }
        public long AttendanceId { get { return teachattend.Attendance_Id; } }
        //public long StaffId { get { return attendance.StaffId; } }
        //public long ClassId { get { return attendance.ClassId; } }
        //public long DivisionId { get { return attendance.DivisionId; } }
        public System.DateTime AttendanceDate { get { return teachattend.AttendanceDate; } }
        public string AttendanceDateOnly { get { return teachattend.AttendanceDate.ToShortDateString(); } }
        public bool AttendanceData { get { return teachattend.AttendanceData; } }     // True : Check In / False : Check Out 
        public System.DateTime TimeStamp { get { return teachattend.TimeStamp; } }
        public System.Guid AttendanceGuid { get { return teachattend.AttendanceGuid; } }
        public bool IsActive { get { return teachattend.IsActive; } }
        public long TeacherId { get { return teachattend.TeacherId; } }
        public int ShiftStatus { get { return teachattend.ShiftStatus; } }  // 0 morning ,1 afternoon
        public string TeacherName { get { return teachattend.tb_Teacher.TeacherName; } }
        public string TeacherFilePath { get { return teachattend.tb_Teacher.FilePath; } }
    }
}
