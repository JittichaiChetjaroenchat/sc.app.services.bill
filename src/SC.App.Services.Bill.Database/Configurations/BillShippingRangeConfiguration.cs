using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Database.Configurations
{
    public class BillShippingRangeConfiguration : BaseEntityTypeConfiguration, IEntityTypeConfiguration<BillShippingRange>
    {
        public void Configure(EntityTypeBuilder<BillShippingRange> builder)
        {
            builder.ToTable(Constants.BillShippingRange.TableName);
            builder.HasOne(a => a.BillShippingRangeRule)
                .WithMany(b => b.BillShippingRanges)
                .HasForeignKey(f => f.BillShippingRangeRuleId);
        }
    }
}