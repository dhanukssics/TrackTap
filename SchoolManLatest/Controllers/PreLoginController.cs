using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using TrackTap.ClassLibrary;
using TrackTap.DataLibrary;
using TrackTap.Repository;

namespace TrackTap.Controllers
{
    public class PreLoginController : Controller
    {
        public DateTime CurrentTime = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.Now.ToUniversalTime(), TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
        public tb_tracktapEntities _Entities = new tb_tracktapEntities();
        public tb_Login _user = new tb_Login();
        public tb_Parent _parentUser = new tb_Parent();

        public SchoolRepository _schoolRepository = new SchoolRepository();
        public ParentRepository _parentRepository = new ParentRepository();
        //public tb_School _schooluser;
        //public tb_Parent _parentUer;
        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    if (User != null && User.Identity.IsAuthenticated)
        //    {
        //        if (Session["School"] == null)
        //        {
        //            var userId = long.Parse(User.Identity.Name);
        //            var user = _schoolRepository.getUserById(userId);
        //            Session["School"] = user;
        //        }
        //        _schooluser = (TrackTap.DataLibrary.tb_School)Session["School"];

        //        filterContext.Result = new RedirectResult("/School/Home", true);
        //        return;
        //    }
        //}
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (User != null && User.Identity.IsAuthenticated)
            {
                if (Request.Cookies["UserType"] != null)
                {
                    long userType = Convert.ToInt16(Server.HtmlEncode(Request.Cookies["UserType"].Value));

                    //if (Session["User"] == null)
                    //{
                    //    var userId = long.Parse(User.Identity.Name);
                    //    var user = _Entities.tb_Login.Where(x => x.UserId == userId).FirstOrDefault();
                    //    Session["User"] = user;
                    //}
                    //_user = (TrackTap.DataLibrary.tb_Login)Session["User"];

                    if (userType == (int)UserRole.School)
                        filterContext.Result = new RedirectResult("/School/Home");
                    else if (userType == (int)UserRole.Staff)
                        filterContext.Result = new RedirectResult("/School/Home");
                    else if (userType == (int)UserRole.Teacher)
                        filterContext.Result = new RedirectResult("/School/Home");
                    else if (userType == (int)UserRole.Parent)
                        filterContext.Result = new RedirectResult("/Parent/Home");
                    return;
                }
            }
        }
    }
}

