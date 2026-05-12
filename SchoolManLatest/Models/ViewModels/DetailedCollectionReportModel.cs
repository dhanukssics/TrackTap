using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrackTap.Models
{
    public class DetailedCollectionReportModel
    {

        public long PaymentId { get; set; }
        public System.Guid FeeGuid { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
        public long FeeId { get; set; }
        public long StudentId { get; set; }
        public long ClassId { get; set; }
        public long SchoolId { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public bool IsActive { get; set; }
        public Nullable<decimal> MaxAmount { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<System.Guid> PaymentGuid { get; set; }
        public Nullable<long> BillNo { get; set; }
        public long PaymentType { get; set; }
        public Nullable<int> BillType { get; set; }
        public Nullable<int> PaymentMode { get; set; }
        public string ChequeNumber { get; set; }
        public Nullable<System.DateTime> ChequeDate { get; set; }
        public Nullable<long> BankId { get; set; }
        public Nullable<long> PartialPaidParentBillNo { get; set; }
        public List<DetailedCollectionReportModel> DetailedCollectionReport_Lists { get; set; }

        // studen Table

        public string StudentName { get; set; }
        //class table
        public string ClassName { get; set; }
        //Divition Table
        public string DivisionName { get; set; }
        // FeeTable
        public string Particulars { get; set; }






    }
}