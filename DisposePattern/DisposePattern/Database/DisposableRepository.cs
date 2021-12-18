using System;
using System.Data.SQLite;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;

namespace DisposePattern.Database
{
    public sealed class DisposableRepository : IDisposable
    {
        private readonly SQLiteConnection connection;
        private readonly ILogger<DisposableRepository> logger;

        public DisposableRepository(SQLiteConnection connection, ILogger<DisposableRepository> logger)
        {
            this.connection = connection;
            this.logger = logger;
            logger.LogInformation("New instance of Repository is created.");
        }

        public Task<int> TestConnection()
        {
            return connection.QuerySingleAsync<int>("select 1");
        }

        public void Dispose()
        {
            connection?.Dispose();
            logger.LogInformation("Instance of Repository is disposed.");
        }
    }

    public class InheritableRepository : IDisposable
    {
        private readonly SQLiteConnection connection;
        private readonly ILogger<DisposableRepository> logger;

        public InheritableRepository(SQLiteConnection connection, ILogger<DisposableRepository> logger)
        {
            this.connection = connection;
            this.logger = logger;
            logger.LogInformation("New instance of Repository is created.");
        }

        public Task<int> TestConnection()
        {
            return connection.QuerySingleAsync<int>("select 1");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                connection?.Dispose();
                logger.LogInformation("Instance of Repository is disposed.");
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}