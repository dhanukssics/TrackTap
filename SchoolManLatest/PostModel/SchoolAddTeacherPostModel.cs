using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTap.ClassLibrary.PostModel
{
    public class SchoolAddTeacherPostModel
    {
        public string schoolId { get; set; }
        public string teacherName { get; set; }
        public string classId { get; set; }
        public string divisionId { get; set; }
        public string contactNumber { get; set; }
        public string emailId { get; set; }
        public string image { get; set; }
        public string filePath { get; set; }
        public decimal SalaryAmount { get; set; }
        public decimal PFPercentage { get; set; }
        public decimal ESIPercentage { get; set; }
        public bool IsPermanent { get; set; }
        public string UserTypeId { get; set; }

        public String DOJstring { get; set; }
        public DateTime DOJ { get; set; }
    }
}
