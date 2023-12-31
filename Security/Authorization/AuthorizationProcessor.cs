﻿using OrdersApi.Encryption.Interfaces;
using OrdersApi.Security.Authentication.Models;
using OrdersApi.Security.Authorization.Interfaces;
using OrdersApi.Security.Authorization.Models;
using OrdersApi.Security.Interfaces;

namespace OrdersApi.Security.Authorization
{
    internal class AuthorizationProcessor : IAuthorizationProcessor
    {
        IEncryptionProcessor _encryptor;
        ISecurityDatabaseQueries _dbQueries;

        public AuthorizationProcessor(IEncryptionProcessor encryptor, ISecurityDatabaseQueries dbQueries)
        {
            _encryptor = encryptor;
            _dbQueries = dbQueries;
        }

        public void AdjustTokenRates(TokenData tokenData)
        {

        }

        public UserData AuthorizeUser(string accessToken)
        {
            TokenData token =  _dbQueries.GetTokenData(accessToken);

            if(token is null)
            {
                return null;
            }

            // TODO
            return null;

        }

        public TokenData GetTokenData(string token)
        {
            return _dbQueries.GetTokenData(token);
        }

        public AccountLoginResult UseRefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
