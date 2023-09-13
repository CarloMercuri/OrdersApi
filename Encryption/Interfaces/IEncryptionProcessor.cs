namespace OrdersApi.Encryption.Interfaces
{
    public interface IEncryptionProcessor
    {
        bool VerifyPassword(string plainTextPassword, string hashedPassword, string salt);
        string HashPassword(string plainTextPassword, out string salt);
    }
}
