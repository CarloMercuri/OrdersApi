namespace OrdersApi.Encryption.Interfaces
{
    internal interface IEncryptionAgent
    {
        string HashPassword(string password, string salt);
        string GenerateSalt();
    }
}
