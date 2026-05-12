using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrackTap.Models
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Required")]
        public string email { get; set; }
    }
}