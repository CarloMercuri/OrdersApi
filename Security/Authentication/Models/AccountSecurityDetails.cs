namespace OrdersApi.Security.Authentication.Models
{
    public class AccountSecurityDetails
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public DateTime LastLogin { get; set; }
    }
}
