using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SC.App.Services.Bill.Database.Configurations
{
    public class BillConfigurationConfiguration : BaseEntityTypeConfiguration, IEntityTypeConfiguration<Models.BillConfiguration>
    {
        public void Configure(EntityTypeBuilder<Models.BillConfiguration> builder)
        {
            builder.ToTable(Constants.BillConfiguration.TableName);
            builder.Property(x => x.CreatedOn).HasDefaultValueSql(CURRENT_DATE);
            builder.HasIndex(u => u.ChannelId)
                .IsUnique();
        }
    }
}