using OrdersApi.Encryption.Interfaces;

namespace OrdersApi.Encryption
{
    internal class CEncryptionProcessor : IEncryptionProcessor
    {
        private IEncryptionAgent _encryptor;
        public CEncryptionProcessor(IEncryptionAgent encryptor)
        {
            _encryptor = encryptor;
        }

        public string CreateAccessToken()
        {
            return _encryptor.CreateAccessToken();
        }

        public string CreateRefreshToken()
        {
            return _encryptor.CreateRefreshToken();
        }

        /// <summary>
        /// Use for FIRST TIME hashing of password, for storage. Returns the hashed password and the salt used.
        /// </summary>
        /// <param name="plainTextPassword"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public string HashPassword(string plainTextPassword, out string salt)
        {
            salt = _encryptor.GenerateSalt();
            return _encryptor.HashPassword(plainTextPassword, salt);
        }

        /// <summary>
        /// Returns TRUE if the input plaintext password matches the stored hashed password after hashing of the input.
        /// </summary>
        /// <param name="plainTextPassword"></param>
        /// <param name="hashedPassword"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public bool VerifyPassword(string plainTextPassword, string hashedPassword, string salt)
        {
            string inputHashed = _encryptor.HashPassword(plainTextPassword, salt);

            return inputHashed == hashedPassword;
        }
    }
}
