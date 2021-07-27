using System;
using Microsoft.Extensions.Logging;
using ReadingIsGood.BusinessLayer.Contracts;
using ReadingIsGood.DataLayer;
using ReadingIsGood.DataLayer.Contracts;
using ReadingIsGood.DataLayer.Repositories;

namespace ReadingIsGood.BusinessLayer
{
    public class BusinessObject : IBusinessObject
    {
        public BusinessObject(SqlDbContext dbContext, ILoggerFactory loggerFactory)
        {
            Id = Guid.NewGuid();
            DatabaseRepository = new DatabaseRepository(loggerFactory.CreateLogger<DatabaseRepository>(), dbContext);
            AuthRepository = new AuthRepository(loggerFactory.CreateLogger<AuthRepository>(), dbContext);
        }

        public IDatabaseRepository DatabaseRepository { get; }
        public IAuthRepository AuthRepository { get; }


        /// <inheritdoc />
        public Guid Id { get; }

        public bool BulkModeIsEnabled => DatabaseRepository.BulkMode;

        public void EnableBulkMode()
        {
            DatabaseRepository.EnableBulkMode();
        }

        public void DisableBulkMode(bool saveChanges = false)
        {
            DatabaseRepository.DisableBulkMode();
        }

        public void Dispose()
        {
            DatabaseRepository?.Dispose();
        }
    }
}