namespace OrdersApi.Security.Authentication.Models
{
    public class AccountLoginResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
