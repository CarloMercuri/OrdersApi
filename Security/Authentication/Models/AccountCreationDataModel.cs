namespace OrdersApi.Authentication.Models
{
    public class AccountCreationDataModel
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PlaintextPassword { get; set; }
    }
}
