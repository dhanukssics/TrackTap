using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.PostModel
{
    public class SchoolChangeUserNamePostModel
    {
        public string schoolId { get; set; }
        public string oldEmailId { get; set; }
        public string newEmailId { get; set; }
    }
}
