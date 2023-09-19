using OrdersApi.Security.Authentication.Models;
using OrdersApi.Security.Authorization.Models;

namespace OrdersApi.Security.Interfaces
{
    public interface ISecurityDatabaseQueries
    {
        bool UserExists(string email);
        AccountSecurityDetails GetUserSecurityDetails(string email);

        bool CreateNewAccount(string userName, string email, string hashedPassword, string salt);

        bool InsertAccessToken(string token, string user_id, DateTime expiration);
        bool InsertRefreshToken(string token, string user_id, DateTime expiration);
        TokenData GetTokenData(string token);
        //UserData GetUserData(int user_id);
    }
}
