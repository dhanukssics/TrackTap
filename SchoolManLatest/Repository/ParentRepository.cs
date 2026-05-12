
using Microsoft.EntityFrameworkCore;
using TrackTap.ClassLibrary;
using TrackTap.Data;
using TrackTap.MapModel;
using TrackTap.Models;
using TrackTap.PostModel;


namespace TrackTap.Repository
{
    public class ParentRepository
    {
        
        public DateTime currentTime = DateTime.UtcNow;
        private readonly SchoolDbContext _Entity;


        public ParentRepository(SchoolDbContext Entity)
        {
            _Entity = Entity;
        }
        public async Task<Tuple<bool, string, TbParent>> AddParentAsync(
    ParentRegistrationPostModel model)
        {
            bool status = false;
            string msg = "Failed";

            try
            {
                var parent = new TbParent
                {
                    ParentGuid = Guid.NewGuid(),
                    ParentName = model.ParentName,
                    Address = model.Address,
                    ContactNumber = model.ContactNumber,
                    Email = model.Email,
                    Password = model.Password,
                    IsActive = true,
                    City = model.City,
                    State = model.State,
                    TimeStamp = DateTime.UtcNow,
                    FilePath = model.FilePath
                };

                await _Entity.TbParents.AddAsync(parent);

                status = await _Entity.SaveChangesAsync() > 0;

                msg = status ? "Success" : "Failed";

                return Tuple.Create(status, msg, parent);
            }
            catch (Exception ex)
            {
                return Tuple.Create(
                    false,
                    ex.Message,
                    new TbParent());
            }
        }

        public Tuple<bool, string, tb_Parent> ParentLogin(ParentLoginPostModel model)
        {
            var status = false;
            var msg = "Failed";
            var parentData = _Entity.tb_Parent.Where(x => x.Email.ToLower() == model.email.ToLower() && x.Password == model.password && x.IsActive).FirstOrDefault();
            if (parentData != null)
            {
                msg = "Success";
                status = true;

                var insertToken = _Entity.tb_DeviceToken.Create();
                insertToken.RoleId = 1;
                insertToken.UserId = parentData.ParentId;
                insertToken.Token = model.deviceToken;
                insertToken.TimeStamp = currentTime;
                insertToken.IsActive = true;
                insertToken.LoginStatus = 1;
                _Entity.tb_DeviceToken.Add(insertToken);
                status = _Entity.SaveChanges() > 0;

                return new Tuple<bool, string, tb_Parent>(status, msg, parentData);

            }
            else
            {
                msg = "Invalid Username or Password";
                return new Tuple<bool, string, tb_Parent>(status, msg, null);
            }
        }

        public object getUserById(long parentId)
        {
            return _Entity.tb_Parent.Where(z => z.ParentId == parentId).FirstOrDefault();
        }

        public Tuple<bool, string, List<Student>> AddKid(ParentAddKidPostModel model)
        {
            var status = false;
            string msg = "Wrong Kid Id";
            long parentId = Convert.ToInt64(model.parentId);
            var studentData = _Entity.tb_Student.Where(x => x.StudentSpecialId == model.kidSpecialId && x.IsActive).FirstOrDefault();
            if (studentData != null)
            {
                studentData.ParentId = parentId;
                status = _Entity.SaveChanges() > 0;
                msg = "Success";
                var fullKidList = _Entity.tb_Student.Where(x => x.ParentId == parentId && x.IsActive).ToList().Select(z => new Student(z)).ToList();
                return new Tuple<bool, string, List<Student>>(status, msg, fullKidList);
            }
            else
            {
                var fullKidList = _Entity.tb_Student.Where(x => x.ParentId == parentId && x.IsActive).ToList().Select(z => new Student(z)).ToList();
                return new Tuple<bool, string, List<Student>>(status, msg, fullKidList);
            }

        }

        public Tuple<bool, string, List<Student>> liststudent(ParentIdPostModel model)
        {
            var status = true;
            string msg = "success";
            long parentId = Convert.ToInt64(model.parentId);
            var studentData = _Entity.tb_Student.Where(x => x.ParentId == parentId && x.IsActive).ToList().Select(z => new Student(z)).ToList();
            return new Tuple<bool, string, List<Student>>(status, msg, studentData);
        }



        public Tuple<bool, string, List<AllMessages>> MessagesList(ParentMessagePostModel model)
        {
            bool status = false;
            string msg = "Failed";
            long studentId = Convert.ToInt64(model.kidId);
            long schoolId = Convert.ToInt64(model.SchoolId);
            long classId = Convert.ToInt64(model.ClassId);
            int indexVal = Convert.ToInt32(model.index);
            int lengthVal = Convert.ToInt32(model.length);
            //var messageData = _Entity.tb_AllMessage.Where(x => (x.ToMsgSentId == studentId || x.ToMsgSentId == schoolId || x.ToMsgSentId == classId) && x.IsActive).ToList().OrderByDescending(x => x.Timestamp).ToList().Skip(indexVal * lengthVal).Take(lengthVal).ToList().Select(x => new AllMessages(x)).ToList();// 22-Jan -2018 Commended this line 
            var messageData= _Entity.tb_AllMessage.Where(x => x.ToMsgSentId == studentId && x.MessageType==1 && x.IsActive).ToList().OrderByDescending(x => x.Timestamp).ToList().Skip(indexVal * lengthVal).Take(lengthVal).ToList().Select(x => new AllMessages(x)).ToList();
            var classMessageData = _Entity.tb_AllMessage.Where(x => x.ToMsgSentId == classId && x.MessageType == 2 && x.IsActive).ToList().OrderByDescending(x => x.Timestamp).ToList().Skip(indexVal * lengthVal).Take(lengthVal).ToList().Select(x => new AllMessages(x)).ToList();
            var schoolMessageData= _Entity.tb_AllMessage.Where(x => x.ToMsgSentId == schoolId && x.MessageType == 3 && x.IsActive).ToList().OrderByDescending(x => x.Timestamp).ToList().Skip(indexVal * lengthVal).Take(lengthVal).ToList().Select(x => new AllMessages(x)).ToList();
            if(classMessageData.Count>0)
            messageData.AddRange(classMessageData);
            if(schoolMessageData.Count>0)
            messageData.AddRange(schoolMessageData);
            if (messageData.Count > 0)
            {
                status = true;
                msg = "Success";
            }
            else
            {
                status = true;
                msg = "No messages";
            }
            return new Tuple<bool, string, List<AllMessages>>(status, msg, messageData);
        }

        public Tuple<bool, string, List<AttendanceDetails>> AttendanceList(ParentKidAttendanceDataPostModel model)
        {
            List<AttendanceDetails> attendanceDataList = new List<AttendanceDetails>();
            bool status = false;
            string msg = "Failed";
            long studentId = Convert.ToInt64(model.kidId);
            int year = Convert.ToInt32(model.year);
            int month = Convert.ToInt32(model.month);
            var attendanceData = _Entity.tb_Attendance.Where(x => x.AttendanceDate.Year == year && x.AttendanceDate.Month == month && x.StudentId == studentId && x.IsActive).ToList().Select(x => new Attendance(x)).ToList();
            List<DateTime> attendanceTime = attendanceData.Select(z => z.AttendanceDate.Date).Distinct().ToList();
            if (attendanceData.Count > 0)
            {
                status = true;
                msg = "Success";
                string oneDate = "";
                foreach (var item in attendanceTime)
                {
                    AttendanceDetails obj = new AttendanceDetails();
                    obj.attendanceDate = item.ToShortDateString();
                    obj.mornignShift = false;
                    obj.eveningShift = false;
                    var eachDay = attendanceData.Where(z => z.AttendanceDate.Date == item.Date).ToList();
                    if (eachDay.Count > 0)
                    {
                        var morningStatus = eachDay.FirstOrDefault(z => z.ShiftStatus == 0);
                        var eveningStatus = eachDay.FirstOrDefault(z => z.ShiftStatus == 1);
                        if (morningStatus != null)
                        {
                            if (morningStatus.AttendanceData)
                                obj.mornignShift = true;
                        }
                        if (eveningStatus != null)
                        {
                            if (eveningStatus.AttendanceData)
                                obj.eveningShift = true;
                        }
                    }

                    attendanceDataList.Add(obj);
                }
                return new Tuple<bool, string, List<AttendanceDetails>>(status, msg, attendanceDataList);
            }
            else
            {
                return new Tuple<bool, string, List<AttendanceDetails>>(status, msg, attendanceDataList);
            }
        }

        public Tuple<bool, string, Driver> KidTravellingData(ParentKidTravellingStatusPostModel model)
        {
            string msg = "Failed";
            bool status = false;
            long kidId = Convert.ToInt64(model.kidId);
            var data = _Entity.SP_StudentCurrentTravellingData(kidId).ToList().Select(x => new SPStudentCurrentTravellingData(x)).ToList();
            if (data.Count > 0)
            {
                var newData = data.FirstOrDefault();
                if (newData.TravellingStatus != 2)
                {
                    status = true;
                    if (newData.TravellingStatus == 0)
                        msg = "Trip Start";
                    else
                        msg = "Running";

                    var driverData = _Entity.tb_Driver.Where(x => x.DriverId == newData.DriverId).ToList().Select(x => new Driver(x)).FirstOrDefault();
                    return new Tuple<bool, string, Driver>(status, msg, driverData);
                }
                else
                {
                    msg = "Trip Completed";
                    return new Tuple<bool, string, Driver>(status, msg, null);
                }
            }
            else
            {
                msg = "No Travelling";
                return new Tuple<bool, string, Driver>(status, msg, null);
            }
        }

        public Tuple<bool, string> ParentLogout(ParentLogoutPostModel model)
        {
            bool status = false;
            string msg = "Failed";
            long parentId = Convert.ToInt64(model.parentId);
            var parentData = _Entity.tb_DeviceToken.Where(x => x.UserId == parentId && x.Token == model.deviceToken && x.LoginStatus == 1 && x.IsActive).ToList();
            if (parentData.Count>0)
            {
                foreach (var item in parentData)
                {
                    item.LoginStatus = 0;
                    _Entity.SaveChanges();
                }
            }
            msg = "Successfully Logout";
            status = true;
            return new Tuple<bool, string>(status, msg);
        }

        public Tuple<bool, string, StudentBillHistoryMapModel> GetStudentBillHistory(StudentBillHistoryPostModel model)
        {
            bool status = false;
            string msg = "Failed";
            StudentBillHistoryMapModel mainData = new StudentBillHistoryMapModel();
            List<PaidHistory> _NewPaidHistory = new List<PaidHistory>();
            List<DueBills> _NewDueHistory = new List<DueBills>();
            long studentId = Convert.ToInt64(model.studentId);
            long classsId = Convert.ToInt64(model.classId);

            var data = _Entity.tb_Payment.Where(z => z.StudentId == studentId && z.ClassId == classsId && z.BillNo != null && z.IsActive == true).ToList();
            if (data.Count > 0)
            {
                long billNo = 0;
                foreach (var item in data)
                {
                    if (billNo != item.BillNo)
                    {
                        billNo = item.BillNo ?? 0;
                        PaidHistory one = new PaidHistory();
                        one.BillNo = billNo;
                        one.Billdate = item.TimeStamp;
                        List<PaidBills> oneBills = new List<PaidBills>();
                        var oneList = data.Where(x => x.BillNo == billNo).ToList();
                        foreach (var item2 in oneList)
                        {
                            string ext="";
                            try
                            {
                                var duedate = item2.tb_Fee.tb_FeeClass.Where(z => z.FeeId == item2.tb_Fee.FeeId).FirstOrDefault().DueDate;
                                ext = String.Format("{0:y}", duedate);
                            }
                            catch (Exception ex)
                            {
                                ext = "Additional";
                            }
                            PaidBills newOne = new PaidBills();
                            newOne.Particulars = item2.tb_Fee.FeesName.ToString() + ' ' + ext;
                            newOne.Discount = item2.Discount ?? 0;
                            newOne.Amount = item2.Amount;
                            oneBills.Add(newOne);
                        }
                        one.PaidBillsData = oneBills;
                        _NewPaidHistory.Add(one);
                    }
                }
            }

            var dueData = _Entity.SP_FullFee(classsId, studentId,0).ToList().Select(x => new SPFullFee(x)).ToList();
            if (dueData.Count > 0)
            {
                foreach (var item2 in dueData)
                {
                    DueBills newOne = new DueBills();
                    newOne.Particulars = item2.FeeName;
                    newOne.Details = item2.DueDate.ToString("MMMM");
                    newOne.Amount = item2.Amount??0;
                    _NewDueHistory.Add(newOne);
                }

            }


            mainData.PaidHistoryData = _NewPaidHistory;
            mainData.DueHistoryData = _NewDueHistory;
            msg = "Successful";
            status = true;
            return new Tuple<bool, string, StudentBillHistoryMapModel>(status, msg, mainData);

        }
        public Tuple<bool,string> SendMessage(ParentMessageSendPostModel model)
        {
            bool status = false;
            string msg = "Failed";
            long parentId = Convert.ToInt64(model.ParentId);
            long studentId = Convert.ToInt64(model.StudentId);
            string filePath = string.Empty;
            if(model.PostFile!=null && model.PostFile.ContentLength>0)
            {
                string folderPath = System.Web.HttpContext.Current.Server.MapPath("/Media/Parent/Message/");
                string fileExtension = Path.GetExtension(model.PostFile.FileName);
                string fileName = Guid.NewGuid().ToString();
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
                var fileSavePath = Path.Combine(folderPath, model.PostFile.FileName);
                model.PostFile.SaveAs(fileSavePath);
                filePath = Path.Combine("/Media/Parent/Message/", model.PostFile.FileName);
            }
            var message = _Entity.tb_ParentMessage.Create();
            message.SenderId = parentId;
            message.StudentId = studentId;
            message.Subject = model.Subject;
            message.Description = model.Description;
            message.FilePath = filePath;
            message.IsActive = true;
            message.TimeStamp = currentTime;
            _Entity.tb_ParentMessage.Add(message);
            status = _Entity.SaveChanges() > 0;
            if(status)
            {
                msg = "Success";
                return new Tuple<bool, string>(status,msg);
            }
            else
            {
                msg = "Sorry, Can't send the message!";
                return new Tuple<bool, string>(status, msg);
            }
        }

        public Tuple<bool, string, List<NewAttendanceDetails>> NewAttendanceList(ParentKidAttendanceDataPostModel model)
        {
            List<NewAttendanceDetails> attendanceDataList = new List<NewAttendanceDetails>();
            bool status = false;
            string msg = "Failed";
            long studentId = Convert.ToInt64(model.kidId);
            int year = Convert.ToInt32(model.year);
            int month = Convert.ToInt32(model.month);
            var attendanceData = _Entity.tb_Attendance.Where(x => x.AttendanceDate.Year == year && x.AttendanceDate.Month == month && x.StudentId == studentId && x.IsActive).ToList().Select(x => new Attendance(x)).ToList();
            List<DateTime> attendanceTime = attendanceData.Select(z => z.AttendanceDate.Date).Distinct().ToList();
            if (attendanceData.Count > 0)
            {
                status = true;
                msg = "Success";
                foreach (var item in attendanceTime)
                {
                    NewAttendanceDetails obj = new NewAttendanceDetails();
                    obj.attendanceDate = item.ToShortDateString();
                    obj.mornignShift =(int)AttendanceStatus.NotTaken;
                    obj.eveningShift = (int)AttendanceStatus.NotTaken;
                    var eachDay = attendanceData.Where(z => z.AttendanceDate.Date == item.Date).ToList();
                    if (eachDay.Count > 0)
                    {
                        var morningStatus = eachDay.FirstOrDefault(z => z.ShiftStatus == 0);
                        var eveningStatus = eachDay.FirstOrDefault(z => z.ShiftStatus == 1);
                        if (morningStatus != null)
                        {
                            if (morningStatus.AttendanceData==true)
                                obj.mornignShift = (int)AttendanceStatus.Present;
                            else
                                obj.mornignShift = (int)AttendanceStatus.Absent;
                        }
                        if (eveningStatus != null)
                        {
                            if (eveningStatus.AttendanceData)
                                obj.eveningShift = (int)AttendanceStatus.Present;
                            else
                                obj.eveningShift = (int)AttendanceStatus.Absent;
                        }
                    }

                    attendanceDataList.Add(obj);
                }
                return new Tuple<bool, string, List<NewAttendanceDetails>>(status, msg, attendanceDataList);
            }
            else
            {
                return new Tuple<bool, string, List<NewAttendanceDetails>>(status, msg, attendanceDataList);
            }
        }

    }
}


