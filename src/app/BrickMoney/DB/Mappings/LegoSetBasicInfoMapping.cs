using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BrickMoney.DB.Mappings
{
    public class LegoSetBasicInfoMapping : IEntityTypeConfiguration<LegoSetBasicInfo>
    {
        public void Configure(EntityTypeBuilder<LegoSetBasicInfo> builder)
        {
            builder.HasKey(t => t.LegoSetId);
            builder.Property(t => t.LegoSetId).ValueGeneratedOnAdd();
        }
    }
}
