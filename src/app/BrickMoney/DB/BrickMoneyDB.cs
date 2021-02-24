using BrickMoney.DB.Mappings;
using BrickMoney.DB.Models;
using Microsoft.EntityFrameworkCore;

namespace BrickMoney.DB
{
    public class BrickMoneyDB : DbContext
    {
        public DbSet<LegoSetUserInfo> UserInfo   { get; set; }
        public DbSet<LegoSetBasicInfo> BasicInfo { get; set; }

        public BrickMoneyDB(DbContextOptions<BrickMoneyDB> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //
            modelBuilder.ApplyConfiguration(new LegoSetUserInfoMapping());
            modelBuilder.ApplyConfiguration(new LegoSetBasicInfoMapping());
        }
    }
}
