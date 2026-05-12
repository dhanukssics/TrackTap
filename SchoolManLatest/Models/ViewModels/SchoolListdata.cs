using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrackTap.Models
{
    public class SchoolListdata
    {
        public List<ModelList> list = new List<ModelList>();
    }
    public class ModelList
    {
        public string Schoolname { get; set; }
        public string Main { get; set; }
        public string Sub { get; set; }
        public long Id { get; set; }
        public int SlNo { get; set; }
        public long SubId { get; set; }
        public string Type { get; set; }
        public long SchoolId { get; set; }
    }
}