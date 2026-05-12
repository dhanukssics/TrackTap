using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrackTap.Data;

namespace TrackTap.Models
{
    public class OutstandingReportModel
    {
        public long SchoolId { get; set; }
        public long ClassId { get; set; }
        public long DivisionId { get; set; }
        public long FeeId { get; set; }
        public List<ReportDateList> ReportList { get; set; }
        public List<ReportDateList_one> ReportList_one { get; set; }
        public long AcademicYearId { get; set; }
        public bool IsSummary { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public List<OutstandingReportMainModel> GetOutStandingData_list { get; set; }

    }
    public class ReportDateList
    {
        public long StudentId { get; set; }
        public string StudentName { get; set; }
        public long ClassId { get; set; }
        public string ClassName { get; set; }
        public long DivisionId { get; set; }
        public string DivisionName { get; set; }
        public decimal Amount { get; set; }
        public string ContactNumber { get; set; }
        public int ClassOrder { get; set; }
        public  bool paidstatus { get; set; }
        public string feename { get; set; }

    }
    public class OutstandingReportNew
    {
        public long SchoolId { get; set; }
        public long StudentId { get; set; }
        public string StudentName { get; set; }
        public string ClassDetails { get; set; }
        public string ContactNumber { get; set; }
        public decimal Total { get; set; }
        public int ClassOrder { get; set; }
        public long DivisionId { get; set; }
        public List<SubList> SubList { get; set; }
    }
    public class SubList
    {
        public long FeeId { get; set; }
        public string FeeName { get; set; }
        public decimal Amount { get; set; }
    }
    public class ReportDateList_one
    {
        public string StudentSpecialId { get; set; }
        public string StudentName { get; set; }
        //public long ClassId { get; set; }
        public string ClassName { get; set; }
        //public long DivisionId { get; set; }
        //public string DivisionName { get; set; }
        public decimal Amount { get; set; }
        public string paidstatus { get; set; }
        public string feename { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}