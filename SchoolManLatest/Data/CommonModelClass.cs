using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.DataLibrary.Data
{
    public class CommonModelClass
    {

    }
    public class FeeClassList
    {
        public string ClassName { get; set; }
        public long ClassId { get; set; }
        public List<FeeDivisionList> list { get; set; }
    }
    public class FeeDivisionList
    {
        public string DivisionName { get; set; }
        public long DivisionId { get; set; }
    }

    //Basheer on 30-09-2019 for role module

    public class UserModule
    {
        public long MainId { get; set; }
        public long SubId { get; set; }
    }

}
