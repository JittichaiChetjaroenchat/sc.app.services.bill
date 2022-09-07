using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SC.App.Services.Bill.Database.Configurations
{
    public class ParcelConfiguration : BaseEntityTypeConfiguration, IEntityTypeConfiguration<Models.Parcel>
    {
        public void Configure(EntityTypeBuilder<Models.Parcel> builder)
        {
            builder.ToTable(Constants.Parcel.TableName);
            builder.Property(x => x.CreatedOn).HasDefaultValueSql(CURRENT_DATE);
        }
    }
}