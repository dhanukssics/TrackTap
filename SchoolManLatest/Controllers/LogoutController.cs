using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using System.Web.Security;

namespace TrackTap.Controllers
{
    public class LogoutController : Controller
    {
        //
        // GET: /Logout/


        public IActionResult SchoolLogout()
        {
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("../Account/SchoolLogin");
        }
        public IActionResult ParentLogout()
        {
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("../Account/ParentLogin");
        }
        public IActionResult StaffLogout()
        {
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("../Account/StaffLogin");
        }
        public IActionResult AdminLogout()
        {
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("../Account/SuperAdminLogin");
        }
    }
}
