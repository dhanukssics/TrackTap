using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrackTap.Models
{
    public class AttendanceModels
    {
        public DateTime maxDate { get; set; }
        public DateTime minDate { get; set; }
        public long classId { get; set; }
        public long studentId { get; set; }
        public long divisionId { get; set; }
        public int shift { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public Int64 SchoolId { get; set; }
    }
}