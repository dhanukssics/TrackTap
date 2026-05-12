using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrackTap.Models
{
    public class PromotionStudentDummyDataModel
    {
        public long PremotionId { get; set; }
        public long StudentId { get; set; }
        public long FromDivision { get; set; }
        public long ToDivision { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public Nullable<long> SchoolId { get; set; }
        public long IsActive { get; set; }
        public Nullable<bool> LastUpdate { get; set; }
        public Nullable<long> OldClass { get; set; }
        public long AcademicYearId { get; set; }
    }
}