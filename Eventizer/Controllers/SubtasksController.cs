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
                return View(C);
            }
            catch (Exception)
            {
                return null; // Need to throw an Error view
            }

        }
        //public ActionResult Index()
        //{
        //    ViewBag.Events = Event.FetchAllEvents((new Database()).SelectAllFromView("vw_all_events"));
        //    return View();
        //}
    }
}
