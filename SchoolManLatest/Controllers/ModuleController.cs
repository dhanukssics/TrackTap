using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using TrackTap.Models;

namespace TrackTap.Controllers
{
    public class ModuleController : AdminBaseController
    {
        // GET: Module
        public IActionResult Index()
        {
            return View();
        }

        #region SuperAdmin school module
        public IActionResult SchoolTypeDefineHome()
        {
            SchoolModuleModel model = new SchoolModuleModel();
            //  model.SchoolId = _user.SchoolId;
            model.mainList = new List<SchoolMainModuleList>();
            var list = _Entities.tb_SchoolSubModule.Where(x => x.IsActive).ToList();
            var mainList = list.Select(x => x.tb_SchoolModuleHome.MainModule).Distinct().ToList();
            foreach (var item in mainList)
            {
                string subIdString = "";
                string main = Convert.ToString(item);
                var sub = list.Where(x => x.tb_SchoolModuleHome.MainModule == main).ToList();
                SchoolMainModuleList one = new SchoolMainModuleList();
                one.Id = sub.FirstOrDefault().Id;
                one.ModuleName = main;
                one.subList = new List<SchoolSubModuleList>();
                foreach (var a in sub)
                {
                    SchoolSubModuleList b = new SchoolSubModuleList();
                    b.MainId = one.Id;
                    b.Id = a.Id;
                    b.SubMosule = a.SchoolSubModule;
                    one.subList.Add(b);
                    if (subIdString == "")
                        subIdString = a.Id.ToString();
                    else
                        subIdString = subIdString + "," + a.Id.ToString();
                }
                one.subIdListString = subIdString;
                model.mainList.Add(one);
            }
            //model.IsAdmin = false;
            return View(model);
        }

        [HttpPost]
        public object SubmitAddSchoolModule(SchoolModuleModel model)
        {
            string msg = "Failed";
            bool status = false;
            if (_Entities.tb_SchoolModuleMain.Any(x => x.SchoolName.ToUpper().Trim() == model.SchoolName.ToUpper().Trim() && x.SchoolId == model.SchoolId && x.IsActive))
            {
                msg = "This School module already exists!";
            }
            else
            {
                var main = _Entities.tb_SchoolModuleMain.Create();
                main.SchoolId = model.SchoolId;
                main.SchoolName = model.SchoolName;
                main.IsActive = true;
                main.TimeStamp = CurrentTime;
                _Entities.tb_SchoolModuleMain.Add(main);
                status = _Entities.SaveChanges() > 0;
                if (status)
                {
                    foreach (var item in model.subListOnly)
                    {
                        var mainDetails = new TrackTap.DataLibrary.Data.SchoolSubModule(item.Id);
                        var sub = _Entities.tb_SchoolModuleDetails.Create();
                        sub.SchoolModuleId = main.Id;
                        sub.MainId = mainDetails.MainId;
                        sub.SchoolSubModuleId = item.Id;
                        sub.IsActive = true;
                        sub.TimeStamp = CurrentTime;
                        sub.SchoolId = model.SchoolId;
                        _Entities.tb_SchoolModuleDetails.Add(sub);
                        status = _Entities.SaveChanges() > 0;
                    }
                }
                if(status)
                {
                    var school = _Entities.tb_School.Where(x => x.SchoolId == model.SchoolId).ToList();
                    if (school != null)
                    {
                        foreach (var item in school)
                        {                            
                            item.SchoolType = main.Id;
                            _Entities.SaveChanges();
                        }
                    }

                }
            }
            if (status)
                msg = "Successfull";
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        public IActionResult SchoolModuleListView()
        {
            int count = 0;
            SchoolListdata model = new SchoolListdata();
            model.list = new List<ModelList>();
            var data = _Entities.tb_SchoolModuleDetails.Where(x => x.IsActive).OrderBy(x => x.tb_SchoolSubModule.SchoolSubModule).OrderBy(x => x.tb_SchoolModuleHome.MainModule).OrderBy(x => x.tb_SchoolModuleMain.SchoolName).ToList();
            foreach (var item in data)
            {
                count = count + 1;
                ModelList one = new ModelList();
                one.Id = item.tb_SchoolModuleMain.Id;
                one.Schoolname = item.tb_SchoolModuleMain.SchoolName;
                one.Main = item.tb_SchoolModuleHome.MainModule;
                one.Sub = item.tb_SchoolSubModule.SchoolSubModule;
                one.SlNo = count;
                one.SubId = item.Id;
                model.list.Add(one);
            }
            return View(model);
        }

        public object DeleteModule(string id)
        {
            bool status = false;
            string msg = "Failed";
            long Id = Convert.ToInt64(id);
            var module = _Entities.tb_SchoolModuleDetails.Where(x => x.Id == Id).FirstOrDefault();
            module.IsActive = false;
            status = _Entities.SaveChanges() > 0;
            if (status)
            {
                //Also Remove from usermodule table also
                var subusermodule = _Entities.tb_UserModuleDetails.Where(x => x.tb_UserModuleMain.SchoolId == module.SchoolId && x.SubModuleId == module.SchoolSubModuleId).FirstOrDefault();
                subusermodule.IsActive = false;
                status = _Entities.SaveChanges() > 0;

                //Remove end here


                var mainData = _Entities.tb_SchoolModuleDetails.Where(x => x.SchoolModuleId == module.SchoolModuleId && x.IsActive).FirstOrDefault();
                if (mainData != null)
                {

                }
                else
                {
                    var main = _Entities.tb_SchoolModuleMain.Where(x => x.Id == module.SchoolModuleId).FirstOrDefault();
                    main.IsActive = false;
                    _Entities.SaveChanges();
                }
                msg = "Successful";
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);

        }

        public IActionResult SchoolTypeDataEdit()
        {
            SchoolModuleModel model = new SchoolModuleModel();
            // model.SchoolId = _user.SchoolId;
            model.mainList = new List<SchoolMainModuleList>();
            return View(model);
        }

        public PartialViewResult EditListOfSchoolType(string id)
        {
            var Schoolid = Convert.ToInt64(id);


            var userData = _Entities.tb_SchoolModuleDetails.Where(x => x.SchoolId == Schoolid).ToList();

            SchoolModuleModel model = new SchoolModuleModel();
            model.SchoolId = Schoolid;
            model.mainList = new List<SchoolMainModuleList>();
            var list = _Entities.tb_SchoolSubModule.Where(x => x.IsActive).ToList();
            var mainList = list.Select(x => x.tb_SchoolModuleHome.MainModule).Distinct().ToList();
            foreach (var item in mainList)
            {
                string subIdString = "";
                string main = Convert.ToString(item);
                var sub = list.Where(x => x.tb_SchoolModuleHome.MainModule == main).ToList();
                var userMainExists = userData.Where(x => x.tb_SchoolModuleHome.MainModule == main && x.IsActive).ToList();
                SchoolMainModuleList one = new SchoolMainModuleList();
                one.Id = sub.FirstOrDefault().Id;
                one.ModuleName = main;
                if (userMainExists.Count == sub.Count)
                    one.IsExistsMain = true;
                else
                    one.IsExistsMain = false;
                one.subList = new List<SchoolSubModuleList>();
                foreach (var a in sub)
                {
                    SchoolSubModuleList b = new SchoolSubModuleList();
                    b.MainId = one.Id;
                    b.Id = a.Id;
                    b.SubMosule = a.SchoolSubModule;
                    one.subList.Add(b);
                    if (subIdString == "")
                        subIdString = a.Id.ToString();
                    else
                        subIdString = subIdString + "," + a.Id.ToString();
                    var userSubExists = userData.Where(x => x.SchoolSubModuleId == a.Id && x.IsActive).FirstOrDefault();
                    if (userSubExists != null)
                        b.IsExists = true;
                    else
                        b.IsExists = false;
                }
                one.subIdListString = subIdString;
                model.mainList.Add(one);
            }
            return PartialView("~/Views/Module/_pv_Edit_SchoolModule.cshtml", model);
        }

        [HttpPost]
        public object SubmitEditSchoolModule(SchoolModuleModel model) 
        {
            string msg = "Failed";
            bool status = false;
            var schoolmodulemain = _Entities.tb_SchoolModuleMain.Where(x => x.SchoolId == model.SchoolId).FirstOrDefault(); 
            if (schoolmodulemain != null)
            {
                var schoolsubmodule = _Entities.tb_SchoolModuleDetails.Where(x => x.SchoolModuleId == schoolmodulemain.Id).ToList();
                if (schoolsubmodule != null)
                {
                    foreach (var item in schoolsubmodule)
                    {
                        //Remove old data from submodules
                        item.IsActive = false;
                        _Entities.SaveChanges();
                    }
                }
              
                    foreach (var item in model.subListOnly)
                    {
                        var mainDetails = new TrackTap.DataLibrary.Data.SchoolSubModule(item.Id);
                        var sub = _Entities.tb_SchoolModuleDetails.Create();
                        sub.SchoolModuleId = schoolmodulemain.Id;
                        sub.MainId = mainDetails.MainId;
                        sub.SchoolSubModuleId = item.Id;
                        sub.IsActive = true;
                        sub.TimeStamp = CurrentTime;
                        sub.SchoolId = model.SchoolId;
                        _Entities.tb_SchoolModuleDetails.Add(sub);
                        status = _Entities.SaveChanges() > 0;
                    }
                
            }

            if (status)
                msg = "Successfull";
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        #endregion

      
    }
}