using System;
using Microsoft.Extensions.Logging;
using ReadingIsGood.DataLayer.Contracts;

namespace ReadingIsGood.DataLayer
{
    public class SqlDbContextSeed
    {
        private readonly SqlDbContext _dbContext;
        private readonly IDatabaseRepository _databaseRepository;

        public SqlDbContextSeed(
            SqlDbContext dbContext,
            IDatabaseRepository databaseRepository)
        {
            this._dbContext = dbContext;
            this._databaseRepository = databaseRepository;
        }
    }
}