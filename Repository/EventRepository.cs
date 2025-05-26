using ExpenseManagerAPI.Model;
using System.Data;
using System.Data.SqlClient;

namespace ExpenseManagerAPI.Repository
{
    public class EventRepository
    {
        private IConfiguration _configuration;
        private readonly String _ConnectionString;

        public EventRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<EventModel> getAllEvent()
        {
            var events = new List<EventModel>();
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("[dbo].[PR_Event_SelectAll]", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                connection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    events.Add(new EventModel
                    {
                        EventID = reader["EventID"] != DBNull.Value ? Convert.ToInt32(reader["EventID"]) : 0,
                        EventName = reader["EventName"] != DBNull.Value ? reader["EventName"].ToString() : string.Empty,
                        EventDate = reader["EventDate"] != DBNull.Value ? (DateTime)reader["EventDate"] : DateTime.Now,
                        Amount = Convert.ToDouble(reader["Amount"])

                    });
                }
            }
            return events;
        }

        public List<PersonModel> GetEventByID(int eventID, int hostID)
        {
            List<PersonModel> userList = new List<PersonModel>();

            using (SqlConnection conn = new SqlConnection(_ConnectionString))
            using (SqlCommand command = new SqlCommand("[PR_User_SelectByEvent]", conn))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EventID", eventID);
                command.Parameters.AddWithValue("@HostID", hostID);

                conn.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var user = new PersonModel
                        {
                            UserID = reader["UserID"] != DBNull.Value ? Convert.ToInt32(reader["UserID"]) : 0,
                            UserName = reader["UserName"]?.ToString() ?? string.Empty,
                            EventID = reader["EventID"] != DBNull.Value ? Convert.ToInt32(reader["EventID"]) : 0,
                            HostId = reader["HostId"] != DBNull.Value ? Convert.ToInt32(reader["HostId"]) : 0
                        };

                        userList.Add(user);
                    }
                }
            }

            return userList;
        }



        public List<EventModel> GetEventsByHostID(int hostID)
        {
            List<EventModel> events = new List<EventModel>();
            using (SqlConnection conn = new SqlConnection(_ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[GetEventsByHostId]", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@HostID", hostID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    events.Add(new EventModel
                    {
                        EventName = reader["EventName"].ToString(),
                        EventDate = Convert.ToDateTime(reader["EventDate"]),
                        HostID = Convert.ToInt16(reader["HostId"]),
                        EventID = Convert.ToInt16(reader["EventID"]),
                        Amount = Convert.ToDouble(reader["Amount"])
                    });
                }

                conn.Close();
            }
            return events;
        }


        public bool deleteEvent(int eventID)
        {
            SqlConnection conn = new SqlConnection(_ConnectionString);
            SqlCommand command = new SqlCommand("[dbo].[Pr_Event_Delete]", conn);
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
            };
            command.Parameters.AddWithValue("@EventID", eventID);
            conn.Open();
            try
            {
                int rowAffected = command.ExecuteNonQuery();
                return rowAffected > 0;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547) // Foreign key conflict error code
                {
                    throw new InvalidOperationException("Cannot delete user due to related data.", ex);
                }

                // For other SQL exceptions, rethrow the error
                throw;
            }
        }

        public bool insertEvent(InsertEvent eventModel)
        {
            using (SqlConnection conn = new SqlConnection(_ConnectionString))
            {
                SqlCommand command = new SqlCommand("[dbo].[Pr_Event_Insert]", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Pass only the required parameters
                command.Parameters.AddWithValue("@EventName", eventModel.EventName);
                command.Parameters.AddWithValue("@EventDate", eventModel.EventDate);
                command.Parameters.AddWithValue("@HostID", eventModel.HostID);


                conn.Open();
                int noOfRowAffected = command.ExecuteNonQuery();

                return noOfRowAffected > 0;
            }
        }


        public bool updateEvent(InsertEvent eventModel)
        {
            using (SqlConnection conn = new SqlConnection(_ConnectionString))
            {
                SqlCommand command = new SqlCommand("[dbo].[Pr_Event_Update]", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Add only the parameters required by the stored procedure
                command.Parameters.AddWithValue("@EventName", eventModel.EventName);
                command.Parameters.AddWithValue("@EventDate", eventModel.EventDate);
                command.Parameters.AddWithValue("@EventID", eventModel.EventID);
                command.Parameters.AddWithValue("@HostID", eventModel.HostID);
                conn.Open();
                int noOfRowAffected = command.ExecuteNonQuery();

                return noOfRowAffected > 0;
            }
        }
    }
}
