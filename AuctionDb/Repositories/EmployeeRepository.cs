using AuctionDb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionDb.Repositories
{
    public class EmployeeRepository:IRepository<Employee>
    {
        string connectionString = ConfigurationManager.ConnectionStrings["AuctionDbConnection"].ConnectionString;
        string TableName = $"[dbo].[Employees]";
        DataSet auctionDbDataSet = new DataSet();


        public void Add(Employee employeeEntity)
        {
            auctionDbDataSet.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectSql = $"select * from {TableName} where [EmployeeId]='{employeeEntity.Id}'";
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectSql, connection))
                {
                    adapter.Fill(auctionDbDataSet,"Employees");
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDbDataSet.Tables["Employees"].Rows.Count != 0)
                        throw new Exception($"Already has an employee with id = {employeeEntity.Id}");

                    auctionDbDataSet.Clear();

                    string insertSql = $"select * from {TableName}";

                    adapter.SelectCommand = new SqlCommand(insertSql, connection);
                    commandBuilder = new SqlCommandBuilder(adapter);

                    adapter.Fill(auctionDbDataSet, "Employees");

                    DataTable table = auctionDbDataSet.Tables["Employees"];
                    DataRow newRow = table.NewRow();

                    newRow["EmployeeId"] = employeeEntity.Id;
                    newRow["FirstName"] = employeeEntity.FirstName;
                    newRow["LastName"] = employeeEntity.LastName;
                    newRow["Email"] = employeeEntity.Email;
                    newRow["PasswordHash"] = employeeEntity.Password;
                    newRow["DoB"] = employeeEntity.DoB.ToString("yyyy-MM-dd");
                    newRow["OrganizationId"] = employeeEntity.OrganizationId;

                    auctionDbDataSet.Tables["Employees"].Rows.Add(newRow);

                    adapter.Update(auctionDbDataSet);
                }
            }
        }

        public void Delete(string id)
        {
            auctionDbDataSet.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectSqlById = $"select * from {TableName} where [EmployeeId]='{id}'";
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectSqlById, connection))
                {
                    adapter.Fill(auctionDbDataSet, "Employees");
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDbDataSet.Tables["Employees"].Rows.Count == 0)
                        throw new Exception($"There is no employee with id = {id}");

                    auctionDbDataSet.Clear();

                    string selectSql = $"select * from {TableName}";

                    adapter.SelectCommand = new SqlCommand(selectSql, connection);
                    commandBuilder = new SqlCommandBuilder(adapter);

                    adapter.Fill(auctionDbDataSet, "Employees");

                    auctionDbDataSet.Tables["Employees"]
                        .Rows
                        .Remove(auctionDbDataSet
                        .Tables["Employees"]
                        .Select($"[EmployeeId]='{id}'")[0]);

                    adapter.Update(auctionDbDataSet);
                }
            }
        }

        public Employee Read(string id)
        {
            auctionDbDataSet.Clear();
            Employee employee = new Employee();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectSqlById = $"select * from {TableName} where [EmployeeId]='{id}'";
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectSqlById, connection))
                {
                    adapter.Fill(auctionDbDataSet, "Employees");
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDbDataSet.Tables["Employees"].Rows.Count == 0)
                        throw new Exception($"There is no employee with id = {id}");

                    DataTable table = auctionDbDataSet.Tables["Employees"];

                    employee.Id = table.Rows[0]["EmployeeId"].ToString();
                    employee.FirstName= table.Rows[0]["FirstName"].ToString();
                    employee.LastName = table.Rows[0]["LastName"].ToString();
                    employee.Email = table.Rows[0]["Email"].ToString();
                    employee.Password = table.Rows[0]["PasswordHash"].ToString();
                    employee.DoB = Convert.ToDateTime(table.Rows[0]["DoB"].ToString());
                    employee.OrganizationId = table.Rows[0]["OrganizationId"].ToString();
                }
            }
            return employee;
        }

        public IEnumerable<Employee> ReadAll()
        {
            List<Employee> employees = new List<Employee>();
            auctionDbDataSet.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectAllSql = $"select * from {TableName}";
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectAllSql, connection))
                {
                    adapter.Fill(auctionDbDataSet, "Employees");
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDbDataSet.Tables["Employees"].Rows.Count == 0)
                        throw new Exception($"There are no employees in database");

                    DataTable table = auctionDbDataSet.Tables["Employees"];
                    foreach (DataRow item in table.Rows)
                    {
                        Employee employee = new Employee()
                        {
                            Id = item["EmployeeId"].ToString(),
                            FirstName=item["FirstName"].ToString(),
                            LastName = item["LastName"].ToString(),
                            Email = item["Email"].ToString(),
                            Password = item["PasswordHash"].ToString(),
                            DoB = Convert.ToDateTime(item["DoB"].ToString()),
                            OrganizationId = item["OrganizationId"].ToString()
                        };
                        employees.Add(employee);
                    }
                }
            }
            return employees;
        }

        public void Update(string id, Employee updated)
        {
            auctionDbDataSet.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectByIdSql = $"select * from {TableName} where [EmployeeId]='{id}'";
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectByIdSql, connection))
                {
                    adapter.Fill(auctionDbDataSet, "Employees");
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDbDataSet.Tables["Employees"].Rows.Count == 0)
                        throw new Exception($"There is no employee with id = {id}");

                    DataTable table = auctionDbDataSet.Tables["Employees"];

                    table.Rows[0]["EmployeeId"] = updated.Id;
                    table.Rows[0]["FirstName"] = updated.FirstName;
                    table.Rows[0]["LastName"] = updated.LastName;
                    table.Rows[0]["Email"] = updated.Email;
                    table.Rows[0]["PasswordHash"] = updated.Password;
                    table.Rows[0]["DoB"] = updated.DoB.ToString("yyyy-MM-dd");
                    table.Rows[0]["OrganizationId"] = updated.OrganizationId;

                    adapter.Update(auctionDbDataSet);
                }
            }
        }
    }
}
