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
    public class LotAttachmentRepository : IRepository<LotAttachment>
    {
        string connectionString = ConfigurationManager.ConnectionStrings["AuctionDbConnection"].ConnectionString;
        string TableName = $"[dbo].[LotItemAttachments]";
        DataSet auctionDbDataSet = new DataSet();


        public void Add(LotAttachment lotAttachmentEntity)
        {
            auctionDbDataSet.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectSql = $"select * from {TableName} where [AttachmentId]='{lotAttachmentEntity.Id}'";
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectSql, connection))
                {
                    adapter.Fill(auctionDbDataSet, "LotItemAttachments");
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDbDataSet.Tables["LotItemAttachments"].Rows.Count != 0)
                        throw new Exception($"Already has an lot attachment with id = {lotAttachmentEntity.Id}");

                    auctionDbDataSet.Clear();

                    string selectAllSql = $"select * from {TableName}";

                    adapter.SelectCommand = new SqlCommand(selectAllSql, connection);
                    commandBuilder = new SqlCommandBuilder(adapter);

                    adapter.Fill(auctionDbDataSet, "LotItemAttachments");

                    DataTable table = auctionDbDataSet.Tables["LotItemAttachments"];
                    DataRow newRow = table.NewRow();

                    newRow["AttachmentId"] = lotAttachmentEntity.Id;
                    newRow["AttachmentName"] = lotAttachmentEntity.Name;
                    newRow["AttachmentExtension"] = lotAttachmentEntity.Extension;
                    newRow["AttachmentBody"] = lotAttachmentEntity.Body;
                    newRow["LotItemId"] = lotAttachmentEntity.LotItemId;
                    table.Rows.Add(newRow);

                    string insertSql = $"INSERT INTO {TableName} ([AttachmentId],[AttachmentName],[AttachmentExtension],[AttachmentBody],[LotItemId])" +
                        $"VALUES (@AttachmentId,@AttachmentName,@AttachmentExtension,@AttachmentBody,@LotItemId)";
                    SqlCommand command = new SqlCommand(insertSql, connection);
                    command.Parameters.Add("@attachmentId", SqlDbType.UniqueIdentifier, int.MaxValue, "AttachmentId");
                    command.Parameters.Add("@attachmentName", SqlDbType.NVarChar, int.MaxValue, "AttachmentName");
                    command.Parameters.Add("@attachmentExtension", SqlDbType.NVarChar, int.MaxValue, "AttachmentExtension");
                    command.Parameters.Add("@attachmentBody", SqlDbType.VarBinary, int.MaxValue, "AttachmentBody");
                    command.Parameters.Add("@lotItemId", SqlDbType.UniqueIdentifier, int.MaxValue, "LotItemId");
                    adapter.InsertCommand = command;

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

                string selectSqlById = $"select * from {TableName} where [AttachmentId]='{id}'";
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectSqlById, connection))
                {
                    adapter.Fill(auctionDbDataSet, "LotItemAttachments");
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDbDataSet.Tables["LotItemAttachments"].Rows.Count == 0)
                        throw new Exception($"There is no lot attachment with id = {id}");

                    auctionDbDataSet.Clear();

                    string selectSql = $"select * from {TableName}";

                    adapter.SelectCommand = new SqlCommand(selectSql, connection);
                    commandBuilder = new SqlCommandBuilder(adapter);

                    adapter.Fill(auctionDbDataSet, "LotItemAttachments");

                    auctionDbDataSet.Tables["LotItemAttachments"]
                        .Rows
                        .Remove(auctionDbDataSet.Tables["LotItemAttachments"]
                        .Select($"[AttachmentId]='{id}'")[0]);

                    adapter.Update(auctionDbDataSet);
                }
            }
        }

        public LotAttachment Read(string id)
        {
            auctionDbDataSet.Clear();
            LotAttachment attachment = new LotAttachment();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectSqlById = $"select * from {TableName} where [AttachmentId]='{id}'";
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectSqlById, connection))
                {
                    adapter.Fill(auctionDbDataSet, "LotItemAttachments");
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDbDataSet.Tables["LotItemAttachments"].Rows.Count == 0)
                        throw new Exception($"There is no lot attachment with id = {id}");

                    DataTable table = auctionDbDataSet.Tables[0];

                    attachment.Id = table.Rows[0]["AttachmentId"].ToString();
                    attachment.Name = table.Rows[0]["AttachmentName"].ToString();
                    attachment.Extension = table.Rows[0]["AttachmentExtension"].ToString();
                    attachment.Body = Encoding.ASCII.GetBytes(table.Rows[0]["AttachmentBody"].ToString());
                    attachment.LotItemId = table.Rows[0]["LotItemId"].ToString();
                }
            }
            return attachment;
        }

        public IEnumerable<LotAttachment> ReadAll()
        {
            List<LotAttachment> attachments = new List<LotAttachment>();
            auctionDbDataSet.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectAllSql = $"select * from {TableName}";
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectAllSql, connection))
                {
                    adapter.Fill(auctionDbDataSet, "LotItemAttachments");
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDbDataSet.Tables["LotItemAttachments"].Rows.Count == 0)
                        throw new Exception($"There are no lot attachments in database");

                    DataTable table = auctionDbDataSet.Tables["LotItemAttachments"];
                    foreach (DataRow item in table.Rows)
                    {
                        LotAttachment attachment = new LotAttachment()
                        {
                            Id = item["AttachmentId"].ToString(),
                            Name = item["AttachmentName"].ToString(),
                            Extension = item["AttachmentExtension"].ToString(),
                            Body = Encoding.ASCII.GetBytes(item["AttachmentBody"].ToString()),
                            LotItemId = item["AttachmentId"].ToString(),
                        };
                        attachments.Add(attachment);
                    }
                }
            }
            return attachments;
        }

        public void Update(string id, LotAttachment updated)
        {
            auctionDbDataSet.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectByIdSql = $"select * from {TableName} where [AttachmentId]='{id}'";
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectByIdSql, connection))
                {
                    adapter.Fill(auctionDbDataSet, "LotItemAttachments");
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDbDataSet.Tables["LotItemAttachments"].Rows.Count == 0)
                        throw new Exception($"There is no lot attachment with id = {id}");

                    DataTable table = auctionDbDataSet.Tables["LotItemAttachments"];

                    table.Rows[0]["AttachmentId"] = updated.Id;
                    table.Rows[0]["AttachmentName"] = updated.Name;
                    table.Rows[0]["AttachmentExtension"] = updated.Extension;
                    table.Rows[0]["AttachmentBody"] = updated.Body;
                    table.Rows[0]["LotItemId"] = updated.LotItemId;

                    adapter.Update(auctionDbDataSet);
                }
            }
        }
    }
}
