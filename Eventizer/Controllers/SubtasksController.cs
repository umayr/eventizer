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
    public class SubtasksController : Controller
    {

        public ActionResult View(int id)
        {
            try
            {
                Subtask S = Subtask.FetchSubtaskByID(id);
                if (S == null)
                {
                    return null; // Need to throw an Error view
                }

                Current C = new Current();
                C.Subtask = S;
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
            ViewBag.Subtasks = Subtask.FetchAllSubtasks((new Database()).SelectAllFromView("vw_all_subtasks"));
            InjectEmployeeDetails();
            Current C = new Current();
            C.Employee = (Employee)Session["Employee"];
            return View(C);
        }
        public ActionResult CreatedBy(int id)
        {
            ViewBag.Subtasks = Subtask.FetchAllSubtasks((new Database()).SelectFromViewWithWhere("vw_all_subtasks", "created_by = " + id));
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
            ViewBag.Subtasks = Subtask.FetchAllSubtasks((new Database()).SelectFromViewWithWhere("vw_all_subtasks", "created_by = " + C.Employee.ID));
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
