using OrdersApi.Encryption.Interfaces;
using System.Data.SqlTypes;
using System.Security.Cryptography;
using System.Text;

namespace OrdersApi.Encryption
{
    internal class CEncryptor : IEncryptionAgent
    {
        private int HashInterations = 10000;
        private int HashSize = 128;
        private int SaltSize = 128;
        private HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA512;
        private int AuthTokenSize = 128;

        public string HashPassword(string password, string salt)
        {
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);

            return HashPasswordExecute(password, saltBytes);
        }

        private string HashPasswordExecute(string password, byte[] salt)
        {
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                       Encoding.UTF8.GetBytes(password),
                       salt,
                       HashInterations,
                       _hashAlgorithm,
                       HashSize);

            return Convert.ToBase64String(hash);           
        }

        public string GenerateSalt()
        {
            return Convert.ToBase64String(GenerateSaltBytes());
        }

        private byte[] GenerateSaltBytes()
        {
            byte[] saltBytes  = RandomNumberGenerator.GetBytes(SaltSize);
         
            return saltBytes;
        }

        public string CreateAccessToken()
        {
            return GenerateBase64Token();
        }

        public string CreateRefreshToken()
        {
            return GenerateBase64Token();
        }

        private string GenerateBase64Token()
        {
            var tokenBytes = new byte[AuthTokenSize];

            using (var provider = new RNGCryptoServiceProvider())
            {
                provider.GetNonZeroBytes(tokenBytes);
            }

            return Convert.ToBase64String(tokenBytes);
        }

        // NOT REFACTORED YET

        public string HashAccessToken(string token)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return Convert.ToBase64String(algorithm.ComputeHash(Encoding.UTF8.GetBytes(token)));
        }
        
    }
}
