using Eventizer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web;
using System.Web.Mvc;
using Eventizer.Models;

namespace Eventizer.Controllers
{
    public class RegisterController : Controller
    {
        //
        // GET: /Register/

        public ActionResult Index()
        {
            MiniEmployeesList o = Employee.GetEmployeeDetails();
            ViewBag.EmployeeNames = o.Names;
            ViewBag.EmployeeIds = o.Ids;

            return View();
        }

        
    }
}
