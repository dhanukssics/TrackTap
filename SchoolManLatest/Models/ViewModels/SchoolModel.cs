using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrackTap.Models
{
    public class SchoolModel
    {
        public string TeacherName;

        public long schoolId { get; set; }
        public long classId { get; set; }
        public long studentId { get; set; }
        public long teacherId { get; set; }
        public long busId { get; set; }
        public DateTime attendanceDate { get; set; }
        public DateTime curredntDateTime { get; set; }

        public int shiftStatus { get; set; }
        public long userId { get; set; }

        public long divisionId { get; set; }
        public string studentName { get; set; }
        public string division { get; set; }
        public string rollNo { get; set; }
        public string className { get; set; }
        public string classInCharge { get; set; }
        public string classNumber { get; set; }//Archana
        public List<Student> studentList { get; set; }
        public DateTime Selecteddate { get; set; }
        public string Selecteddate_From { get; set; }
        public string Selecteddate_To { get; set; }
        public HttpPostedFileBase fileData { get; set; }
        public int opr { get; set; }
        public int month { get; set; }
        public int year { get; set; }

        public string DivisionStringId { get; set; }
        public long CurrentAddedStudent { get; set; }

        public string StudentSpecialId { get; set; }//   9/24/2020 jibin

        public static implicit operator SchoolModel(SchoolModel v)
        {
            throw new NotImplementedException();
        }
    }
    public class Student
    {
        public string studentId { get; set; }
        public string attendaneStatus { get; set; } //Check In=1 Check Out =0;

    }
    public class SchoolModelForId
    {
        public long SchoolId { get; set; }
        public string Numbers { get; set; }
        public string StudentId { get; set; }
        public string Description { get; set; }
        public string Data { get; set; }
        public int MessageSentPerStudent { get; set; }
        public bool SmartPhoneUser { get; set; }
    }

    public class SchoolModelForIdSupreAdmin
    {
        public long SchoolId { get; set; }
        public string Numbers { get; set; }
        public string StudentId { get; set; }
        public string Description { get; set; }
        public string Data { get; set; }
        public int MessageSentPerStudent { get; set; }
        public bool SmartPhoneUser { get; set; }

        public string SchoolNames { get; set; }

    }

    public class IncomeData
    {
        public string HeadValue { get; set; }
        public string ParticularValue { get; set; }
        public string Amount { get; set; }
        public string SelectedDate { get; set; }
        public long Id { get; set; }
    }
    public class SendMessage
    {
        public string Description { get; set; }
        public string Phone { get; set; }
        public List<PhoneNumbers> list { get; set; }
    }
    public class PhoneNumbers
    {
        public string Number { get; set; }
        public string StudentId { get; set; }
        public string SendStatus { get; set; }
    }
    public class DateClass
    {
        public DateTime Selecteddate { get; set; }
        public long SchoolId { get; set; }
    }
    public class SendStaffMessage
    {
        public string Description { get; set; }
        public string Phone { get; set; }
        public List<StaffPhoneNumbers> list { get; set; }
    }
    public class StaffPhoneNumbers
    {
        public string Number { get; set; }
        public string StaffId { get; set; }
        public string SendStatus { get; set; }
        public string Type { get; set; }
    }
    //for email by basheer

    public class SchoolModelForEmail
    {
        public long SchoolId { get; set; }
        public string Email { get; set; }
        public string StudentId { get; set; }
        public string Parentname { get; set; }
        public string Description { get; set; }
        public string Data { get; set; }
        public int MessageSentPerStudent { get; set; }
       
    }

    public class SendEmail
    {
        public string Description { get; set; }
        public string Email { get; set; }
        public List<Emailsids> list { get; set; }
    }
    public class Emailsids
    {
        public string Email { get; set; }
        public string StudentId { get; set; }
        public string SendStatus { get; set; }
    }

    //for email by basheer

    //for accound handling by basheer

    public class AccountHide
    {
        public long SchoolId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }

        public long VoucherType { get; set; }
        public string SchoolName { get; set; }
        public string Heading { get; set; }

        public int IsContra { get; set; } 

        public string SelectedData { get; set; }

        public string accounthidedetails { get; set; }

        public string startdate { get; set; }
        public string endingdate { get; set; }

    }
    public class AccountHideDetails
    {
        public DateTime EnterDate { get; set; }
        public string VoucherNo { get; set; }
        public string AccountHeadName { get; set; }
        public string SubLedger { get; set; }
        public decimal IncomeAmount { get; set; }
        public decimal ExpenseAmount { get; set; }
        public string TransactionType { get; set; }
        public string Narration { get; set; }
        public string FromStatus { get; set; }
    }
}