using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SC.App.Services.Bill.Database.Models
{
    public class BillRecipient
    {
        [Key]
        [Column(Constants.BillRecipient.Column.Id, TypeName = "varchar(36)")]
        public Guid Id { get; set; }

        [Column(Constants.BillRecipient.Column.BillId, TypeName = "varchar(36)")]
        public Guid BillId { get; set; }

        [Column(Constants.BillRecipient.Column.CustomerId, TypeName = "varchar(36)")]
        public Guid CustomerId { get; set; }

        [Required]
        [MaxLength(128)]
        [Column(Constants.BillRecipient.Column.Name, TypeName = "varchar(128)")]
        public string Name { get; set; }

        [MaxLength(128)]
        [Column(Constants.BillRecipient.Column.AliasName, TypeName = "varchar(128)")]
        public string AliasName { get; set; }

        [Column(Constants.BillRecipient.Column.CreatedBy, TypeName = "varchar(36)")]
        public Guid CreatedBy { get; set; }

        [Column(Constants.BillRecipient.Column.CreatedOn, TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }

        [Column(Constants.BillRecipient.Column.UpdatedBy, TypeName = "varchar(36)")]
        public Guid UpdatedBy { get; set; }

        [Column(Constants.BillRecipient.Column.UpdatedOn, TypeName = "datetime")]
        public DateTime UpdatedOn { get; set; }

        #region Relationships

        public virtual Bill Bill { get; set; }

        public virtual BillRecipientContact BillRecipientContact { get; set; }

        #endregion
    }
}