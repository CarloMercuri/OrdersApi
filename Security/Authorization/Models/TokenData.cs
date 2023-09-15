using OrdersApi.Authorization;

namespace OrdersApi.Security.Authorization.Models
{
    public class TokenData
    {
        public TokenType Type { get; set; }
        public int UserID { get; set; }
        public DateTime Expiration { get; set; }
    }
}
