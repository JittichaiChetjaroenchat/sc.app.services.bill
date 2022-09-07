using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SC.App.Services.Bill.Database.Configurations
{
    public class PaymentConfiguration : BaseEntityTypeConfiguration, IEntityTypeConfiguration<Models.Payment>
    {
        public void Configure(EntityTypeBuilder<Models.Payment> builder)
        {
            builder.ToTable(Constants.Payment.TableName);
            builder.Property(x => x.PayOn).HasDefaultValueSql(CURRENT_DATE);
            builder.Property(x => x.CreatedOn).HasDefaultValueSql(CURRENT_DATE);
            builder.HasIndex(u => new { u.BillId, u.PaymentNo })
                .IsUnique();
        }
    }
}