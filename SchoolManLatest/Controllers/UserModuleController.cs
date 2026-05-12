using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using TrackTap.Models;

namespace TrackTap.Controllers
{
    public class UserModuleController : BaseController
    {
        // GET: UserModule
        public IActionResult Index()
        {
            return View();
        }
        #region School Admin Module
        public IActionResult UserTypeDefineHome()
        {
            UserModuleModel model = new UserModuleModel();
            model.SchoolId = _user.SchoolId;
            model.mainList = new List<MainModuleList>();
            //var list = _Entities.tb_SubModule.Where(x => x.IsActive).ToList();
            var list = _Entities.tb_SchoolModuleDetails.Where(x =>x.IsActive && x.SchoolId == _user.SchoolId).ToList();

            var mainList = list.Select(x => x.tb_SchoolModuleHome.Id).Distinct().ToList();
            foreach (var item in mainList)
            {
                string subIdString = "";
                long main = Convert.ToInt32(item);
                var sub = list.Where(x => x.tb_SchoolModuleHome.Id == main).ToList();
                MainModuleList one = new MainModuleList();
                one.Id = sub.FirstOrDefault().Id;
                one.ModuleName = sub.FirstOrDefault().tb_SchoolModuleHome.MainModule;
                one.subList = new List<SubModuleList>();
                foreach (var a in sub)
                {
                    SubModuleList b = new SubModuleList();
                    b.MainId = one.Id;
                    b.Id = a.SchoolSubModuleId;
                    b.SubMosule = a.tb_SchoolSubModule.SchoolSubModule;
                    one.subList.Add(b);
                    if (subIdString == "")
                        subIdString = a.SchoolSubModuleId.ToString();
                    else
                        subIdString = subIdString + "," + a.SchoolSubModuleId.ToString();
                }
                one.subIdListString = subIdString;
                model.mainList.Add(one);
            }
            model.IsAdmin = false;
            return View(model);
        }

        [HttpPost]
        public object SubmitAddUserModule(UserModuleModel model)
        {
            string msg = "Failed";
            bool status = false;
            if (_Entities.tb_UserModuleMain.Any(x => x.UserTypeName.ToUpper().Trim() == model.UserTypeName.ToUpper().Trim() && x.SchoolId == _user.SchoolId && x.IsActive))
            {
                msg = "This user type already exists!";
            }
            else
            {
                var main = _Entities.tb_UserModuleMain.Create();
                main.IsTeacher = Convert.ToBoolean(model.userType);
                main.SchoolId = _user.SchoolId;
                main.UserTypeName = model.UserTypeName;
                main.IsActive = true;
                main.TimeStamp = CurrentTime;
                main.IsAdmin = model.IsAdmin;
                _Entities.tb_UserModuleMain.Add(main);
                status = _Entities.SaveChanges() > 0;
                if (status)
                {
                    foreach (var item in model.subListOnly)
                    {
                        var mainDetails = new TrackTap.Data.SchoolSubModule(item.Id);
                        var sub = _Entities.tb_UserModuleDetails.Create();
                        sub.UserModuleId = main.Id;
                        sub.MainId = mainDetails.MainId;
                        sub.SubModuleId = item.Id;
                        sub.IsActive = true;
                        sub.TimeStamp = CurrentTime;
                        _Entities.tb_UserModuleDetails.Add(sub);
                        status = _Entities.SaveChanges() > 0;
                    }
                }
            }
            if (status)
                msg = "Successfull";
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        public IActionResult UserModuleListView()
        {
            int count = 0;
            UserTypeListData model = new UserTypeListData();
            model.list = new List<ModelLists>();
            var data = _Entities.tb_UserModuleDetails.Where(x => x.tb_UserModuleMain.SchoolId == _user.SchoolId && x.IsActive).OrderBy(x => x.tb_SchoolSubModule.SchoolSubModule).OrderBy(x => x.tb_SchoolModuleHome.MainModule).OrderBy(x => x.tb_UserModuleMain.UserTypeName).ToList();
            foreach (var item in data)
            {
                var isactive = _Entities.tb_SchoolModuleDetails.Where(x => x.SchoolSubModuleId == item.SubModuleId && x.IsActive && x.SchoolId == _user.SchoolId).Count();
                if (isactive > 0)
                {
                    count = count + 1;
                    ModelLists one = new ModelLists();
                    one.Id = item.tb_UserModuleMain.Id;
                    one.UserType = item.tb_UserModuleMain.UserTypeName;
                    one.Main = item.tb_SchoolModuleHome.MainModule;
                    one.Sub = item.tb_SchoolSubModule.SchoolSubModule;
                    one.SlNo = count;
                    if (item.tb_UserModuleMain.IsTeacher == true)
                        one.Type = "Teacher";
                    else
                        one.Type = "Staff";
                    one.SubId = item.Id;
                    model.list.Add(one);
                }
                               
            }
            return View(model);
        }

        public object DeleteModule(string id)
        {
            bool status = false;
            string msg = "Failed";
            long Id = Convert.ToInt64(id);
            var module = _Entities.tb_UserModuleDetails.Where(x => x.Id == Id).FirstOrDefault();
            module.IsActive = false;
            status = _Entities.SaveChanges() > 0;
            if (status)
            {
                var mainData = _Entities.tb_UserModuleDetails.Where(x => x.UserModuleId == module.UserModuleId && x.IsActive).FirstOrDefault();
                if (mainData != null)
                {

                }
                else
                {
                    var main = _Entities.tb_UserModuleMain.Where(x => x.Id == module.UserModuleId).FirstOrDefault();
                    main.IsActive = false;
                    _Entities.SaveChanges();
                    if (main.IsTeacher == true)
                    {
                        var teachers = _Entities.tb_Teacher.Where(x => x.UserType == main.Id && x.IsActive).ToList();
                        foreach (var item in teachers)
                        {
                            item.UserType = null;
                            _Entities.SaveChanges();
                        }
                    }
                    else
                    {
                        var staff = _Entities.tb_Staff.Where(x => x.UserType == main.Id && x.IsActive).ToList();
                        foreach (var item in staff)
                        {
                            item.UserType = null;
                            _Entities.SaveChanges();
                        }
                    }
                }
                msg = "Successful";
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);

        }
        public IActionResult UserTypeDataEdit()
        {
            UserModuleModel model = new UserModuleModel();
            model.SchoolId = _user.SchoolId;
            model.mainList = new List<MainModuleList>();
            return View(model);
        }
        public PartialViewResult EditListOfUserType(string id)
        {
            string[] splitData = id.Split('~');
            var UserType = Convert.ToInt64(splitData[0]);
            var TypeId = Convert.ToInt64(splitData[1]);

            var userData = _Entities.tb_UserModuleDetails.Where(x => x.UserModuleId == UserType).ToList();

            UserModuleModel model = new UserModuleModel();
            model.SchoolId = _user.SchoolId;
            model.mainList = new List<MainModuleList>();
            var list = _Entities.tb_SchoolModuleDetails.Where(x => x.IsActive && x.SchoolId == _user.SchoolId).ToList();

            var mainList = list.Select(x => x.tb_SchoolModuleHome.Id).Distinct().ToList();
            foreach (var item in mainList)
            {
                string subIdString = "";
                long main = Convert.ToInt32(item);
                var sub = list.Where(x => x.tb_SchoolModuleHome.Id == main).ToList();
                var userMainExists = userData.Where(x => x.tb_SchoolModuleHome.Id == main && x.IsActive).ToList();
                MainModuleList one = new MainModuleList();
                one.Id = sub.FirstOrDefault().Id;
                one.ModuleName = sub.FirstOrDefault().tb_SchoolModuleHome.MainModule;
                if (userMainExists.Count == sub.Count)
                    one.IsExistsMain = true;
                else
                    one.IsExistsMain = false;
                one.subList = new List<SubModuleList>();
                foreach (var a in sub)
                {
                    SubModuleList b = new SubModuleList();
                    b.MainId = one.Id;
                    b.Id = a.SchoolSubModuleId;
                    b.SubMosule = a.tb_SchoolSubModule.SchoolSubModule;
                    one.subList.Add(b);
                    if (subIdString == "")
                        subIdString = a.SchoolSubModuleId.ToString();
                    else
                        subIdString = subIdString + "," + a.SchoolSubModuleId.ToString();
                    var userSubExists = userData.Where(x => x.SubModuleId == a.SchoolSubModuleId && x.IsActive).FirstOrDefault();
                    if (userSubExists != null)
                        b.IsExists = true;
                    else
                        b.IsExists = false;
                }
                one.subIdListString = subIdString;
                model.mainList.Add(one);
            }
            return PartialView("~/Views/UserModule/_pv_Edit_UserType.cshtml", model);


        }

        public object LoadAllUserTypes(string id)
        {
            string[] splitData = id.Split('~');
            int IsTeacher = Convert.ToInt32(splitData[0]);
            long schoolId = Convert.ToInt64(splitData[1]);
            if (IsTeacher == 0)//Staff
            {
                var result = TrackTap.Data.DropdownData.GetAllStaffUserTypesData(schoolId);
                return Json(new { status = result.Count > 0, list = result }, JsonRequestBehavior.AllowGet);
            }
            else // Teacher
            {
                var result = TrackTap.Data.DropdownData.GetAllTeacherUserTypeData(schoolId);
                return Json(new { status = result.Count > 0, list = result }, JsonRequestBehavior.AllowGet);
            }
        }

        public object CheckThatIsAnAdmin(string id)
        {
            long usertTypeId = Convert.ToInt64(id);
            string msg = "Failed";
            bool status = false;
            bool isAdmin = false;
            var data = _Entities.tb_UserModuleMain.Where(x => x.Id == usertTypeId && x.IsActive).FirstOrDefault();
            if (data != null)
                isAdmin = data.IsAdmin ?? false;
            if (status)
                msg = "Successfull";
            return Json(new { status = status, msg = msg, isAdmin = isAdmin }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public object SubmitEditUserModule(UserModuleModel model)
        {
            string msg = "Failed";
            bool status = false;
            var data1 = _Entities.tb_UserModuleMain.Where(x => x.Id == model.UserTypeId).FirstOrDefault(); // For change the admin crdentials to the main table
            if (data1 != null)
            {
                if (data1.IsAdmin == model.IsAdmin)
                {

                }
                else
                {
                    data1.IsAdmin = model.IsAdmin;
                    _Entities.SaveChanges();
                }

            }
            var UserModuleDetails = _Entities.tb_UserModuleDetails.Where(x => x.UserModuleId == data1.Id && x.IsActive).ToList();
            if (UserModuleDetails != null)
            {
                foreach (var item in UserModuleDetails)
                {
                    //Remove old data from submodules
                    item.IsActive = false;
                    _Entities.SaveChanges();
                }
            }

            foreach (var item in model.subListOnly)
            {
                var mainDetails = new TrackTap.Data.SchoolSubModule(item.Id);
                var addData = _Entities.tb_UserModuleDetails.Create();
                addData.UserModuleId = model.UserTypeId;
                addData.MainId = mainDetails.MainId;
                addData.SubModuleId = item.Id;
                addData.IsActive = true;
                addData.TimeStamp = CurrentTime;
                _Entities.tb_UserModuleDetails.Add(addData);
                status = _Entities.SaveChanges() > 0;
            }
                       
            if (status)
                msg = "Successfull";
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }


        #endregion
    }
}