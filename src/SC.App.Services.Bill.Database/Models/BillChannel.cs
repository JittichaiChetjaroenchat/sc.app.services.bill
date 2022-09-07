using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SC.App.Services.Bill.Database.Models
{
    public class BillChannel
    {
        [Key]
        [Column(Constants.BillChannel.Column.Id, TypeName = "varchar(36)")]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(16)]
        [Column(Constants.BillChannel.Column.Code, TypeName = "varchar(16)")]
        public string Code { get; set; }

        [Required]
        [MaxLength(128)]
        [Column(Constants.BillChannel.Column.Description, TypeName = "varchar(128)")]
        public string Description { get; set; }

        [Column(Constants.BillChannel.Column.Index)]
        public int Index { get; set; }

        #region Relationships

        #endregion
    }
}