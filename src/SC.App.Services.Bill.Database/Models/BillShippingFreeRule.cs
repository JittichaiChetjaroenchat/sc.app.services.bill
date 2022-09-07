using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SC.App.Services.Bill.Database.Models
{
    public class BillShippingFreeRule
    {
        [Key]
        [Column(Constants.BillShippingFreeRule.Column.Id, TypeName = "varchar(36)")]
        public Guid Id { get; set; }

        [Column(Constants.BillShippingFreeRule.Column.BillShippingId, TypeName = "varchar(36)")]
        public Guid BillShippingId { get; set; }

        [Column(Constants.BillShippingFreeRule.Column.Amount)]
        public int Amount { get; set; }

        [Column(Constants.BillShippingFreeRule.Column.Price, TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Column(Constants.BillShippingFreeRule.Column.Enabled)]
        public bool Enabled { get; set; }

        #region Relationships

        public virtual BillShipping BillShipping { get; set; }

        #endregion
    }
}