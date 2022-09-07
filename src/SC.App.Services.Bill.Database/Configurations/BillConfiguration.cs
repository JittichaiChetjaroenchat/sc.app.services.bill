using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SC.App.Services.Bill.Database.Configurations
{
    public class BillConfiguration : BaseEntityTypeConfiguration, IEntityTypeConfiguration<Models.Bill>
    {
        public void Configure(EntityTypeBuilder<Models.Bill> builder)
        {
            builder.ToTable(Constants.Bill.TableName);
            builder.Property(x => x.CreatedOn).HasDefaultValueSql(CURRENT_DATE);
            builder.Property(x => x.UpdatedOn).HasDefaultValueSql(CURRENT_DATE);
            builder.HasIndex(u => new { u.ChannelId, u.BillNo })
                .IsUnique();
        }
    }
}