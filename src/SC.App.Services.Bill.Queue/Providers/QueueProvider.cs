using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SC.App.Services.Bill.Common.Constants;
using SC.App.Services.Bill.Queue.Providers.Interface;
using Serilog;

namespace SC.App.Services.Bill.Queue.Providers
{
    public class QueueProvider : IQueueProvider, IDisposable
    {
        private readonly IConfiguration _configuration = null;
        private readonly IConnection _connection = null;
        private readonly IModel _channel = null;

        public QueueProvider(IConfiguration configuration)
        {
            _configuration = configuration;

            try
            {
                var host = _configuration.GetValue<string>(AppSettings.Queues.HostName);
                var username = _configuration.GetValue<string>(AppSettings.Queues.UserName);
                var password = _configuration.GetValue<string>(AppSettings.Queues.Password);
                var factory = new ConnectionFactory { HostName = host, UserName = username, Password = password };

                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, string.Empty);
            }
        }

        public void Publish<T>(string queue, string exchange, string routingKey, T payload)
        {
            try
            {
                var json = JsonConvert.SerializeObject(payload);
                var body = Encoding.UTF8.GetBytes(json);

                _channel.BasicPublish(exchange, routingKey, null, body);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
            }
        }

        public void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
            }

            if (_connection.IsOpen)
            {
                _connection.Close();
            }
        }
    }
}