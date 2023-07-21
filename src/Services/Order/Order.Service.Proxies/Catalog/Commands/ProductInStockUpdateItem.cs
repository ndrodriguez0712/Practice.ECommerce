using static Order.Common.Enums;

namespace Order.Service.Proxies.Catalog.Commands
{
    public class ProductInStockUpdateItem
    {
        public int ProductId { get; set; }
        public int Stock { get; set; }
        public ProductInStockAction Action { get; set; }
    }
}
