using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{
    public class Attendance: BaseReference
    {
        private tb_Attendance attendance;
        public Attendance(tb_Attendance obj) { attendance = obj; }
        public Attendance(long id) { attendance = _Entities.tb_Attendance.FirstOrDefault(z => z.AttendanceId == id); }
        public long AttendanceId { get { return attendance.AttendanceId; } }
        public long StaffId { get { return attendance.StaffId; } }
        public long ClassId { get { return attendance.ClassId; } }
        public long DivisionId { get { return attendance.DivisionId; } }
        public System.DateTime AttendanceDate { get { return attendance.AttendanceDate; } }
        public string AttendanceDateOnly { get { return attendance.AttendanceDate.ToShortDateString(); } }
        public bool AttendanceData { get { return attendance.AttendanceData; } }     // True : Check In / False : Check Out 
        public System.DateTime TimeStamp { get { return attendance.TimeStamp; } }
        public System.Guid AttendanceGuid { get { return attendance.AttendanceGuid; } }
        public bool IsActive { get { return attendance.IsActive; } }
        public long StudentId { get { return attendance.StudentId; } }
        public int ShiftStatus { get { return attendance.ShiftStatus; } }  // 0 morning ,1 afternoon
        public string StundentName { get { return attendance.tb_Student.StundentName; } }
        public string StundentFilePath { get { return attendance.tb_Student.FilePath; } }

       
    }

}
