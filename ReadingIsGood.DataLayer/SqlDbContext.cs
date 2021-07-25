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
        public Guid Id { get; }
        public string ConnectionString { get; }
        public IEntityMapper EntityMapper { get; }

        private readonly bool _useLoggerFactory;
        private static readonly ILoggerFactory _sqlLoggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

        public SqlDbContext(IOptions<SqlOptions> dbContextOptions, IEntityMapper entityMapper)
        {
            this.Id = Guid.NewGuid();
            this.ConnectionString = dbContextOptions.Value.ConnectionString;
            this.EntityMapper = entityMapper;
            this._useLoggerFactory = dbContextOptions.Value.UseLoggerFactory;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connection = new SqlConnection
            {
                ConnectionString = this.ConnectionString
            };

            Console.WriteLine("######################### DataSource #########################");
            Console.WriteLine(connection.DataSource);

            var accessToken = Environment.GetEnvironmentVariable("AZURE_SQL_ACCESS_TOKEN");

            if (accessToken == null)
            {
                if (!(connection.DataSource != null
                   && (connection.DataSource.Equals(@"(localdb)\mssqllocaldb", StringComparison.InvariantCultureIgnoreCase)
                    || connection.DataSource.Equals("host.docker.internal,1337", StringComparison.InvariantCultureIgnoreCase))))
                {
                    connection.AccessToken = new AzureServiceTokenProvider()
                                            .GetAccessTokenAsync("https://database.windows.net/")
                                            .GetAwaiter()
                                            .GetResult()
                        ;
                }
            }
            else
            {
                connection.AccessToken = accessToken;
            }

            if (this._useLoggerFactory)
            {
                optionsBuilder.UseLoggerFactory(SqlDbContext._sqlLoggerFactory);
            }

            optionsBuilder
               .UseSqlServer(connection, builder =>
                {
                    builder.MigrationsHistoryTable("Migrations", "System");
                    builder.MigrationsAssembly("Mdr.Nikz.Shared.Migrations");
                })
               .ConfigureWarnings(warnings => warnings.Default(WarningBehavior.Log));
            //optionsBuilder.EnableSensitiveDataLogging();

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            this.EntityMapper.MapEntities(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}