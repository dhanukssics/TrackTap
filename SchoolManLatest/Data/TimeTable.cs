using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{
    public class TimeTable : BaseReference
    {
        private tb_TimeTable table;
        public TimeTable(tb_TimeTable obj) { table = obj; }
        public TimeTable(long id) { table = _Entities.tb_TimeTable.FirstOrDefault(z => z.Id == id); }
        public long Id { get { return table.Id; } }
        public long SchoolId { get { return table.SchoolId; } }
        public long ClassId { get { return table.ClassId; } }
        public long DivisionId { get { return table.DivisionId; } }
        public long TeacherId { get { return table.TeacherId; } }
        public long SubjectId { get { return table.SubjectId; } }
        public bool IsActive { get { return table.IsActive; } }
        public int DayId { get { return table.DayId; } }
        public int Periods { get { return table.Periods; } }
        public string ClassName { get { return table.tb_Class.Class; } }
        public string DivisionName { get { return table.tb_Division.Division; } }
        public string Teacher { get { return table.tb_Teacher.TeacherName; } }
        public string Subject { get { return table.tb_Subjects.SubjectName; } }
        public string DayName { get { return Day(); } }
        public string Day()
        {
            string Day = "";
            if (table.DayId == 0)
                Day = "Monday";
            else if (table.DayId == 1)
                Day = "Tuesday";
            else if (table.DayId == 2)
                Day = "Wednesday";
            else if (table.DayId == 3)
                Day = "Thursday";
            else if (table.DayId == 4)
                Day = "Friday";
            if (table.DayId == 5)
                Day = "Saturday";

            return Day;
        }
    }
}
