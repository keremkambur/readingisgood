using System.Threading.Tasks;
using ReadingIsGood.BusinessLayer.Contracts;
using ReadingIsGood.DataLayer;

namespace ReadingIsGood.BusinessLayer.Initializer
{
    public class DatabaseInitializer : IAsyncInitializer
    {
        private readonly SqlDbContextSeed _seed;

        public DatabaseInitializer(
            SqlDbContext dbContext,
            IBusinessObject businessObject
        )
        {
            this._seed = new SqlDbContextSeed(
                dbContext,
                businessObject.DatabaseRepository
            );
        }

        public Task InitializeAsync()
        {
            //this._seed.MigrateIfAny();
            //this._seed.Seed(); // seeding happens in linq pad script "/operations/Sql.SeedData"

            return Task.CompletedTask;
        }
    }
}