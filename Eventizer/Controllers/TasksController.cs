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
    public class TasksController : Controller
    {
        
        public ActionResult View(int id)
        {
            try
            {
                Task T = Task.FetchTaskByID(id);
                if (T == null)
                {
                    return null; // Need to throw an Error view
                }

                Current C = new Current();
                C.Task = T;
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
