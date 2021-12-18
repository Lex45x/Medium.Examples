using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading.Tasks;
using Dapper;

namespace QuickAnswer.Database
{
    public class Repository
    {
        private readonly SQLiteConnection connection;

        public Repository(SQLiteConnection connection)
        {
            this.connection = connection;
        }

        public Task<int> TestConnection()
        {
            return connection.QuerySingleAsync<int>("select 1");
        }
    }
}