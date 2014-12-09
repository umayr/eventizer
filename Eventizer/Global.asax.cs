using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Eventizer
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                    "All Assets", // Route name
                    "Dashboard/assets/", // URL with parameters
                    new { controller = "Assets", action = "Index" } // Parameter defaults
                );

            routes.MapRoute(
                    "All Employee", // Route name
                    "Dashboard/employees", // URL with parameters
                    new { controller = "Employee", action = "Index" } // Parameter defaults
                );
            routes.MapRoute(
                    "Register Employee", // Route name
                    "Dashboard/employee/add", // URL with parameters
                    new { controller = "Employee", action = "Add" } // Parameter defaults
                );
            routes.MapRoute(
                   "View Employee Details", // Route name
                   "Dashboard/employees/view/{id}", // URL with parameters
                   new { controller = "Employee", action = "View", id = UrlParameter.Optional } // Parameter defaults
               );


            #region Event Routes
            routes.MapRoute(
                    "Events by all", // Route name
                    "Dashboard/events/", // URL with parameters
                    new { controller = "Events", action = "Index" } // Parameter defaults
                );


            routes.MapRoute(
                "Events", // Route name
                "Dashboard/events/view/{id}", // URL with parameters
                new { controller = "Events", action = "View", id = UrlParameter.Optional }, // Parameter defaults
                new { id = @"\d+" }
            );

            routes.MapRoute(
                "Events by ID", // Route name
                "Dashboard/events/by/{id}", // URL with parameters
                new { controller = "Events", action = "CreatedBy", id = UrlParameter.Optional }, // Parameter defaults
                new { id = @"\d+" }
            );

            routes.MapRoute(
                "Events by me", // Route name
                "Dashboard/events/by/me", // URL with parameters
                new { controller = "Events", action = "Me" }
            );

            #endregion
            #region Task Routes
            routes.MapRoute(
                    "Tasks by all", // Route name
                    "Dashboard/tasks/", // URL with parameters
                    new { controller = "Tasks", action = "Index" } // Parameter defaults
                );


            routes.MapRoute(
                "Tasks", // Route name
                "Dashboard/tasks/view/{id}", // URL with parameters
                new { controller = "Tasks", action = "View", id = UrlParameter.Optional }, // Parameter defaults
                new { id = @"\d+" }
            );

            routes.MapRoute(
                "Tasks by ID", // Route name
                "Dashboard/tasks/by/{id}", // URL with parameters
                new { controller = "Tasks", action = "CreatedBy", id = UrlParameter.Optional }, // Parameter defaults
                new { id = @"\d+" }
            );

            routes.MapRoute(
                "Tasks by me", // Route name
                "Dashboard/tasks/by/me", // URL with parameters
                new { controller = "Tasks", action = "Me" }
            );

            #endregion
            #region Subtask Routes
            routes.MapRoute(
                    "Subtasks by all", // Route name
                    "Dashboard/subtasks/", // URL with parameters
                    new { controller = "Subtasks", action = "Index" } // Parameter defaults
                );


            routes.MapRoute(
                "Subtasks", // Route name
                "Dashboard/subtasks/view/{id}", // URL with parameters
                new { controller = "Subtasks", action = "View", id = UrlParameter.Optional }, // Parameter defaults
                new { id = @"\d+" }
            );

            routes.MapRoute(
                "Subtasks by ID", // Route name
                "Dashboard/subtasks/by/{id}", // URL with parameters
                new { controller = "Subtasks", action = "CreatedBy", id = UrlParameter.Optional }, // Parameter defaults
                new { id = @"\d+" }
            );

            routes.MapRoute(
                "Subtasks by me", // Route name
                "Dashboard/subtasks/by/me", // URL with parameters
                new { controller = "Subtasks", action = "Me" }
            );

            #endregion

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            Database.DefaultConnectionFactory = new SqlConnectionFactory(@"Data Source=(localdb)\v11.0; Integrated Security=True; MultipleActiveResultSets=True");

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);


        }
        protected void Application_Error(object sender, EventArgs e)
        {

            var ex = Server.GetLastError().GetBaseException();

            Server.ClearError();
            var routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("action", "Index");

            if (ex.GetType() == typeof(HttpException))
            {
                var httpException = (HttpException)ex;
                var code = httpException.GetHttpCode();
                routeData.Values.Add("status", code);
            }
            else
            {
                routeData.Values.Add("status", 500);
            }

            routeData.Values.Add("error", ex);

            IController errorController = new Eventizer.Controllers.ErrorController();
            errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            if (Context.Response.StatusCode == 401)
            { // this is important, because the 401 is not an error by default!!!
                throw new HttpException(401, "You are not authorised");
            }
        }
    }
}