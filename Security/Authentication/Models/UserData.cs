namespace OrdersApi.Security.Authentication.Models
{
    public class UserData
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }
    }
}
