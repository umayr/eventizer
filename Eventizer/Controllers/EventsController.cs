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
    public class EventsController : Controller
    {

        public ActionResult View(int id)
        {
            try
            {
                Event E = Event.FetchEventByID(id);
                if (E == null)
                {
                    return null; // Need to throw an Error view
                }
                InjectEmployeeDetails();
                Current C = new Current();
                C.Event = E;
                C.Employee = (Employee)Session["Employee"];
                return View(C);
            }
            catch (Exception)
            {
                return null; // Need to throw an Error view
            }

        }
        public ActionResult Index()
        {
            ViewBag.Events = Event.FetchAllEvents((new Database()).SelectAllFromView("vw_all_events"));
            InjectEmployeeDetails();
            Current C = new Current();
            C.Employee = (Employee)Session["Employee"];
            return View(C);
        }
        public ActionResult CreatedBy(int id)
        {
            ViewBag.Events = Event.FetchAllEvents((new Database()).SelectFromViewWithWhere("vw_all_events", "created_by = " + id));
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
            ViewBag.Events = Event.FetchAllEvents((new Database()).SelectFromViewWithWhere("vw_all_events", "created_by = " + C.Employee.ID));
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
