using System;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using ReadingIsGood.DataLayer.Contracts;
using ReadingIsGood.DataLayer.Extensions;
using ReadingIsGood.EntityLayer.Database.Auth;
using ReadingIsGood.EntityLayer.QueryModels;
using ReadingIsGood.Utils.Crypto;

namespace ReadingIsGood.DataLayer.Repositories
{
    public class AuthRepository : DatabaseRepository, IAuthRepository
    {
        public AuthRepository(
            ILogger<AuthRepository> logger,
            SqlDbContext dbContext
        ) : base(logger, dbContext)
        {
            UserCrudOperations = new CrudOperations<User>(this);
            RefreshTokenCrudOperations = new CrudOperations<RefreshToken>(this);
        }

        public override void Dispose()
        {
            DbContext?.Dispose();
        }

        public ICrudOperations<User> UserCrudOperations { get; }
        public ICrudOperations<RefreshToken> RefreshTokenCrudOperations { get; }

        public bool CheckIfEmailExists(string email)
        {
            return DbContext
                .Set<User>()
                .Any(p => p.Email == email); // using == since to use string.Equals() we need to introduce db function.
        }
        
        public QueryUserFromLoginResponse QueryUserFromLogin(string email, string password, string salt)
        {
            var hashedPassword = PasswordHelper.GenerateHashedPassword(password, Encoding.ASCII.GetBytes(salt));

            var user = DbContext
                    .Set<User>()
                    .SingleOrDefault(p =>
                        p.Email == email &&
                        p.PasswordHashed ==
                        hashedPassword) // using == since to use string.Equals() we need to introduce db function.
                ;

            return user?.ToQueryUserResponse(Guid.NewGuid());
        }

        /// <summary>
        ///     Get refresh token and load user.
        /// </summary>
        /// <param name="refreshToken">Refresh token</param>
        /// <param name="clientId"></param>
        /// <param name="newRefreshToken"></param>
        /// <param name="refreshTokenExpiresAt"></param>
        /// <returns></returns>
        public QueryUserFromLoginResponse QueryAndCreateNewRefreshTokenFromLogin(
            string refreshToken,
            Guid clientId,
            string newRefreshToken,
            DateTime refreshTokenExpiresAt
        )
        {
            var refreshTokenObj = RefreshTokenCrudOperations
                    .QuerySingle(x => x.Token == refreshToken, r => r.User)
                ;

            if (refreshTokenObj?.IsExpired() != false) return null;

            var user = DbContext
                .Set<User>()
                .SingleOrDefault(x => x.Uuid == refreshTokenObj.User.Uuid);

            if (user == null)
                //base.Logger.LogWarning($"Unable to find user with uuid [{refreshTokenObj.User.Uuid}].");

                return null;

            RefreshTokenCrudOperations.Update(refreshTokenObj.Uuid, x => x.Rejected = true);
            RefreshTokenCrudOperations.Create(new RefreshToken
            {
                ClientId = clientId,
                Token = newRefreshToken,
                User = refreshTokenObj.User,
                ExpiresAt = refreshTokenExpiresAt
            });

            return user.ToQueryUserResponse(clientId);
        }

        /// <summary>
        ///     Get refresh token and load user.
        /// </summary>
        /// <param name="userUuid"></param>
        /// <param name="clientId"></param>
        /// <param name="newRefreshToken"></param>
        /// <param name="refreshTokenExpiresAt"></param>
        /// <returns></returns>
        public bool RejectExistingAndCreateNewRefreshTokenFromLogin(Guid userUuid, Guid clientId,
            string newRefreshToken, DateTime refreshTokenExpiresAt)
        {
            var user = UserCrudOperations.Read(userUuid);
            if (user == null) return false;

            var refreshTokens = RefreshTokenCrudOperations
                .QueryList(p => p.User == user
                                && p.ClientId == clientId
                                && !p.Rejected
                );

            EnableBulkMode();

            foreach (var token in refreshTokens) RefreshTokenCrudOperations.Update(token.Uuid, x => x.Rejected = true);

            RefreshTokenCrudOperations.Create(new RefreshToken
            {
                Token = newRefreshToken,
                User = user,
                ExpiresAt = refreshTokenExpiresAt,
                ClientId = clientId
            });

            DisableBulkMode(true);

            return true;
        }

        /// <summary>
        ///     Get refresh token and load user.
        /// </summary>
        /// <param name="userUuid">User Uuid</param>
        /// <param name="clientId"></param>
        /// <param name="newRefreshToken"></param>
        /// <param name="refreshTokenExpiresAt"></param>
        /// <returns></returns>
        public bool CreateNewRefreshTokenFromLogin(Guid userUuid, Guid clientId, string newRefreshToken,
            DateTime refreshTokenExpiresAt)
        {
            var user = UserCrudOperations.Read(userUuid);

            if (user == null) return false;

            RefreshTokenCrudOperations.Create(new RefreshToken
            {
                Token = newRefreshToken,
                User = user,
                ExpiresAt = refreshTokenExpiresAt,
                ClientId = clientId
            });

            return true;
        }

        public QueryUserFromLoginResponse QueryUserFromLogin(Guid uuid, Guid clientId)
        {
            var user = DbContext
                    .Set<User>()
                    .SingleOrDefault(p => p.Uuid == uuid)
                ;

            return user?.ToQueryUserResponse(clientId);
        }
    }
}