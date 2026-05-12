using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using TrackTap.ClassLibrary;
using TrackTap.MapModel;
using TrackTap.PostModel;
using TrackTap.ClassLibrary.Utility;
using TrackTap.Data;
using TrackTap.Service.Helper;

namespace TrackTap.Repository
{
    public class SchoolRepository
    {
        Random rnd = new Random();


        public tb_tracktapEntities _Entity = new tb_tracktapEntities();


        public DateTime currentTime = DateTime.UtcNow;

        public Tuple<bool, string, School> AddNewSchool(SchoolRegistrationPostModel model)
        {
            var status = false;
            var msg = "Failed";
            Tuple<string, string> locationData = GetdtLatLong(model.Address);//Find current lat and long
            if (locationData.Item1 != "")
            {
                var school = _Entity.tb_School.Create();
                school.SchoolGuidId = Guid.NewGuid();
                school.SchoolName = model.SchoolName;
                school.Address = model.Address;
                school.Contact = model.Contact;
                school.IsActive = true;
                school.City = model.City;
                school.State = model.State;
                school.TimeStamp = currentTime;
                school.FilePath = model.FilePath;
                school.Website = model.Website;
                school.Latitude = locationData.Item1;
                school.Longitude = locationData.Item2;
                _Entity.tb_School.Add(school);
                status = _Entity.SaveChanges() > 0;
                if (status)
                {
                    var login = _Entity.tb_Login.Create();
                    login.SchoolId = school.SchoolId;
                    login.RoleId = 1;
                    login.Name = school.SchoolName;
                    login.Username = model.Email;
                    login.Password = model.Password;
                    login.IsActive = true;
                    login.TimeStamp = currentTime;
                    login.DisableStatus = false;
                    login.LoginGuid = Guid.NewGuid();
                    _Entity.tb_Login.Add(login);
                    status = _Entity.SaveChanges() > 0;
                    msg = "Success";
                }
                var schoolData = _Entity.tb_School.Where(x => x.SchoolId == school.SchoolId && x.IsActive).ToList().Select(x => new School(x)).FirstOrDefault();
                return new Tuple<bool, string, School>(status, msg, schoolData);
            }
            else
            {
                msg = "Invalid Address";
                return new Tuple<bool, string, School>(status, msg, null);
            }
        }

        private Tuple<string, string> GetdtLatLong(string address)
        {
            try
            {
                var requestUri = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false", Uri.EscapeDataString(address));
                var request = WebRequest.Create(requestUri);
                var response = request.GetResponse();
                var xdoc = XDocument.Load(response.GetResponseStream());
                var result = xdoc.Element("GeocodeResponse").Element("result");
                var locationElement = result.Element("geometry").Element("location");
                var lat = locationElement.Element("lat");
                var lon = locationElement.Element("lng");
                string latData = lat.Value.ToString();
                string longData = lon.Value.ToString();
                return Tuple.Create(latData, longData);
            }
            catch (Exception ex)
            {

                return Tuple.Create("", "");
            }
        }


        public Tuple<bool, string, School> SchoolLogin(SchoolLoginPostModel model)
        {
            var status = false;
            var msg = "Failed";
            var schoolData = _Entity.tb_Login.Where(x => x.Username.ToLower() == model.Email.ToLower() && x.Password == model.Password && x.IsActive).FirstOrDefault();
            if (schoolData != null)
            {
                var schoolFullData = _Entity.tb_School.Where(x => x.SchoolId == schoolData.SchoolId).ToList().Select(x => new School(x)).FirstOrDefault();
                msg = "Success";
                status = true;
                return new Tuple<bool, string, School>(status, msg, schoolFullData);
            }
            else
            {
                msg = "Invalid Username or Password!";
                return new Tuple<bool, string, School>(status, msg, null);
            }
        }

        public Tuple<bool, string, tb_School> AddNewClassAndDivision(SchoolAddClassAndDivisionPostModel model)
        {
            var status = false;
            var msg = "Failed";
            long schoolId = Convert.ToInt64(model.SchoolId);
            var academicYear = _Entity.tb_AcademicYear.Where(x=>x.IsActive==true && x.CurrentYear==true).FirstOrDefault();
            int order = Convert.ToInt32(model.ClassNameOrdr);
            var classData = _Entity.tb_Class.Where(x => x.ClassOrder == order && x.SchoolId == schoolId && x.IsActive == true && x.AcademicYearId == academicYear.YearId).FirstOrDefault();
            if (classData != null)
            {
                var divisionData = _Entity.tb_Division.Where(x => x.Division == model.Division && x.ClassId == classData.ClassId && x.IsActive == true).FirstOrDefault();
                if (divisionData != null)
                {
                    msg = "Division Already Exists";
                    status = true;
                    var schoolDivisionData = _Entity.tb_School.Where(x => x.SchoolId == schoolId && x.IsActive).FirstOrDefault();
                    return new Tuple<bool, string, tb_School>(status, msg, schoolDivisionData);
                }
                else
                {
                    var division = _Entity.tb_Division.Create();
                    division.DivisionGuid = Guid.NewGuid();
                    division.Division = model.Division;
                    division.ClassId = classData.ClassId;
                    division.IsActive = true;
                    division.TimeStamp = currentTime;
                    _Entity.tb_Division.Add(division);
                    status = _Entity.SaveChanges() > 0;
                    msg = status ? "Success" : "Failed";
                    var schoolDivisionData = _Entity.tb_School.Where(x => x.SchoolId == schoolId && x.IsActive).FirstOrDefault();
                    return new Tuple<bool, string, tb_School>(status, msg, schoolDivisionData);
                }
            }
            else
            {
                var classOrder = _Entity.tb_ClassList.Where(x => x.OrderValue == order && x.IsActive).FirstOrDefault();
                var addClass = _Entity.tb_Class.Create();
                addClass.ClassGuild = Guid.NewGuid();
                addClass.SchoolId = Convert.ToInt64(model.SchoolId);
                addClass.Class = classOrder.ClassName;
                addClass.Timestamp = currentTime;
                addClass.IsActive = true;
                addClass.AcademicYearId = academicYear.YearId;
                addClass.ClassOrder = order;
                //if (Convert.ToInt32(model.ClassName) == 1)
                //    addClass.ClassOrder = 1;
                //if (Convert.ToInt32(model.ClassName) == 2)
                //    addClass.ClassOrder = 2;
                //if (Convert.ToInt32(model.ClassName) == 3)
                //    addClass.ClassOrder = 3;
                //if (Convert.ToInt32(model.ClassName) == 4)
                //    addClass.ClassOrder = 4;
                //if (Convert.ToInt32(model.ClassName) == 5)
                //    addClass.ClassOrder = 5;
                //if (Convert.ToInt32(model.ClassName) == 6)
                //    addClass.ClassOrder = 6;
                //if (Convert.ToInt32(model.ClassName) == 7)
                //    addClass.ClassOrder = 7;
                //if (Convert.ToInt32(model.ClassName) == 8)
                //    addClass.ClassOrder = 8;
                //if (Convert.ToInt32(model.ClassName) == 9)
                //    addClass.ClassOrder = 9;
                //if (Convert.ToInt32(model.ClassName) == 10)
                //    addClass.ClassOrder = 10;
                //if (Convert.ToInt32(model.ClassName) == 11)
                //    addClass.ClassOrder = 11;
                //if (Convert.ToInt32(model.ClassName) == 12)
                //    addClass.ClassOrder = 12;
                _Entity.tb_Class.Add(addClass);
                status = _Entity.SaveChanges() > 0;
                {
                    var division = _Entity.tb_Division.Create();
                    division.DivisionGuid = Guid.NewGuid();
                    division.Division = model.Division;
                    division.ClassId = addClass.ClassId;
                    division.IsActive = true;
                    division.TimeStamp = currentTime;
                    _Entity.tb_Division.Add(division);
                    status = _Entity.SaveChanges() > 0;
                    {
                        msg = status ? "Success" : "Failed";
                        var schoolDivisionData = _Entity.tb_School.Where(x => x.SchoolId == schoolId && x.IsActive).FirstOrDefault();
                        return new Tuple<bool, string, tb_School>(status, msg, schoolDivisionData);
                    }
                }
            }
        }

        public Tuple<bool, string, List<tb_Class>> FullClassWithDivision(SchoolClassListWithDivisionPostModel model)
        {
            var status = false;
            var msg = "Failed";
            long schoolId = Convert.ToInt64(model.SchoolId);
            var classData = _Entity.tb_Class.Where(x => x.SchoolId == schoolId && x.IsActive).OrderBy(x => x.ClassOrder).ToList();
            if (classData != null)
            {
                msg = "Success";
                status = true;
                return new Tuple<bool, string, List<tb_Class>>(status, msg, classData);
            }
            else
            {
                return new Tuple<bool, string, List<tb_Class>>(status, msg, null);
            }
        }

        //public Tuple<bool, string, string> AddNewTeacher(SchoolAddTeacherPostModel model)
        //{
        //    var status = false;
        //    var msg = "Failed";

        //    long schoolId = Convert.ToInt64(model.schoolId);

        //    long classId = 0;
        //    if (model.classId != null && model.classId != string.Empty)
        //        classId = Convert.ToInt64(model.classId);

        //    long divisionId = 0;
        //    if (model.divisionId != null && model.divisionId != string.Empty)
        //        divisionId = Convert.ToInt64(model.divisionId);
        //    //if (_Entity.tb_TeacherClass.Any(x => x.DivisionId == divisionId))
        //    //{
        //    //    msg = "Class Already Assigned";
        //    //    return new Tuple<bool, string, string>(status, msg, "");
        //    //}
        //    //else
        //    //{

        //    var addTeacher = _Entity.tb_Teacher.Create();
        //    addTeacher.TeacherGuid = Guid.NewGuid();
        //    addTeacher.TeacherName = model.teacherName;
        //    addTeacher.TeacherSpecialId = "TR" + RandomStringGenerator.RandomString() + rnd.Next(1, 100);
        //    addTeacher.SchoolId = schoolId;
        //    addTeacher.Email = model.emailId;
        //    addTeacher.ContactNumber = model.contactNumber;
        //    addTeacher.TimeStamp = currentTime;
        //    addTeacher.IsActive = true;
        //    addTeacher.FilePath = model.filePath;
        //    _Entity.tb_Teacher.Add(addTeacher);
        //    status = _Entity.SaveChanges() > 0;
        //    {
        //        var teacherLogin = _Entity.tb_Login.Create();
        //        teacherLogin.SchoolId = schoolId;
        //        teacherLogin.RoleId = Convert.ToInt32(ClassLibrary.UserRole.Teacher);
        //        teacherLogin.Name = model.teacherName;
        //        teacherLogin.Username = model.emailId;
        //        teacherLogin.Password = addTeacher.TeacherSpecialId;
        //        teacherLogin.IsActive = true;
        //        teacherLogin.DisableStatus = false;
        //        teacherLogin.LoginGuid = Guid.NewGuid();
        //        _Entity.tb_Login.Add(teacherLogin);
        //        status = _Entity.SaveChanges() > 0;
        //        if (classId != 0 && divisionId != 0)
        //        {

        //            var addTeacherClass = _Entity.tb_TeacherClass.Create();
        //            addTeacherClass.TeacherId = addTeacher.TeacherId;
        //            addTeacherClass.ClassId = classId;
        //            addTeacherClass.DivisionId = divisionId;
        //            _Entity.tb_TeacherClass.Add(addTeacherClass);
        //            status = _Entity.SaveChanges() > 0;
        //        }
        //        msg = "Success";
        //        status = true;
        //    }
        //    return new Tuple<bool, string, string>(status, msg, teacherLogin.Password);
        //}
        public Tuple<bool, string, string> AddNewTeacher(SchoolAddTeacherPostModel model)
        {
            var status = false;
            var msg = "Failed";

            long schoolId = Convert.ToInt64(model.schoolId);

            long classId = 0;
            if (model.classId != null && model.classId != string.Empty)
                classId = Convert.ToInt64(model.classId);

            long divisionId = 0;
            if (model.divisionId != null && model.divisionId != string.Empty)
                divisionId = Convert.ToInt64(model.divisionId);
            //if (_Entity.tb_TeacherClass.Any(x => x.DivisionId == divisionId))
            //{
            //    msg = "Class Already Assigned";
            //    return new Tuple<bool, string, string>(status, msg, "");
            //}
            //else
            //{
            var teacherLogin = _Entity.tb_Login.Create();
            teacherLogin.SchoolId = schoolId;
            teacherLogin.RoleId = Convert.ToInt32(ClassLibrary.UserRole.Teacher);
            teacherLogin.Name = model.teacherName;
            teacherLogin.Username = model.emailId;
            teacherLogin.Password = "TR" + RandomStringGenerator.RandomString() + rnd.Next(1, 100);
            teacherLogin.IsActive = true;
            teacherLogin.TimeStamp = currentTime;
            teacherLogin.DisableStatus = false;
            teacherLogin.LoginGuid = Guid.NewGuid();
            _Entity.tb_Login.Add(teacherLogin);
            status = _Entity.SaveChanges() > 0;
            if (status)
            {
                var addTeacher = _Entity.tb_Teacher.Create();
                addTeacher.TeacherGuid = Guid.NewGuid();
                addTeacher.TeacherName = model.teacherName;
                addTeacher.TeacherSpecialId = teacherLogin.Password;
                addTeacher.SchoolId = schoolId;
                addTeacher.Email = model.emailId;
                addTeacher.ContactNumber = model.contactNumber;
                addTeacher.TimeStamp = currentTime;
                addTeacher.IsActive = true;
                addTeacher.FilePath = model.filePath;
                addTeacher.UserId = teacherLogin.UserId;
                addTeacher.SalaryAmount = model.SalaryAmount;
                addTeacher.PFPercentage = model.PFPercentage;
                addTeacher.ESIPercentage = model.ESIPercentage;
                addTeacher.IsPermanent = model.IsPermanent;
                addTeacher.DOJ = model.DOJ;
                if (model.UserTypeId != null && model.UserTypeId != string.Empty)
                    addTeacher.UserType = Convert.ToInt64(model.UserTypeId);
                _Entity.tb_Teacher.Add(addTeacher);
                status = _Entity.SaveChanges() > 0;
                {

                    if (classId != 0 && divisionId != 0)
                    {

                        var addTeacherClass = _Entity.tb_TeacherClass.Create();
                        addTeacherClass.TeacherId = addTeacher.TeacherId;
                        addTeacherClass.ClassId = classId;
                        addTeacherClass.DivisionId = divisionId;
                        _Entity.tb_TeacherClass.Add(addTeacherClass);
                        status = _Entity.SaveChanges() > 0;
                    }
                    msg = "Success";
                    status = true;
                }
            }
            return new Tuple<bool, string, string>(status, msg, teacherLogin.Password);
            //}

        }
        //}

        public Tuple<bool, string, List<tb_Teacher>> FullTeacherList(SchoolTeacherListPostModel model)
        {
            var status = false;
            var msg = "Failed";
            long schoolId = Convert.ToInt64(model.schoolId);
            var teacherList = _Entity.tb_Teacher.Where(x => x.SchoolId == schoolId && x.IsActive).OrderBy(z => z.TeacherName).ToList();
            if (teacherList != null)
            {
                msg = "Success";
                status = true;
                return new Tuple<bool, string, List<tb_Teacher>>(status, msg, teacherList);
            }
            else
            {
                msg = "No data found";
                status = true;
                return new Tuple<bool, string, List<tb_Teacher>>(status, msg, null);
            }
        }

        public Tuple<bool, string, string> AddNewBus(SchoolAddBusPostModel model)
        {
            var status = false;
            var msg = "Failed";
            long schoolId = Convert.ToInt64(model.schoolId);
            var schoolPlace = _Entity.tb_School.Where(x => x.SchoolId == schoolId && x.IsActive).FirstOrDefault();
            var addBus = _Entity.tb_Bus.Create();
            addBus.SchoolId = schoolId;
            addBus.BusName = model.busName;
            addBus.TripNumber = Convert.ToInt32(model.tripNumber);
            addBus.IsActive = true;
            addBus.TimeStamp = currentTime;
            addBus.LocationEnd = schoolPlace.City;
            addBus.BusType = model.busType;
            addBus.BusGuid = Guid.NewGuid();
            addBus.LocationStart = model.endLocation;
            addBus.BusSpecialId = "BS" + RandomStringGenerator.RandomString() + rnd.Next(1, 100);
            _Entity.tb_Bus.Add(addBus);
            status = _Entity.SaveChanges() > 0;
            {
                msg = "Success";
                return new Tuple<bool, string, string>(status, msg, addBus.BusSpecialId);
            }
        }

        public Tuple<bool, string, List<tb_Bus>> FullBusList(SchoolBusListPostModel model)
        {
            var status = false;
            var msg = "Failed";
            long schoolId = Convert.ToInt64(model.schoolId);
            var busList = _Entity.tb_Bus.Where(x => x.SchoolId == schoolId && x.IsActive).ToList();
            if (busList != null)
            {
                msg = "Success";
                status = true;
                return new Tuple<bool, string, List<tb_Bus>>(status, msg, busList);
            }
            else
            {
                msg = "No data found";
                status = true;
                return new Tuple<bool, string, List<tb_Bus>>(status, msg, null);
            }
        }

        public Tuple<bool, string, string> AddStudent(SchoolAddStudentPostModel model)
        {
            var status = false;
            var msg = "Failed";
            long schoolId = Convert.ToInt64(model.schoolId);
            long busId = Convert.ToInt64(model.busId);
            long classId = Convert.ToInt64(model.classId);
            long divisionId = Convert.ToInt64(model.divisionId);
            var addStudent = _Entity.tb_Student.Create();
            //addStudent.StudentSpecialId = "ST" + RandomStringGenerator.RandomString() + rnd.Next(1, 100);
            addStudent.StudentSpecialId = model.admissionId;
            addStudent.SchoolId = schoolId;
            addStudent.StundentName = model.studentName;
            addStudent.ParentName = model.parentName;
            addStudent.Address = model.address;
            addStudent.City = model.city;
            addStudent.ContactNumber = model.contact;
            addStudent.ClasssNumber = model.classNo;
            addStudent.ClassId = classId;
            addStudent.DivisionId = divisionId;
            addStudent.BusId = busId;
            addStudent.TripNo = model.tripNo;
            addStudent.FilePath = model.filePath;
            addStudent.TimeStamp = currentTime;
            addStudent.StudentGuid = Guid.NewGuid();
            addStudent.IsActive = true;
            addStudent.State = model.state;
            addStudent.Gender = model.gender;
            addStudent.BloodGroup = model.bloodGroup;
            addStudent.ParentEmail = "";
            if (model.DOB != null && model.DOB.Year != 1)
            {
                addStudent.DOB = Convert.ToDateTime(model.DOB);
            }
            _Entity.tb_Student.Add(addStudent);
            status = _Entity.SaveChanges() > 0;
            {
                msg = "Success";
                return new Tuple<bool, string, string>(status, msg, addStudent.StudentSpecialId);
            }
        }

        public Tuple<bool, string, List<tb_Student>> FullStudentList(SchoolFullStudentListPostModel model)
        {
            var status = false;
            var msg = "Failed";
            long schoolId = Convert.ToInt64(model.schoolId);
            long classId = Convert.ToInt64(model.classId);
            long divisionId = Convert.ToInt64(model.divisionId);
            var studentList = _Entity.tb_Student.Where(x => x.SchoolId == schoolId && x.IsActive && x.ClassId == classId && x.DivisionId == divisionId).OrderBy(z => z.StundentName).ToList();
            if (studentList != null)
            {
                msg = "Success";
                status = true;
                return new Tuple<bool, string, List<tb_Student>>(status, msg, studentList);
            }
            else
            {
                msg = "No data found";
                status = true;
                return new Tuple<bool, string, List<tb_Student>>(status, msg, null);
            }
        }

        public Tuple<bool, string, List<tb_Student>> FullStudentList1(SchoolFullStudentListPostModel model)
        {
            var status = false;
            var msg = "Failed";
            long schoolId = Convert.ToInt64(model.schoolId);
            long classId = Convert.ToInt64(model.classId);
            long divisionId = Convert.ToInt64(model.divisionId);
            var studentList = _Entity.tb_Student.Where(x => x.SchoolId == schoolId && x.IsActive && x.ClassId == classId && x.DivisionId == divisionId && x.IsSamrtPhoneUser == false).OrderBy(z => z.StundentName).ToList();
            if (studentList != null)
            {
                msg = "Success";
                status = true;
                return new Tuple<bool, string, List<tb_Student>>(status, msg, studentList);
            }
            else
            {
                msg = "No data found";
                status = true;
                return new Tuple<bool, string, List<tb_Student>>(status, msg, null);
            }
        }

        public Tuple<bool, string, string> AddDriver(SchoolAddDriverPostModel model)
        {
            var status = false;
            var msg = "Failed";
            long schoolId = Convert.ToInt64(model.SchoolId);
            var addDriver = _Entity.tb_Driver.Create();
            addDriver.DriverSpecialId = "DR" + RandomStringGenerator.RandomString() + rnd.Next(1, 100);
            addDriver.SchoolId = schoolId;
            addDriver.DriverName = model.DriverName;
            addDriver.LicenseNumber = model.LicenseNumber;
            addDriver.ContactNumber = model.ContactNumber;
            addDriver.Address = model.Address;
            addDriver.FilePath = model.FilePath;
            addDriver.TimeStamp = currentTime;
            addDriver.DriverGuid = Guid.NewGuid();
            addDriver.IsActive = true;
            addDriver.City = model.City;
            addDriver.State = model.State;
            _Entity.tb_Driver.Add(addDriver);
            status = _Entity.SaveChanges() > 0;
            {
                msg = "Success";
                return new Tuple<bool, string, string>(status, msg, addDriver.DriverSpecialId);
            }
        }

        public Tuple<bool, string, List<tb_Driver>> FullDriverList(SchoolDriverListPostModel model)
        {
            var status = false;
            var msg = "Failed";
            long schoolId = Convert.ToInt64(model.schoolId);
            var driverList = _Entity.tb_Driver.Where(x => x.SchoolId == schoolId && x.IsActive).OrderBy(x => x.DriverName).ToList();
            if (driverList != null)
            {
                msg = "Success";
                status = true;
                return new Tuple<bool, string, List<tb_Driver>>(status, msg, driverList);
            }
            else
            {
                msg = "No data found";
                status = true;
                return new Tuple<bool, string, List<tb_Driver>>(status, msg, null);
            }
        }

        public Tuple<bool, string, List<tb_Class>> DeleteDivision(SchoolDeleteDivisionPostModel model)
        {
            var status = false;
            var msg = "Failed";
            long schoolId = Convert.ToInt64(model.schoolId);
            long divisionId = Convert.ToInt64(model.divisionId);
            var divisionData = _Entity.tb_Division.Where(x => x.DivisionId == divisionId && x.tb_Class.SchoolId == schoolId && x.IsActive == true).FirstOrDefault();
            divisionData.IsActive = false;
            status = _Entity.SaveChanges() > 0;
            {
                var division = _Entity.tb_Division.Where(z => z.ClassId == divisionData.ClassId && z.IsActive).ToList();
                if (division.Count <= 0)
                {
                    var classRow = _Entity.tb_Class.FirstOrDefault(z => z.ClassId == divisionData.ClassId && z.IsActive);
                    if (classRow != null)
                    {
                        classRow.IsActive = false;
                        _Entity.SaveChanges();
                    }
                }

                var teacherData = _Entity.tb_TeacherClass.Where(x => x.DivisionId == divisionId).FirstOrDefault();
                if (teacherData != null)
                {
                    _Entity.tb_TeacherClass.Remove(teacherData);
                    _Entity.SaveChanges();
                }
                var classData = _Entity.tb_Class.Where(x => x.SchoolId == schoolId && x.IsActive).ToList();
                if (classData != null)
                {
                    msg = "Success";
                    status = true;
                    return new Tuple<bool, string, List<tb_Class>>(status, msg, classData);
                }
                else
                {
                    return new Tuple<bool, string, List<tb_Class>>(status, msg, null);
                }

            }
        }

        public Tuple<bool, string, List<tb_Teacher>> DeleteTeacher(SchoolDeleteTeacherPostModel model)
        {
            var status = false;
            var msg = "Failed";
            long schoolId = Convert.ToInt64(model.schoolId);
            long teacherId = Convert.ToInt64(model.teacherId);
            var teacherData = _Entity.tb_Teacher.Where(x => x.TeacherId == teacherId && x.SchoolId == schoolId && x.IsActive == true).FirstOrDefault();
            teacherData.IsActive = false;
            status = _Entity.SaveChanges() > 0;
            {
                var teacherClassData = _Entity.tb_TeacherClass.Where(x => x.TeacherId == teacherId).FirstOrDefault();
                if (teacherClassData != null)
                {
                    _Entity.tb_TeacherClass.Remove(teacherClassData);
                    _Entity.SaveChanges();
                }
                var teacherList = _Entity.tb_Teacher.Where(x => x.SchoolId == schoolId && x.IsActive).ToList();
                if (teacherList.Count > 0)
                {
                    msg = "Success";
                    status = true;
                    return new Tuple<bool, string, List<tb_Teacher>>(status, msg, teacherList);
                }
                else
                {
                    msg = "No data found";
                    status = true;
                    return new Tuple<bool, string, List<tb_Teacher>>(status, msg, null);
                }

            }
        }

        public Tuple<bool, string, List<tb_Bus>> DeleteBus(SchoolDeleteBusPostModel model)
        {
            bool status = false;
            string msg = "Failed";
            long schoolId = Convert.ToInt64(model.schoolId);
            long busId = Convert.ToInt64(model.busId);
            var busData = _Entity.tb_Bus.Where(x => x.SchoolId == schoolId && x.BusId == busId).FirstOrDefault();
            busData.IsActive = false;
            status = _Entity.SaveChanges() > 0;
            {
                var studentList = _Entity.tb_Student.Where(x => x.SchoolId == schoolId && x.BusId == busId && x.IsActive).ToList();
                if (studentList.Count > 0)
                {
                    foreach (var item in studentList)
                    {
                        item.BusId = 1;
                        item.TripNo = null;
                        status = _Entity.SaveChanges() > 0;
                    }
                }
                var busList = _Entity.tb_Bus.Where(x => x.SchoolId == schoolId && x.IsActive).ToList();
                if (busList != null)
                {
                    msg = "Success";
                    status = true;
                    return new Tuple<bool, string, List<tb_Bus>>(status, msg, busList);
                }
                else
                {
                    msg = "No data found";
                    status = true;
                    return new Tuple<bool, string, List<tb_Bus>>(status, msg, null);
                }
            }

        }

        public Tuple<bool, string, List<tb_Student>> DeleteStudent(SchoolDeleteStudentPostModel model)
        {
            bool status = false;
            string msg = "Failed";
            long schoolId = Convert.ToInt64(model.schoolId);
            long studentId = Convert.ToInt64(model.studentId);
            var studentData = _Entity.tb_Student.Where(x => x.SchoolId == schoolId && x.StudentId == studentId).FirstOrDefault();
            studentData.IsActive = false;
            status = _Entity.SaveChanges() > 0;
            {
                var studentList = _Entity.tb_Student.Where(x => x.SchoolId == schoolId && x.IsActive).ToList();
                if (studentList != null)
                {
                    msg = "Success";
                    status = true;
                    return new Tuple<bool, string, List<tb_Student>>(status, msg, studentList);
                }
                else
                {
                    msg = "No data found";
                    status = true;
                    return new Tuple<bool, string, List<tb_Student>>(status, msg, null);
                }
            }
        }

        public Tuple<bool, string, List<tb_Driver>> DeleteDriver(SchoolDeleteDriverPostModel model)
        {
            bool status = false;
            string msg = "Failed";
            long schoolId = Convert.ToInt64(model.schoolId);
            long driverId = Convert.ToInt64(model.driverId);
            var driverData = _Entity.tb_Driver.Where(x => x.SchoolId == schoolId && x.DriverId == driverId).FirstOrDefault();
            driverData.IsActive = false;
            status = _Entity.SaveChanges() > 0;
            {
                var driverList = _Entity.tb_Driver.Where(x => x.SchoolId == schoolId && x.IsActive).ToList();
                if (driverList != null)
                {
                    msg = "Success";
                    status = true;
                    return new Tuple<bool, string, List<tb_Driver>>(status, msg, driverList);
                }
                else
                {
                    msg = "No data found";
                    status = true;
                    return new Tuple<bool, string, List<tb_Driver>>(status, msg, null);
                }
            }
        }

        public Tuple<bool, string, tb_School> EditSchoolData(SchoolEditPostModel model)
        {
            var status = false;
            var msg = "Failed";
            long scholId = Convert.ToInt64(model.schoolId);
            var school = _Entity.tb_School.Where(x => x.SchoolId == scholId && x.IsActive).FirstOrDefault();

            if (model.Address != string.Empty && model.Address != null)
                school.Address = model.Address;

            if (model.Contact != string.Empty && model.Contact != null)
                school.Contact = model.Contact;

            if (model.City != string.Empty && model.City != null)
                school.City = model.City;

            if (model.State != string.Empty && model.State != null)
                school.State = model.State;

            if (model.FilePath != string.Empty && model.FilePath != null)
                school.FilePath = model.FilePath;

            if (model.Website != string.Empty && model.Website != null)
                school.Website = model.Website;

            if (model.SchoolName != string.Empty && model.SchoolName != null)
                school.SchoolName = model.SchoolName;

            status = _Entity.SaveChanges() > 0;
            msg = status ? "Success" : "Failed";
            return new Tuple<bool, string, tb_School>(status, msg, school);
        }

        public Tuple<bool, string, string> ChangeUsername(SchoolChangeUserNamePostModel model)
        {
            bool status = false;
            string msg = "Failed";
            long schoolId = Convert.ToInt64(model.schoolId);
            var schoolData = _Entity.tb_Login.Where(x => x.SchoolId == schoolId && x.Username.ToLower() == model.oldEmailId.ToLower()).FirstOrDefault();
            schoolData.Username = model.newEmailId;
            status = _Entity.SaveChanges() > 0;
            msg = status ? "Success" : "Failed";
            return new Tuple<bool, string, string>(status, msg, schoolData.Username);
        }

        public Tuple<bool, string> ChangePassword(SchoolChangePasswordPostModel model)
        {
            bool status = false;
            string msg = "Failed";
            long schoolId = Convert.ToInt64(model.schoolId);
            var schoolData = _Entity.tb_Login.Where(x => x.SchoolId == schoolId && x.Password == model.oldPassword).FirstOrDefault();
            schoolData.Password = model.newPassword;
            status = _Entity.SaveChanges() > 0;
            msg = status ? "Success" : "Failed";
            return new Tuple<bool, string>(status, msg);
        }

        public Tuple<bool, string, List<Class>> FullClassList(SchoolClassListPostModel model)
        {
            var status = false;
            var msg = "Failed";
            long schoolId = Convert.ToInt64(model.schoolId);
            var classList = _Entity.tb_Class.Where(x => x.SchoolId == schoolId && x.IsActive && x.PublishStatus).OrderBy(x => x.ClassOrder).ToList().Select(z => new Class(z)).ToList();
            if (classList != null)
            {
                msg = "Success";
                status = true;
                return new Tuple<bool, string, List<Class>>(status, msg, classList);
            }
            else
            {
                msg = "No data found";
                status = true;
                return new Tuple<bool, string, List<Class>>(status, msg, null);
            }
        }

        public Tuple<bool, string, List<Division>> FullDivisionList(SchoolDivisionListPostModel model)
        {
            var status = false;
            var msg = "Failed";
            long schoolId = Convert.ToInt64(model.schoolId);
            long classId = Convert.ToInt64(model.classId);
            var classList = _Entity.tb_Division.Where(x => x.tb_Class.SchoolId == schoolId && x.ClassId == classId && x.IsActive).OrderBy(x => x.Division).ToList().Select(z => new Division(z)).ToList();
            if (classList != null)
            {
                msg = "Success";
                status = true;
                return new Tuple<bool, string, List<Division>>(status, msg, classList);
            }
            else
            {
                msg = "No data found";
                status = true;
                return new Tuple<bool, string, List<Division>>(status, msg, null);
            }
        }
        
        public object getUserById(long schoolId)
        {
            return _Entity.tb_School.Where(z => z.SchoolId == schoolId).FirstOrDefault();
        }

        public Tuple<bool, string, Trip> TravelHistory(SchoolTravelHistoryPostModel model)
        {
            bool status = false;
            string msg = "Failed";
            long busId = Convert.ToInt64(model.busId);
            DateTime historyDate = Convert.ToDateTime(model.dateTime);
            int shiftStatus = Convert.ToInt32(model.timeStatus);
            //var data = _Entity.tb_Trip.Where(x => x.BusId == busId && EntityFunctions.TruncateTime(x.TripDate) == historyDate.Date && x.TripNo == model.tripNumber && x.IsActive && x.ShiftStatus == shiftStatus).ToList().Select(z => new Trip(z)).ToList();
            //if (data.Count > 0)
            //{
            //    //msg = "Success";
            //    //status = true;
            //    //if (model.timeStatus == "0")
            //    //{
            //    //    return new Tuple<bool, string, Trip>(status, msg, data.FirstOrDefault());
            //    //}
            //    //else
            //    //{
            //    //    if (data.Count > 1)
            //    //        return new Tuple<bool, string, Trip>(status, msg, data.LastOrDefault());
            //    //    else
            //    //        return new Tuple<bool, string, Trip>(status, msg, null);
            //    //}
            //    msg = "Success";
            //    status = true;
            //    return new Tuple<bool, string, Trip>(status, msg, data.FirstOrDefault());
            //}
            var data = _Entity.SP_TravelHistory(busId, model.tripNumber, historyDate, shiftStatus).FirstOrDefault();
            if (data != null)
            {
                long tripId = Convert.ToInt64(data);
                Trip tripData = _Entity.tb_Trip.Where(x => x.TripId == tripId).ToList().Select(x => new Trip(x)).FirstOrDefault();
                msg = "Success";
                status = true;
                return new Tuple<bool, string, Trip>(status, msg, tripData);
            }

            else
            {
                msg = "No History";
                return new Tuple<bool, string, Trip>(status, msg, null);
            }

        }

        public Tuple<bool, string, List<Bus>> FullRunningBusList(SchoolBusListPostModel model)
        {
            var status = false;
            var msg = "Failed";
            long schoolId = Convert.ToInt64(model.schoolId);
            var currentDateTime = DateTime.UtcNow.Date;
            //var busList = _Entity.tb_Bus.Where(x => x.SchoolId == schoolId && x.IsActive).SelectMany(y => y.tb_Trip).Where(z => EntityFunctions.TruncateTime(z.StartTime) == currentDateTime && z.TravellingStatus == 1 && z.IsActive).ToList().Select(c => new Bus(c.tb_Bus)).ToList();
            var busList = _Entity.tb_Bus.Where(x => x.SchoolId == schoolId && x.IsActive).SelectMany(y => y.tb_Trip).Where(z => z.TravellingStatus == 1 && z.IsActive).ToList().Select(c => new Bus(c.tb_Bus)).ToList();
            //var data = _Entity.SP_CurrentRunningBus(schoolId, currentDateTime).ToList();
            //var busList = data.Select(x => new Bus(x.BusId)).ToList();
            if (busList != null)
            {
                msg = "Success";
                status = true;
                return new Tuple<bool, string, List<Bus>>(status, msg, busList);
            }
            else
            {
                msg = "No data found";
                status = true;
                return new Tuple<bool, string, List<Bus>>(status, msg, null);
            }
        }

        public Tuple<string, bool, List<SPUnassignedTeachers>> GetFreeClass(UnassignedClassPostModel model)
        {
            string msg = "Failed";
            bool status = false;
            long schoolId = Convert.ToInt64(model.schoolId);
            var freeClassList = _Entity.SP_UnassignedTeachers(schoolId).ToList().Select(x => new SPUnassignedTeachers(x)).ToList();
            if (freeClassList != null)
            {
                msg = "Success";
                status = true;
                return new Tuple<string, bool, List<SPUnassignedTeachers>>(msg, status, freeClassList);
            }
            else
            {
                msg = "No data found";
                status = false;
                return new Tuple<string, bool, List<SPUnassignedTeachers>>(msg, status, null);
            }
        }

        public Tuple<string, bool, List<SPUnassignedDivisions>> GetUnassignedDivision(UnassignedDivisionPostModel model)
        {
            string msg = "Failed";
            bool status = false;
            long schoolId = Convert.ToInt64(model.schoolId);
            long classId = Convert.ToInt64(model.classId);
            var freeDivisionList = _Entity.SP_UnassignedDivisions(schoolId, classId).ToList().Select(x => new SPUnassignedDivisions(x)).ToList();
            if (freeDivisionList.Count > 0)
            {
                status = true;
                msg = "Success";
                return new Tuple<string, bool, List<SPUnassignedDivisions>>(msg, status, freeDivisionList);
            }
            else
            {
                msg = "No such divisions";
                return new Tuple<string, bool, List<SPUnassignedDivisions>>(msg, status, null);
            }
        }

        public Tuple<string, bool, Driver> GetCurrentTravellingBusDriverData(TravellingBusDriverDataPostModel model)
        {
            long busId = Convert.ToInt64(model.busId);
            string msg = "Failed";
            bool status = false;
            var busData = _Entity.tb_Trip.Where(x => x.BusId == busId && x.IsActive && x.TravellingStatus != 2).FirstOrDefault();
            if (busData != null)
            {
                var driverData = _Entity.tb_Driver.Where(x => x.DriverId == busData.DriverId && x.IsActive).ToList().Select(x => new Driver(x)).FirstOrDefault();
                if (driverData != null)
                {
                    status = true;
                    msg = "Success";
                    return new Tuple<string, bool, Driver>(msg, status, driverData);
                }
                else
                {
                    msg = "No SDriver Details";
                    return new Tuple<string, bool, Driver>(msg, status, null);
                }
            }
            else
            {
                msg = "Current bus is not travelling";
                return new Tuple<string, bool, Driver>(msg, status, null);
            }
        }

        public Tuple<bool, string, List<CircularNotificationListMapModel>> GetAllCircularList(CircularNotificationListPostModel model)
        {
            bool status = false;
            string msg = "Faile";
            long schoolId = Convert.ToInt64(model.schoolId);
            List<CircularNotificationListMapModel> data = new List<CircularNotificationListMapModel>();
            //var dataFull = _Entity.tb_Circular.Where(x => x.SchoolId == schoolId && x.IsActive).OrderBy(x => x.CircularId).ToList();
            //if (dataFull.Count > 0)
            //{
            //    foreach (var item in dataFull)
            //    {
            //        CircularNotificationListMapModel one = new CircularNotificationListMapModel();
            //        one.Head = "Circular ";
            //        one.CircularId = item.CircularId;
            //        one.SchoolId = item.SchoolId;
            //        one.CircularDate = item.CircularDate;
            //        one.Description = item.Description;
            //        one.FilePath = item.FilePath ?? "";
            //        one.TimeStamp = item.TimeStamp;
            //        one.FromStatus = 0;
            //        data.Add(one);
            //    }
            //}
            var eventData = _Entity.tb_CalenderEvent.Where(x => x.SchoolId == schoolId && x.IsActive).OrderBy(x => x.EventId).ToList();
            if (eventData.Count > 0)
            {
                foreach (var item in eventData)
                {
                    CircularNotificationListMapModel one = new CircularNotificationListMapModel();
                    one.Head = item.EventHead;
                    one.CircularId = item.EventId;
                    one.SchoolId = item.SchoolId;
                    one.CircularDate = item.EventDate;
                    one.Description = item.EventDetails;
                    one.FilePath = "";
                    one.TimeStamp = item.TimeStamp;
                    one.FromStatus = 1;
                    data.Add(one);
                }
            }
            if (data.Count > 0)
            {
                msg = "Success";
                status = true;
                return new Tuple<bool, string, List<CircularNotificationListMapModel>>(status, msg, data);
            }
            else
            {
                msg = "No Circulars";
                status = true;
                return new Tuple<bool, string, List<CircularNotificationListMapModel>>(status, msg, data);
            }
        }

        public Tuple<bool, string, List<CircularNotificationListMapModel>> GetCircularEventWithDate(CircularEventWithDatePostModel model)
        {
            bool status = false;
            string msg = "Faile";
            long schoolId = Convert.ToInt64(model.ScholId);
            List<CircularNotificationListMapModel> data = new List<CircularNotificationListMapModel>();
            var dataFull = _Entity.sp_CircularNotificationPerDate(model.EventDate, schoolId).ToList();
            if (dataFull.Count > 0)
            {
                foreach (var item in dataFull)
                {
                    CircularNotificationListMapModel one = new CircularNotificationListMapModel();
                    one.Head = item.EventHead;
                    one.CircularId = item.CircularId;
                    one.SchoolId = schoolId;
                    one.CircularDate = item.CircularDate;
                    one.Description = item.Description;
                    one.FilePath = item.FilePath ?? "";
                    one.TimeStamp = currentTime;
                    one.FromStatus = 0;
                    data.Add(one);
                }
            }
            if (dataFull.Count > 0)
            {
                msg = "Success";
                status = true;
                return new Tuple<bool, string, List<CircularNotificationListMapModel>>(status, msg, data);
            }
            else
            {
                msg = "No Circulars";
                status = true;
                return new Tuple<bool, string, List<CircularNotificationListMapModel>>(status, msg, data);
            }
        }


        public Tuple<bool, string, List<StaffSMSMapModel>> GetAllStaffData(SchoolClassListPostModel model)
        {
            bool status = false;
            string msg = "Failed";
            long schoolId = Convert.ToInt64(model.schoolId);
            List<StaffSMSMapModel> data = new List<StaffSMSMapModel>();
            try
            {
                #region Teacher
                StaffSMSMapModel oneData = new StaffSMSMapModel();
                oneData.MemberList = new List<MemberList>();
                var teacher = _Entity.tb_Teacher.Where(x => x.SchoolId == schoolId && x.IsActive).ToList();
                if (teacher.Count > 0)
                {
                    oneData.MemberType = 1;//For Teacher
                    oneData.TypeName = "Teaching Staff ";
                    foreach (var item in teacher)
                    {
                        MemberList mem = new MemberList();
                        mem.MemmberName = item.TeacherName;
                        mem.MemberId = item.UserId;
                        mem.ContactNumber = item.ContactNumber;
                        oneData.MemberList.Add(mem);
                    }
                    data.Add(oneData);
                }
                #endregion Teacher
                #region Staff
                oneData = new StaffSMSMapModel();
                oneData.MemberList = new List<MemberList>();
                var nonteacher = _Entity.tb_Staff.Where(x => x.tb_Login.SchoolId == schoolId && x.IsActive).ToList();
                if (nonteacher.Count > 0)
                {
                    oneData.MemberType = 2;//For Staff
                    oneData.TypeName = "Non Teaching Staff";
                    foreach (var item in nonteacher)
                    {
                        MemberList mem = new MemberList();
                        mem.MemmberName = item.StaffName;
                        mem.MemberId = item.UserId;
                        mem.ContactNumber = item.Contact;
                        oneData.MemberList.Add(mem);
                    }
                    data.Add(oneData);
                }
                #endregion Staff
                status = true;
                msg = "Successful";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return new Tuple<bool, string, List<StaffSMSMapModel>>(status, msg, data);
        }

        public Tuple<bool, string, List<sp_ParentTeacherConversation_Result>> GetParentTeacherConversation(ParentTeacherConversationMapModel model)
        {
            bool status = false;
            string msg = "Failed";
            List<sp_ParentTeacherConversation_Result> data = new List<sp_ParentTeacherConversation_Result>();
            try
            {
                long studentId = Convert.ToInt64(model.StudentId);
                data = _Entity.sp_ParentTeacherConversation(studentId, model.start, model.length).ToList();
                status = true;
                msg = "Successful";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return new Tuple<bool, string, List<sp_ParentTeacherConversation_Result>>(status, msg, data);
        }

        public Tuple<bool, string> MessageFromSchool(MessageFromSchoolPostModel.MessageDetailsFromSchoolPostModel model)
        {
            bool status = false;
            string msg = "Failed";
            try
            {
                long schoolid = Convert.ToInt64(model.SchoolId);
                HttpClient client = new HttpClient();
                var history = new tb_SmsHistory();
                var numbers = new List<string>();
                var MsgId = new List<string>();
                var numb = "";
                var senderName = "MYSCHO";
                var senderData = _Entity.tb_SchoolSenderId.Where(x => x.SchoolId == schoolid && x.IsActive == true).FirstOrDefault();
                if (senderData != null)
                    senderName = senderData.SenderId;
                var smsHead = new tb_SmsHead();

                if (model.TypeId == (int)SchoolMsgFromApp.FullSchool)
                {
                    smsHead.Head = "Message From SchoolMan App(All Classes)";
                    smsHead.SchoolId = schoolid;
                    smsHead.TimeStamp = currentTime;
                    smsHead.IsActive = true;
                    smsHead.SenderType = (int)SMSSendType.Student;
                    _Entity.tb_SmsHead.Add(smsHead);
                    status = _Entity.SaveChanges() > 0;
                    msg = "Successful";
                    var list = _Entity.tb_Student.Where(x => x.IsActive && x.SchoolId == schoolid).ToList();
                    foreach (var item in list)
                    {
                        string messagepre = "Dear Parent of " + item.StundentName;
                        messagepre = messagepre + ", " + model.Message;
                        //-----SPECIAL CHARACTER SENDING -------------------
                        messagepre = messagepre.Replace("#", "%23");
                        messagepre = messagepre.Replace("&", "%26");
                        //--------------------------------------------------
                        var phone = item.ContactNumber.ToString();
                        int length = messagepre.Length;
                        int que = length / 160;
                        int rem = length % 160;
                        if (rem > 0)
                            que++;
                        int smsCount = que;
                        var url = "http://alvosms.in/api/v1/send?token=ivku4o2r6gjdq98bm3aesl50pyz7h1&numbers=" + phone + "&route=2&message=" + messagepre + "&sender=" + senderName;
                        ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                        HttpWebRequest request = this.GetRequest(url);
                        WebResponse webResponse = request.GetResponse();
                        var responseText = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();
                        var newresponse = responseText.Remove(responseText.Length - 2).TrimEnd();
                        alvosmsResp respList = new JavaScriptSerializer().Deserialize<alvosmsResp>(responseText);
                        if (status)
                        {
                            tb_SmsHistory sms = new tb_SmsHistory();
                            sms.IsActive = true;
                            sms.MessageContent = model.Message;
                            sms.MessageDate = currentTime;
                            sms.ScholId = schoolid;
                            sms.StuentId = item.StudentId;
                            sms.MobileNumber = phone;
                            sms.HeadId = smsHead.HeadId;
                            sms.SendStatus = Convert.ToString(respList.success);
                            if (respList.data != null)
                            {
                                sms.MessageReturnId = respList.data[0].messageId;
                                sms.DelivaryStatus = "Pending";
                            }
                            sms.SmsSentPerStudent = smsCount;
                            _Entity.tb_SmsHistory.Add(sms);
                            _Entity.SaveChanges();
                        }
                    }

                }
                else
                {
                    smsHead.Head = "Message From SchoolMan App ";
                    smsHead.SchoolId = schoolid;
                    smsHead.TimeStamp = currentTime;
                    smsHead.IsActive = true;
                    smsHead.SenderType = (int)SMSSendType.Student;
                    _Entity.tb_SmsHead.Add(smsHead);
                    status = _Entity.SaveChanges() > 0;
                    msg = "Successful";
                    foreach (var item in model.MultipleClass)
                    {
                        long classid = Convert.ToInt64(item.ClassId);
                        long divisionid = Convert.ToInt64(item.DivisionId);
                        var list = _Entity.tb_Student.Where(x => x.IsActive && x.SchoolId == schoolid && x.ClassId == classid && x.DivisionId == divisionid).ToList();
                        foreach (var student in list)
                        {
                            string messagepre = "Dear Parent of " + student.StundentName;
                            messagepre = messagepre + ", " + model.Message;
                            //-----SPECIAL CHARACTER SENDING -------------------
                            messagepre = messagepre.Replace("#", "%23");
                            messagepre = messagepre.Replace("&", "%26");
                            //--------------------------------------------------
                            var phone = student.ContactNumber.ToString();
                            int length = messagepre.Length;
                            int que = length / 160;
                            int rem = length % 160;
                            if (rem > 0)
                                que++;
                            int smsCount = que;
                            var url = "http://alvosms.in/api/v1/send?token=ivku4o2r6gjdq98bm3aesl50pyz7h1&numbers=" + phone + "&route=2&message=" + messagepre + "&sender=" + senderName;
                            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                            HttpWebRequest request = this.GetRequest(url);
                            WebResponse webResponse = request.GetResponse();
                            var responseText = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();
                            var newresponse = responseText.Remove(responseText.Length - 2).TrimEnd();
                            alvosmsResp respList = new JavaScriptSerializer().Deserialize<alvosmsResp>(responseText);
                            if (status)
                            {
                                tb_SmsHistory sms = new tb_SmsHistory();
                                sms.IsActive = true;
                                sms.MessageContent = model.Message;
                                sms.MessageDate = currentTime;
                                sms.ScholId = schoolid;
                                sms.StuentId = student.StudentId;
                                sms.MobileNumber = phone;
                                sms.HeadId = smsHead.HeadId;
                                sms.SendStatus = Convert.ToString(respList.success);
                                if (respList.data != null)
                                {
                                    sms.MessageReturnId = respList.data[0].messageId;
                                    sms.DelivaryStatus = "Pending";
                                }
                                sms.SmsSentPerStudent = smsCount;
                                _Entity.tb_SmsHistory.Add(sms);
                                _Entity.SaveChanges();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return new Tuple<bool, string>(status, msg);
        }
        private HttpWebRequest GetRequest(string url, string httpMethod = "GET", bool allowAutoRedirect = true)
        {
            Uri uri = new Uri(url);
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.UserAgent = @"Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";
            request.Timeout = Convert.ToInt32(new TimeSpan(0, 5, 0).TotalMilliseconds);
            request.Method = httpMethod;
            return request;
        }

        /// create by sibi
        /// 
        public Tuple<string, bool, List<ClassLibrary.PostModel.Exams>> SelectExams(ExamModel model)
        {
            string msg = "Failed";
            bool status = false;
            long SchoolId = Convert.ToInt64(model.SchoolId);
            long ClassId = Convert.ToInt64(model.ClassId);
            long DivisionId = Convert.ToInt64(model.DivisionId);

            List<ClassLibrary.PostModel.Exams> li_Exam = new List<ClassLibrary.PostModel.Exams>();
            List<ExamsSubjects> li_ExamsSubjects = new List<ExamsSubjects>();


            var var_Exams = _Entity.tb_Exams.Where(z => z.SchoolId == SchoolId && z.ClassId == ClassId && z.DivisionId == DivisionId && z.IsActive == true).ToList();
            foreach (var a1 in var_Exams)
            {
                ClassLibrary.PostModel.Exams Exams = new ClassLibrary.PostModel.Exams();
                Exams.ExamId = a1.ExamId;
                Exams.SchoolId = a1.SchoolId;
                Exams.ClassId = a1.ClassId;
                Exams.DivisionId = a1.DivisionId;
                Exams.ExamName = a1.ExamName;
                Exams.ExamDate = a1.ExamDate;
                Exams.TimeStamp = a1.TimeStamp;               

                    long ExamId = Exams.ExamId;
                    var var_ExamSubjects = _Entity.tb_ExamSubjects.Where(z => z.ExamId == ExamId && z.IsActive == true).ToList();
                    foreach (var a2 in var_ExamSubjects)
                    {
                        ExamsSubjects ExamsSubjects = new ExamsSubjects();
                        ExamsSubjects.ExamId = a2.ExamId;
                        ExamsSubjects.SubId = a2.SubId;
                        ExamsSubjects.Subject = a2.Subject;
                        ExamsSubjects.Mark = a2.Mark;
                        ExamsSubjects.TimeStamp = a2.TimeStamp;
                        ExamsSubjects.InternalMarks = a2.InternalMarks;
                        ExamsSubjects.ExternalMark = a2.ExternalMark;
                        ExamsSubjects.ExamDate = a2.ExamDate;
                        li_ExamsSubjects.Add(ExamsSubjects);                        
                    }
                Exams.ExamsSubjectsLists = li_ExamsSubjects;
                li_Exam.Add(Exams);

            }

            

            //var freeDivisionList = _Entity.SP_UnassignedDivisions(schoolId, classId).ToList().Select(x => new SPUnassignedDivisions(x)).ToList();
            if (var_Exams.Count > 0)
            {
                status = true;
                msg = "Success";
                return new Tuple<string, bool, List<ClassLibrary.PostModel.Exams>>(msg, status, li_Exam);
            }
            else
            {
                msg = "No such divisions";
                return new Tuple<string, bool, List<ClassLibrary.PostModel.Exams>>(msg, status, null);
            }
        }

        public Tuple<string , bool> SaveMark (TrackTap.ClassLibrary.PostModel.StudentMarks models)
        {
            string msg = "Failed";
            bool status = false;
            try
            {
                foreach (var model in models.StudentList)
                {
                    if (model.StudentId != 0 && models.ExamId != 0 && models.SubjectId != 0)
                    {
                        var searchItems = _Entity.tb_StudentMarks.Where(z =>
                                        z.StudentId == model.StudentId &&
                                        z.ExamId == models.ExamId &&
                                        z.SubjectId == models.SubjectId &&
                                        z.IsActive == true
                        ).FirstOrDefault();

                        long inter = Convert.ToInt64(model.InternalMark);
                        long Exte = Convert.ToInt64(model.ExternalMark);
                        long TotalMark = inter + Exte;

                        if (searchItems == null)
                        {
                            var results = _Entity.tb_StudentMarks.Create();

                            results.StudentId = model.StudentId;
                            results.ExamId = models.ExamId;
                            results.SubjectId = models.SubjectId;
                            results.Mark = TotalMark;
                            results.IsActive = true;
                            results.TimeStamp = DateTime.Now;
                            results.InternalMark = model.InternalMark;
                            results.ExternalMark = model.ExternalMark;

                            _Entity.tb_StudentMarks.Add(results);
                            _Entity.SaveChanges();

                            status = true;
                            msg = "Success";                                                       

                        }
                        else
                        {
                            searchItems.Mark = TotalMark;
                            searchItems.TimeStamp = DateTime.Now;
                            searchItems.InternalMark = model.InternalMark;
                            searchItems.ExternalMark = model.ExternalMark;
                            _Entity.SaveChanges();

                            status = true;
                            msg = "Success";

                            

                        }

                    }
                    else
                    {
                        msg = "StudentId or ExamId or SubjectId is Empty";
                        return new Tuple<string, bool>(msg, status);

                    }



                }
                return new Tuple<string, bool>(msg, status);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return new Tuple<string, bool>(msg, status);
            }
                      

        }

        public Tuple<string, bool,List<TrackTap.ClassLibrary.PostModel.ViewStudentMark>> MarkListView(ExamModel model)
        {
            string msg = "Failed";
            bool status = false;
            try
            {
                List<TrackTap.ClassLibrary.PostModel.ViewStudentMark> lis_StudentMArk = new List<TrackTap.ClassLibrary.PostModel.ViewStudentMark>();
                List<TrackTap.ClassLibrary.PostModel.ViewStudentMark> _Obj = new List<TrackTap.ClassLibrary.PostModel.ViewStudentMark>();

                long SchoolId = Convert.ToInt64(model.SchoolId);
                long ClassId = Convert.ToInt64(model.ClassId);
                long DivisionId = Convert.ToInt64(model.DivisionId);
                var R1 = _Entity.tb_Student.Where(z => 
                          z.SchoolId == SchoolId &&
                          z.ClassId == ClassId &&
                          z.DivisionId == DivisionId &&
                          z.IsActive == true).ToList();
                if (R1 != null || R1.Count != 0)
                {
                    
                    foreach (var a1 in R1)
                    {
                        TrackTap.ClassLibrary.PostModel.ViewStudentMark Items = new ClassLibrary.PostModel.ViewStudentMark();
                        var R2 = _Entity.tb_StudentMarks.Where(x =>
                                  x.StudentId == a1.StudentId &&
                                  x.ExamId == model.ExamId &&
                                  x.SubjectId == model.SubjectId && x.IsActive == true).FirstOrDefault();
                        if (R2 != null)
                        {
                            Items.InternalMark = R2.InternalMark;
                            Items.ExternalMark = R2.ExternalMark;
                            Items.Mark = R2.Mark;
                            Items.TimeStamp = R2.TimeStamp;
                            Items.StundentName = a1.StundentName;
                            Items.ContactNumber = a1.ContactNumber;
                            Items.ParentName = a1.ParentName;
                            Items.StudentId = a1.StudentId;

                            lis_StudentMArk.Add(Items);
                        }
                        else
                        {
                            Items.InternalMark = 0;
                            Items.ExternalMark = 0;
                            Items.Mark = 0;                            
                            Items.StundentName = a1.StundentName;
                            Items.ContactNumber = a1.ContactNumber;
                            Items.ParentName = a1.ParentName;
                            Items.StudentId = a1.StudentId;
                            lis_StudentMArk.Add(Items);
                        }
                    }
                    _Obj = lis_StudentMArk.OrderBy(x=>x.StundentName).ToList();
                    status = true;
                    msg = "Success";

                    return new Tuple<string, bool, List<TrackTap.ClassLibrary.PostModel.ViewStudentMark>>(msg, status, _Obj);
                }
                else
                {
                    return new Tuple<string, bool, List<TrackTap.ClassLibrary.PostModel.ViewStudentMark>>(msg, status, null);

                }
            }
            catch (Exception ex) {
                msg = ex.Message;
                return new Tuple<string, bool, List<TrackTap.ClassLibrary.PostModel.ViewStudentMark>>(msg, status, null);
            }
           
        }

        public Tuple<string, bool, List<TrackTap.ClassLibrary.PostModel.ViewStudentMark>> ProgressCardExamNameList(ExamModel model)
        {
            string msg = "Failed";
            bool status = false;
            try
            {
                List<TrackTap.ClassLibrary.PostModel.ViewStudentMark> lis_StudentMArk = new List<TrackTap.ClassLibrary.PostModel.ViewStudentMark>();
                List<TrackTap.ClassLibrary.PostModel.ViewStudentMark> _Obj = new List<TrackTap.ClassLibrary.PostModel.ViewStudentMark>();

                 
                var R1 = _Entity.tb_Student.Where(z =>
                          z.StudentId == model.StudentId &&
                          z.IsActive == true).FirstOrDefault();
                if (R1 != null)
                {

                    var R2 = _Entity.tb_Exams.Where(z => z.DivisionId == R1.DivisionId && z.IsActive == true).ToList();
                    foreach (var a1 in R2)
                    {
                        TrackTap.ClassLibrary.PostModel.ViewStudentMark Mo = new ViewStudentMark();
                        Mo.ExamId = a1.ExamId;
                        Mo.ExamName = a1.ExamName;
                        lis_StudentMArk.Add(Mo);
                    }

                   
                    _Obj = lis_StudentMArk;
                    status = true;
                    msg = "Success";

                    return new Tuple<string, bool, List<TrackTap.ClassLibrary.PostModel.ViewStudentMark>>(msg, status, _Obj);
                }
                else
                {
                    return new Tuple<string, bool, List<TrackTap.ClassLibrary.PostModel.ViewStudentMark>>(msg, status, null);

                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return new Tuple<string, bool, List<TrackTap.ClassLibrary.PostModel.ViewStudentMark>>(msg, status, null);
            }

        }

        public Tuple<string, bool, TrackTap.ClassLibrary.PostModel.ViewStudentMark> ProgressCardExamResults(ExamModel model)
        {
            string msg = "Failed";
            bool status = false;
            try
            {
                 TrackTap.ClassLibrary.PostModel.ViewStudentMark lis_StudentMArk = new TrackTap.ClassLibrary.PostModel.ViewStudentMark();
                 TrackTap.ClassLibrary.PostModel.ViewStudentMark _Obj = new TrackTap.ClassLibrary.PostModel.ViewStudentMark();


                var R1 = _Entity.tb_StudentMarks.Where(z => z.StudentId == model.StudentId && z.ExamId == model.ExamId).FirstOrDefault();
                if (R1 != null)
                {
                    
                    var R2 = _Entity.tb_Subjects.Where(z => z.SubId == R1.SubjectId && z.IsActive == true).FirstOrDefault();
                    var R3 = _Entity.tb_Student.Where(z => z.StudentId == model.StudentId && z.IsActive == true).FirstOrDefault();

                    if (R3 != null)
                    {
                        lis_StudentMArk.StundentName = R3.StundentName;
                    }
                    else
                    {
                        lis_StudentMArk.StundentName = null;
                    }
                    


                    if (R2 != null)
                    {
                        lis_StudentMArk.SubjectName = R2.SubjectName;
                    }
                    else
                    {
                        lis_StudentMArk.SubjectName = null;
                    }
                    
                    lis_StudentMArk.Mark = R1.Mark;
                    lis_StudentMArk.ExternalMark = R1.ExternalMark;
                    lis_StudentMArk.InternalMark = R1.InternalMark;
                    lis_StudentMArk.TimeStamp = R1.TimeStamp;

                    _Obj = lis_StudentMArk;
                    status = true;
                    msg = "Success";

                    return new Tuple<string, bool,  TrackTap.ClassLibrary.PostModel.ViewStudentMark>(msg, status, _Obj);
                }
                else
                {
                    return new Tuple<string, bool, TrackTap.ClassLibrary.PostModel.ViewStudentMark>(msg, status, null);

                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return new Tuple<string, bool, TrackTap.ClassLibrary.PostModel.ViewStudentMark>(msg, status, null);
            }

        }



    }
}

