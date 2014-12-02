using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Eventizer.Helpers
{
    public class Database
    {
        SqlConnection con;

        public SqlConnection Connection
        {
            get { return con; }
            set { con = value; }
        }

        public Database()
        {
            Connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["LocalDB"].ToString());
        }


        public SqlDataReader SelectAllFromView(string viewName)
        {

            try
            {
                Connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = Connection;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = string.Format("SELECT * FROM {0};", viewName);
                return cmd.ExecuteReader();

            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public SqlDataReader SelectFromViewWithWhere(string viewName, string whereClause)
        {

            try
            {
                Connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = Connection;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = string.Format("SELECT * FROM {0} where {1};", viewName, whereClause);
                return cmd.ExecuteReader();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public bool ExecuteProcedure(string ProcedureName, List<SqlParameter> Params)
        {
            try
            {
                Connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = Connection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = ProcedureName;
                cmd.Parameters.AddRange(Params.ToArray());
                int rowaffected = cmd.ExecuteNonQuery();
                Connection.Close();

                if (rowaffected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }
        public int ExecuteProcedureWithScopeID(string ProcedureName, List<SqlParameter> Params)
        {
            try
            {
                Connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = Connection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = ProcedureName;
                cmd.Parameters.AddRange(Params.ToArray());
                object returnObj = cmd.ExecuteScalar();
                Connection.Close();

                try
                {
                    //int returnID = (int)returnObj;
                    return Convert.ToInt32(returnObj);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return 0;
            }

        }
        public object ExecProcedureWithReturnValue(string ProcedureName)
        {
            try
            {
                Connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = Connection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = ProcedureName;
                SqlParameter returnValue = new SqlParameter("@return", System.Data.SqlDbType.Int);
                returnValue.Direction = System.Data.ParameterDirection.ReturnValue;

                cmd.ExecuteNonQuery();
                Connection.Close();
                return returnValue.Value;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public object ExecProcedureWithReturnValue(string ProcedureName, List<SqlParameter> Params)
        {
            try
            {
                Connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = Connection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = ProcedureName;

                cmd.Parameters.AddRange(Params.ToArray());

                SqlParameter returnValue = cmd.Parameters.Add("@return", System.Data.SqlDbType.Int);
                returnValue.Direction = System.Data.ParameterDirection.ReturnValue;

                cmd.ExecuteNonQuery();
                Connection.Close();
                return returnValue.Value;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public SqlDataReader ExecProcedureWithResult(string ProcedureName, List<SqlParameter> Params)
        {
            try
            {
                Connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = Connection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = ProcedureName;
                cmd.Parameters.AddRange(Params.ToArray());
                return cmd.ExecuteReader();
            }
            catch (Exception)
            {
                return null;
            }

        }

        public IEnumerable<Dictionary<string, object>> Serialize(SqlDataReader reader)
        {
            var results = new List<Dictionary<string, object>>();
            var cols = new List<string>();
            for (var i = 0; i < reader.FieldCount; i++)
                cols.Add(reader.GetName(i));

            while (reader.Read())
                results.Add(SerializeRow(cols, reader));

            return results;
        }
        private Dictionary<string, object> SerializeRow(IEnumerable<string> cols, SqlDataReader reader)
        {
            var result = new Dictionary<string, object>();
            foreach (var col in cols)
                result.Add(col, reader[col]);
            return result;
        }
    }
}