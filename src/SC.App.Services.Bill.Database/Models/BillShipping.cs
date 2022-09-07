using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SC.App.Services.Bill.Database.Models
{
    public class BillShipping
    {
        [Key]
        [Column(Constants.BillShipping.Column.Id, TypeName = "varchar(36)")]
        public Guid Id { get; set; }

        [Column(Constants.BillShipping.Column.BillId, TypeName = "varchar(36)")]
        public Guid BillId { get; set; }

        [Column(Constants.BillShipping.Column.CreatedBy, TypeName = "varchar(36)")]
        public Guid CreatedBy { get; set; }

        [Column(Constants.BillShipping.Column.CreatedOn, TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }

        [Column(Constants.BillShipping.Column.UpdatedBy, TypeName = "varchar(36)")]
        public Guid UpdatedBy { get; set; }

        [Column(Constants.BillShipping.Column.UpdatedOn, TypeName = "datetime")]
        public DateTime UpdatedOn { get; set; }

        #region Relationships

        public virtual BillShippingFreeRule BillShippingFreeRule { get; set; }

        public virtual BillShippingTotalRule BillShippingTotalRule { get; set; }

        public virtual BillShippingRangeRule BillShippingRangeRule { get; set; }

        #endregion
    }
}