namespace OrdersApi.Security.Authorization.Models
{
    public class TokenRateLimits
    {
        public int RequestsRemaining { get; set; }
        public int MaxRequestsPerPeriod { get; set; }
        public int PeriodSeconds { get; set; }
        public DateTime PeriodEnd { get; set; }
    }
}
