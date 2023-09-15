using OrdersApi.Authentication.Models;
using OrdersApi.Security.Authentication.Models;

namespace OrdersApi.Authentication.Interfaces
{
    public interface IAuthenticatorProcess
    {
        AccountCreationResult CreateNewAccount(string email, string userName, string plainTextPassword);
        AccountLoginResult PerformLogin(string email, string plainTextPassword);
    }
}
