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

        public bool TestConnectionString()
        {

            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(
                        @"Data Source=199.193.116.14,1433;Network Library=DBMSSOCN;
                        Initial Catalog=eventizer;User ID=umayr;Password=c3ab2k11;"))
            {
                try
                {
                    conn.Open();

                    return (conn.State == System.Data.ConnectionState.Open);
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
