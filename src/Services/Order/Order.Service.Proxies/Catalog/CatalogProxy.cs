using Order.Service.Proxies.Catalog.Commands;
using Order.Service.Proxies.Interfaces;
using ServiceBusProvider.Interfaces;
using System.Text.Json;

namespace Order.Service.Proxies.Catalog
{
    public class CatalogProxy : ICatalogProxy
    {
        #region Variables
        private readonly IServiceBusQueue _serviceBusQueue;
        #endregion

        #region Constructor
        public CatalogProxy(IServiceBusQueue serviceBusQueue)
        {
            _serviceBusQueue = serviceBusQueue;
        }
        #endregion

        public async Task UpdateStockAsync(ProductInStockUpdateStockCommand command)
        {
            string message = JsonSerializer.Serialize(command);
            await _serviceBusQueue.SendMessageAsync("order-stock-handler", message);
        }
    }
}
