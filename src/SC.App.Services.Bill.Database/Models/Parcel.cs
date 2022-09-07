using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SC.App.Services.Bill.Database.Models
{
    public class Parcel
    {
        [Key]
        [Column(Constants.Parcel.Column.Id, TypeName = "varchar(36)")]
        public Guid Id { get; set; }

        [Column(Constants.Parcel.Column.BillId, TypeName = "varchar(36)")]
        public Guid BillId { get; set; }

        [Column(Constants.Parcel.Column.ParcelStatusId, TypeName = "varchar(36)")]
        public Guid ParcelStatusId { get; set; }

        [MaxLength(256)]
        [Column(Constants.Parcel.Column.Remark, TypeName = "varchar(256)")]
        public string Remark { get; set; }

        [Column(Constants.Parcel.Column.CreatedOn, TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }

        [Column(Constants.Parcel.Column.IsPrinted)]
        public bool IsPrinted { get; set; }

        [Column(Constants.Parcel.Column.IsPacked)]
        public bool IsPacked { get; set; }

        #region Relationships

        public virtual Bill Bill { get; set; }

        public virtual ParcelStatus ParcelStatus { get; set; }

        #endregion
    }
}