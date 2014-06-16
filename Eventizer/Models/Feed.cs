using Eventizer.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Eventizer.Models
{
    public class Feed
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public string Type { set; get; }
        public DateTime DateCreated { set; get; }
        public Employee CreatedBy { set; get; }
        public Employee AssignedTo { set; get; }
        public string Description { set; get; }
        public DateTime Deadline { set; get; }
        public bool Status { set; get; }

        public static List<Feed> FetchAllFeeds()
        {
            List<Feed> Feeds = new List<Feed>();
            SqlDataReader reader = (new Database()).SelectAllFromView("vw_fetch_feeds");
            while (reader.Read())
            {

                Feed F = new Feed();
                F.ID = (int)(reader["id"] as int?);
                F.Name = reader["name"].ToString();
                F.Type = Convert.ToString(reader["type"]);
                F.Description = reader["description"].ToString();
                F.Status = (bool)reader["status"];
                F.CreatedBy = Employee.FetchEmployeeByID((int)reader["created_by"]);
                F.DateCreated = DateTime.Parse(reader["date_created"].ToString());
                F.Deadline = DateTime.Parse(reader["deadline"].ToString());
                F.AssignedTo = ((int)reader["assigned_to"] != -1 || (int)reader["assigned_to"] != 0) ? Employee.FetchEmployeeByID((int)reader["assigned_to"]) : null;
                Feeds.Add(F);
            }

            reader.Close();
            reader.Dispose();
            return Feeds.OrderBy(x => x.DateCreated).Reverse().ToList();
        }

    }
}