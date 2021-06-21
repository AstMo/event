using PartyMaker.Common.Account;
using System;
using System.Linq;
using System.Text;

namespace PartyMaker.Common.Impl.Admin
{
    public class AccountService : IAccountService
    {
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password", "Password is null");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            }

            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        public string GeneratePassword()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZqwertyuiopasdfghjklzxcvbnm!@#$%^&*()0123456789";
            return new string(Enumerable.Repeat(chars, 16)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public void GenerateLinkHash(string email, string password, DateTime dateTime, out byte[] linkHash)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            linkHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(email+password+dateTime.ToString()));
        }

        public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password", "Password is null");
            }

            if (storedHash == null)
            {
                throw new ArgumentNullException("storedHash", "Stored hash is null");
            }

            if (storedSalt == null)
            {
                throw new ArgumentNullException("storedSalt", "Stored salt is null");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            }

            if (storedHash.Length != 64)
            {
                throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "storedHash");
            }

            if (storedSalt.Length != 128)
            {
                throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "storedSalt");
            }

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computeHash.Length; i++)
                {
                    if (computeHash[i] != storedHash[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
