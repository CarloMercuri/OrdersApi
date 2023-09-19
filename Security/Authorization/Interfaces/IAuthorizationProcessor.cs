using OrdersApi.Security.Authentication.Models;
using OrdersApi.Security.Authorization.Models;

namespace OrdersApi.Security.Authorization.Interfaces
{
    public interface IAuthorizationProcessor
    {
        AccountLoginResult UseRefreshToken(string refreshToken);
        UserData AuthorizeUser(string accessToken);
        TokenData GetTokenData(string token);
    }
}
