using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceBusProvider.Interfaces;

namespace ServiceBusProvider
{
    public class ServiceBusQueue : IServiceBusQueue
    {
        #region Variables
        private readonly IConfiguration _configuration;
        private readonly ILogger<ServiceBusQueue> _logger;
        #endregion

        #region Constructor
        public ServiceBusQueue(IConfiguration configuration, ILogger<ServiceBusQueue> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        #endregion

        public async Task SendMessageAsync(string queueName, string serviceBusMessage)
        {
            try
            {
                ServiceBusClient serviceBusClient = new(_configuration.GetConnectionString("AzurServiceBusConnectionString"), new ServiceBusClientOptions()
                {
                    TransportType = ServiceBusTransportType.AmqpWebSockets
                });

                ServiceBusSender serviceBusSender = serviceBusClient.CreateSender(queueName);
                await serviceBusSender.SendMessageAsync(new ServiceBusMessage(serviceBusMessage));
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                _logger.LogError($"An error has occurred sending a message to the queue {queueName}. {msg}");
            }
        }
    }
}
