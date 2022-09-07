using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SC.App.Services.Bill.Database.Models
{
    public class BillShippingTotalRule
    {
        [Key]
        [Column(Constants.BillShippingTotalRule.Column.Id, TypeName = "varchar(36)")]
        public Guid Id { get; set; }

        [Column(Constants.BillShippingTotalRule.Column.BillShippingId, TypeName = "varchar(36)")]
        public Guid BillShippingId { get; set; }

        [Column(Constants.BillShippingTotalRule.Column.Cost, TypeName = "decimal(18, 2)")]
        public decimal Cost { get; set; }

        [Column(Constants.BillShippingTotalRule.Column.Enabled)]
        public bool Enabled { get; set; }

        #region Relationships

        public virtual BillShipping BillShipping { get; set; }

        #endregion
    }
}