using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.App.Services.Bill.Lib.Helpers;

namespace SC.App.Services.Bill.Database.Configurations
{
    public class BillPaymentTypeConfiguration : BaseEntityTypeConfiguration, IEntityTypeConfiguration<Models.BillPaymentType>
    {
        public void Configure(EntityTypeBuilder<Models.BillPaymentType> builder)
        {
            builder.ToTable(Constants.BillPaymentType.TableName);
            builder.HasIndex(u => u.Code)
                .IsUnique();
            builder.HasData(
                new Models.BillPaymentType { Id = GuidHelper.Generate(Data.BillPaymentType.PrePaid.Code), Code = Data.BillPaymentType.PrePaid.Code, Description = Data.BillPaymentType.PrePaid.Description, Index = 1 },
                new Models.BillPaymentType { Id = GuidHelper.Generate(Data.BillPaymentType.PostPaid.Code), Code = Data.BillPaymentType.PostPaid.Code, Description = Data.BillPaymentType.PostPaid.Description, Index = 2 }
            );
        }
    }
}