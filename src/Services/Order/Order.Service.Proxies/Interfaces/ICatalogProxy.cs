using Order.Service.Proxies.Catalog.Commands;

namespace Order.Service.Proxies.Interfaces
{
    public interface ICatalogProxy
    {
        Task UpdateStockAsync(ProductInStockUpdateStockCommand command);
    }
}
