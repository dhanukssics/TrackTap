
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
    public class BaseController : Controller
    {
        public SchoolRepository _schoolRepository = new SchoolRepository();
        public ParentRepository _parentRepository = new ParentRepository();
        public TeacherRepository _teacherRepository = new TeacherRepository();
        public DateTime CurrentTime = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.Now.ToUniversalTime(), TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
        public tb_tracktapEntities _Entities = new tb_tracktapEntities();
        public tb_School _schoolUser;
        public tb_Parent _parentUser;
        public tb_Login _user;
        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    if (User != null && User.Identity.IsAuthenticated)
        //    {
        //        if (Session["School"] != null)
        //        {
        //            var userId = long.Parse(User.Identity.Name);
        //            var user = _schoolRepository.getUserById(userId);
        //            Session["School"] = user;
        //            _schoolUser = (tb_School)Session["School"];
        //            return;
        //        }
        //        else
        //        {
        //            filterContext.Result = new RedirectResult("/Account/LoginPage");
        //            return;
        //        }

        //    }
        //    else
        //    {
        //        filterContext.Result = new RedirectResult("/Account/LoginPage");
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
                    if ((userType == (int)UserRole.School) || (userType == (int)UserRole.Staff) || (userType == (int)UserRole.Teacher))
                    {
                        var userId = long.Parse(User.Identity.Name);
                        if (Session["User"] == null)
                        {
                            var user = _Entities.tb_Login.Where(x => x.UserId == userId).FirstOrDefault();
                            Session["User"] = user;
                            Session["UserType"] = userType;
                         }
                        _user = (tb_Login)Session["User"];
                        var routeValues = HttpContext.Request.RequestContext.RouteData.Values;
                    }
                    else if (userType == (int)UserRole.Parent)
                    {
                        var parentId = long.Parse(User.Identity.Name);
                        if (Session["User"] == null)
                        {
                            var parent = _Entities.tb_Parent.Where(x => x.ParentId == parentId).FirstOrDefault();
                            _parentUser = parent;
                            Session["Parent"] = parent;
                            Session["UserType"] = userType;
                        }
                        _parentUser = (tb_Parent)Session["Parent"];
                        var routeValues = HttpContext.Request.RequestContext.RouteData.Values;
                    }
                }

                else
                {
                    filterContext.Result = new RedirectResult("/Account/Home");
                    return;
                }
            }
            else
            {
                filterContext.Result = new RedirectResult("/Account/Home");
                return;
            }
        }

    }
}  