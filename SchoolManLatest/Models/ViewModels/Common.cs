using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TrackTap.ClassLibrary;

namespace TrackTap.Models
{
    public class Common
    {
    }
    public class SchoolValue
    {
        public long schoolId { get; set; }
        public string className { get; set; }
        public long classId { get; set; }
        public long divId { get; set; }
        public string Password { get; set; }
    }
    public class FilterModel
    {
        public long schoolId { get; set; }
        public string ClassName { get; set; }
        public string DivName { get; set; }
        public long FeeId { get; set; }

    }
    public class SchoolData
    {
        public SchoolValue value { get; set; }
        public TrackTap.Data.School Data { get; set; }
    }
    public class AddFee
    {
        public long SchoolId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string FeeName { get; set; }
        public int FeeType { get; set; }
        public bool CheckInterval { get; set; }
        public int Interval { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DueDateString { get; set; }
        public string EndDateString { get; set; }
        public int IsReccuring { get; set; }
        public int IsDueDate { get; set; }
        public DateTime HaveFineDate { get; set; }//New
        public string HaveFineDateString { get; set; }//New
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid Amount")]
        public decimal FineAmount { get; set; }//New
        [RegularExpression("^[0-9]*$", ErrorMessage = "FineDays must be numeric")]
        public int FineDays { get; set; }//New
    }
    public class AddClassFees
    {
        public long SchoolId { get; set; }


        //[Required(ErrorMessage = "Required")]
        public string FeeId { get; set; }
        public string DataList { get; set; }
        public int Interval { get; set; }
        public DateTime DueDate { get; set; }
    }
    public class ViewFeeClass
    {
        public long SchoolId { get; set; }
        public long FeeId { get; set; }
    }
    public class Datalist
    {
        public long classId { get; set; }
        public long feeStudentId { get; set; }
        public long divisionId { get; set; }
        public string amount { get; set; }
    }
    public class FeeDetails
    {
        public long feeStudentId { get; set; }
        public Decimal amount { get; set; }
    }
    public class Datavalue
    {
        public List<Datalist> list { get; set; }
    }
    public class ListFee
    {
        public long schoolId { get; set; }
    }
    public class EditFee
    {
        //public long schoolId { get; set; }
        public long feeId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string feename { get; set; }
        public int? feeType { get; set; }
    }
    public class FeeclassList
    {
        public long schoolId { get; set; }
        public long feeId { get; set; }
    }
    public class FeeBilling
    {
        public long feeId { get; set; }
        public decimal amount { get; set; }
        public string feeName { get; set; }
        public long studentFeeId { get; set; }


        public DateTime timeStamp { get; set; }
    }
    public class EditStudent
    {
        public string BusId { get; set; }
        public long schoolId { get; set; }

        [Required(ErrorMessage = "Class Required")]
        public long classId { get; set; }
        public long studentId { get; set; }
        [Required(ErrorMessage = "Division Required")]
        public long divisionId { get; set; }
        [Required(ErrorMessage = "Name Required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        public string studentName { get; set; }
        public string division { get; set; }
        public string rollNo { get; set; }
        public string className { get; set; }
        [StringLength(10, ErrorMessage = "Number cannot be longer than 10 characters.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Contact must be numeric")]
        public string contactNo { get; set; }
        [Required(ErrorMessage = "Parent Name Required")]
        public string parentName { get; set; }
        public string tripNumber { get; set; }
        public string address { get; set; }
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string state { get; set; }
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string city { get; set; }
        public string classInCharge { get; set; }
        public string profilePic { get; set; }
        public string filePath { get; set; }
    }
    public class EditFeeClass
    {
        public long feeId { get; set; }
        public long feeClassId { get; set; }
        public string classname { get; set; }
        public string amount { get; set; }
        public DateTime DueDate { get; set; }

    }
    public class DiscountDetails
    {
        public long StudentId { get; set; }
    }
    public class TeacherAddModel
    {
        public long schoolId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string teacherName { get; set; }
        public string classId { get; set; }
        public string classs { get; set; }
        public string divisionId { get; set; }
        [Required(ErrorMessage = "Required")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Contact must be numeric")]
        public string contactNumber { get; set; }
        [Required(ErrorMessage = "Required")]
        [EmailAddress]
        public string emailId { get; set; }
        public string image { get; set; }
        public string filePath { get; set; }
        public string SalaryType { get; set; }
        public decimal SalaryAmount { get; set; }
        public decimal PFPercentage { get; set; }
        public decimal ESIPercentage { get; set; }
        public bool IsPermanent { get; set; }
     
        public long? UserTypeId { get; set; }
        //[Required(ErrorMessage = "DOJ Required")]
        public DateTime DOJ { get; set; }
        [Required(ErrorMessage = "Required")]
        public String DOJstring { get; set; }
    }

    public class TeacherEditModel
    {
        public long schoolId { get; set; }
        public long teacherId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string teacherName { get; set; }
        public string classId { get; set; }
        public string classs { get; set; }
        public string divisionId { get; set; }
        [Required(ErrorMessage = "Required")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Contact must be numeric")]
        public string contactNumber { get; set; }
        [Required(ErrorMessage = "Required")]
        [EmailAddress]
        public string emailId { get; set; }
        public string image { get; set; }
        public string filePath { get; set; }
        public string SalaryType { get; set; }
        public decimal SalaryAmount { get; set; }
        public decimal PFPercentage { get; set; }
        public decimal ESIPercentage { get; set; }
        public bool IsPermanent { get; set; }
        public long? UserTypeId { get; set; }
        [Required(ErrorMessage = "DOJ Required")]
        public DateTime DOJ { get; set; }

        public String DOJstring { get; set; }
    }

    public class SchoolId
    {
        public long schoolId { get; set; }
    }
    public class TeacherId
    {
        public long teacherId { get; set; }
    }
    public class PrintBill
    {
        public long studentId { get; set; }
        public long billNumber { get; set; }
        public DateTime CurrentDate { get; set; }

        public long Amount { get; set; }
        public long UserId { get; set; }//jibin 9/27/2020
        public string SalesId { get; set; }//jibin 9/27/2020
    }
    public class CircularList
    {
        public long schoolId { get; set; }
    }
    public class TrialBalanceModel
    {
        public DateTime Today { get; set; }
        public DateTime StartDate { get; set; }
        public long SchoolId { get; set; }
    }
    public class UnPublishedClass
    {
        public long ClassId { get; set; }
        public string ClassName { get; set; }
        public long DivisionId { get; set; }
        public string DivisionName { get; set; }
        public string AccademicYear { get; set; }
        public bool CurrentYearStatus { get; set; }
        public int SlNo { get; set; }
    }
    public class UpPublishedClassList
    {
        public long SchoolId { get; set; }
        public List<UnPublishedClass> list { get; set; }
    }
    public class PromoteStudents
    {
        public long SchoolId { get; set; }
        public long OldClassId { get; set; }
        public long OldDivId { get; set; }
        public long OldAcademicyearId { get; set; }
        public long NewCLassId { get; set; }
        public long NewDivId { get; set; }
        public long NewAcademicYearId { get; set; }
        public List<StudentListForPromote> StudentList { get; set; }
        public string StudentListString { get; set; }
    }
    public class StudentListForPromote
    {
        public int SlNo { get; set; }
        public long StudentId { get; set; }
        public string StudentName { get; set; }
        public string SpecialId { get; set; }
        public string ContactNumber { get; set; }
    }
    public class StudentFileUploadModel
    {
        public long StudentId { get; set; }
        public StudentFileType TypeId { get; set; }

        public DateTime ReceivedDate { get; set; }
        [Required(ErrorMessage = "Required")]
        public string ReceivedDateString { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Required")]
        public string FilePath { get; set; }
    }
    public class SingleTeacherDetails
    {
        public long TeacherId { get; set; }
        public string TeacherName { get; set; }
        public string EmailId { get; set; }
        public string ContactNo { get; set; }
        public string ClassDivision { get; set; }
        public string SpecialId { get; set; }
    }
    public class TeacherFileUploadModel
    {
        public long TeacherId { get; set; }
        public StudentFileType TypeId { get; set; }

        public DateTime ReceivedDate { get; set; }
        [Required(ErrorMessage = "Required")]
        public string ReceivedDateString { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Required")]
        public string FilePath { get; set; }
    }
    public class SingleStaffDetails
    {
        public long StaffId { get; set; }
        public string StaffName { get; set; }
        public string EmailId { get; set; }
        public string ContactNo { get; set; }
        public string SpecialId { get; set; }
    }
    public class StaffFileUploadModel
    {
        public long StaffId { get; set; }
        public StudentFileType TypeId { get; set; }

        public DateTime ReceivedDate { get; set; }
        [Required(ErrorMessage = "Required")]
        public string ReceivedDateString { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Required")]
        public string FilePath { get; set; }
    }
    public class DatalistCommon
    {
        public long classId { get; set; }
        public long feeStudentId { get; set; }
        public long divisionId { get; set; }
        public string amount { get; set; }
        public DateTime dueDate { get; set; }
        public decimal oldAmount { get; set; }
        public long StudentId { get; set; }
    }
}
