using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SC.App.Services.Bill.Database.Configurations
{
    public class BillTagConfiguration : BaseEntityTypeConfiguration, IEntityTypeConfiguration<Models.BillTag>
    {
        public void Configure(EntityTypeBuilder<Models.BillTag> builder)
        {
            builder.ToTable(Constants.BillTag.TableName);
            builder.Property(x => x.CreatedOn).HasDefaultValueSql(CURRENT_DATE);
            builder.Property(x => x.UpdatedOn).HasDefaultValueSql(CURRENT_DATE);
            builder.HasIndex(u => new { u.BillId, u.TagId })
                .IsUnique();
            builder.HasOne(a => a.Bill)
                .WithMany(b => b.BillTags)
                .HasForeignKey(f => f.BillId);
        }
    }
}