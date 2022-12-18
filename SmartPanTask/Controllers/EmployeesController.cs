using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SmartPanTask.Models;
using System.IO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SmartPanTask.Controllers
{
    public class EmployeesController : Controller
    {
        private SmartPanEntities db = new SmartPanEntities();

        [Authorize(Roles = "AdminRole, ManagerRole")]
        // GET: Employees
        public ActionResult Index()
        {
            var employers = db.Employees.ToList();
            var userid = User.Identity.GetUserId();
            if (User.IsInRole("AdminRole"))
            {
                List<string> ManagerName = new List<string>();
                employers = db.Employees.Include(e => e.AspNetUser).Include(e => e.Department).Where(a => a.Type == "Employee").ToList();
                foreach (var item in employers)
                {
                    var managerFirst = db.Employees.Where(a => a.Id == item.ManagerID).FirstOrDefault().FirstName;
                    var managerLast = db.Employees.Where(a => a.Id == item.ManagerID).FirstOrDefault().LastName;
                    string Totalname = managerFirst + " " + managerLast;
                    ManagerName.Add(Totalname);
                }
                ViewBag.managername = ManagerName;
            }
            else
            {
                List<string> ManagerName = new List<string>();
                var managerid = db.Employees.Where(a => a.UserId == userid).FirstOrDefault().Id;
                employers = db.Employees.Where(a => a.Type == "Employee" && a.ManagerID == managerid).ToList();
                foreach (var item in employers)
                {
                    var managerFirst = db.Employees.Where(a => a.Id == item.ManagerID).FirstOrDefault().FirstName;
                    var managerLast = db.Employees.Where(a => a.Id == item.ManagerID).FirstOrDefault().LastName;
                    string Totalname = managerFirst + " " + managerLast;
                    ManagerName.Add(Totalname);
                }
                ViewBag.managername = ManagerName;
            }
           
            return View(employers);
        }


        [Authorize]
        // GET: Employees/Create
        public ActionResult Create()
        {

            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.managername = db.Employees.Where(a => a.Type == "Manager").ToList();
            ViewBag.DepartmentID = new SelectList(db.Departments, "Id", "DepartmentName");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                var userid = User.Identity.GetUserId();
                var countusers = db.Employees.Where(a => a.UserId == userid).ToList().Count();
                if(countusers != 0)
                {
                    TempData["Failed"] = "You are already register before for this form.";
                    return Redirect("~/Employees/Create");
                }
                else if (User.IsInRole("AdminRole"))
                {
                    TempData["Failed"] = "You are an admin, couldn't apply this form";
                    return Redirect("~/Employees/Index");
                }

                if (Image != null)
                {
                    String unique = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    var filename = Path.GetFileName(Image.FileName);
                    var path = Server.MapPath("~/Content/Images/" + unique + filename);
                    var path1 = "Content/Images/" + unique + filename;
                    Image.SaveAs(path);
                    employee.Image = path1;
                }
                if (employee.Type == "Manager")
                {
                    var context = new ApplicationDbContext();
                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);
                    userManager.AddToRole(userid, "ManagerRole");
                }
                if (employee.Type == "Employee")
                {
                    var context = new ApplicationDbContext();
                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);
                    userManager.AddToRole(userid, "EmployeeRole");
                }
                employee.UserId = userid;
                db.Employees.Add(employee);
                db.SaveChanges();
                return Redirect("~/Home/Index");
            }

            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", employee.UserId);
            ViewBag.DepartmentID = new SelectList(db.Departments, "Id", "DepartmentName", employee.DepartmentID);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", employee.UserId);
            ViewBag.DepartmentID = new SelectList(db.Departments, "Id", "DepartmentName", employee.DepartmentID);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Salary,Image,ManagerID,DepartmentID,UserId,Type")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", employee.UserId);
            ViewBag.DepartmentID = new SelectList(db.Departments, "Id", "DepartmentName", employee.DepartmentID);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            var imagepath = employee.Image;
            string fullPath = Request.MapPath("~/" + imagepath);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
