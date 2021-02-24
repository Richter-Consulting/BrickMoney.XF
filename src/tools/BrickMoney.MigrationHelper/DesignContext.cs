using BrickMoney.DB;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BrickMoney.MigrationHelper
{
    public class DesignContext : IDesignTimeDbContextFactory<BrickMoneyDB>
    {
        public BrickMoneyDB CreateDbContext(string[] args)
        {
            var con = new SqliteConnectionStringBuilder
            {
                Mode = SqliteOpenMode.Memory
            };

            return new BrickMoneyDB(new DbContextOptionsBuilder<BrickMoneyDB>().UseSqlite(con.ConnectionString).Options);
        }
    }
}
