using System;
using ReadingIsGood.EntityLayer.Database.Auth;

namespace ReadingIsGood.DataLayer.Extensions
{
    public static class RefreshTokenExtensions
    {
        public static bool IsExpired(this RefreshToken refreshToken)
        {
            return refreshToken.Rejected || refreshToken.ExpiresAt < DateTime.UtcNow;
        }
    }
}