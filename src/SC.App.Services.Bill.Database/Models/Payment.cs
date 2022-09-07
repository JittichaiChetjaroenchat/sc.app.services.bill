using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SC.App.Services.Bill.Database.Models
{
    public class Payment
    {
        [Key]
        [Column(Constants.Payment.Column.Id, TypeName = "varchar(36)")]
        public Guid Id { get; set; }

        [Column(Constants.Payment.Column.BillId, TypeName = "varchar(36)")]
        public Guid BillId { get; set; }

        [Required]
        [MaxLength(16)]
        [Column(Constants.Payment.Column.PaymentNo, TypeName = "varchar(16)")]
        public string PaymentNo { get; set; }

        [Column(Constants.Payment.Column.Amount, TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Column(Constants.Payment.Column.EvidenceId, TypeName = "varchar(36)")]
        public Guid? EvidenceId { get; set; }

        [Column(Constants.Payment.Column.PayOn, TypeName = "datetime")]
        public DateTime PayOn { get; set; }

        [MaxLength(256)]
        [Column(Constants.Payment.Column.Remark, TypeName = "varchar(256)")]
        public string Remark { get; set; }

        [Column(Constants.Payment.Column.PaymentStatusId, TypeName = "varchar(36)")]
        public Guid PaymentStatusId { get; set; }

        [Column(Constants.Payment.Column.CreatedOn, TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }

        #region Relationships

        public virtual Bill Bill { get; set; }

        public virtual PaymentVerification PaymentVerification { get; set; }

        public virtual PaymentStatus PaymentStatus { get; set; }

        #endregion
    }
}