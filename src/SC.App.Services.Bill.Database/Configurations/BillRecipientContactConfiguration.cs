using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SC.App.Services.Bill.Database.Configurations
{
    public class BillRecipientContactConfiguration : BaseEntityTypeConfiguration, IEntityTypeConfiguration<Models.BillRecipientContact>
    {
        public void Configure(EntityTypeBuilder<Models.BillRecipientContact> builder)
        {
            builder.ToTable(Constants.BillRecipientContact.TableName);
            builder.Property(x => x.CreatedOn).HasDefaultValueSql(CURRENT_DATE);
            builder.Property(x => x.UpdatedOn).HasDefaultValueSql(CURRENT_DATE);
        }
    }
}