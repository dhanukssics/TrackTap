using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TrackTap.ClassLibrary;

namespace TrackTap.Models
{
    public class StudentModel
    {
        public string BusId { get; set; }
        public long schoolId { get; set; }
        public string stringStudentId { get; set; }

        [Required(ErrorMessage = "Class Required")]
        public long classId { get; set; }
        public long studentId { get; set; }
        [Required(ErrorMessage = "Division Required")]
        public long divisionId { get; set; }
        public long toDivisionId { get; set; }

        [Required(ErrorMessage = "Admission No Required")]
        public string admissionNo { get; set; }
        [Required(ErrorMessage = "Name Required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        public string studentName { get; set; }
        public string division { get; set; }
        public string rollNo { get; set; }
        public string className { get; set; }
        //[StringLength(10, ErrorMessage = "Number cannot be longer than 10 characters.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Number must be 10 digit")]
        //[MaxLength(10, ErrorMessage = "Number cannot be longer than 10 characters"), MinLength(10, ErrorMessage = "Number cannot be less than 10 characters")]

        [RegularExpression("^[0-9]*$", ErrorMessage = "Contact must be numeric")]
        [Required(ErrorMessage = "ContactNo Required")]
        public string contactNo { get; set; }
        [Required(ErrorMessage = "Parent Name Required")]
        public string parentName { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Address")]

      
        public string parentEmail { get; set; }

        public string tripNumber { get; set; }
        public string address { get; set; }
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        public string state { get; set; }
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        public string city { get; set; }
        public string classInCharge { get; set; }
        public string profilePic { get; set; }
        public string filePath { get; set; }
        [Required(ErrorMessage = "Gender Required")]
        public string gender { get; set; }
        public string bloodGroup { get; set; }
        //[Required(ErrorMessage = "DOB Required")]
        public DateTime DOB { get; set; }
        //[Required(ErrorMessage = "DOB Required")]
        public String DOBstring { get; set; }
        public string CurrentDate { get; set; }
        public string biometricId { get; set; }

        public string AcademinYear { get; set; }
        public string SchoolName { get; set; }
        public string Data { get; set; }
        public string IsSmartPhoneUser { get; set; }
        public long StudentIdAfterSave { get; set; }
        public long CurrentStudentId { get; set; }
    }
   



    public class StudentDetailedView
    {
        public long Id { get; set; }
        public long StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentClassDivision { get; set; }

        [Required(ErrorMessage = "Guardian Name Required")]
        public string GuardianName { get; set; }
        public string RelationShip { get; set; }
        public string Occupation { get; set; }
        public string Guardian_Address { get; set; }
        public string Contact_LandLine { get; set; }
        public string Contact_OfficeLine { get; set; }
        public string Previous_SchoolName { get; set; }
        public string Previous_Standard { get; set; }
        public Nullable<System.DateTime> Previous_DateOfAdmission { get; set; }
        public string Previous_DateOfAdmissionString { get; set; }
        public Nullable<System.DateTime> Previous_DateOfLeaving { get; set; }
        public string Previous_DateOfLeavingString { get; set; }
        public string TC_Filepath { get; set; }
        public string TC_Number { get; set; }
        public string TC_Date { get; set; }
        public string PlaceOFBirth { get; set; }
        public string DOBCertificate_FilePath { get; set; }
        public Nullable<long> ReligionId { get; set; }
        public Nullable<long> CategoryId { get; set; }
        public string Caste { get; set; }
        public Nationality NationalityId { get; set; }
        public string MotherTongue { get; set; }
        public string PermanentBodyMark1 { get; set; }
        public string PermanentBodyMark2 { get; set; }
        public string BoardingPoint { get; set; }
        public string KnownLanguage1 { get; set; }
        public string KnownLanguage2 { get; set; }
        public string Taluk { get; set; }
        public string Revenue_District { get; set; }
        public PanchayathirajSystem PanchayatiRajSystemId { get; set; }
        public string DistrictPanchayath { get; set; }
        public string BlockPanchayath { get; set; }
        public InstructionMedium InstructionMediumId { get; set; }
        public FirstLanguagePaper1 FirstLanguagePaper1 { get; set; }
        public FirstLanguagePaper2 FirstLanguagePaper2 { get; set; }
        public ThirdLanguage ThirdLanguage { get; set; }
        public bool IsVaccinated { get; set; }
        public Nullable<System.DateTime> VaccinatedDate { get; set; }
        public string VaccinatedDateString { get; set; }
        public LearningDisability LearingDisabilityId { get; set; }
        public EconomicalStatus EconomicalStatus { get; set; }
    }

    public class ParentDetailsView
    {
        public long StudentId { get; set; }
        public string StudentName { get; set;}
        public string StudentClassDivision { get; set; }
        public long ParentId { get; set; }
        [Required(ErrorMessage = "Parent Name Required")]
        public string ParentName { get; set; }
        [Required(ErrorMessage = "Address Required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "City Required")]
        public string City { get; set; }
        [Required(ErrorMessage = "Email Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [RegularExpression("^[0-9]*$", ErrorMessage = "Contact must be numeric")]
        [Required(ErrorMessage = "ContactNumber Required")]
        public string ContactNumber { get; set; }
        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "State Required")]
        public string State { get; set; }
        public string FilePath { get; set; }
        [Required(ErrorMessage = "MotherName Required")]
        public string MotherName { get; set; }
        [Required(ErrorMessage = "Gender Required")]
        public long SchoolId { get; set; }
        [Required(ErrorMessage = "PostelCode Required")]
        public string PostelCode { get; set; }
        public bool IsExists { get; set; }
    }
}