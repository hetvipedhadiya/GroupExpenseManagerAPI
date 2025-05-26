using ExpenseManagerAPI.Model;
using System.Data;
using System.Data.SqlClient;

namespace ExpenseManagerAPI.Repository
{
    public class UserRepository
    {
        private IConfiguration _configuration;
        private readonly String _ConnectionString;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<PersonModel> getAllUser()
        {
            var users = new List<PersonModel>();
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {


                SqlCommand sqlCommand = new SqlCommand("[dbo].[PR_User_SelectAll]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                connection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    users.Add(new PersonModel
                    {
                        UserID = reader["UserID"] != DBNull.Value ? Convert.ToInt32(reader["UserID"]) : 0,
                        UserName = reader["UserName"] != DBNull.Value ? reader["UserName"].ToString() : string.Empty,
                        HostId = reader["HostId"] != DBNull.Value ? Convert.ToInt32(reader["HostId"]) : 0,
                        // UserImage = reader["UserImage"] != DBNull.Value ? reader["UserImage"].ToString() : string.Empty
                    });
                }
            }
            return users;
        }

        public List<PersonModel> GetUserByHost(int hostId)
        {
            List<PersonModel> userList = new List<PersonModel>();
            SqlConnection conn = new SqlConnection(_ConnectionString);

            SqlCommand command = new SqlCommand("[GetUsersByHostId]", conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@HostId", hostId);

            conn.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                var user = new PersonModel
                {
                    UserName = reader["UserName"] != DBNull.Value ? reader["UserName"].ToString() : string.Empty,
                    UserID = Convert.ToInt32(reader["UserID"]),
                    HostId = Convert.ToInt32(reader["HostId"]),
                    EventID = Convert.ToInt16(reader["EventID"] != DBNull.Value ? reader["EventID"] : 0)
                };

                userList.Add(user);
            }

            conn.Close();
            return userList;
        }

        public PersonModel GetUserByID(int userID)
        {
            PersonModel userlist = null;
            SqlConnection conn = new SqlConnection(_ConnectionString);


            SqlCommand command = new SqlCommand("[PR_User_SelectByID]", conn);
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
            };
            command.Parameters.AddWithValue("@UserID", userID);
            conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                userlist = new PersonModel
                {
                    UserID = reader["UserID"] != DBNull.Value ? Convert.ToInt32(reader["UserID"]) : 0,
                    UserName = reader["UserName"] != DBNull.Value ? reader["UserName"].ToString() : string.Empty,
                   // UserImage = reader["UserImage"] != DBNull.Value ? reader["UserImage"].ToString() : string.Empty

                };

            }


            return userlist;
        }

        public bool deleteUser(int userID)
        {
            SqlConnection conn = new SqlConnection(_ConnectionString);
            SqlCommand command = new SqlCommand("[dbo].[PR_User_Delete]", conn);
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
            };
            command.Parameters.AddWithValue("@UserID", userID);
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

        public bool insertUser(PersonModel insertUser)
        {
            using (SqlConnection conn = new SqlConnection(_ConnectionString))
            {
                SqlCommand command = new SqlCommand("[dbo].[PR_User_Insert]", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@UserName", insertUser.UserName);
                command.Parameters.AddWithValue("@EventID", insertUser.EventID);
                command.Parameters.AddWithValue("@HosiId", insertUser.HostId);
                // command.Parameters.AddWithValue("@UserImage", insertUser.UserImage);




                conn.Open();
                int noOfRowAffected = command.ExecuteNonQuery();

                return noOfRowAffected > 0;
            }
        }


        public bool updateUser(PersonModel user)
        {
            using (SqlConnection conn = new SqlConnection(_ConnectionString))
            {
                SqlCommand command = new SqlCommand("[dbo].[PR_User_Update]", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Add only the parameters required by the stored procedure
                command.Parameters.AddWithValue("@UserName", user.UserName);
                command.Parameters.AddWithValue("@UserID", user.UserID);
                command.Parameters.AddWithValue("@HostId", user.HostId);
                // command.Parameters.AddWithValue("@UserImage", user.UserImage);
                //command.Parameters.AddWithValue("@EventID", user.EventID);

                conn.Open();
                int noOfRowAffected = command.ExecuteNonQuery();

                return noOfRowAffected > 0;
            }
        }

        public IEnumerable<UserDropDownModel> GetUserDropDown(int EventID)
        {
            var users = new List<UserDropDownModel>();

            using (SqlConnection conn = new SqlConnection(_ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_User_DropDown", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@EventID", EventID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(new UserDropDownModel
                    {
                        UserID = Convert.ToInt32(reader["UserID"]),
                        UserName = reader["UserName"].ToString()
                    });
                }
            }

            return users;
        }
    }
}
