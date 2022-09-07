using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.App.Services.Bill.Lib.Helpers;

namespace SC.App.Services.Bill.Database.Configurations
{
    public class ParcelStatusConfiguration : BaseEntityTypeConfiguration, IEntityTypeConfiguration<Models.ParcelStatus>
    {
        public void Configure(EntityTypeBuilder<Models.ParcelStatus> builder)
        {
            builder.ToTable(Constants.ParcelStatus.TableName);
            builder.HasIndex(u => u.Code)
                .IsUnique();
            builder.HasData(
                new Models.ParcelStatus { Id = GuidHelper.Generate(Data.ParcelStatus.Active.Code), Code = Data.ParcelStatus.Active.Code, Description = Data.ParcelStatus.Active.Description, Index = 1 },
                new Models.ParcelStatus { Id = GuidHelper.Generate(Data.ParcelStatus.Cancelled.Code), Code = Data.ParcelStatus.Cancelled.Code, Description = Data.ParcelStatus.Cancelled.Description, Index = 2 }
            );
        }
    }
}