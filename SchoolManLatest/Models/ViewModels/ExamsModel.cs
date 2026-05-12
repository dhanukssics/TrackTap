using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrackTap.Models
{
    public class ExamsModel
    {
        public long ExamId { get; set; }
        public long SchoolId { get; set; }
        public long UserId { get; set; }
        [Required(ErrorMessage = "Class Required")]
        public long ClassId { get; set; }
        [Required(ErrorMessage = "Division Required")]
        public long DivisionId { get; set; }
        [Required(ErrorMessage = "Exam Name Required")]
        public string ExamName { get; set; }
        [Required(ErrorMessage = "Exam Date Required")]
        public DateTime ExamDate { get; set; }
    }
}