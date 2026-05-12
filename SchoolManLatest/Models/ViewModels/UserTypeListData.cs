using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrackTap.Models
{
    public class UserTypeListData
    {
        public List<ModelLists> list = new List<ModelLists>();
    }
    public class ModelLists
    {
        public string UserType { get; set; }
        public string Main { get; set; }
        public string Sub { get; set; }
        public long Id { get; set; }
        public int SlNo { get; set; }
        public long SubId { get; set; }
        public string Type { get; set; }
    }
}