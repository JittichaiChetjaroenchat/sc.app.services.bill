using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SC.App.Services.Bill.Database.Configurations
{
    public class BillRecipientConfiguration : BaseEntityTypeConfiguration, IEntityTypeConfiguration<Models.BillRecipient>
    {
        public void Configure(EntityTypeBuilder<Models.BillRecipient> builder)
        {
            builder.ToTable(Constants.BillRecipient.TableName);
            builder.Property(x => x.CreatedOn).HasDefaultValueSql(CURRENT_DATE);
            builder.Property(x => x.UpdatedOn).HasDefaultValueSql(CURRENT_DATE);
        }
    }
}