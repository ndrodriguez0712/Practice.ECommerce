using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Order.Service.Proxies.Catalog.Commands;
using Order.Service.Proxies.Interfaces;
using System.Text;
using System.Text.Json;

namespace Order.Service.Proxies.Catalog
{
    public class CatalogProxy : ICatalogProxy
    {
        #region Variables
        private readonly string _connectionString;
        #endregion

        #region Constructor
        public CatalogProxy(IOptions<AzureServiceBus> connectionString, HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            _connectionString = connectionString.Value.ConnectionString;
            httpClient.AddBearerToken(httpContextAccessor);
        }
        #endregion

        public async Task UpdateStockAsync(ProductInStockUpdateStockCommand command)
        {
            var serviceBusClient = new ServiceBusClient(_connectionString);
            var queueClient = serviceBusClient.CreateSender("order-stock-update");

            string body = JsonSerializer.Serialize(command);
            ServiceBusMessage message = new ServiceBusMessage(Encoding.UTF8.GetBytes(body));

            await queueClient.SendMessageAsync(message);
            await queueClient.CloseAsync();
        }
    }
}
