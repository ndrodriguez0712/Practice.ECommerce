namespace Order.Service.Proxies.Catalog.Commands
{
    public class ProductInStockUpdateStockCommand
    {
        public IEnumerable<ProductInStockUpdateItem> Items { get; set; } = new List<ProductInStockUpdateItem>();
    }    
}
