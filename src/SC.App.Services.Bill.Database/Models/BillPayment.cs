using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SC.App.Services.Bill.Database.Models
{
    public class BillPayment
    {
        [Key]
        [Column(Constants.BillPayment.Column.Id, TypeName = "varchar(36)")]
        public Guid Id { get; set; }

        [Column(Constants.BillPayment.Column.BillId, TypeName = "varchar(36)")]
        public Guid BillId { get; set; }

        [Column(Constants.BillPayment.Column.BillPaymentTypeId, TypeName = "varchar(36)")]
        public Guid BillPaymentTypeId { get; set; }

        [Column(Constants.BillPayment.Column.HasCodAddOn)]
        public bool HasCodAddOn { get; set; }

        [Column(Constants.BillPayment.Column.CodAddOnAmount, TypeName = "decimal(18, 2)")]
        public decimal? CodAddOnAmount { get; set; }

        [Column(Constants.BillPayment.Column.CodAddOnPercentage, TypeName = "decimal(18, 2)")]
        public decimal? CodAddOnPercentage { get; set; }

        [Column(Constants.BillPayment.Column.HasVat)]
        public bool HasVat { get; set; }

        [Column(Constants.BillPayment.Column.IncludedVat)]
        public bool IncludedVat { get; set; }

        [Column(Constants.BillPayment.Column.VatPercentage, TypeName = "decimal(18, 2)")]
        public decimal VatPercentage { get; set; }

        [Column(Constants.BillPayment.Column.CreatedBy, TypeName = "varchar(36)")]
        public Guid CreatedBy { get; set; }

        [Column(Constants.BillPayment.Column.CreatedOn, TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }

        [Column(Constants.BillPayment.Column.UpdatedBy, TypeName = "varchar(36)")]
        public Guid UpdatedBy { get; set; }

        [Column(Constants.BillPayment.Column.UpdatedOn, TypeName = "datetime")]
        public DateTime UpdatedOn { get; set; }

        #region Relationships

        public virtual Bill Bill { get; set; }

        public virtual BillPaymentType BillPaymentType { get; set; }

        #endregion
    }
}