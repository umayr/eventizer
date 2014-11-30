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
    public class EmployeeController : Controller
    {
        //
        // GET: /Employee/

        public ActionResult Index()
        {
            Tuple<List<Employee>, Employee> T = new Tuple<List<Employee>, Employee>(Employee.FetchAllEmployees(), (Employee)Session["Employee"]);
            return View(T);
        }
        public ActionResult Add()
        {
            MiniEmployeesList o = Employee.GetEmployeeDetails();
            ViewBag.EmployeeNames = o.Names;
            ViewBag.EmployeeIds = o.Ids;

            return View((Employee)Session["Employee"]);
        }
        public ActionResult View(int id)
        {
            try
            {
                Employee employee = Employee.FetchEmployeeByID(id);
                if (employee.Name == null) {
                    return RedirectToAction("Index", "Error", new { status = 404 });
                }
                Tuple<Employee, Employee> T = new Tuple<Employee, Employee>((Employee)Session["Employee"], employee);

                List<Event> Events = Event.FetchAllEvents((new Database()).SelectFromViewWithWhere("vw_all_events", "created_by = " + id));
                List<Task> Tasks = Task.FetchAllTasks((new Database()).SelectFromViewWithWhere("vw_all_tasks", "created_by = " + id));
                List<Subtask> Subtasks = Subtask.FetchAllSubtasks((new Database()).SelectFromViewWithWhere("vw_all_subtasks", "created_by = " + id));

                ViewBag.Tasks = Tasks;
                ViewBag.Events = Events;
                ViewBag.Subtasks = Subtasks;
                ViewBag.PendingTasks = Task.GetPendingTasks(Tasks);
                ViewBag.PendingEvents = Event.GetPendingEvents(Events);
                ViewBag.PendingSubtasks = Subtask.GetPendingSubtasks(Subtasks);
                return View(T);
            }
            catch
            {
                throw new HttpException(404, "Not Found");
            }
        }

    }
}
