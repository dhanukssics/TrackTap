using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackTap.ClassLibrary;

namespace TrackTap.DataLibrary.Data
{
    public class Admin : BaseReference
    {
        public Admin()
        {

        }

        public List<Login> GetSchoolList()
        {
            return _Entities.tb_Login.Where(x => x.IsActive == true && x.RoleId == (int)UserRole.School).ToList().Select(y => new Login(y)).OrderBy(c => c.Name).ToList();
        }
        public List<SP_GetPaymentGatewayList_Result> GetPaymentList(long schoolId,int month , int year, int opr)
        {
                return _Entities.SP_GetPaymentGatewayList(opr,schoolId,month,year).ToList();
        }
        public List<SP_GetMonthlyAttendance_Result> GeteMonthlyAttendance(long studentId, int month, int year)
        {
            return _Entities.SP_GetMonthlyAttendance(studentId, month, year).ToList();
        }

        public List<Division> GetAllAppDivision(long schoolId)
        {

            var xyyy = _Entities.tb_BiometricDivision.Where(x => x.SchoolId == schoolId).ToList();
            return xyyy.Select(y => new Division(y.tb_Division.DivisionId)).ToList();



        }

        public List<SpMessageForParent> ParentTeacherConversation(string studentId, int start, int length)
        {
            long StudentId = Convert.ToInt64(studentId);
            var data = _Entities.sp_ParentTeacherConversationFull(StudentId, start, length).ToList().Select(y => new SpMessageForParent(y)).ToList(); 
            return data;
        }

        public List<SpSmsPackage> GetSmsPackageBySchoolId(long schoolId)
        {
            return _Entities.SP_GetSmsPackage(schoolId).ToList().Select(y => new SpSmsPackage(y)).ToList();
        }
    }
}
