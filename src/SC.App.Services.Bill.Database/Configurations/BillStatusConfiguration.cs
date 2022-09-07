using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.App.Services.Bill.Lib.Helpers;

namespace SC.App.Services.Bill.Database.Configurations
{
    public class BillStatusConfiguration : BaseEntityTypeConfiguration, IEntityTypeConfiguration<Models.BillStatus>
    {
        public void Configure(EntityTypeBuilder<Models.BillStatus> builder)
        {
            builder.ToTable(Constants.BillStatus.TableName);
            builder.HasIndex(u => u.Code)
                .IsUnique();
            builder.HasData(
                new Models.BillStatus { Id = GuidHelper.Generate(Data.BillStatus.Pending.Code), Code = Data.BillStatus.Pending.Code, Description = Data.BillStatus.Pending.Description, Index = 1 },
                new Models.BillStatus { Id = GuidHelper.Generate(Data.BillStatus.Notified.Code), Code = Data.BillStatus.Notified.Code, Description = Data.BillStatus.Notified.Description, Index = 2 },
                new Models.BillStatus { Id = GuidHelper.Generate(Data.BillStatus.Rejected.Code), Code = Data.BillStatus.Rejected.Code, Description = Data.BillStatus.Rejected.Description, Index = 3 },
                new Models.BillStatus { Id = GuidHelper.Generate(Data.BillStatus.Confirmed.Code), Code = Data.BillStatus.Confirmed.Code, Description = Data.BillStatus.Confirmed.Description, Index = 4 },
                new Models.BillStatus { Id = GuidHelper.Generate(Data.BillStatus.Cancelled.Code), Code = Data.BillStatus.Cancelled.Code, Description = Data.BillStatus.Cancelled.Description, Index = 5 },
                new Models.BillStatus { Id = GuidHelper.Generate(Data.BillStatus.Done.Code), Code = Data.BillStatus.Done.Code, Description = Data.BillStatus.Done.Description, Index = 6 },
                new Models.BillStatus { Id = GuidHelper.Generate(Data.BillStatus.Archived.Code), Code = Data.BillStatus.Archived.Code, Description = Data.BillStatus.Archived.Description, Index = 7 },
                new Models.BillStatus { Id = GuidHelper.Generate(Data.BillStatus.Deleted.Code), Code = Data.BillStatus.Deleted.Code, Description = Data.BillStatus.Deleted.Description, Index = 8 }
            );
        }
    }
}