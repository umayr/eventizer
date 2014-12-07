using Eventizer.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Eventizer.Models
{
    public class Employee
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
        public string Phone { set; get; }
        public string Designation { set; get; }
        public Employee Manager { set; get; }

        public static MiniEmployeesList GetEmployeeDetails()
        {
            List<string> names = new List<string>();
            List<int> ids = new List<int>();
            using (System.Data.SqlClient.SqlDataReader reader = (new Database()).SelectAllFromView("vw_employees_names"))
            {
                while (reader.Read())
                {
                    names.Add(reader["name"].ToString());
                    ids.Add(Convert.ToInt16(reader["id"]));
                }
            }

            return new MiniEmployeesList { Ids = (new JavaScriptSerializer()).Serialize(ids), Names = (new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(names) };
        }

        public static Employee FetchEmployeeByID(int ID)
        {
            List<SqlParameter> Param = new List<SqlParameter>();
            Param.Add(new SqlParameter("@id", ID));
            Employee employee = new Employee();

            using (SqlDataReader reader = (new Database()).ExecProcedureWithResult("usp_get_employee_by_id", Param))
            {
                while (reader.Read())
                {
                    employee.ID = (int)reader["id"];
                    employee.Name = reader["name"].ToString();
                    employee.Email = reader["email"].ToString();
                    employee.Password = reader["password"].ToString();
                    employee.Phone = reader["phone"].ToString();
                    employee.Designation = reader["designation"].ToString();
                    if ((int)reader["manager_id"] != 0)
                    {
                        employee.Manager = Employee.FetchEmployeeByID((int)reader["manager_id"]);
                    }
                    else
                    {
                        employee.Manager = null;
                    }
                }
            }

            return employee;
        }
        public static Employee FetchEmployeeByEmail(string Email)
        {
            List<SqlParameter> Param = new List<SqlParameter>();    
            Param.Add(new SqlParameter("@email", Email));
            Employee employee = new Employee();

            using (SqlDataReader reader = (new Database()).ExecProcedureWithResult("usp_get_employee_by_email", Param))
            {
                while (reader.Read())
                {
                    employee.ID = (int)reader["id"];
                    employee.Name = reader["name"].ToString();
                    employee.Email = reader["email"].ToString();
                    employee.Password = reader["password"].ToString();
                    employee.Phone = reader["phone"].ToString();
                    employee.Designation = reader["designation"].ToString();
                    if ((int)reader["manager_id"] != 0)
                    {
                        employee.Manager = Employee.FetchEmployeeByID((int)reader["manager_id"]);
                    }
                    else
                    {
                        employee.Manager = null;
                    }
                }
            }

            return employee;
        }

        public static List<Employee> FetchAllEmployees()
        {
            List<Employee> employees = new List<Employee>();
            using (System.Data.SqlClient.SqlDataReader reader = (new Database()).SelectAllFromView("vw_all_employees"))
            {
                while (reader.Read())
                {
                    Employee E = new Employee();
                    E.ID = (int)reader["id"];
                    E.Name = reader["name"].ToString();
                    E.Email = reader["email"].ToString();
                    E.Password = reader["password"].ToString();
                    E.Phone = reader["phone"].ToString();
                    E.Designation = reader["designation"].ToString();
                    if ((int)reader["manager_id"] != 0)
                    {
                        E.Manager = Employee.FetchEmployeeByID((int)reader["manager_id"]);
                    }
                    else
                    {
                        E.Manager = null;
                    }
                    employees.Add(E);
                }
            }
            return employees;
        }
    }
}