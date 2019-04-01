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
    public class OrganizationRepository : IRepository<Organization>
    {
        string connectionString = ConfigurationManager.ConnectionStrings["AuctionDbConnection"].ConnectionString;
        string TableName = $"[dbo].[Organizations]";
        DataSet auctionDbDataSet = new DataSet();


        public void Add(Organization organizationEntity)
        {
            auctionDbDataSet.Clear();

            using (SqlConnection connection=new SqlConnection(connectionString))
            {
                connection.Open();

                string selectSql = $"select * from {TableName} where [OrganizationName]='{organizationEntity.Name}'";
                using (SqlDataAdapter adapter=new SqlDataAdapter(selectSql, connection))
                {
                    adapter.Fill(auctionDbDataSet, "Organizations");
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDbDataSet.Tables[0].Rows.Count!=0)                    
                        throw new Exception($"Already has an organization with name = {organizationEntity.Name}");

                    auctionDbDataSet.Clear();
                    string insertSql = $"select * from {TableName}";
                    adapter.SelectCommand = new SqlCommand(insertSql, connection);
                    commandBuilder = new SqlCommandBuilder(adapter);

                    adapter.Fill(auctionDbDataSet, "Organizations");

                    DataTable table = auctionDbDataSet.Tables[0];
                    DataRow newRow = table.NewRow();

                    newRow["OrganizationId"] = organizationEntity.Id;
                    newRow["OrganizationName"] = organizationEntity.Name;

                    auctionDbDataSet.Tables["Organizations"].Rows.Add(newRow);

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

                string selectSqlById = $"select * from {TableName} where [OrganizationId]='{id}'";
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectSqlById, connection))
                {
                    adapter.Fill(auctionDbDataSet, "Organizations");
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDbDataSet.Tables["Organizations"].Rows.Count == 0)
                        throw new Exception($"There is no organization with id = {id}");

                    auctionDbDataSet.Clear();

                    string selectSql = $"select * from {TableName}";

                    adapter.SelectCommand = new SqlCommand(selectSql, connection);
                    commandBuilder = new SqlCommandBuilder(adapter);

                    adapter.Fill(auctionDbDataSet, "Organizations");

                    auctionDbDataSet.Tables["Organizations"]
                        .Rows
                        .Remove(auctionDbDataSet
                        .Tables["Organizations"]
                        .Select($"[OrganizationId]='{id}'")[0]);

                    adapter.Update(auctionDbDataSet);
                }
            }
        }

        public Organization Read(string id)
        {
            auctionDbDataSet.Clear();
            Organization organization = new Organization();

            using (SqlConnection connection=new SqlConnection(connectionString))
            {
                connection.Open();

                string selectByIdSql = $"select * from {TableName} where [OrganizationId]='{id}'";
                using (SqlDataAdapter adapter=new SqlDataAdapter(selectByIdSql, connection))
                {
                    adapter.Fill(auctionDbDataSet, "Organizations");
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDbDataSet.Tables["Organizations"].Rows.Count==0)
                        throw new Exception($"There is no organization with id = {id}");
                    
                    DataTable table = auctionDbDataSet.Tables[0];
                    organization.Id = table.Rows[0]["OrganizationId"].ToString();
                    organization.Name = table.Rows[0]["OrganizationName"].ToString();                    
                }                
            }
            return organization;
        }

        public IEnumerable<Organization> ReadAll()
        {
            List<Organization> organizations = new List<Organization>();
            auctionDbDataSet.Clear();

            using (SqlConnection connection=new SqlConnection(connectionString))
            {
                connection.Open();

                string selectAllSql = $"select * from {TableName}";
                using (SqlDataAdapter adapter=new SqlDataAdapter(selectAllSql, connection))
                {
                    adapter.Fill(auctionDbDataSet, "Organizations");
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDbDataSet.Tables["Organizations"].Rows.Count==0)
                        throw new Exception($"There is no organization in database");
                    
                    DataTable table = auctionDbDataSet.Tables["Organizations"];
                    foreach (DataRow item in table.Rows)
                    {
                        Organization organization = new Organization()
                        {
                            Id = item["OrganizationId"].ToString(),
                            Name = item["OrganizationName"].ToString()
                        };
                        organizations.Add(organization);
                    }
                }
            }

            return organizations;
        }

        public void Update(string id, Organization updated)
        {
            auctionDbDataSet.Clear();

            using (SqlConnection connection=new SqlConnection(connectionString))
            {
                connection.Open();

                string selectByIdSql = $"select * from {TableName} where [OrganizationId]='{id}'";
                using (SqlDataAdapter adapter=new SqlDataAdapter(selectByIdSql, connection))
                {
                    adapter.Fill(auctionDbDataSet, "Organizations");
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDbDataSet.Tables["Organizations"].Rows.Count==0)
                        throw new Exception($"There is no organization with id = {id}");

                    DataTable table = auctionDbDataSet.Tables["Organizations"];

                    table.Rows[0]["OrganizationId"] = updated.Id;
                    table.Rows[0]["OrganizationName"] = updated.Name;

                    adapter.Update(auctionDbDataSet);                    
                }
            }
        }
    }
}
