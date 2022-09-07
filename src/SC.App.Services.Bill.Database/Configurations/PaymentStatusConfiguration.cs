using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.App.Services.Bill.Lib.Helpers;

namespace SC.App.Services.Bill.Database.Configurations
{
    public class PaymentStatusConfiguration : BaseEntityTypeConfiguration, IEntityTypeConfiguration<Models.PaymentStatus>
    {
        public void Configure(EntityTypeBuilder<Models.PaymentStatus> builder)
        {
            builder.ToTable(Constants.PaymentStatus.TableName);
            builder.HasIndex(u => u.Code)
                .IsUnique();
            builder.HasData(
                new Models.PaymentStatus { Id = GuidHelper.Generate(Data.PaymentStatus.Pending.Code), Code = Data.PaymentStatus.Pending.Code, Description = Data.PaymentStatus.Pending.Description, Index = 1 },
                new Models.PaymentStatus { Id = GuidHelper.Generate(Data.PaymentStatus.Rejected.Code), Code = Data.PaymentStatus.Rejected.Code, Description = Data.PaymentStatus.Rejected.Description, Index = 2 },
                new Models.PaymentStatus { Id = GuidHelper.Generate(Data.PaymentStatus.Accepted.Code), Code = Data.PaymentStatus.Accepted.Code, Description = Data.PaymentStatus.Accepted.Description, Index = 3 }
            );
        }
    }
}