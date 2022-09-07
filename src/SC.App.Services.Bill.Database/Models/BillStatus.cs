﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SC.App.Services.Bill.Database.Models
{
    public class BillStatus
    {
        [Key]
        [Column(Constants.BillStatus.Column.Id, TypeName = "varchar(36)")]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(16)]
        [Column(Constants.BillStatus.Column.Code, TypeName = "varchar(16)")]
        public string Code { get; set; }

        [Required]
        [MaxLength(128)]
        [Column(Constants.BillStatus.Column.Description, TypeName = "varchar(128)")]
        public string Description { get; set; }

        [Column(Constants.BillStatus.Column.Index)]
        public int Index { get; set; }

        #region Relationships

        #endregion
    }
}