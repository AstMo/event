using System;

namespace PartyMaker.Common.Account
{
    public interface IAccountService
    {
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);

        bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt);

        string GeneratePassword();

        void GenerateLinkHash(string email, string password, DateTime dateTime, out byte[] linkHash);
    }
}
