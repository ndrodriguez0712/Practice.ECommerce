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
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;
        #endregion

        #region Constructor
        public CatalogProxy(
            HttpClient httpClient,
            IOptions<ApiUrls> apiUrls,
            IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }
        #endregion

        public async Task UpdateStockAsync(ProductInStockUpdateStockCommand command)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(command),
                Encoding.UTF8,
                "application/json"
            );

            var request = await _httpClient.PutAsync(_apiUrls.CatalogUrl + "v1/stocks", content);

            request.EnsureSuccessStatusCode();
        }
    }
}
