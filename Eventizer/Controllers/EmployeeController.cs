using Eventizer.Models;
using System;
using System.Collections.Generic;
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
            Tuple<List<Employee>, Employee> T = new Tuple<List<Employee>,Employee>(Employee.FetchAllEmployees(),(Employee)Session["Employee"]);
            return View(T);
        }
        public ActionResult Add()
        {
            MiniEmployeesList o = Employee.GetEmployeeDetails();
            ViewBag.EmployeeNames = o.Names;
            ViewBag.EmployeeIds = o.Ids;

            return View((Employee)Session["Employee"]);
        }

    }
}
