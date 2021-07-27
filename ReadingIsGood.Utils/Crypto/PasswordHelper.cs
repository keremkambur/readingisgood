using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ReadingIsGood.Utils.Crypto
{
    // https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-2.1
    public static class PasswordHelper
    {
        public static string GenerateHashedPassword(string password, byte[] salt, int iterations = 10_000,
            int bytesRequested = 1024)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password,
                salt,
                KeyDerivationPrf.HMACSHA512,
                iterations,
                bytesRequested / 8));
        }
    }
}