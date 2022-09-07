using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SC.App.Services.Bill.Database.Configurations
{
    public class TagConfiguration : BaseEntityTypeConfiguration, IEntityTypeConfiguration<Models.Tag>
    {
        public void Configure(EntityTypeBuilder<Models.Tag> builder)
        {
            builder.ToTable(Constants.Tag.TableName);
            builder.Property(x => x.CreatedOn).HasDefaultValueSql(CURRENT_DATE);
            builder.Property(x => x.UpdatedOn).HasDefaultValueSql(CURRENT_DATE);
            builder.HasIndex(u => new { u.ChannelId, u.Name })
                .IsUnique();
        }
    }
}