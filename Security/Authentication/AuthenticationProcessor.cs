using OrdersApi.Authentication.Interfaces;
using OrdersApi.Authentication.Models;
using OrdersApi.Encryption.Interfaces;
using OrdersApi.Security.Authentication.Models;
using OrdersApi.Security.Interfaces;

namespace OrdersApi.Security.Authentication
{
    internal class AuthenticationProcessor : IAuthenticatorProcess
    {
        IEncryptionProcessor _encryptor;
        ISecurityDatabaseQueries _dbQueries;

        public AuthenticationProcessor(IEncryptionProcessor encryptor, ISecurityDatabaseQueries dbQueries)
        {
            _encryptor = encryptor;
            _dbQueries = dbQueries;
        }

        public AccountCreationResult CreateNewAccount(string email, string userName, string plainTextPassword)
        {
            AccountCreationResult result = new AccountCreationResult();

            try
            {
                // Sanitize inputs

                // Enforce eventual password requirements

                // See if user already exists
                bool exists = _dbQueries.UserExists(email);

                if (exists)
                {
                    result.Success = false;
                    result.Message = "User with that email already exists.";
                    return result;
                }

                string saltUsed;
                string pwHash = _encryptor.HashPassword(plainTextPassword, out saltUsed);

                bool creation = _dbQueries.CreateNewAccount(userName, email, pwHash, saltUsed);

                if (!creation)
                {
                    result.Success = false;
                    result.Message = "Something went wrong with account creation";
                    return result;
                }

                // SUCCESS

                result.Success = true;
                result.Message = "Account created successfully";
                return result;

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Critical error occourred while performing action.";

                return result;
            }
           
        }


        public AccountLoginResult PerformLogin(string email, string plainTextPassword)
        {
            AccountSecurityDetails details = _dbQueries.GetUserSecurityDetails(email);
            AccountLoginResult result = new AccountLoginResult();


            if (details is null)
            {
                result.Success = false;
                result.Message = "User not found.";
                return result;
            }

            bool match = _encryptor.VerifyPassword(plainTextPassword, details.PasswordHash, details.Salt);

            if (!match)
            {
                result.Success = false;
                result.Message = "Email/Password don't match.";
                return result;
            }

            // Login successful.

            string accessToken = _encryptor.CreateAccessToken();
            string refreshToken = _encryptor.CreateRefreshToken();

            DateTime AccessTokenExpiration = DateTime.Now.AddHours(1);
            DateTime RefreshTokenExpiration = DateTime.Now.AddHours(2);

            bool accessTokenSuccess = _dbQueries.InsertAccessToken(accessToken, details.UserID, AccessTokenExpiration);

            if (!accessTokenSuccess)
            {
                result.Success = false;
                result.Message = "Error generating token.";
                return result;
            }

            bool refreshTokenSuccess = _dbQueries.InsertAccessToken(refreshToken, details.UserID, RefreshTokenExpiration);

            if (!refreshTokenSuccess)
            {
                result.Success = false;
                result.Message = "Error generating token.";
                return result;
            }

            // All good!

            AccountLoginResult successResult = new AccountLoginResult();
            result.Success = true;
            result.AccessToken = accessToken;
            result.RefreshToken = refreshToken;
            return result;

        }
    }
}
