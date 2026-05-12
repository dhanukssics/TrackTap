using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.ClassLibrary.PostModel
{
    public class ForgotPasswordPostModel
    {
        public string fromType { get; set; }// School =0 ,Parent =1
        public string emailId { get; set; }
    }
}
