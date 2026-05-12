using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.ClassLibrary.PostModel
{
    public class SchoolChangePasswordPostModel
    {
        public string schoolId { get; set; }
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
    }
}
