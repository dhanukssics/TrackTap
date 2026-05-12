using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.Data
{
   public class SPMonthlyAttendanceTeacher:BaseReference
    {
        public sp_MonthlyAttendanceTeacher_Result month;
      
        public SPMonthlyAttendanceTeacher(sp_MonthlyAttendanceTeacher_Result obj) { month = obj; }
        //public long ClassId { get { return month.ClassId; } }
        //public long DivisionId { get { return month.DivisionId; } }
        public string TeacherName { get { return month.TeacherName; } }
        public Nullable<int> C1 { get { return month.C1; } }
        public Nullable<int> C2 { get { return month.C2; } }
        public Nullable<int> C3 { get { return month.C3; } }
        public Nullable<int> C4 { get { return month.C4; } }
        public Nullable<int> C5 { get { return month.C5; } }
        public Nullable<int> C6 { get { return month.C6; } }
        public Nullable<int> C7 { get { return month.C7; } }
        public Nullable<int> C8 { get { return month.C8; } }
        public Nullable<int> C9 { get { return month.C9; } }
        public Nullable<int> C10 { get { return month.C10; } }
        public Nullable<int> C11 { get { return month.C11; } }
        public Nullable<int> C12 { get { return month.C12; } }
        public Nullable<int> C13 { get { return month.C13; } }
        public Nullable<int> C14 { get { return month.C14; } }
        public Nullable<int> C15 { get { return month.C15; } }
        public Nullable<int> C16 { get { return month.C16; } }
        public Nullable<int> C17 { get { return month.C17; } }
        public Nullable<int> C18 { get { return month.C18; } }
        public Nullable<int> C19 { get { return month.C19; } }
        public Nullable<int> C20 { get { return month.C20; } }
        public Nullable<int> C21 { get { return month.C21; } }
        public Nullable<int> C22 { get { return month.C22; } }
        public Nullable<int> C23 { get { return month.C23; } }
        public Nullable<int> C24 { get { return month.C24; } }
        public Nullable<int> C25 { get { return month.C25; } }
        public Nullable<int> C26 { get { return month.C26; } }
        public Nullable<int> C27 { get { return month.C27; } }
        public Nullable<int> C28 { get { return month.C28; } }
        public Nullable<int> C29 { get { return month.C29; } }
        public Nullable<int> C30 { get { return month.C30; } }
        public Nullable<int> C31 { get { return month.C31; } }
    }
}
