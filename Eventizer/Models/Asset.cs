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

            using (SqlDataReader reader = (new Database()).ExecProcedureWithResult("usp_get_asset_by_id", Param))
            {
                while (reader.Read())
                {
                    Asset.ID = (int)reader["id"];
                    Asset.Name = reader["name"].ToString();
                    Asset.Type = reader["type"].ToString();
                    Asset.CreatedBy = Employee.FetchEmployeeByID((int)reader["created_by"]);
                    Asset.DateCreated = DateTime.Parse(reader["created_on"].ToString());
                }
            }

            return Asset;
        }
        public static List<Asset> FetchAllAssets(SqlDataReader reader)
        {
            List<Asset> Assets = new List<Asset>();

            while (reader.Read())
            {

                Asset E = new Asset();
                E.ID = (int)reader["id"];
                E.Name = reader["name"].ToString();
                E.Type = reader["type"].ToString();
                E.CreatedBy = Employee.FetchEmployeeByID((int)reader["created_by"]);
                E.DateCreated = DateTime.Parse(reader["created_on"].ToString());
                Assets.Add(E);
            }
            reader.Close();
            reader.Dispose();
            return Assets;
        }

        public static List<AssetName> FetchAllAssetNames()
        {
            List<AssetName> names = new List<AssetName>();
            SqlDataReader reader = (new Database()).SelectAllFromView("vw_all_assets_names");
            while (reader.Read())
            {
                AssetName A = new AssetName();
                A.ID = (int)reader["id"];
                A.Name = reader["name"].ToString();
                names.Add(A);
            }
            return names;
        }
    }

    public class AssetName
    {
        public int ID { set; get; }
        public string Name { set; get; }
    }
}