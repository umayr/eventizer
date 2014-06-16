using Eventizer.Helpers;
using Eventizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eventizer.Controllers
{
    public class TasksController : Controller
    {

        public ActionResult View(int id)
        {
            try
            {
                Task T = Task.FetchTaskByID(id);
                if (T == null)
                {
                    return null; // Need to throw an Error view
                }

                Current C = new Current();
                C.Task = T;
                C.Employee = (Employee)Session["Employee"];
                InjectEmployeeDetails();
                return View(C);
            }
            catch (Exception)
            {
                return null; // Need to throw an Error view
            }

        }
        public ActionResult Index()
        {
            ViewBag.Tasks = Task.FetchAllTasks((new Database()).SelectAllFromView("vw_all_tasks"));
            InjectEmployeeDetails();
            Current C = new Current();
            C.Employee = (Employee)Session["Employee"];
            return View(C);
        }
        public ActionResult CreatedBy(int id)
        {
            ViewBag.Tasks = Task.FetchAllTasks((new Database()).SelectFromViewWithWhere("vw_all_tasks", "created_by = " + id));
            InjectEmployeeDetails();
            Current C = new Current();
            C.Employee = (Employee)Session["Employee"];
            return View(C);
        }
        public ActionResult Me()
        {
            InjectEmployeeDetails();
            Current C = new Current();
            C.Employee = (Employee)Session["Employee"];
            ViewBag.Tasks = Task.FetchAllTasks((new Database()).SelectFromViewWithWhere("vw_all_tasks", "created_by = " + C.Employee.ID));
            return View(C);
        }

        /// <summary>
        /// Injecting Employee Details into Viewbag 
        /// for Assignation.
        /// </summary>
        private void InjectEmployeeDetails()
        {
            MiniEmployeesList o = Employee.GetEmployeeDetails();
            ViewBag.EmployeeNames = o.Names;
            ViewBag.EmployeeIds = o.Ids;
        }
    }
}
