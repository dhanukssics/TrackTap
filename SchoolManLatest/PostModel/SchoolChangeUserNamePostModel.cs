using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.ClassLibrary.PostModel
{
    public class SchoolChangeUserNamePostModel
    {
        public string schoolId { get; set; }
        public string oldEmailId { get; set; }
        public string newEmailId { get; set; }
    }
}
