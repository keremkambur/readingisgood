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
        public IDatabaseRepository DatabaseRepository { get; }


        /// <inheritdoc />
        public Guid Id { get; }

        public bool BulkModeIsEnabled => this.DatabaseRepository.BulkMode;

        public void EnableBulkMode()
        {
            this.DatabaseRepository.EnableBulkMode();
        }

        public void DisableBulkMode(bool saveChanges = false)
        {
            this.DatabaseRepository.DisableBulkMode();
        }

        public BusinessObject(SqlDbContext dbContext, ILoggerFactory loggerFactory)
        {
            this.Id = Guid.NewGuid();
            this.DatabaseRepository = new DatabaseRepository(loggerFactory.CreateLogger<DatabaseRepository>(),  dbContext);
        }

        public void Dispose()
        {
            this.DatabaseRepository?.Dispose();
        }
    }
}