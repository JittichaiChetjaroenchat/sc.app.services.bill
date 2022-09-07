using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SC.App.Services.Bill.Database.Models
{
    public class Bill
    {
        [Key]
        [Column(Constants.Bill.Column.Id, TypeName = "varchar(36)")]
        public Guid Id { get; set; }

        [Column(Constants.Bill.Column.ChannelId, TypeName = "varchar(36)")]
        public Guid ChannelId { get; set; }

        [Required]
        [MaxLength(16)]
        [Column(Constants.Bill.Column.BillNo, TypeName = "varchar(16)")]
        public string BillNo { get; set; }

        [Required]
        [MaxLength(16)]
        [Column(Constants.Bill.Column.RunningNo, TypeName = "varchar(16)")]
        public string RunningNo { get; set; }

        [Column(Constants.Bill.Column.BillChannelId, TypeName = "varchar(36)")]
        public Guid BillChannelId { get; set; }

        [Column(Constants.Bill.Column.BillStatusId, TypeName = "varchar(36)")]
        public Guid BillStatusId { get; set; }

        [Column(Constants.Bill.Column.IsDeposit)]
        public bool IsDeposit { get; set; }

        [Column(Constants.Bill.Column.IsNewCustomer)]
        public bool IsNewCustomer { get; set; }

        [MaxLength(256)]
        [Column(Constants.Bill.Column.Remark, TypeName = "varchar(256)")]
        public string Remark { get; set; }

        [MaxLength(128)]
        [Column(Constants.Bill.Column.Key, TypeName = "varchar(128)")]
        public string Key { get; set; }

        [Column(Constants.Bill.Column.CreatedBy, TypeName = "varchar(36)")]
        public Guid CreatedBy { get; set; }

        [Column(Constants.Bill.Column.CreatedOn, TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }

        [Column(Constants.Bill.Column.UpdatedBy, TypeName = "varchar(36)")]
        public Guid UpdatedBy { get; set; }

        [Column(Constants.Bill.Column.UpdatedOn, TypeName = "datetime")]
        public DateTime UpdatedOn { get; set; }

        #region Relationships

        public virtual BillChannel BillChannel { get; set; }

        public virtual BillDiscount BillDiscount { get; set; }

        public virtual BillNotification BillNotification { get; set; }

        public virtual BillRecipient BillRecipient { get; set; }

        public virtual BillPayment BillPayment { get; set; }

        public virtual BillShipping BillShipping { get; set; }

        public virtual BillStatus BillStatus { get; set; }

        public virtual ICollection<BillTag> BillTags { get; set; }

        public virtual ICollection<Parcel> Parcels { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }

        #endregion
    }
}