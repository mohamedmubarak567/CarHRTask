using Microsoft.AspNet.Identity;
using SmartPanTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SmartPanTask.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private SmartPanEntities db = new SmartPanEntities();
        public ActionResult Index()
        {
            if (User.IsInRole("EmployeeRole"))
            {
                var userid = User.Identity.GetUserId();
                var employeeid = db.Employees.Where(a => a.UserId == userid).Count();
                if (employeeid == 0)
                {
                    FormsAuthentication.SignOut();
                    TempData["failed"] = "You have been removed by the system, Conact with them or Register a new Acoount";
                    return Redirect("~/Account/Login");
                }
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //public ActionResult _Blog2()
        //{
        //    var CompanyInfo = db.Companies.Where(c => c.CompanyName == id).FirstOrDefault();
        //    var companyId = db.Companies.Where(c => c.CompanyName == id).FirstOrDefault().UserId;
        //    ViewBag.news = db.News.Where(c => c.Userid == companyId).OrderByDescending(a => a.Id).ToList();
        //    return PartialView(CompanyInfo);
        //}
    }
}