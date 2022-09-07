using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SC.App.Services.Bill.Database.Configurations
{
    public class PaymentVerificationDetailConfiguration : BaseEntityTypeConfiguration, IEntityTypeConfiguration<Models.PaymentVerificationDetail>
    {
        public void Configure(EntityTypeBuilder<Models.PaymentVerificationDetail> builder)
        {
            builder.ToTable(Constants.PaymentVerificationDetail.TableName);
            builder.Property(x => x.TransactionDate).HasDefaultValueSql(CURRENT_DATE);
            builder.HasIndex(u => u.PaymentVerificationId)
                .IsUnique();
        }
    }
}