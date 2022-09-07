using System;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SC.App.Services.Area.Client;
using SC.App.Services.Bill.Business.Managers;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Business.Repositories;
using SC.App.Services.Bill.Business.Repositories.Interface;
using SC.App.Services.Bill.Client.Area;
using SC.App.Services.Bill.Client.Courier;
using SC.App.Services.Bill.Client.Credit;
using SC.App.Services.Bill.Client.Customer;
using SC.App.Services.Bill.Client.Document;
using SC.App.Services.Bill.Client.Inventory;
using SC.App.Services.Bill.Client.Notification;
using SC.App.Services.Bill.Client.Order;
using SC.App.Services.Bill.Client.Security;
using SC.App.Services.Bill.Client.Setting;
using SC.App.Services.Bill.Common.Constants;
using SC.App.Services.Bill.Queue.Managers;
using SC.App.Services.Bill.Queue.Managers.Interface;
using SC.App.Services.Bill.Queue.Providers;
using SC.App.Services.Bill.Queue.Providers.Interface;
using SC.App.Services.Courier.Client;
using SC.App.Services.Credit.Client;
using SC.App.Services.Customer.Client;
using SC.App.Services.Document.Client;
using SC.App.Services.Inventory.Client;
using SC.App.Services.Notification.Client;
using SC.App.Services.Order.Client;
using SC.App.Services.Security.Client;
using SC.App.Services.Setting.Client;

namespace SC.App.Services.Bill.Configurations.Extensions
{
    public static class DependencyExtension
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddManagers();
            services.AddRepositories();
            services.AddQueues();
            services.AddHandlers();
            services.AddClients(configuration);
        }

        private static void AddManagers(this IServiceCollection services)
        {
            services.AddScoped<IBillManager, BillManager>();
            services.AddScoped<IBillNotificationManager, BillNotificationManager>();
            services.AddScoped<IBillRecipientManager, BillRecipientManager>();
            services.AddScoped<IOfflineBillManager, OfflineBillManager>();
            services.AddScoped<IOnlineBillManager, OnlineBillManager>();
            services.AddScoped<IBillConfigurationManager, BillConfigurationManager>();
            services.AddScoped<IBillTagManager, BillTagManager>();
            services.AddScoped<IPaymentManager, PaymentManager>();
            services.AddScoped<IPaymentVerificationManager, PaymentVerificationManager>();
            services.AddScoped<IParcelManager, ParcelManager>();
            services.AddScoped<ITagManager, TagManager>();
            services.AddScoped<IStreamingManager, StreamingManager>();
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBillRepository, BillRepository>();
            services.AddScoped<IBillChannelRepository, BillChannelRepository>();
            services.AddScoped<IBillConfigurationRepository, BillConfigurationRepository>();
            services.AddScoped<IBillDiscountRepository, BillDiscountRepository>();
            services.AddScoped<IBillNotificationRepository, BillNotificationRepository>();
            services.AddScoped<IBillPaymentRepository, BillPaymentRepository>();
            services.AddScoped<IBillPaymentTypeRepository, BillPaymentTypeRepository>();
            services.AddScoped<IPaymentStatusRepository, PaymentStatusRepository>();
            services.AddScoped<IBillRecipientRepository, BillRecipientRepository>();
            services.AddScoped<IBillRecipientContactRepository, BillRecipientContactRepository>();
            services.AddScoped<IBillShippingRepository, BillShippingRepository>();
            services.AddScoped<IBillShippingRangeRuleRepository, BillShippingRangeRuleRepository>();
            services.AddScoped<IBillShippingRangeRepository, BillShippingRangeRepository>();
            services.AddScoped<IBillShippingTotalRuleRepository, BillShippingTotalRuleRepository>();
            services.AddScoped<IBillShippingFreeRuleRepository, BillShippingFreeRuleRepository>();
            services.AddScoped<IBillStatusRepository, BillStatusRepository>();
            services.AddScoped<IBillTagRepository, BillTagRepository>();
            services.AddScoped<IParcelRepository, ParcelRepository>();
            services.AddScoped<IParcelStatusRepository, ParcelStatusRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IPaymentVerificationRepository, PaymentVerificationRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
        }

        private static void AddQueues(this IServiceCollection services)
        {
            services.AddScoped<IQueueManager, QueueManager>();
            services.AddScoped<IQueueProvider, QueueProvider>();
        }

        private static void AddHandlers(this IServiceCollection services)
        {
            services.AddMediatR(typeof(Business.Startup));
        }

        private static void AddClients(this IServiceCollection services, IConfiguration configuration)
        {
            // Area
            var areaBaseUrl = configuration.GetValue<string>(AppSettings.Services.Area.BaseUrl);
            services.AddClient<IAreaClient, AreaClient>(areaBaseUrl);
            services.AddScoped<IAreaManager, AreaManager>();

            // Customer
            var customerBaseUrl = configuration.GetValue<string>(AppSettings.Services.Customer.BaseUrl);
            services.AddClient<ICustomerClient, CustomerClient>(customerBaseUrl);
            services.AddScoped<ICustomerManager, CustomerManager>();

            // Courier
            var courierBaseUrl = configuration.GetValue<string>(AppSettings.Services.Courier.BaseUrl);
            services.AddClient<ICourierClient, CourierClient>(courierBaseUrl);
            services.AddScoped<ICourierManager, CourierManager>();
            
            // Credit
            var creditBaseUrl = configuration.GetValue<string>(AppSettings.Services.Credit.BaseUrl);
            services.AddClient<ICreditClient, CreditClient>(creditBaseUrl);
            services.AddScoped<ICreditManager, CreditManager>();

            // Document
            var documentBaseUrl = configuration.GetValue<string>(AppSettings.Services.Document.BaseUrl);
            services.AddClient<IDocumentClient, DocumentClient>(documentBaseUrl);
            services.AddScoped<IDocumentManager, DocumentManager>();

            // Inventory
            var inventoryBaseUrl = configuration.GetValue<string>(AppSettings.Services.Inventory.BaseUrl);
            services.AddClient<IInventoryClient, InventoryClient>(inventoryBaseUrl);
            services.AddScoped<IInventoryManager, InventoryManager>();

            // Notification
            var notificationBaseUrl = configuration.GetValue<string>(AppSettings.Services.Notification.BaseUrl);
            services.AddClient<INotificationClient, NotificationClient>(notificationBaseUrl);
            services.AddScoped<INotificationManager, NotificationManager>();

            // Order
            var orderBaseUrl = configuration.GetValue<string>(AppSettings.Services.Order.BaseUrl);
            services.AddClient<IOrderClient, OrderClient>(orderBaseUrl);
            services.AddScoped<IOrderManager, OrderManager>();

            // Security
            var securityBaseUrl = configuration.GetValue<string>(AppSettings.Services.Security.BaseUrl);
            services.AddClient<ISecurityClient, SecurityClient>(securityBaseUrl);
            services.AddScoped<ISecurityManager, SecurityManager>();

            // Setting
            var settingBaseUrl = configuration.GetValue<string>(AppSettings.Services.Setting.BaseUrl);
            services.AddClient<ISettingClient, SettingClient>(settingBaseUrl);
            services.AddScoped<ISettingManager, SettingManager>();

            // Streaming
            var streamingBaseUrl = configuration.GetValue<string>(AppSettings.Services.Streaming.BaseUrl);
            services.AddClient<Streaming.Client.IStreamingClient, Streaming.Client.StreamingClient>(streamingBaseUrl);
            services.AddScoped<Client.Streaming.IStreamingManager, Client.Streaming.StreamingManager>();
        }

        private static void AddClient<TInterface, IImplementation>(this IServiceCollection services, string baseUrl)
            where TInterface : class
            where IImplementation : class, TInterface
        {
            services.AddHttpClient<TInterface, IImplementation>((client) =>
            {
                client.BaseAddress = new Uri(baseUrl);
            });
        }
    }
}