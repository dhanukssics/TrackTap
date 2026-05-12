using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.PostModel
{
    public class ParentLoginPostModel
    {
        public string email { get; set; }
        public string password { get; set; }
        public string deviceToken { get; set; }
    }
}
