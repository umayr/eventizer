using Eventizer.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Eventizer.Models
{
    public class Asset
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public string Type { set; get; }
        public DateTime DateCreated { set; get; }
        public Employee CreatedBy { set; get; }

        public static Asset FetchAssetByID(int ID)
        {
            List<SqlParameter> Param = new List<SqlParameter>();
            Param.Add(new SqlParameter("@id", ID));
            Asset Asset = new Asset();

            using (SqlDataReader reader = (new Database()).ExecProcedureWithResult("usp_get_Asset_by_id", Param))
            {
                while (reader.Read())
                {
                    Asset.ID = (int)reader["id"];
                    Asset.Name = reader["name"].ToString();
                    Asset.CreatedBy = Employee.FetchEmployeeByID((int)reader["created_by"]);
                    Asset.DateCreated = DateTime.Parse(reader["created_on"].ToString());
                }
            }

            return Asset;
        }
    }
}