using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TrackTap.PostModel
{
    public class TeacherSentMessageModel
    {
        public string TeacherId { get; set; }
        public string ToSentId { get; set; }
        public string Subject { get; set; }
        public string Descritpion { get; set; }
        public ClassLibrary.MessageType MessageType { get; set; }
        public HttpPostedFile PostFile { get; set; }
    }
}
