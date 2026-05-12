using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TrackTap.ClassLibrary;

namespace TrackTap.Models
{
    public class StockUpdateModel
    {
        public long CategoryId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Item { get; set; }
        [Required(ErrorMessage = "Required")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Required")]
        public string PurchaseId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string SupplirName { get; set; }
        [Required(ErrorMessage = "Required")]

        public string Supplier { get; set; }
        [Required(ErrorMessage = "Required")]
        public StockStatus Status { get; set; }
        [Required(ErrorMessage = "Required")]
        public long SchoolId { get; set; }
        public long StockId { get; set; }

        public int Quantity { get; set; }
        [Required(ErrorMessage = "Required")]

        public string Unit { get; set; }
        [Required(ErrorMessage = "Required")] //jibin 9/15/2020
        public DateTime StartDate { get; set; }//jibin 9/15/2020
        public DateTime EndDate { get; set; }//jibin 9/15/2020

        public string LaboratoryName { get; set; }

        public Int64 CatId { get; set; }


    }
}