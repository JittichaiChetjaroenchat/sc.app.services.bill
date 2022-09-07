using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SC.App.Services.Bill.Database.Models
{
    public class BillTag
    {
        [Key]
        [Column(Constants.BillTag.Column.Id, TypeName = "varchar(36)")]
        public Guid Id { get; set; }

        [Column(Constants.BillTag.Column.BillId, TypeName = "varchar(36)")]
        public Guid BillId { get; set; }

        [Column(Constants.BillTag.Column.TagId, TypeName = "varchar(36)")]
        public Guid TagId { get; set; }

        [Column(Constants.BillTag.Column.CreatedBy, TypeName = "varchar(36)")]
        public Guid CreatedBy { get; set; }

        [Column(Constants.BillTag.Column.CreatedOn, TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }

        [Column(Constants.BillTag.Column.UpdatedBy, TypeName = "varchar(36)")]
        public Guid UpdatedBy { get; set; }

        [Column(Constants.BillTag.Column.UpdatedOn, TypeName = "datetime")]
        public DateTime UpdatedOn { get; set; }

        #region Relationships

        public virtual Bill Bill { get; set; }

        public virtual Tag Tag { get; set; }

        #endregion
    }
}