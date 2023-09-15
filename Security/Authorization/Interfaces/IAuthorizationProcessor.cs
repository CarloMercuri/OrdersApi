using OrdersApi.Security.Authentication.Models;

namespace OrdersApi.Security.Authorization.Interfaces
{
    internal interface IAuthorizationProcessor
    {
        AccountLoginResult UseRefreshToken(string refreshToken);
        UserData AuthorizeUser(string accessToken);
    }
}
