using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SC.App.Services.Bill.Database.Models
{
    public class BillDiscount
    {
        [Key]
        [Column(Constants.BillDiscount.Column.Id, TypeName = "varchar(36)")]
        public Guid Id { get; set; }

        [Column(Constants.BillDiscount.Column.BillId, TypeName = "varchar(36)")]
        public Guid BillId { get; set; }

        [Column(Constants.BillDiscount.Column.Amount, TypeName = "decimal(18, 2)")]
        public decimal? Amount { get; set; }

        [Column(Constants.BillDiscount.Column.Percentage, TypeName = "decimal(18, 2)")]
        public decimal? Percentage { get; set; }

        [Column(Constants.BillDiscount.Column.CreatedBy, TypeName = "varchar(36)")]
        public Guid CreatedBy { get; set; }

        [Column(Constants.BillDiscount.Column.CreatedOn, TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }

        [Column(Constants.BillDiscount.Column.UpdatedBy, TypeName = "varchar(36)")]
        public Guid UpdatedBy { get; set; }

        [Column(Constants.BillDiscount.Column.UpdatedOn, TypeName = "datetime")]
        public DateTime UpdatedOn { get; set; }

        #region Relationships

        public virtual Bill Bill { get; set; }

        #endregion
    }
}