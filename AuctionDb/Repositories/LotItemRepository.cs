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
    public class LotItemRepository : IRepository<LotItem>
    {
        string connectionString = ConfigurationManager.ConnectionStrings["AuctionDbConnection"].ConnectionString;
        string TableName = $"[dbo].[LotItems]";
        DataSet auctionDbDataSet = new DataSet();


        public void Add(LotItem lotItemEntity)
        {
            auctionDbDataSet.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectByIdSql = $"select * from {TableName} where [LotId]='{lotItemEntity.Id}'";
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectByIdSql, connection))
                {
                    adapter.Fill(auctionDbDataSet, "LotItems");
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDbDataSet.Tables["LotItems"].Rows.Count != 0)
                        throw new Exception($"Already has an lot with id = {lotItemEntity.Id}");

                    auctionDbDataSet.Clear();

                    string selectSql = $"select * from {TableName}";
                    adapter.SelectCommand = new SqlCommand(selectSql, connection);
                    commandBuilder = new SqlCommandBuilder(adapter);

                    adapter.Fill(auctionDbDataSet, "LotItems");
                    DataTable table = auctionDbDataSet.Tables["LotItems"];
                    DataRow newRow = table.NewRow();

                    newRow["LotId"] = lotItemEntity.Id;
                    newRow["LotName"] = lotItemEntity.Name;
                    newRow["LotDescription"] = lotItemEntity.Description;
                    newRow["PublishedDate"] = lotItemEntity.PublishedDate.ToString("yyyy-MM-dd");
                    newRow["InitialCost"] = lotItemEntity.InitialCost;                    
                    newRow["CreatedByEmployeeId"] = lotItemEntity.CreatedByEmployeeId;

                    auctionDbDataSet.Tables["LotItems"].Rows.Add(newRow);

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

                string selectByIdSql = $"select * from {TableName} where [LotId]='{id}'";
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectByIdSql, connection))
                {
                    adapter.Fill(auctionDbDataSet, "LotItems");
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDbDataSet.Tables["LotItems"].Rows.Count == 0)
                        throw new Exception($"There is no lot with id = {id}");

                    auctionDbDataSet.Clear();

                    string selectSql = $"select * from {TableName}";

                    adapter.SelectCommand = new SqlCommand(selectSql, connection);
                    commandBuilder = new SqlCommandBuilder(adapter);

                    adapter.Fill(auctionDbDataSet, "LotItems");

                    auctionDbDataSet.Tables["LotItems"]
                        .Rows
                        .Remove(auctionDbDataSet.Tables["LotItems"]
                        .Select($"[LotId]='{id}'")[0]);

                    adapter.Update(auctionDbDataSet);
                }
            }
        }

        public LotItem Read(string id)
        {
            auctionDbDataSet.Clear();
            LotItem lotItem = new LotItem();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectByIdSql = $"select * from {TableName} where [LotId]='{id}'";

                using (SqlDataAdapter adapter = new SqlDataAdapter(selectByIdSql, connection))
                {
                    adapter.Fill(auctionDbDataSet, "LotItems");

                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDbDataSet.Tables["LotItems"].Rows.Count == 0)
                        throw new Exception($"There is no lot with id = {id}");

                    DataTable table = auctionDbDataSet.Tables["LotItems"];

                    lotItem.Id = table.Rows[0]["LotId"].ToString();
                    lotItem.Name = table.Rows[0]["LotName"].ToString();
                    lotItem.Description = table.Rows[0]["LotDescription"].ToString();
                    lotItem.PublishedDate = Convert.ToDateTime(table.Rows[0]["PublishedDate"].ToString());
                    lotItem.InitialCost = Convert.ToDecimal(table.Rows[0]["InitialCost"].ToString());
                    lotItem.CreatedByEmployeeId = table.Rows[0]["CreatedByEmployeeId"].ToString();
                }
            }
            return lotItem;
        }

        public IEnumerable<LotItem> ReadAll()
        {
            List<LotItem> lotItems = new List<LotItem>();
            auctionDbDataSet.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectAllSql = $"select * from {TableName}";
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectAllSql, connection))
                {
                    adapter.Fill(auctionDbDataSet, "LotItems");
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDbDataSet.Tables["LotItems"].Rows.Count == 0)
                        throw new Exception($"There are no lots in database");

                    DataTable table = auctionDbDataSet.Tables["LotItems"];
                    foreach (DataRow item in table.Rows)
                    {
                        LotItem lot = new LotItem()
                        {
                            Id = item["LotId"].ToString(),
                            Name = item["LotName"].ToString(),
                            Description = item["LotDescription"].ToString(),
                            PublishedDate = Convert.ToDateTime(item["PublishedDate"].ToString()),
                            InitialCost = Convert.ToDecimal(item["InitialCost"].ToString()),
                            CreatedByEmployeeId = item["CreatedByEmployeeId"].ToString()
                        };
                        lotItems.Add(lot);
                    }
                }
            }
            return lotItems;
        }

        public void Update(string id, LotItem updated)
        {
            auctionDbDataSet.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectByIdSql = $"select * from {TableName} where [LotId]='{id}'";
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectByIdSql, connection))
                {
                    adapter.Fill(auctionDbDataSet, "LotItems");

                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDbDataSet.Tables["LotItems"].Rows.Count == 0)
                        throw new Exception($"There is no lot with id = {id}");

                    DataTable table = auctionDbDataSet.Tables["LotItems"];

                    table.Rows[0]["LotId"] = updated.Id;
                    table.Rows[0]["LotName"] = updated.Name;
                    table.Rows[0]["LotDescription"] = updated.Description;
                    table.Rows[0]["PublishedDate"] = updated.PublishedDate.ToString("yyyy-MM-dd");
                    table.Rows[0]["InitialCost"] = updated.InitialCost;                    
                    table.Rows[0]["CreatedByEmployeeId"] = updated.CreatedByEmployeeId;

                    adapter.Update(auctionDbDataSet);
                }
            }
        }
    }
}
