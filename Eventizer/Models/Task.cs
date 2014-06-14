using Eventizer.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Eventizer.Models
{
    public class Task
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public DateTime DateCreated { set; get; }
        public Employee CreatedBy { set; get; }
        public Employee AssignedTo { set; get; }
        public string Description { set; get; }
        public DateTime Deadline { set; get; }
        public bool Status { set; get; }
        public List<Subtask> Subtasks { set; get; }

        public bool AddSubtask(int SubTaskID)
        {
            try
            {
                List<SqlParameter> Params = new List<SqlParameter>();
                Params.Add(new SqlParameter("@subtask_id", SubTaskID));
                Params.Add(new SqlParameter("@task_id", ID));
                return (new Database()).ExecuteProcedure("usp_add_subtask_to_task", Params);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int TotalSubtasks()
        {
            return Subtasks.Count;
        }
        public int TotalAssets()
        {
            int count = 0;
            foreach (Subtask S in Subtasks)
            {
                count += S.TotalAssets();
            }
            return count;
        }
        public int TotalLaboursRequired()
        {
            int count = 0;
            foreach (Subtask S in Subtasks)
            {
                count += S.LaboursRequired;
            }
            return count;
        }

        public static Task FetchTaskByID(int ID)
        {
            List<SqlParameter> Param = new List<SqlParameter>();
            Param.Add(new SqlParameter("@id", ID));
            Task task = new Task();

            using (SqlDataReader reader = (new Database()).ExecProcedureWithResult("usp_get_task_by_id", Param))
            {
                while (reader.Read())
                {
                    task.ID = (int)reader["id"];
                    task.Name = reader["name"].ToString();
                    task.Description = reader["description"].ToString();
                    task.Status = (bool)reader["status"];
                    task.CreatedBy = Employee.FetchEmployeeByID((int)reader["created_by"]);
                    task.AssignedTo = Employee.FetchEmployeeByID((int)reader["assigned_to"]);
                    task.DateCreated = DateTime.Parse(reader["date_created"].ToString());
                    task.Deadline = DateTime.Parse(reader["deadline"].ToString());
                    task.Subtasks = (reader["subtasks"].ToString() != "0,") ? Task.FetchSubtasksArray(reader["subtasks"].ToString()) : new List<Subtask>();
                }
            }

            return task;
        }
        public static List<Subtask> FetchSubtasksArray(string rawSubtasks)
        {
            string[] IDArray = rawSubtasks.TrimEnd(',').Split(',');
            List<Subtask> subtasks = new List<Subtask>();
            for (int i = 0; i < IDArray.Length; i++)
            {
                Subtask S = Subtask.FetchSubtaskByID(Convert.ToInt32(IDArray[i]));
                subtasks.Add(S);
            }
            return subtasks;
        }


    }
}