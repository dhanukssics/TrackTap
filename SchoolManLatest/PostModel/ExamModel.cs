using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.ClassLibrary.PostModel
{
    public class ExamModel
    {
        public string SchoolId { get; set; }
        public string ClassId { get; set; }
        public string DivisionId { get; set; }
        public long ExamId { get; set; }
        public long SubjectId { get; set; }
        public long StudentId { get; set; }
    }

    public class Exams
    {
        public long ExamId { get; set; }
        public long SchoolId { get; set; }
        public long UserId { get; set; }
        public long ClassId { get; set; }
        public long DivisionId { get; set; }
        public string ExamName { get; set; }
        public System.DateTime ExamDate { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public List<ExamsSubjects> ExamsSubjectsLists { get; set; }

    }

    public class ExamsSubjects
    {
        public long SubId { get; set; }
        public long ExamId { get; set; }
        public string Subject { get; set; }
        public decimal Mark { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public decimal InternalMarks { get; set; }
        public decimal ExternalMark { get; set; }
        public System.DateTime ExamDate { get; set; }
        public long SubjectId { get; set; }
    }

    public class StudentMarks
    {
        public long MarkId { get; set; }
        public long StudentId { get; set; }
        public long ExamId { get; set; }
        public long SubjectId { get; set; }
        public long Mark { get; set; }
        public bool IsActive { get; set; }
        public string ContactNumber { get; set; }
        public string StundentName { get; set; }
        public string ParentName { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public Nullable<decimal> InternalMark { get; set; }
        public Nullable<decimal> ExternalMark { get; set; }
        public List<StudentMarks> StudentList { get; set; }
        public Students students { get; set; }

    }

    public class Students
    {
        public long StudentId { get; set; }
        public long SchoolId { get; set; }
        public string StudentSpecialId { get; set; }
        public string StundentName { get; set; }
        public string ParentName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ContactNumber { get; set; }
        public string ClasssNumber { get; set; }
        public long ClassId { get; set; }
        public long DivisionId { get; set; }
        public long BusId { get; set; }
        public string TripNo { get; set; }
        public string FilePath { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public Nullable<System.Guid> StudentGuid { get; set; }
        public bool IsActive { get; set; }
        public Nullable<long> ParentId { get; set; }
        public string State { get; set; }
        public string Gender { get; set; }
        public string BloodGroup { get; set; }
        public string ParentEmail { get; set; }
        public string PostalCode { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string Aadhaar { get; set; }
        public string BioNumber { get; set; }
        public Nullable<bool> IsSamrtPhoneUser { get; set; }
    }
    public class ViewStudentMark
    {
        public long MarkId { get; set; }
        public long StudentId { get; set; }
        public long ExamId { get; set; }
        public long SubjectId { get; set; }
        public long Mark { get; set; }
        public bool IsActive { get; set; }
        public string ContactNumber { get; set; }
        public string StundentName { get; set; }
        public string ParentName { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public Nullable<decimal> InternalMark { get; set; }
        public Nullable<decimal> ExternalMark { get; set; }

        public string ExamName { get; set; }
        public string SubjectName { get; set; }
    }

}
