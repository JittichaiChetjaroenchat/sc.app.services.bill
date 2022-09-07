using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SC.App.Services.Bill.Database.Models
{
    public class Tag
    {
        [Key]
        [Column(Constants.Tag.Column.Id, TypeName = "varchar(36)")]
        public Guid Id { get; set; }

        [Column(Constants.Tag.Column.ChannelId, TypeName = "varchar(36)")]
        public Guid ChannelId { get; set; }

        [Required]
        [MaxLength(8)]
        [Column(Constants.Tag.Column.Color, TypeName = "varchar(8)")]
        public string Color { get; set; }

        [Required]
        [MaxLength(16)]
        [Column(Constants.Tag.Column.Name, TypeName = "varchar(16)")]
        public string Name { get; set; }

        [MaxLength(64)]
        [Column(Constants.Tag.Column.Description, TypeName = "varchar(64)")]
        public string Description { get; set; }

        [Column(Constants.Tag.Column.CreatedBy, TypeName = "varchar(36)")]
        public Guid CreatedBy { get; set; }

        [Column(Constants.Tag.Column.CreatedOn, TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }

        [Column(Constants.Tag.Column.UpdatedBy, TypeName = "varchar(36)")]
        public Guid UpdatedBy { get; set; }

        [Column(Constants.Tag.Column.UpdatedOn, TypeName = "datetime")]
        public DateTime UpdatedOn { get; set; }

        #region Relationships

        #endregion
    }
}