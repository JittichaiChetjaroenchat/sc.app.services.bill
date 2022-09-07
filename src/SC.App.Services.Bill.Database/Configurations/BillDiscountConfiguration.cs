using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Database.Configurations
{
    public class BillDiscountConfiguration : BaseEntityTypeConfiguration, IEntityTypeConfiguration<BillDiscount>
    {
        public void Configure(EntityTypeBuilder<BillDiscount> builder)
        {
            builder.ToTable(Constants.BillDiscount.TableName);
            builder.Property(x => x.CreatedOn).HasDefaultValueSql(CURRENT_DATE);
            builder.Property(x => x.UpdatedOn).HasDefaultValueSql(CURRENT_DATE);
            builder.HasIndex(u => u.BillId)
                .IsUnique();
        }
    }
}