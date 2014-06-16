using Eventizer.Helpers;
using Eventizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eventizer.Controllers
{
    public class AjaxController : Controller
    {

        [HttpPost]
        public int Login(string email, string password)
        {
            try
            {
                List<System.Data.SqlClient.SqlParameter> Params = new List<System.Data.SqlClient.SqlParameter>();
                Params.Add(new System.Data.SqlClient.SqlParameter("@email", email));
                Params.Add(new System.Data.SqlClient.SqlParameter("@password", Essentials.CalculateSHA1(password)));

                object check = (new Database()).ExecProcedureWithReturnValue("usp_login_employee", Params);
                if ((int)check == 1)
                {
                    Employee employee = Employee.FetchEmployeeByEmail(email);
                    Session["Auth"] = true;
                    Session["Employee"] = employee;
                }
                return check != null ? (int)check : -1;
            }
            catch (Exception)
            {
                return -1;
            }

        }
        [HttpPost]
        public int Register(string name, string email, string password, string designation, string phone, int manager_id = 0)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(designation) || string.IsNullOrEmpty(phone))
            {
                return 0;
            }
            try
            {
                List<System.Data.SqlClient.SqlParameter> Params = new List<System.Data.SqlClient.SqlParameter>();
                Params.Add(new System.Data.SqlClient.SqlParameter("@name", name));
                Params.Add(new System.Data.SqlClient.SqlParameter("@email", email));
                Params.Add(new System.Data.SqlClient.SqlParameter("@designation", designation));
                Params.Add(new System.Data.SqlClient.SqlParameter("@password", Essentials.CalculateSHA1(password)));
                Params.Add(new System.Data.SqlClient.SqlParameter("@phone", phone));
                Params.Add(new System.Data.SqlClient.SqlParameter("@manager_id", manager_id));

                return (new Database()).ExecuteProcedure("usp_register_employee", Params) ? 1 : -1;
            }
            catch (Exception)
            {
                return -1;
            }
        }
        [HttpPost]
        public int CreateEvent(string name, string description, string deadline)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(deadline))
            {
                return 0;
            }
            try
            {
                List<System.Data.SqlClient.SqlParameter> Params = new List<System.Data.SqlClient.SqlParameter>();
                Params.Add(new System.Data.SqlClient.SqlParameter("@name", name));
                Params.Add(new System.Data.SqlClient.SqlParameter("@created_by", ((Employee)Session["Employee"]).ID));
                Params.Add(new System.Data.SqlClient.SqlParameter("@description", description));
                Params.Add(new System.Data.SqlClient.SqlParameter("@deadline", deadline));

                return (new Database()).ExecuteProcedureWithScopeID("usp_add_event", Params);
            }
            catch (Exception)
            {
                return -1;
            }
        }
        [HttpPost]
        public int CreateTask(string name, string description, string deadline, int event_id_for_task = 0, int assigned_to = 0)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(deadline) || event_id_for_task == 0)
            {
                return 0;
            }
            try
            {
                List<System.Data.SqlClient.SqlParameter> Params = new List<System.Data.SqlClient.SqlParameter>();
                Params.Add(new System.Data.SqlClient.SqlParameter("@name", name));
                Params.Add(new System.Data.SqlClient.SqlParameter("@created_by", ((Employee)Session["Employee"]).ID));
                Params.Add(new System.Data.SqlClient.SqlParameter("@description", description));
                Params.Add(new System.Data.SqlClient.SqlParameter("@deadline", deadline));
                Params.Add(new System.Data.SqlClient.SqlParameter("@assigned_to", assigned_to));
                Database Db = new Database();
                int taskID = Db.ExecuteProcedureWithScopeID("usp_add_task", Params);
                if (taskID > 0)
                {
                    Params.Clear();
                    Params.Add(new System.Data.SqlClient.SqlParameter("@event_id", event_id_for_task));
                    Params.Add(new System.Data.SqlClient.SqlParameter("@task_id", taskID));
                    return (Db.ExecuteProcedure("usp_add_task_to_event", Params) ? taskID : 0);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }
        [HttpPost]
        public int CreateSubtask(string name, string description, string deadline, int task_id_for_subtask = 0, int assigned_to = 0, int labours_required = 0, int asset_id = 0, int asset_quantity = 0)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(deadline) || assigned_to == 0 || labours_required == 0 || task_id_for_subtask == 0 || asset_id == 0 || asset_quantity == 0)
            {
                return 0;
            }
            try
            {
                List<System.Data.SqlClient.SqlParameter> Params = new List<System.Data.SqlClient.SqlParameter>();
                Params.Add(new System.Data.SqlClient.SqlParameter("@name", name));
                Params.Add(new System.Data.SqlClient.SqlParameter("@created_by", ((Employee)Session["Employee"]).ID));
                Params.Add(new System.Data.SqlClient.SqlParameter("@description", description));
                Params.Add(new System.Data.SqlClient.SqlParameter("@deadline", deadline));
                Params.Add(new System.Data.SqlClient.SqlParameter("@assigned_to", assigned_to));
                Params.Add(new System.Data.SqlClient.SqlParameter("@labours_required", labours_required));
                Database Db = new Database();
                int subtaskID = Db.ExecuteProcedureWithScopeID("usp_add_subtask", Params);
                if (subtaskID > 0)
                {
                    if (AddAssetToSubtask(subtaskID, asset_id, asset_quantity))
                    {
                        Params.Clear();
                        Params.Add(new System.Data.SqlClient.SqlParameter("@task_id", task_id_for_subtask));
                        Params.Add(new System.Data.SqlClient.SqlParameter("@subtask_id", subtaskID));
                        return (Db.ExecuteProcedure("usp_add_subtask_to_task", Params) ? subtaskID : 0);
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }
        [HttpPost]
        public int AddAsset(string name, string type)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(type))
            {
                return 0;
            }
            try
            {
                List<System.Data.SqlClient.SqlParameter> Params = new List<System.Data.SqlClient.SqlParameter>();
                Params.Add(new System.Data.SqlClient.SqlParameter("@name", name));
                Params.Add(new System.Data.SqlClient.SqlParameter("@created_by", ((Employee)Session["Employee"]).ID));
                Params.Add(new System.Data.SqlClient.SqlParameter("@type", type));

                return (new Database()).ExecuteProcedure("usp_add_asset", Params) ? 1 : -1;
            }
            catch (Exception)
            {
                return -1;
            }
        }
        [HttpPost]
        public bool MarkComplete(int ID = -1, int type = -1)
        {

            if (ID == -1 || type == -1)
            {
                return false;
            }
            try
            {
                List<System.Data.SqlClient.SqlParameter> Params = new List<System.Data.SqlClient.SqlParameter>();
                Params.Add(new System.Data.SqlClient.SqlParameter("@id", ID));
                Params.Add(new System.Data.SqlClient.SqlParameter("@type", type));
                return (new Database()).ExecuteProcedure("usp_mark_complete", Params);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool AddAssetToSubtask(int subtask_id, int asset_id = 0, int asset_quantity = 0)
        {
            if (asset_id == 0 || asset_quantity == 0)
            {
                return false;
            }
            try
            {
                List<System.Data.SqlClient.SqlParameter> Params = new List<System.Data.SqlClient.SqlParameter>();
                Params.Add(new System.Data.SqlClient.SqlParameter("@subtask_id", subtask_id));
                Params.Add(new System.Data.SqlClient.SqlParameter("@asset_id", asset_id));
                Params.Add(new System.Data.SqlClient.SqlParameter("@quantity", asset_quantity));

                return (new Database()).ExecuteProcedure("usp_add_asset_to_subtask", Params);
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
