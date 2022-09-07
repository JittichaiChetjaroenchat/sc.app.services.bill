using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Database.Configurations
{
    public class BillShippingConfiguration : BaseEntityTypeConfiguration, IEntityTypeConfiguration<BillShipping>
    {
        public void Configure(EntityTypeBuilder<BillShipping> builder)
        {
            builder.ToTable(Constants.BillShipping.TableName);
            builder.Property(x => x.CreatedOn).HasDefaultValueSql(CURRENT_DATE);
            builder.Property(x => x.UpdatedOn).HasDefaultValueSql(CURRENT_DATE);
            builder.HasIndex(u => u.BillId)
                .IsUnique();
        }
    }
}