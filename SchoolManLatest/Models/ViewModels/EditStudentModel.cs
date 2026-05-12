using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrackTap.Models
{
  
    public class EditStudentModel
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
        //[RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        public string studentName { get; set; }
        public string division { get; set; }
        public string rollNo { get; set; }
        public string className { get; set; }
        //[StringLength(10, ErrorMessage = "Number cannot be longer than 10 characters.")]
        //[StringLength(10, MinimumLength = 10, ErrorMessage = "Number must be 10 digit")]
        //[MaxLength(10, ErrorMessage = "Number cannot be longer than 10 characters"), MinLength(10, ErrorMessage = "Number cannot be less than 10 characters")]

        //[RegularExpression("^[0-9]*$", ErrorMessage = "Contact must be numeric")]
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
    }

}