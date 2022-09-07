using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SC.App.Services.Bill.Database.Models
{
    public class BillRecipientContact
    {
        [Key]
        [Column(Constants.BillRecipientContact.Column.Id, TypeName = "varchar(36)")]
        public Guid Id { get; set; }

        [Column(Constants.BillRecipientContact.Column.BillRecipientId, TypeName = "varchar(36)")]
        public Guid BillRecipientId { get; set; }

        [Required]
        [MaxLength(256)]
        [Column(Constants.BillRecipientContact.Column.Address, TypeName = "varchar(256)")]
        public string Address { get; set; }

        [Column(Constants.BillRecipientContact.Column.AreaId, TypeName = "varchar(36)")]
        public Guid? AreaId { get; set; }

        [Required]
        [MaxLength(16)]
        [Column(Constants.BillRecipientContact.Column.PrimaryPhone, TypeName = "varchar(16)")]
        public string PrimaryPhone { get; set; }

        [MaxLength(16)]
        [Column(Constants.BillRecipientContact.Column.SecondaryPhone, TypeName = "varchar(16)")]
        public string SecondaryPhone { get; set; }

        [MaxLength(256)]
        [Column(Constants.BillRecipientContact.Column.Email, TypeName = "varchar(256)")]
        public string Email { get; set; }

        [Column(Constants.BillRecipientContact.Column.CreatedBy, TypeName = "varchar(36)")]
        public Guid CreatedBy { get; set; }

        [Column(Constants.BillRecipientContact.Column.CreatedOn, TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }

        [Column(Constants.BillRecipientContact.Column.UpdatedBy, TypeName = "varchar(36)")]
        public Guid UpdatedBy { get; set; }

        [Column(Constants.BillRecipientContact.Column.UpdatedOn, TypeName = "datetime")]
        public DateTime UpdatedOn { get; set; }

        #region Relationships

        public virtual BillRecipient BillRecipient { get; set; }

        #endregion
    }
}