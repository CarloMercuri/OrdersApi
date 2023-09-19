using OrdersApi.Authorization;
using OrdersApi.Data.Database;
using OrdersApi.Security.Authentication.Models;
using OrdersApi.Security.Authorization.Models;
using OrdersApi.Security.Interfaces;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace OrdersApi.Security.Database
{
    public class SecurityDatabaseQueries : ISecurityDatabaseQueries
    {
        ISecurityDatabaseConnections _db;
        DatabaseUtilities _utilities = new DatabaseUtilities();

        public SecurityDatabaseQueries(ISecurityDatabaseConnections db)
        {
            _db = db;
        }

        public AccountSecurityDetails GetUserSecurityDetails(string email)
        {
            try
            {
                string query = "SELECT * FROM UserData WHERE user_email = @email";
                DataTable queryResponse = _db.ExecuteQuery(query, new SqlParameter("@email", email));

                if(queryResponse.Rows.Count <= 0)
                {
                    return null;
                }

                AccountSecurityDetails details = new AccountSecurityDetails();

                details.UserID = _utilities.FetchAsString(queryResponse.Rows[0]["user_id"]);
                details.UserName = _utilities.FetchAsString(queryResponse.Rows[0]["user_name"]);
                details.Email = _utilities.FetchAsString(queryResponse.Rows[0]["user_email"]);
                details.PasswordHash = _utilities.FetchAsString(queryResponse.Rows[0]["password_hash"]);
                details.Salt = _utilities.FetchAsString(queryResponse.Rows[0]["user_salt"]);
                details.LastLogin = _utilities.FetchAsDateTime(queryResponse.Rows[0]["last_login"], DateTime.Now);

                return details;
            }
            catch (Exception ex)
            {                
                return null;
            }
        }

        public AccountSecurityDetails GetUserData(int userId)
        {
            try
            {
                string query = "SELECT * FROM UserData WHERE user_id = @userId";
                DataTable queryResponse = _db.ExecuteQuery(query, new SqlParameter("@userId", userId));

                if (queryResponse.Rows.Count <= 0)
                {
                    return null;
                }

                // Get user data  + role

                //UserData details = new UserData();

                //details. = _utilities.FetchAsString(queryResponse.Rows[0]["user_id"]);
                //details.UserName = _utilities.FetchAsString(queryResponse.Rows[0]["user_name"]);
                //details.Email = _utilities.FetchAsString(queryResponse.Rows[0]["user_email"]);
                //details.PasswordHash = _utilities.FetchAsString(queryResponse.Rows[0]["password_hash"]);
                //details.Salt = _utilities.FetchAsString(queryResponse.Rows[0]["user_salt"]);
                //details.LastLogin = _utilities.FetchAsDateTime(queryResponse.Rows[0]["last_login"], DateTime.Now);

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool UserExists(string email)
        {
            try
            {
                string query = "SELECT user_id FROM UserData WHERE user_email = @email";

                DataTable queryResponse = _db.ExecuteQuery(query, new SqlParameter("@email", email));

                if(queryResponse.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CreateNewAccount(string userName, string email, string hashedPassword, string salt)
        {
            try
            {
                string userDataQuery = "INSERT INTO UserData (user_name, user_email, password_hash, user_salt, last_login)" +
                                       " VALUES (@userName, @email, @hash, @salt, @lastLogin)";

                int response = _db.ExecuteNonQuery(userDataQuery, new SqlParameter("@userName", userName),
                                                                  new SqlParameter("@email", email),
                                                                  new SqlParameter("@hash", hashedPassword),
                                                                  new SqlParameter("@salt", salt),
                                                                  new SqlParameter("@lastLogin", DBNull.Value));

                return response > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //
        // TOKENS
        //

        public TokenData GetTokenData(string token)
        {
            try
            {
                string query = "SELECT token_type, user_id, expiration_datetime FROM access_tokens WHERE token_uuid = @token";

                DataTable queryResponse = _db.ExecuteQuery(query, new SqlParameter("@token", token));

                if (queryResponse.Rows.Count > 0)
                {
                    TokenData data = new TokenData();
                    data.Type = (TokenType)_utilities.FetchAsInt32(queryResponse.Rows[0]["token_type"]);
                    data.UserID = _utilities.FetchAsInt32(queryResponse.Rows[0]["user_id"]);
                    data.Expiration = _utilities.FetchAsDateTime(queryResponse.Rows[0]["expiration_datetime"], DateTime.Now.AddDays(-5));

                    return data;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool InsertAccessToken(string token, string user_id, DateTime expiration)
        {
            try
            {
                // Disable all the user tokens 
                string disableQuery = "UPDATE access_tokens SET expiration_datetime = @disabledDateTime WHERE user_id = @userId";


                int disableResponse = _db.ExecuteNonQuery(disableQuery,
                                                          _utilities.AddSqlParameter("@disabledDateTime", DateTime.Now.AddDays(-20)),
                                                          _utilities.AddSqlParameter("@userId", user_id));

                // Create a new token
                string query = "INSERT INTO access_tokens (token_type, token_uuid, user_id, expiration_datetime)" +
                                       " VALUES (@tokenType, @tokenUuid, @userId, @expiration)";

                int response = _db.ExecuteNonQuery(query, _utilities.AddSqlParameter("@tokenType", (int)TokenType.AccessToken),
                                                          _utilities.AddSqlParameter("@tokenUuid", token),
                                                          _utilities.AddSqlParameter("@userId", user_id),
                                                          _utilities.AddSqlParameter("@expiration", expiration));

                return response > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool InsertRefreshToken(string token, string user_id, DateTime expiration)
        {
            try
            {
                string query = "INSERT INTO access_tokens (token_type, token_uuid, user_id, expiration_datetime)" +
                                      " VALUES (@tokenType, @tokenUuid, @userId, @expiration)";

                int response = _db.ExecuteNonQuery(query, _utilities.AddSqlParameter("@tokenType", (int)TokenType.RefreshToken),
                                                          _utilities.AddSqlParameter("@tokenUuid", token),
                                                          _utilities.AddSqlParameter("@userId", user_id),
                                                          _utilities.AddSqlParameter("@expiration", expiration));

                return response > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
