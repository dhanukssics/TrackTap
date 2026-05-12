using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrackTap.Models
{
    public class SubjectsModel
    {
        public string SubjectName { get; set; }
        [Required(ErrorMessage = "SubjectName  Required")]
        public long SchoolId { get; set; }
    }
}