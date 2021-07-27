using System;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReadingIsGood.DataLayer.Mappings.Base;

namespace ReadingIsGood.DataLayer
{
    public class SqlDbContext : DbContext
    {
        private static readonly ILoggerFactory
            _sqlLoggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

        private readonly bool _useLoggerFactory;

        public SqlDbContext(IOptions<SqlOptions> dbContextOptions, IEntityMapper entityMapper)
        {
            Id = Guid.NewGuid();
            ConnectionString = dbContextOptions.Value.ConnectionString;
            EntityMapper = entityMapper;
            _useLoggerFactory = dbContextOptions.Value.UseLoggerFactory;
        }

        public Guid Id { get; }
        public string ConnectionString { get; }
        public IEntityMapper EntityMapper { get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connection = new SqlConnection
            {
                ConnectionString = ConnectionString
            };

            Console.WriteLine("######################### DataSource #########################");
            Console.WriteLine(connection.DataSource);

            var accessToken = Environment.GetEnvironmentVariable("AZURE_SQL_ACCESS_TOKEN");

            if (accessToken == null)
            {
                if (!(connection.DataSource != null
                      && (connection.DataSource.Equals(@"(localdb)\mssqllocaldb",
                              StringComparison.InvariantCultureIgnoreCase)
                          || connection.DataSource.Equals("host.docker.internal,1337",
                              StringComparison.InvariantCultureIgnoreCase))))
                    connection.AccessToken = new AzureServiceTokenProvider()
                            .GetAccessTokenAsync("https://database.windows.net/")
                            .GetAwaiter()
                            .GetResult()
                        ;
            }
            else
            {
                connection.AccessToken = accessToken;
            }

            if (_useLoggerFactory) optionsBuilder.UseLoggerFactory(_sqlLoggerFactory);

            optionsBuilder
                .UseSqlServer(connection, builder => { builder.MigrationsHistoryTable("Migrations", "System"); })
                .ConfigureWarnings(warnings => warnings.Default(WarningBehavior.Log));

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            EntityMapper.MapEntities(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}