using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrackTap.Models
{
    public class SalaryReportModel
    {
        public int RoleId { get; set; }
        public long SchoolId { get; set; }
        public DateTime StartDate { get; set; }
        public long ClassId { get; set; }
        public long DivisionId { get; set; }
        public long FeeId { get; set; }

        public Staff staff { get; set; }
        public Teacher Teacher { get; set; }
        public List<SalaryReportModel> SalaryReportModelList { get; set; }

        //staff

        public long StaffId { get; set; }
        public long UserId { get; set; }
        public string StaffName { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public System.DateTime DOB { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public Nullable<decimal> SalaryAmount { get; set; }
        public Nullable<decimal> PFPercentage { get; set; }
        public Nullable<decimal> ESIPercentage { get; set; }
        public Nullable<bool> IsPermanent { get; set; }

        //teacher...

        public long TeacherId { get; set; }
        public string TeacherSpecialId { get; set; }
        public string TeacherName { get; set; }
        //public long SchoolId { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        //public System.DateTime TimeStamp { get; set; }
        public System.Guid TeacherGuid { get; set; }
        //public bool IsActive { get; set; }
        public string FilePath { get; set; }
        //public long UserId { get; set; }
        //public Nullable<decimal> SalaryAmount { get; set; }
        //public Nullable<decimal> PFPercentage { get; set; }
        //public Nullable<decimal> ESIPercentage { get; set; }
        //public Nullable<bool> IsPermanent { get; set; }

    }

    public class Staff
    {
        public long StaffId { get; set; }
        public long UserId { get; set; }
        public string StaffName { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public System.DateTime DOB { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public Nullable<decimal> SalaryAmount { get; set; }
        public Nullable<decimal> PFPercentage { get; set; }
        public Nullable<decimal> ESIPercentage { get; set; }
        public Nullable<bool> IsPermanent { get; set; }
    }

    public class Teacher
    {
        public long TeacherId { get; set; }
        public string TeacherSpecialId { get; set; }
        public string TeacherName { get; set; }
        public long SchoolId { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public System.Guid TeacherGuid { get; set; }
        public bool IsActive { get; set; }
        public string FilePath { get; set; }
        public long UserId { get; set; }
        public Nullable<decimal> SalaryAmount { get; set; }
        public Nullable<decimal> PFPercentage { get; set; }
        public Nullable<decimal> ESIPercentage { get; set; }
        public Nullable<bool> IsPermanent { get; set; }
    }
}