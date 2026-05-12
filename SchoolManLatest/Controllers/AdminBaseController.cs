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
    public class AdminBaseController : Controller
    {
        public SchoolRepository _schoolRepository = new SchoolRepository();
        public ParentRepository _parentRepository = new ParentRepository();
        public TeacherRepository _teacherRepository = new TeacherRepository();
        public DateTime CurrentTime = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.Now.ToUniversalTime(), TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
        public tb_tracktapEntities _Entities = new tb_tracktapEntities();
        public tb_School _schoolUser;
        public tb_Parent _parentUser;
        public tb_Login _user;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (User != null && User.Identity.IsAuthenticated)
            {

                var routeValues = HttpContext.Request.RequestContext.RouteData.Values;

            }

            else
            {
                filterContext.Result = new RedirectResult("/Account/Home");
                return;
            }
        }

    }

}
