using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SC.App.Services.Bill.Database.Models
{
    public class BillShippingRange
    {
        [Key]
        [Column(Constants.BillShippingRange.Column.Id, TypeName = "varchar(36)")]
        public Guid Id { get; set; }

        [Column(Constants.BillShippingRange.Column.BillShippingRangeRuleId, TypeName = "varchar(36)")]
        public Guid BillShippingRangeRuleId { get; set; }

        [Column(Constants.BillShippingRange.Column.Begin)]
        public int Begin { get; set; }

        [Column(Constants.BillShippingRange.Column.End)]
        public int End { get; set; }

        [Required]
        [Column(Constants.BillShippingRange.Column.Cost, TypeName = "decimal(18, 2)")]
        public decimal Cost { get; set; }

        #region Relationships

        public virtual BillShippingRangeRule BillShippingRangeRule { get; set; }

        #endregion
    }
}