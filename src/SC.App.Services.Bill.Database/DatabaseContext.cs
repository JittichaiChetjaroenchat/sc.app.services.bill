using Microsoft.EntityFrameworkCore;

namespace SC.App.Services.Bill.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Models.Bill> Bills { get; set; }
        public DbSet<Models.BillConfiguration> BillConfigurations { get; set; }
        public DbSet<Models.BillDiscount> BillDiscounts { get; set; }
        public DbSet<Models.BillNotification> BillNotifications { get; set; }
        public DbSet<Models.BillPayment> BillPayments { get; set; }
        public DbSet<Models.BillRecipient> BillRecipients { get; set; }
        public DbSet<Models.BillRecipientContact> BillRecipientContacts { get; set; }
        public DbSet<Models.BillShipping> BillShippings { get; set; }
        public DbSet<Models.BillShippingRangeRule> BillShippingRangeRules { get; set; }
        public DbSet<Models.BillShippingRange> BillShippingRanges { get; set; }
        public DbSet<Models.BillShippingTotalRule> BillShippingTotalRules { get; set; }
        public DbSet<Models.BillShippingFreeRule> BillShippingFreeRules { get; set; }
        public DbSet<Models.BillTag> BillTags { get; set; }
        public DbSet<Models.Parcel> Parcels { get; set; }
        public DbSet<Models.Payment> Payments { get; set; }
        public DbSet<Models.PaymentVerification> PaymentVerifications { get; set; }
        public DbSet<Models.PaymentVerificationDetail> PaymentVerificationDetails { get; set; }
        public DbSet<Models.Tag> Tags { get; set; }

        #region Master
        public DbSet<Models.BillChannel> BillChannels { get; set; }
        public DbSet<Models.BillPaymentType> BillPaymentTypes { get; set; }
        public DbSet<Models.BillStatus> BillStatuses { get; set; }
        public DbSet<Models.ParcelStatus> ParcelStatuses { get; set; }
        public DbSet<Models.PaymentStatus> PaymentStatuses { get; set; }
        public DbSet<Models.PaymentVerificationStatus> PaymentVerificationStatuses { get; set; }

        #endregion

        /// <summary>
        /// Initialize database context
        /// </summary>
        /// <param name="options">The options</param>
        public DatabaseContext(DbContextOptions options)
            : base(options)
        {
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
        }
    }
}