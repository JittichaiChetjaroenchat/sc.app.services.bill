using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.App.Services.Bill.Lib.Helpers;

namespace SC.App.Services.Bill.Database.Configurations
{
    public class BillChannelConfiguration : BaseEntityTypeConfiguration, IEntityTypeConfiguration<Models.BillChannel>
    {
        public void Configure(EntityTypeBuilder<Models.BillChannel> builder)
        {
            builder.ToTable(Constants.BillChannel.TableName);
            builder.HasIndex(u => u.Code)
                .IsUnique();
            builder.HasData(
                new Models.BillChannel { Id = GuidHelper.Generate(Data.BillChannel.Offline.Code), Code = Data.BillChannel.Offline.Code, Description = Data.BillChannel.Offline.Description, Index = 1 },
                new Models.BillChannel { Id = GuidHelper.Generate(Data.BillChannel.Facebook.Code), Code = Data.BillChannel.Facebook.Code, Description = Data.BillChannel.Facebook.Description, Index = 2 }
            );
        }
    }
}