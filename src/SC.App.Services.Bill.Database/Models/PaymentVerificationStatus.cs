using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SC.App.Services.Bill.Database.Models
{
    public class PaymentVerificationStatus
    {
        [Key]
        [Column(Constants.PaymentVerificationStatus.Column.Id, TypeName = "varchar(36)")]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(32)]
        [Column(Constants.PaymentVerificationStatus.Column.Code, TypeName = "varchar(32)")]
        public string Code { get; set; }

        [Required]
        [MaxLength(128)]
        [Column(Constants.PaymentVerificationStatus.Column.Description, TypeName = "varchar(128)")]
        public string Description { get; set; }

        [Column(Constants.PaymentVerificationStatus.Column.Index)]
        public int Index { get; set; }

        #region Relationships

        #endregion
    }
}