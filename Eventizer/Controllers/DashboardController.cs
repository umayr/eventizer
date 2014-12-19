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
                List<Feed> Feeds = Feed.FetchAllFeeds();
                if (C.Employee.Designation == "Manager")
                {
                    ViewBag.Feeds = Feeds;
                }
                else
                {
                    ViewBag.Feeds =
                        (from feed in Feeds
                        where feed.AssignedTo.ID == C.Employee.ID
                        orderby feed.DateCreated descending
                        select feed).ToList<Feed>();
                }

                return View(C);
            }
            else { return RedirectToAction("Index", "Home"); }
        }

    }
}
