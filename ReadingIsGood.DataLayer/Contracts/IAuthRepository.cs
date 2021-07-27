using System;
using ReadingIsGood.EntityLayer.Database.Auth;
using ReadingIsGood.EntityLayer.QueryModels;

namespace ReadingIsGood.DataLayer.Contracts
{
    public interface IAuthRepository : IDatabaseRepository
    {
        ICrudOperations<User> UserCrudOperations { get; }

        ICrudOperations<RefreshToken> RefreshTokenCrudOperations { get; }

        QueryUserFromLoginResponse QueryUserFromLogin(string email, string password, string salt);

        QueryUserFromLoginResponse QueryAndCreateNewRefreshTokenFromLogin(
            string refreshToken,
            Guid clientId,
            string newRefreshToken,
            DateTime refreshTokenExpiresAt
        );

        bool RejectExistingAndCreateNewRefreshTokenFromLogin(
            Guid userUuid,
            Guid clientId,
            string newRefreshToken,
            DateTime refreshTokenExpiresAt
        );

        bool CreateNewRefreshTokenFromLogin(
            Guid userUuid,
            Guid clientId,
            string newRefreshToken,
            DateTime refreshTokenExpiresAt
        );

        bool CheckIfEmailExists(string email);
    }
}