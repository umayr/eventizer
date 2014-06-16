using Eventizer.Helpers;
using Eventizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eventizer.Controllers
{
    public class AssetsController : Controller
    {
        //
        // GET: /Assets/

        public ActionResult Index()
        {
            ViewBag.Assets = Asset.FetchAllAssets((new Database()).SelectAllFromView("assets"));
            InjectEmployeeDetails();
            Current C = new Current();
            C.Employee = (Employee)Session["Employee"];
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
