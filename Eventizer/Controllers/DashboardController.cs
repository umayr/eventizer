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
                Current C = new Current();
                C.Employee = (Employee)Session["Employee"];
                ViewBag.Feeds = Feed.FetchAllFeeds();
                return View(C);
            }
            else { return RedirectToAction("Index", "Home"); }
        }

    }
}
