using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SC.App.Services.Bill.Database.Models
{
    public class BillShippingRangeRule
    {
        [Key]
        [Column(Constants.BillShippingRangeRule.Column.Id, TypeName = "varchar(36)")]
        public Guid Id { get; set; }

        [Column(Constants.BillShippingRangeRule.Column.BillShippingId, TypeName = "varchar(36)")]
        public Guid BillShippingId { get; set; }

        [Column(Constants.BillShippingRangeRule.Column.Enabled)]
        public bool Enabled { get; set; }

        #region Relationships

        public virtual BillShipping BillShipping { get; set; }

        public virtual ICollection<BillShippingRange> BillShippingRanges { get; set; }

        #endregion
    }
}