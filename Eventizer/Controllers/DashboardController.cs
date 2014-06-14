using Eventizer.Helpers;
using Eventizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eventizer.Controllers
{
    public class DashboardController : Controller
    {
        //
        // GET: /Dashboard/

        public ActionResult Index()
        {
            if (Essentials.CheckIfAuthenticated())
            {
                //ViewBag.LoggedInEmployee = (Employee)Session["Employee"];
                Current C = new Current();
                C.Employee = (Employee)Session["Employee"];
                return View(C);
            }
            else { return RedirectToAction("Index", "Home"); }
        }

    }
}
