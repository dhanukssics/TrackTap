using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrackTap.ClassLibrary;

namespace TrackTap.Models
{
    public class SchoolModuleModel
    {

        public long SchoolId { get; set; }
        public List<SchoolMainModuleList> mainList { get; set; }      
        public string SchoolName { get; set; }
        public List<SchoolSubModuleList> subListOnly { get; set; }
    }
    public class SchoolMainModuleList
    {
        public long Id { get; set; }
        public string ModuleName { get; set; }
        public List<SchoolSubModuleList> subList { get; set; }
        public string subIdListString { get; set; }
        public bool IsExistsMain { get; set; }
    }
    public class SchoolSubModuleList
    {
        public long Id { get; set; }
        public long MainId { get; set; }
        public string SubMosule { get; set; }
        public bool IsExists { get; set; }
    }
}