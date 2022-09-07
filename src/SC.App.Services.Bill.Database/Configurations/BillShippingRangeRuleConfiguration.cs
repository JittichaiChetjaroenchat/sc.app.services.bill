using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Database.Configurations
{
    public class BillShippingRangeRuleConfiguration : BaseEntityTypeConfiguration, IEntityTypeConfiguration<BillShippingRangeRule>
    {
        public void Configure(EntityTypeBuilder<BillShippingRangeRule> builder)
        {
            builder.ToTable(Constants.BillShippingRangeRule.TableName);
        }
    }
}