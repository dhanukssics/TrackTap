using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{
    public class ExamSubjects : BaseReference
    {
        private tb_ExamSubjects examSubjects;
        public ExamSubjects(tb_ExamSubjects obj) { examSubjects = obj; }
        public ExamSubjects(long id) { examSubjects = _Entities.tb_ExamSubjects.FirstOrDefault(z => z.SubId == id); }
        public long SubId { get { return examSubjects.SubId; } }
        public long ExamId { get { return examSubjects.ExamId; } }
        public string Subject { get { return examSubjects.Subject; } }
        public decimal Mark { get { return examSubjects.Mark; } }
        public bool IsActive { get { return examSubjects.IsActive; } }
        public System.DateTime TimeStamp { get { return examSubjects.TimeStamp; } }
        public decimal InternalMarks { get { return examSubjects.InternalMarks; } }
        public decimal ExternalMark { get { return examSubjects.ExternalMark; } }
        public System.DateTime ExamDate { get { return examSubjects.ExamDate; } }
        public long SubjectId { get { return examSubjects.SubjectId; } }
    }
}
