using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.App.Services.Bill.Lib.Helpers;

namespace SC.App.Services.Bill.Database.Configurations
{
    public class PaymentVerificationStatusConfiguration : BaseEntityTypeConfiguration, IEntityTypeConfiguration<Models.PaymentVerificationStatus>
    {
        public void Configure(EntityTypeBuilder<Models.PaymentVerificationStatus> builder)
        {
            builder.ToTable(Constants.PaymentVerificationStatus.TableName);
            builder.HasIndex(u => u.Code)
                .IsUnique();
            builder.HasData(
                new Models.PaymentVerificationStatus { Id = GuidHelper.Generate(Data.PaymentVerificationStatus.NotVerify.Code), Code = Data.PaymentVerificationStatus.NotVerify.Code, Description = Data.PaymentVerificationStatus.NotVerify.Description, Index = 1 },
                new Models.PaymentVerificationStatus { Id = GuidHelper.Generate(Data.PaymentVerificationStatus.Unverifiable.Code), Code = Data.PaymentVerificationStatus.Unverifiable.Code, Description = Data.PaymentVerificationStatus.Unverifiable.Description, Index = 2 },
                new Models.PaymentVerificationStatus { Id = GuidHelper.Generate(Data.PaymentVerificationStatus.Duplicate.Code), Code = Data.PaymentVerificationStatus.Duplicate.Code, Description = Data.PaymentVerificationStatus.Duplicate.Description, Index = 3 },
                new Models.PaymentVerificationStatus { Id = GuidHelper.Generate(Data.PaymentVerificationStatus.IncorrectBankAccountNumber.Code), Code = Data.PaymentVerificationStatus.IncorrectBankAccountNumber.Code, Description = Data.PaymentVerificationStatus.IncorrectBankAccountNumber.Description, Index = 4 },
                new Models.PaymentVerificationStatus { Id = GuidHelper.Generate(Data.PaymentVerificationStatus.IncorrectBankAccountName.Code), Code = Data.PaymentVerificationStatus.IncorrectBankAccountName.Code, Description = Data.PaymentVerificationStatus.IncorrectBankAccountName.Description, Index = 5 },
                new Models.PaymentVerificationStatus { Id = GuidHelper.Generate(Data.PaymentVerificationStatus.IncorrectAmount.Code), Code = Data.PaymentVerificationStatus.IncorrectAmount.Code, Description = Data.PaymentVerificationStatus.IncorrectAmount.Description, Index = 6 },
                new Models.PaymentVerificationStatus { Id = GuidHelper.Generate(Data.PaymentVerificationStatus.Verified.Code), Code = Data.PaymentVerificationStatus.Verified.Code, Description = Data.PaymentVerificationStatus.Verified.Description, Index = 7 }
            );
        }
    }
}