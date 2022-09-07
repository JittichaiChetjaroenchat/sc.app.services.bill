using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SC.App.Services.Bill.Database.Models
{
    public class BillNotification
    {
        [Key]
        [Column(Constants.BillNotification.Column.Id, TypeName = "varchar(36)")]
        public Guid Id { get; set; }

        [Column(Constants.BillNotification.Column.BillId, TypeName = "varchar(36)")]
        public Guid BillId { get; set; }

        [Column(Constants.BillNotification.Column.IsIssueNotified)]
        public bool IsIssueNotified { get; set; }

        [Column(Constants.BillNotification.Column.IssueNotifiedOn, TypeName = "datetime")]
        public DateTime? IssueNotifiedOn { get; set; }

        [Column(Constants.BillNotification.Column.CanNotifyIssue)]
        public bool? CanNotifyIssue { get; set; }

        [Column(Constants.BillNotification.Column.IsBeforeCancelNotified)]
        public bool IsBeforeCancelNotified { get; set; }

        [Column(Constants.BillNotification.Column.BeforeCancelNotifiedOn, TypeName = "datetime")]
        public DateTime? BeforeCancelNotifiedOn { get; set; }

        [Column(Constants.BillNotification.Column.CanNotifyBeforeCancel)]
        public bool? CanNotifyBeforeCancel { get; set; }

        [Column(Constants.BillNotification.Column.IsCancelNotified)]
        public bool IsCancelNotified { get; set; }

        [Column(Constants.BillNotification.Column.CancelNotifiedOn, TypeName = "datetime")]
        public DateTime? CancelNotifiedOn { get; set; }

        [Column(Constants.BillNotification.Column.CanNotifyCancel)]
        public bool? CanNotifyCancel { get; set; }

        [Column(Constants.BillNotification.Column.IsSummaryNotified)]
        public bool IsSummaryNotified { get; set; }

        [Column(Constants.BillNotification.Column.SummaryNotifiedOn, TypeName = "datetime")]
        public DateTime? SummaryNotifiedOn { get; set; }

        [Column(Constants.BillNotification.Column.CanNotifySummary)]
        public bool? CanNotifySummary { get; set; }

        [Column(Constants.BillNotification.Column.CreatedBy, TypeName = "varchar(36)")]
        public Guid CreatedBy { get; set; }

        [Column(Constants.BillNotification.Column.CreatedOn, TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }

        [Column(Constants.BillNotification.Column.UpdatedBy, TypeName = "varchar(36)")]
        public Guid UpdatedBy { get; set; }

        [Column(Constants.BillNotification.Column.UpdatedOn, TypeName = "datetime")]
        public DateTime UpdatedOn { get; set; }

        #region Relationships

        public virtual Bill Bill { get; set; }

        #endregion
    }
}