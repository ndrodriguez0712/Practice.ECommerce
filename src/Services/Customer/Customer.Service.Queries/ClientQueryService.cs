using Customer.Persistence.Database;
using Customer.Service.Queries.DTOs;
using Customer.Service.Queries.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service.Common.Collection;
using Service.Common.Mapping;
using Service.Common.Paging;

namespace Order.Service.Queries
{
    public class ClientQueryService : IClientQueryService
    {
        #region Variables
        private readonly ApplicationDbContext _context;
        #endregion

        #region Constructor
        public ClientQueryService(
            ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        public async Task<DataCollection<ClientDto>> GetAllAsync(int page, int take, IEnumerable<int> clients = null)
        {
            var collection = await _context.Clients
                .Where(x => clients == null || clients.Contains(x.ClientId))
                .OrderBy(x => x.Name)
                .GetPagedAsync(page, take);

            return collection.MapTo<DataCollection<ClientDto>>();
        }

        public async Task<ClientDto> GetAsync(int id)
        {
            return (await _context.Clients.SingleAsync(x => x.ClientId == id)).MapTo<ClientDto>();
        }
    }
}
