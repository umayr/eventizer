using Eventizer.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Eventizer.Models
{
    public class Event
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public DateTime DateCreated { set; get; }
        public Employee CreatedBy { set; get; }
        public string Description { set; get; }
        public DateTime Deadline { set; get; }
        public bool Status { set; get; }
        public List<Task> Tasks { set; get; }

        public static List<Event> FetchAllEvents(SqlDataReader reader)
        {
            List<Event> Events = new List<Event>();

            while (reader.Read())
            {

                Event E = new Event();
                E.ID = (int)reader["id"];
                E.Name = reader["name"].ToString();
                E.Description = reader["description"].ToString();
                E.Status = (bool)reader["status"];
                E.CreatedBy = Employee.FetchEmployeeByID((int)reader["created_by"]);
                E.DateCreated = DateTime.Parse(reader["date_created"].ToString());
                E.Deadline = DateTime.Parse(reader["deadline"].ToString());
                E.Tasks = Event.FetchTasksArray(reader["tasks"].ToString());
                Events.Add(E);
            }
            reader.Close();
            reader.Dispose();
            return Events;
        }
        
        public static Event FetchEventByID(int ID)
        {
            List<SqlParameter> Param = new List<SqlParameter>();
            Param.Add(new SqlParameter("@id", ID));
            Event Event = new Event();

            using (SqlDataReader reader = (new Database()).ExecProcedureWithResult("usp_get_event_by_id", Param))
            {
                while (reader.Read())
                {
                    Event.ID = (int)reader["id"];
                    Event.Name = reader["name"].ToString();
                    Event.Description = reader["description"].ToString();
                    Event.Status = (bool)reader["status"];
                    Event.CreatedBy = Employee.FetchEmployeeByID((int)reader["created_by"]);
                    Event.DateCreated = DateTime.Parse(reader["date_created"].ToString());
                    Event.Deadline = DateTime.Parse(reader["deadline"].ToString());
                    Event.Tasks = (reader["tasks"].ToString() != "0,") ? Event.FetchTasksArray(reader["tasks"].ToString()) : new List<Task>();
                }
            }

            return Event;
        }
        public static List<Task> FetchTasksArray(string rawTasks)
        {
            string[] IDArray = rawTasks.TrimEnd(',').Split(',');
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < IDArray.Length; i++)
            {
                Task T = Task.FetchTaskByID(Convert.ToInt32(IDArray[i]));
                tasks.Add(T);
            }
            return tasks;
        }
        public bool AddTask(int TaskID)
        {
            try
            {
                List<SqlParameter> Params = new List<SqlParameter>();
                Params.Add(new SqlParameter("@task_id", TaskID));
                Params.Add(new SqlParameter("@event_id", ID));
                return (new Database()).ExecuteProcedure("usp_add_task_to_event", Params);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public int TotalTasks()
        {
            return Tasks.Count;
        }
        public int TotalSubtasks()
        {
            int count = 0;
            foreach (Task T in Tasks)
            {
                count += T.TotalSubtasks();
            }
            return count;
        }
        public int TotalAssets()
        {
            int count = 0;
            foreach (Task T in Tasks)
            {
                count += T.TotalAssets();
            }
            return count;
        }
        public int TotalLaboursRequired()
        {
            int count = 0;
            foreach (Task T in Tasks)
            {
                count += T.TotalLaboursRequired();
            }
            return count;
        }
    }
}