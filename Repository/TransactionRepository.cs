using ExpenseManagerAPI.Model;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;

namespace ExpenseManagerAPI.Repository
{
    public class TransactionRepository
    {
        private IConfiguration _configuration;
        private readonly String _ConnectionString;

        public TransactionRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<TransactionsModel> getAllTransaction()
        {
            var transaction = new List<TransactionsModel>();
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("[dbo].[PR_Transactions_SelectAll]", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                connection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    transaction.Add(new TransactionsModel
                    {
                        ExpenseID = reader["ExpenseID"] != DBNull.Value ? Convert.ToInt32(reader["ExpenseID"]) : 0,
                        UserID = reader["UserID"] != DBNull.Value ? Convert.ToInt32(reader["UserID"]) : 0,
                        UserName = reader["UserName"] != DBNull.Value ? reader["UserName"].ToString() : string.Empty,
                        EventID = reader["EventID"] != DBNull.Value ? Convert.ToInt32(reader["EventID"]) : 0,
                        Amount = reader["Amount"] != DBNull.Value
                                              ? Convert.ToDouble(reader["Amount"])
                                              : 0.0,
                        HostId = reader["HostID"] != DBNull.Value ? Convert.ToInt32(reader["HostID"]) : 0,
                        TransactionDate = reader["TransactionDate"] != DBNull.Value ? (DateTime)reader["TransactionDate"] : DateTime.MinValue,
                        TransactionType = reader["TransactionType"] != DBNull.Value ? reader["TransactionType"].ToString() : string.Empty,
                        Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : string.Empty

                    });
                }
            }
            return transaction;
        }

        public List<TransactionsModel> GetTransactionByEventID(int eventID,int HostID)
        {
            List<TransactionsModel> transactionList = new List<TransactionsModel>();

            using (SqlConnection conn = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("[PR_Transactions_CalculateIncomeExpense]", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EventID", eventID);
                    command.Parameters.AddWithValue("@HostID", HostID);

                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var transactionModel = new TransactionsModel
                            {
                                UserID = Convert.ToInt32(reader["UserID"]),
                                EventID = Convert.ToInt32(reader["EventID"]),
                                UserName = reader["UserName"]?.ToString() ?? string.Empty,
                                ExpenseID = Convert.ToInt32(reader["ExpenseID"]),
                                TransactionType = reader["TransactionType"]?.ToString() ?? "Unknown",
                                TransactionDate = reader["TransactionDate"] != DBNull.Value
                                                 ? Convert.ToDateTime(reader["TransactionDate"])
                                                 : DateTime.MinValue,
                                Amount = reader["Amount"] != DBNull.Value
                                              ? Convert.ToDouble(reader["Amount"])
                                              : 0.0,
                                HostId = reader["HostID"] != DBNull.Value ? Convert.ToInt32(reader["HostID"]) : 0

                            };

                            transactionList.Add(transactionModel);
                        }
                    }
                }
            }

            return transactionList;
        }

        public List<TransactionsModel> GetTransactionByHostID(int HostId)
        {
            List<TransactionsModel> transactionList = new List<TransactionsModel>();

            using (SqlConnection conn = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("[GetTransactionsByHostId]", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@HostId", HostId);

                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var transactionModel = new TransactionsModel
                            {
                                UserID = Convert.ToInt32(reader["UserID"]),
                                HostId = Convert.ToInt32(reader["HostId"]),
                                UserName = reader["UserName"]?.ToString() ?? string.Empty,
                                ExpenseID = Convert.ToInt32(reader["ExpenseID"]),
                                TransactionType = reader["TransactionType"]?.ToString() ?? "Unknown",
                                TransactionDate = reader["TransactionDate"] != DBNull.Value
                                                 ? Convert.ToDateTime(reader["TransactionDate"])
                                                 : DateTime.MinValue,
                                Amount = reader["Amount"] != DBNull.Value
                                              ? Convert.ToDouble(reader["Amount"]): 0.0,
                                 EventID = Convert.ToInt16(reader["EventID"])

                            };

                            transactionList.Add(transactionModel);
                        }
                    }
                }
            }

            return transactionList;
        }

        public TransactionsModel GetTransactionByID(int TransactionID)
        {
            TransactionsModel transactionlist = null;
            SqlConnection conn = new SqlConnection(_ConnectionString);


            SqlCommand command = new SqlCommand("[PR_Transactions_SelectByID]", conn);
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
            };
            command.Parameters.AddWithValue("@ExpenseID", TransactionID);
            conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                transactionlist = new TransactionsModel
                {
                    ExpenseID = reader["ExpenseID"] != DBNull.Value ? Convert.ToInt32(reader["ExpenseID"]) : 0,
                    UserID = reader["UserID"] != DBNull.Value ? Convert.ToInt32(reader["UserID"]) : 0,
                    UserName = reader["UserName"] != DBNull.Value ? reader["UserName"].ToString() : string.Empty,
                    EventID = reader["EventID"] != DBNull.Value ? Convert.ToInt32(reader["EventID"]) : 0,
                    Amount = reader["Amount"] != DBNull.Value
                                              ? Convert.ToDouble(reader["Amount"])
                                              : 0.0,
                    TransactionDate = reader["TransactionDate"] != DBNull.Value ? (DateTime)reader["TransactionDate"] : DateTime.MinValue,
                    TransactionType = reader["TransactionType"] != DBNull.Value ? reader["TransactionType"].ToString() : string.Empty,
                    Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : string.Empty

                };

            }


            return transactionlist;
        }

        public bool deleteTransaction(int transactionID)
        {
            SqlConnection conn = new SqlConnection(_ConnectionString);
            SqlCommand command = new SqlCommand("[dbo].[PR_Transactions_Delete]", conn);
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
            };
            command.Parameters.AddWithValue("@ExpenseID", transactionID);
            conn.Open();
            int rowAffected = command.ExecuteNonQuery();
            return rowAffected > 0;
        }

        public bool insertTransaction(InsertTransaction transactionModel)
        {
            using (SqlConnection conn = new SqlConnection(_ConnectionString))
            {
                SqlCommand command = new SqlCommand("[dbo].[PR_Transactions_Insert]", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Pass only the required parameters
                command.Parameters.AddWithValue("@UserID", transactionModel.UserID);
                command.Parameters.AddWithValue("@EventID", transactionModel.EventID);
                command.Parameters.AddWithValue("@Amount", transactionModel.Amount);
                command.Parameters.AddWithValue("@TransactionDate", transactionModel.TransactionDate);
                command.Parameters.AddWithValue("@TransactionType", transactionModel.TransactionType);
                command.Parameters.AddWithValue("@Description", transactionModel.Description);
                command.Parameters.AddWithValue("@HostId", transactionModel.HostId);


                conn.Open();
                int noOfRowAffected = command.ExecuteNonQuery();

                return noOfRowAffected > 0;
            }
        }


        public bool updateTransaction(InsertTransaction transactionModel)
        {
            using (SqlConnection conn = new SqlConnection(_ConnectionString))
            {
                SqlCommand command = new SqlCommand("[dbo].[PR_Transactions_Update]", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Add only the parameters required by the stored procedure
                command.Parameters.AddWithValue("@UserID", transactionModel.UserID);
                command.Parameters.AddWithValue("@ExpenseID", transactionModel.ExpenseID);
                command.Parameters.AddWithValue("@EventID", transactionModel.EventID);
                command.Parameters.AddWithValue("@Amount", transactionModel.Amount);
                command.Parameters.AddWithValue("@TransactionDate", transactionModel.TransactionDate);
                command.Parameters.AddWithValue("@TransactionType", transactionModel.TransactionType);
                command.Parameters.AddWithValue("@Description", transactionModel.Description);
                command.Parameters.AddWithValue("@HostId", transactionModel.HostId);
                conn.Open();
                int noOfRowAffected = command.ExecuteNonQuery();

                return noOfRowAffected > 0;
            }
        }

        public TransactionsReportModel GetOverallReport(int eventId)
        {
            TransactionsReportModel report = new TransactionsReportModel();

            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                SqlCommand command = new SqlCommand("[dbo].[PR_Report_Generate]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@eventid", eventId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    report.TotalMembers = reader["TotalMembers"] != DBNull.Value ? Convert.ToInt32(reader["TotalMembers"]) : 0;
                    report.TotalIncome = reader["TotalIncome"] != DBNull.Value ? Convert.ToDouble(reader["TotalIncome"]) : 0.0;
                    report.TotalExpense = reader["TotalExpense"] != DBNull.Value ? Convert.ToDouble(reader["TotalExpense"]) : 0.0;
                    report.ExpensePerHead = reader["ExpensePerHead"] != DBNull.Value ? Convert.ToDouble(reader["ExpensePerHead"]) : 0.0;
                }
            }

            return report;
        }

        public IEnumerable<MemberTransactionModel> GetMemberWiseReport(int eventId)
        {
            List<MemberTransactionModel> transactions = new List<MemberTransactionModel>();

            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                SqlCommand command = new SqlCommand("[dbo].[PR_Report_Generate]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@eventid", eventId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.NextResult()) // Move to the second result set
                {
                    while (reader.Read())
                    {
                        transactions.Add(new MemberTransactionModel
                        {
                            Member = reader["Member"] != DBNull.Value ? reader["Member"].ToString() : string.Empty,
                            Income = reader["Income"] != DBNull.Value ? Convert.ToDouble(reader["Income"]) : 0.0,
                            Expense = reader["Expense"] != DBNull.Value ? Convert.ToDouble(reader["Expense"]) : 0.0
                        });
                    }
                }
            }

            return transactions;
        }
    }
}
