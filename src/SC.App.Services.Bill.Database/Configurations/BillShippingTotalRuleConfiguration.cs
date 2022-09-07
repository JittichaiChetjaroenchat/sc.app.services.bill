using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Database.Configurations
{
    public class BillShippingTotalRuleConfiguration : BaseEntityTypeConfiguration, IEntityTypeConfiguration<BillShippingTotalRule>
    {
        public void Configure(EntityTypeBuilder<BillShippingTotalRule> builder)
        {
            builder.ToTable(Constants.BillShippingTotalRule.TableName);
        }
    }
}