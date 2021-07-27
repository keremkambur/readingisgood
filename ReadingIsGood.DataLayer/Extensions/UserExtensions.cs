using System;
using ReadingIsGood.EntityLayer.Database.Auth;
using ReadingIsGood.EntityLayer.QueryModels;

namespace ReadingIsGood.DataLayer.Extensions
{
    public static class UserExtensions
    {
        public static QueryUserFromLoginResponse ToQueryUserResponse(this User user, Guid clientId)
        {
            return new()
            {
                Uuid = user.Uuid,
                ClientId = clientId
            };
        }
    }
}