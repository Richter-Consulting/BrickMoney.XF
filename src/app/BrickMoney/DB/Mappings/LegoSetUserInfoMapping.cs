using BrickMoney.DB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BrickMoney.DB.Mappings
{
    public class LegoSetUserInfoMapping : IEntityTypeConfiguration<LegoSetUserInfo>
    {
        public void Configure(EntityTypeBuilder<LegoSetUserInfo> builder)
        {
            builder.ToTable("LegoSetUserInfo");
            builder.HasKey(k => k.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            // Relations
            builder.HasOne(t => t.LegoSetBasic)
                .WithMany()
                .HasForeignKey(testc => testc.LegoSetId);
        }
    }
}
