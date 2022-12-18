using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SmartPanTask.Models;
using Microsoft.AspNet.Identity;

namespace SmartPanTask.Controllers
{
    [Authorize(Roles = "EmployeeRole")]
    public class MyTasksController : Controller
    {
        private SmartPanEntities db = new SmartPanEntities();

        // GET: MyTasks
        public ActionResult Index()
        {
            var userid = User.Identity.GetUserId();
            var employeeid = db.Employees.Where(a => a.UserId == userid).FirstOrDefault().Id;
            //if(employeeid == null)
            //{
            //    TempData["failed"] = "You have been removed by the system, Conact with them or Register a new Acoount";
            //    return Redirect("~/Account/Login");
            //}
            var employeeTasks = db.EmployeeTasks.Include(e => e.Employee).Where(a => a.Employee.Id == employeeid);
            return View(employeeTasks.ToList());
        }


        // GET: MyTasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeTask employeeTask = db.EmployeeTasks.Find(id);
            if (employeeTask == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeID = new SelectList(db.Employees, "Id", "FirstName", employeeTask.EmployeeID);
            return View(employeeTask);
        }

        // POST: MyTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TaskTitle,TaskDescription,EmployeeID,DateAssigned,TaskStatus,DateUpdated,DateStarted")] EmployeeTask employeeTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employeeTask).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeID = new SelectList(db.Employees, "Id", "FirstName", employeeTask.EmployeeID);
            return View(employeeTask);
        }

        // GET: MyTasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeTask employeeTask = db.EmployeeTasks.Find(id);
            if (employeeTask == null)
            {
                return HttpNotFound();
            }
            return View(employeeTask);
        }

        // POST: MyTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmployeeTask employeeTask = db.EmployeeTasks.Find(id);
            db.EmployeeTasks.Remove(employeeTask);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult StartTask(int TaskId)
        {

            if (TaskId != null)
            {
                EmployeeTask EmpTask = db.EmployeeTasks.Find(TaskId);
                EmpTask.TaskStatus = "Started";
                EmpTask.DateStarted = DateTime.Now;
                db.Entry(EmpTask).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { message = "You Started This Task successfully, you now work in this task.", status = EmpTask.TaskStatus, startdate = EmpTask.DateStarted.ToString() });
            }
            return Json(new { message = "Something went wrong" });
        }
        public ActionResult UpdateTask(int TaskId, string TaskStats)
        {

            if (TaskId != null)
            {
                EmployeeTask EmpTask = db.EmployeeTasks.Find(TaskId);
                EmpTask.TaskStatus = TaskStats;
                EmpTask.DateUpdated = DateTime.Now;
                db.Entry(EmpTask).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { message = "You Updated This Task successfully, you are now "+TaskStats+" this task.", status = EmpTask.TaskStatus, updatedate = EmpTask.DateUpdated.ToString() });
            }
            return Json(new { message = "Something went wrong" });
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
