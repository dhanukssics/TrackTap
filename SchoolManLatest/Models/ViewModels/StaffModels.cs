using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrackTap.Models
{
    public class StaffModels
    {
        public long SchoolId { get; set; }
        public long UserId { get; set; }
        public long staffId { get; set; }


        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "Entered e-mail is not a valid mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "Entered e-mail is not a valid mail")]
        public string emailId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Contact { get; set; }
        [Required(ErrorMessage = "Required")]
        public string DOBstring { get; set; }
        public int userType { get; set; }
        public string type { get; set; }
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
}