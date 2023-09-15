namespace OrdersApi.Encryption.Interfaces
{
    internal interface IEncryptionProcessor
    {
        bool VerifyPassword(string plainTextPassword, string hashedPassword, string salt);
        string HashPassword(string plainTextPassword, out string salt);
        string CreateAccessToken();
        string CreateRefreshToken();
    }
}
