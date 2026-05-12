using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrackTap.Models
{
    public class FeeModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "0 not allowed")]
        [Required(ErrorMessage = "Required")]
        public decimal Amount { get; set; }
        public string FeeDetails { get; set; }
        public string FeeStudentId { get; set; }
        public string StudentName { get; set; }
        [Required(ErrorMessage = "Required")]
        public string FeeName { get; set; }
        public long SpecialFeeId { get; set; }

        public long BillNumber { get; set; }

        public long DivisionId { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal PaidAmount { get; set; }


        public long BankId { get; set; }
        public long SchoolId { get; set; }
        public long ClassId { get; set; }
        public long StudentId { get; set; }
        public long FeeId { get; set; }
        public string ChequeDate { get; set; }
        public string ChequeNumber { get; set; }
        public int PaymentType { get; set; }
        public long AcademicYearId { get; set; }

        public DateTime TimeStamp { get; set; }
        public SchoolModel SchoolModel { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int From { get; set; } // 12-12-2018 Archana 
        public long BillNo { get; set; }// 12-12-2018 Archana 
        public string AdmissionNo { get; set; }

        
        public DateTime DueDate { get; set; }// Archana 11/10/2019
        public decimal OldAmount { get; set; }// Archana 11/10/2019

        public long CategoryId { get; set; }// jibin 9/24/2020

        public long StockId { get; set; }// jibin 9/24/2020
        public string Unit { get; set; }// jibin 9/24/2020
        public string Item { get; set; }// jibin 9/24/2020
        public decimal Price { get; set; }// jibin 9/24/2020
        public int Quantity { get; set; }// jibin 9/24/2020
        public long UserId { get; set; }// jibin 9/24/2020
        public string StudentSpecialId { get; set; }// jibin 9/24/2020
        public string salesid { get; set; }// jibin 9/24/2020

    }
    public class PrepaidInvoiceData
    {
        public int SlNo { get; set; }
        public string FeeName { get; set; }
        public DateTime DueDate { get; set; }
        public decimal ActualAmt { get; set; }
        public decimal DiscountAmt { get; set; }
        public decimal AfterDiscountAmt { get; set; }
    }
    public class PrepaidInvoiceDataList
    {
        public List<PrepaidInvoiceData> data { get; set; }
        public long StudentId { get; set; }
        public DateTime CurrentDatetime { get; set; }
    }
}