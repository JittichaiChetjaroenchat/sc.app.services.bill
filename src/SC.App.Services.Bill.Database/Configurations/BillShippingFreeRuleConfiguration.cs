using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Database.Configurations
{
    public class BillShippingFreeRuleConfiguration : BaseEntityTypeConfiguration, IEntityTypeConfiguration<BillShippingFreeRule>
    {
        public void Configure(EntityTypeBuilder<BillShippingFreeRule> builder)
        {
            builder.ToTable(Constants.BillShippingFreeRule.TableName);
        }
    }
}