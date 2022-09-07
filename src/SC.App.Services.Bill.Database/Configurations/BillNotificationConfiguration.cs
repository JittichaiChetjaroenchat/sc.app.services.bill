using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Database.Configurations
{
    public class BillNotificationConfiguration : BaseEntityTypeConfiguration, IEntityTypeConfiguration<BillNotification>
    {
        public void Configure(EntityTypeBuilder<BillNotification> builder)
        {
            builder.ToTable(Constants.BillNotification.TableName);
            builder.Property(x => x.CreatedOn).HasDefaultValueSql(CURRENT_DATE);
            builder.Property(x => x.UpdatedOn).HasDefaultValueSql(CURRENT_DATE);
            builder.HasIndex(u => u.BillId)
                .IsUnique();
        }
    }
}