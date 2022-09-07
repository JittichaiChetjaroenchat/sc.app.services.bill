using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SC.App.Services.Bill.Database.Models
{
    public class PaymentVerificationDetail
    {
        [Key]
        [Column(Constants.PaymentVerificationDetail.Column.Id, TypeName = "varchar(36)")]
        public Guid Id { get; set; }

        [Column(Constants.PaymentVerificationDetail.Column.PaymentVerificationId, TypeName = "varchar(36)")]
        public Guid PaymentVerificationId { get; set; }

        [Required]
        [MaxLength(8)]
        [Column(Constants.PaymentVerificationDetail.Column.SourceBankCode, TypeName = "varchar(8)")]
        public string SourceBankCode { get; set; }

        [MaxLength(16)]
        [Column(Constants.PaymentVerificationDetail.Column.SourceBankAccountType, TypeName = "varchar(16)")]
        public string SourceBankAccountType { get; set; }

        [MaxLength(32)]
        [Column(Constants.PaymentVerificationDetail.Column.SourceBankAccountNumber, TypeName = "varchar(32)")]
        public string SourceBankAccountNumber { get; set; }

        [MaxLength(128)]
        [Column(Constants.PaymentVerificationDetail.Column.SourceBankAccountName, TypeName = "varchar(128)")]
        public string SourceBankAccountName { get; set; }

        [MaxLength(128)]
        [Column(Constants.PaymentVerificationDetail.Column.SourceBankAccountDisplayName, TypeName = "varchar(128)")]
        public string SourceBankAccountDisplayName { get; set; }

        [Required]
        [MaxLength(8)]
        [Column(Constants.PaymentVerificationDetail.Column.DestinationBankCode, TypeName = "varchar(8)")]
        public string DestinationBankCode { get; set; }

        [MaxLength(16)]
        [Column(Constants.PaymentVerificationDetail.Column.DestinationBankAccountType, TypeName = "varchar(16)")]
        public string DestinationBankAccountType { get; set; }

        [MaxLength(32)]
        [Column(Constants.PaymentVerificationDetail.Column.DestinationBankAccountNumber, TypeName = "varchar(32)")]
        public string DestinationBankAccountNumber { get; set; }

        [MaxLength(128)]
        [Column(Constants.PaymentVerificationDetail.Column.DestinationBankAccountName, TypeName = "varchar(128)")]
        public string DestinationBankAccountName { get; set; }

        [MaxLength(128)]
        [Column(Constants.PaymentVerificationDetail.Column.DestinationBankAccountDisplayName, TypeName = "varchar(128)")]
        public string DestinationBankAccountDisplayName { get; set; }

        [Column(Constants.PaymentVerificationDetail.Column.Amount, TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(32)]
        [Column(Constants.PaymentVerificationDetail.Column.TransactionRefNo, TypeName = "varchar(32)")]
        public string TransactionRefNo { get; set; }

        [Column(Constants.PaymentVerificationDetail.Column.TransactionDate, TypeName = "datetime")]
        public DateTime TransactionDate { get; set; }

        #region Relationships

        public virtual PaymentVerification PaymentVerification { get; set; }

        #endregion
    }
}